import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import './EventDetail.css';

export default function EventDetail() {
    const { eventId } = useParams();
    const navigate = useNavigate();
    const [seats, setSeats] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetch(`https://localhost:7285/api/Seat/${eventId}`)
            .then(res => res.json())
            .then(data => {
                setSeats(data);
                setLoading(false);
            })
            .catch(err => console.error("Error:", err));
    }, [eventId]);

    const handleReserva = (seatId, currentStatus) => {
        if (currentStatus !== 'Available') return;
        navigate(`/login?seatId=${seatId}&eventId=${eventId}`);
    };

    if (loading) return <div className="text-center mt-5 text-white">Cargando asientos...</div>;

    return (
        <div className="container mt-4 detail-container">
            <Header />
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

                    <div className="mt-4 d-flex justify-content-around border-top pt-3">
                        <div className="legend-item text-success"><div className="legend-dot available"></div> Disponible</div>
                        <div className="legend-item text-warning"><div className="legend-dot reserved"></div> Reservado</div>
                        <div className="legend-item text-danger"><div className="legend-dot sold"></div> Vendido</div>
                    </div>
                </div>
            </div>
        </div>
    );
}