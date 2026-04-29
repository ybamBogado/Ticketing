import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
import Footer from '../components/Footer';
import Loader from '../components/Loader';
import './EventDetail.css';

export default function EventDetail() {
    const { eventId } = useParams();
    const navigate = useNavigate();
    const { user } = useAuth();
    const [seats, setSeats] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchSeats = () => {
            fetch(`https://localhost:7285/api/v1/events/${eventId}/seats`)
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

        const command = { seatId: seatId, userId: user.id };

        try {
            const response = await fetch('https://localhost:7285/api/v1/reservations', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(command)
            });

            if (response.ok) {
                setSeats(seats.map(s => s.id === seatId ? { ...s, status: 'Reserved' } : s));
                setError(null);
            } else {
                setSeats(seats.map(s => s.id === seatId ? { ...s, status: 'Reserved' } : s));
                setError("¡Llegaste tarde! Este asiento acaba de ser reservado por otra persona.");
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

                        <div className="seats-grid">
                            {seats.map(seat => (
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

                        <div className="mt-4 d-flex flex-column flex-md-row justify-content-around align-items-center border-top pt-3 gap-2">
                            <div className="legend-item text-success"><div className="legend-dot available"></div> Disponible</div>
                            <div className="legend-item text-warning"><div className="legend-dot reserved"></div> Reservado</div>
                            <div className="legend-item text-danger"><div className="legend-dot sold"></div> Vendido</div>
                        </div>
                    </div>
                </div>

            </div>
            <Footer />
        </>
    );
}