import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
import Footer from '../components/Footer';
import PaymentForm from '../components/PaymentForm';

export default function Payment() {
    const { cartItems, clearCart, formatTime, timeLeft, totalAmount } = useCart();
    const { user } = useAuth();
    const navigate = useNavigate();

    const handlePaymentSuccess = () => {
        clearCart();
        
        navigate('/', { state: { successMessage: "¡Pago realizado con éxito! Tus entradas han sido enviadas a tu correo." } });
    };

    if (cartItems.length === 0) {
        return (
            <div className="bg-dark min-vh-100 d-flex flex-column align-items-center justify-content-center text-white">
                <i className="bi bi-cart-x display-1 text-secondary mb-4"></i>
                <h3 className="fw-bold">No hay reservas pendientes</h3>
                <p className="text-secondary">Tu carrito está vacío o el tiempo de reserva expiró.</p>
                <button className="btn btn-primary px-4 rounded-pill mt-3 fw-bold" onClick={() => navigate('/')}>
                    VOLVER AL CATÁLOGO
                </button>
            </div>
        );
    }

    return (
        <div className="bg-dark min-vh-100 d-flex flex-column">
            <Header />
            <div className="container flex-grow-1 py-5">
                <div className="row g-4 align-items-stretch">
                
                    <div className="col-lg-4 order-2 order-lg-1">
                        <div className="card bg-secondary bg-opacity-10 text-white shadow-lg border-0 rounded-4 h-100">
                            <div className="card-body p-4">
                                <h4 className="fw-bold mb-4 text-white bold text-uppercase small ls-1">Resumen de Compra</h4>
                                
                                <div className="cart-items-preview mb-4">
                                    {cartItems.map((item, index) => (
                                        <div key={index} className="d-flex justify-content-between align-items-center mb-3 p-2 rounded bg-dark bg-opacity-50">
                                            <div>
                                                <div className="fw-bold small">Butaca Reservada</div>
                                                <small className="text-white-50">
                                                    <i className="bi bi-geo-alt me-1 text-primary"></i>
                                                    Fila: <span className="text-white fw-bold">{item.row}</span> - 
                                                    As: <span className="text-white fw-bold">{item.number}</span>
                                                </small>
                                            </div>
                                            <span className="fw-bold text-success">${item.price}</span>
                                        </div>
                                    ))}
                                </div>

                                
                                <div className="d-flex justify-content-between mb-2 text-white-50 small">
                                    <span>Subtotal</span>
                                    <span>${totalAmount}</span>
                                </div>
                                <div className="d-flex justify-content-between mb-4">
                                    <h5 className="fw-bold">Total</h5>
                                    <h5 className="fw-bold text-success h4">${totalAmount}</h5>
                                </div>

                                <div className="alert bg-transparent border border-white text-center mb-0 py-3 rounded-3 shadow-sm">
                                    <small className="d-block text-uppercase fw-bold text-white bold mb-1 ls-1" >
                                        Tu lugar está asegurado por
                                    </small>
                                    <span className={`h3 fw-bold font-monospace ${timeLeft < 60 ? 'text-danger animate__animated animate__pulse animate__infinite' : 'text-warning'}`}>
                                        {formatTime(timeLeft)}
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="col-lg-8 order-1 order-lg-2">
                        <div className="card bg-secondary bg-opacity-10 text-white shadow-2xl border-0 rounded-4 overflow-hidden h-100">
                            <div className="card-header py-3 border-0 text-center">
                                <h4 className="mb-0 fw-bold text-uppercase ls-1">Información de Pago</h4>
                            </div>
                            <div className="card-body p-4 p-md-5">
                                

                                <PaymentForm 
                                    reservationId={cartItems[0].reservationId}
                                    userId={user.userId}
                                    onSuccess={handlePaymentSuccess}
                                    onCancel={() => navigate('/cart')}
                                />
                                
                                <div className="mt-4 pt-4 border-top border-secondary">
                                    <div className="d-flex justify-content-center gap-4 text-white-50 small">
                                        <span><i className="bi bi-shield-lock-fill text-success me-2"></i>Pago Seguro SSL</span>
                                        <span><i className="bi bi-check-circle-fill text-primary me-2"></i>Garantía de Compra</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <Footer />
        </div>
    );
}
