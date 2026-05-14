import { createContext, useContext, useState, useEffect } from 'react';
import { useAuth } from './AuthContext'; 
import API_BASE_URL from '../config';

const CartContext = createContext();

export const CartProvider = ({ children }) => {
    const [cartItems, setCartItems] = useState([]);
    const [expirationTime, setExpirationTime] = useState(null);
    const [timeLeft, setTimeLeft] = useState(null);
    const { user } = useAuth(); 

    useEffect(() => {
        if (!expirationTime) return;

        const timer = setInterval(() => {
            const now = new Date().getTime();
            const distance = new Date(expirationTime).getTime() - now;
            
            if (distance <= 0) {
                clearInterval(timer);
                setTimeLeft(0);
                clearCart(); 
            } else {
                setTimeLeft(Math.floor(distance / 1000));
            }
        }, 1000);

        return () => clearInterval(timer);
    }, [expirationTime]);

    const addToCart = (reservation) => {
        setCartItems(prev => [...prev, reservation]);
        if (!expirationTime && reservation.expiresAt) {
            setExpirationTime(reservation.expiresAt);
        }
    };

    const removeFromCart = async (reservationId) => {
        
        setCartItems(prev => prev.filter(item => item.reservationId !== reservationId));
        
        if (cartItems.length <= 1) {
            setExpirationTime(null);
            setTimeLeft(null);
        }

        try {
           
            await fetch(`${API_BASE_URL}/reservations/${reservationId}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ 
                    ReservationId: reservationId, 
                    UserId: user?.userId || 0 
                })
            });
            console.log("Servidor notificado de la cancelación.");
        } catch (error) {
            console.error("Error al avisar al servidor (se borró localmente igualmente):", error);
        }
    };

    const clearCart = () => {
        setCartItems([]);
        setExpirationTime(null);
        setTimeLeft(null);
    };

    const formatTime = (seconds) => {
        if (seconds === null || seconds <= 0) return "00:00";
        const mins = Math.floor(seconds / 60);
        const secs = seconds % 60;
        return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
    };

    const totalAmount = cartItems.reduce((total, item) => total + (item.price || 0), 0);

    return (
        <CartContext.Provider value={{ 
            cartItems, 
            addToCart, 
            removeFromCart, 
            clearCart, 
            timeLeft, 
            formatTime,
            totalAmount 
        }}>
            {children}
        </CartContext.Provider>
    );
};

export const useCart = () => useContext(CartContext);
