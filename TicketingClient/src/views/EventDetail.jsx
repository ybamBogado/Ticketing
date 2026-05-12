import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import API_BASE_URL from '../config';
import Header from '../components/Header';
import Footer from '../components/Footer';
import Loader from '../components/Loader';
import PaymentForm from '../components/PaymentForm';
import './EventDetail.css';

export default function EventDetail() {
    const { eventId } = useParams();
    const navigate = useNavigate();
    const { user } = useAuth();
    const [seats, setSeats] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [currentReservationId, setCurrentReservationId] = useState(null);
    const [showPayment, setShowPayment] = useState(false);
    const [selectedSeatId, setSelectedSeatId] = useState(null);

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

    const handleReserva = async (seatId, currentStatus) => {
        if (currentStatus !== 'Available') return;

        if (!user) {
            setError("Debes iniciar sesión para reservar una butaca.");
            setTimeout(() => navigate('/login'), 2000);
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
                setSeats(seats.map(s => s.id === seatId ? { ...s, status: 'Reserved' } : s));
                setCurrentReservationId(data.reservationId);
                setSelectedSeatId(seatId);
                setShowPayment(true);
                setError(null);
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

    const handlePaymentSuccess = () => {
        setSeats(seats.map(s => s.id === selectedSeatId ? { ...s, status: 'Sold' } : s));
        setShowPayment(false);
        setCurrentReservationId(null);
        setSelectedSeatId(null);
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
                <h4 className="text-center mb-4 text-white-50 fw-bold" style={{ letterSpacing: '2px' }}>{title}</h4>
                <div className="d-flex flex-column gap-2 align-items-center">
                    {sortedKeys.map(rowKey => (
                        <div key={rowKey} className="d-flex justify-content-center gap-2" style={{ minWidth: 'max-content' }}>
                            <div className="text-muted d-flex align-items-center justify-content-end fw-bold" style={{ width: '60px', fontSize: '0.9rem' }}>
                                {rowKey}
                            </div>
                            {grouped[rowKey]
                                .sort((a, b) => a.seatNumber - b.seatNumber)
                                .map(seat => (
                                    <div
                                        key={seat.id}
                                        className={`seat-box ${seat.status.toLowerCase()}`}
                                        onClick={() => handleReserva(seat.id, seat.status)}
                                        title={`Fila: ${seat.rowIdentifier} - Asiento: ${seat.seatNumber}`}
                                    >
                                        {seat.seatNumber}
                                    </div>
                                ))}
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

                <h1 className="text-center my-4 fw-bold">Mapa de Asientos</h1>

                <div className="card detail-card shadow-lg p-4">
                    <div className="card-body">
                        <div className="escenario mb-5 text-center p-2 rounded">ESCENARIO</div>

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

                {showPayment && currentReservationId && (
                    <div className="mt-4" ref={(el) => el && el.scrollIntoView({ behavior: 'smooth' })}>
                        <PaymentForm
                            reservationId={currentReservationId}
                            userId={user.userId}
                            onSuccess={handlePaymentSuccess}
                            onCancel={() => setShowPayment(false)}
                        />
                    </div>
                )}

            </div>
            <Footer />
        </>
    );
}