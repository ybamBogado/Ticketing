import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import API_BASE_URL from '../config';
import Header from '../components/Header';
import Footer from '../components/Footer';
import Loader from '../components/Loader';
import './EventDetail.css';

export default function EventDetail() {
    const { eventId } = useParams();
    const navigate = useNavigate();
    const { user } = useAuth();
    const { cartItems, addToCart, formatTime, timeLeft, totalAmount } = useCart();
    const [seats, setSeats] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [successMessage, setSuccessMessage] = useState(null);

    useEffect(() => {
        const fetchSeats = () => {
            fetch(`${API_BASE_URL}/events/${eventId}/seats`)
                .then(res => {
                    if (!res.ok) throw new Error("No se pudieron cargar los asientos");
                    return res.json();
                })
                .then(data => {
                    setSeats(data);
                    setLoading(false);
                })
                .catch(err => {
                    console.error(err);
                    setError("Error al cargar los asientos");
                    setLoading(false);
                });
        };

        fetchSeats();
        const interval = setInterval(fetchSeats, 5000);

        return () => clearInterval(interval);
    }, [eventId]);

    const handleReserva = async (seatId, currentStatus, Price) => {
        if (currentStatus !== 'Available') return;

        if (!user) {
            setError("Debes iniciar sesión para reservar una butaca.");
            setTimeout(() => navigate('/login'), 2000);
            return;
        }

        if (cartItems.some(item => item.seatId === seatId)) {
            setError("Ya tienes este asiento en tu carrito.");
            return;
        }

        const command = { SeatId: seatId, UserId: user.userId };

        try {
            const response = await fetch(`${API_BASE_URL}/reservations`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(command)
            });

            if (response.ok) {
                const data = await response.json();
                const seatInfo = seats.find(s => s.id === seatId); 
                
                setSeats(seats.map(s => s.id === seatId ? { ...s, status: 'Reserved' } : s));

                addToCart({
                    reservationId: data.reservationId,
                    seatId: seatId,
                    expiresAt: data.expiresAt,
                    price: Price || 500,
                    row: seatInfo?.rowIdentifier,
                    number: seatInfo?.seatNumber   
                });

                setError(null);
                setSuccessMessage(`¡Butaca ${seatInfo?.rowIdentifier}-${seatInfo?.seatNumber} agregada al carrito!`);
                setTimeout(() => setSuccessMessage(null), 2000); 
            } else if (response.status === 409) {
                setError("Este asiento ya fue reservado por otra persona.");
            } else if (response.status === 400) {
                const errorText = await response.text();
                setError(errorText || "La solicitud no es válida (butaca inexistente o ya ocupada).");
            } else {
                setError("Ocurrió un problema al procesar la reserva. Por favor, intenta de nuevo.");
            }
        } catch (error) {
            console.error("Error:", error);
            setError("Error de conexión al intentar reservar.");
        }
    };

    if (loading) {
        return (
            <>
                <Header />
                <Loader />
                <Footer />
            </>
        );
    }

    const renderSector = (sectorSeats, title) => {
        const grouped = sectorSeats.reduce((acc, seat) => {
            if (!acc[seat.rowIdentifier]) acc[seat.rowIdentifier] = [];
            acc[seat.rowIdentifier].push(seat);
            return acc;
        }, {});

        const sortedKeys = Object.keys(grouped).sort((a, b) => a.localeCompare(b, undefined, { numeric: true }));

        return (
            <div className="sector-container">
                <h4 className="text-center mb-4 text-white-50 fw-bold ls-2">{title}</h4>
                <div className="d-flex flex-column gap-2 align-items-center">
                    {sortedKeys.map(rowKey => (
                        <div key={rowKey} className="d-flex justify-content-center gap-2 min-w-max">
                            <div className="d-flex align-items-center justify-content-center fw-bold row-label bg-muted text-white rounded px-2 shadow-sm small">
                                {rowKey}
                            </div>
                            {grouped[rowKey]
                                .sort((a, b) => a.seatNumber - b.seatNumber)
                                .map(seat => {
                                    const inCart = cartItems.some(item => item.seatId === seat.id);
                                    const statusClass = inCart ? 'reserved' : seat.status.toLowerCase();

                                    return (
                                        <div
                                            key={seat.id}
                                            className={`seat-box ${statusClass.toLowerCase()}`}
                                            onClick={() => handleReserva(seat.id, seat.status, seat.price)}
                                            title={`Fila: ${seat.rowIdentifier} - Asiento: ${seat.seatNumber}`}
                                        >
                                            {seat.seatNumber}
                                        </div>
                                    );
                                })}
                        </div>
                    ))}
                </div>
            </div>
        );
    };

    return (
        <>
            <Header />
            <div className="container mt-4 detail-container">

                {error && (
                    <div className="alert alert-danger alert-dismissible fade show shadow" role="alert">
                        <strong>¡Atención!</strong> {error}
                        <button type="button" className="btn-close" onClick={() => setError(null)}></button>
                    </div>
                )}

                {successMessage && (
                    <div className="alert alert-success alert-dismissible fade show shadow" role="alert">
                        <strong>¡Éxito!</strong> {successMessage}
                        <button type="button" className="btn-close" onClick={() => setSuccessMessage(null)}></button>
                    </div>
                )}

                <h1 className="text-center my-4 fw-bold text-white text-uppercase">Mapa de Asientos</h1>

                <div className="card detail-card shadow-lg p-4 border-0 bg-dark text-white">
                    <div className="card-body">
                        <div className="escenario mb-5 text-center p-2 rounded">ESCENARIO</div>
                        {cartItems.length > 0 && (
                            
                            <div className="text-center  p-3 border border-white rounded-4 bg-dark bg-opacity-50 shadow-sm mb-4 ">
                                <small className="text-white bold d-block">Tiempo restante</small>
                                <span className={`fw-bold ${timeLeft < 60 ? 'text-white animate__animated animate__flash animate__infinite' : 'text-warning'}`}>
                                    {formatTime(timeLeft)}
                                </span>
                            </div>
                        )}

                        <div className="d-flex flex-column flex-xl-row gap-5 justify-content-center mb-4 overflow-auto">
                            {renderSector(seats.filter(s => s.rowIdentifier.startsWith('PL')), "PLATEA")}
                            {renderSector(seats.filter(s => s.rowIdentifier.startsWith('PO')), "POPULAR")}
                        </div>

                        <div className="mt-4 d-flex flex-column flex-md-row justify-content-around align-items-center border-top pt-3 gap-2">
                            <div className="legend-item text-success"><div className="legend-dot available"></div> Disponible</div>
                            <div className="legend-item text-warning"><div className="legend-dot reserved"></div> Reservado</div>
                            <div className="legend-item text-danger"><div className="legend-dot sold"></div> Vendido</div>
                        </div>
                    </div>
                </div>
            </div>

            {cartItems.length > 0 && (
                <div className="fixed-bottom bg-dark border-top border-primary p-3 shadow-lg animate__animated animate__slideInUp" >
                    <div className="container d-flex flex-column flex-md-row justify-content-between align-items-center gap-3">
                        <div className="d-flex align-items-center gap-4">
                            <div className="text-white">
                                <h5 className="mb-0 fw-bold">{cartItems.length} {cartItems.length === 1 ? 'Butaca' : 'Butacas'}</h5>
                                <small className="text-primary">Total: ${totalAmount}</small>
                            </div>
                            <div className="vr text-white-50 d-none d-md-block"></div>
                        </div>
                        <div className="d-flex gap-2">
                            <button className="btn btn-outline-light  px-4 btn-sm fw-bold text-uppercase" onClick={() => navigate('/cart')}>
                                Ver Carrito
                            </button>
                            <button className="btn btn-primary  fw-bold px-4 text-uppercase shadow" onClick={() => navigate('/payment')}>
                                Continuar al Pago
                            </button>
                        </div>
                    </div>
                </div>
            )}

            <Footer />
        </>
    );
}