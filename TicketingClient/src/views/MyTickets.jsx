import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import API_BASE_URL from '../config';
import Header from '../components/Header';
import Footer from '../components/Footer';
import Loader from '../components/Loader';
import './MyTickets.css';

export default function MyTickets() {
    const { user } = useAuth();
    const [tickets, setTickets] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (!user) return;

        const fetchTickets = async () => {
            const userId = user?.userId || user?.id; 
            const url = `${API_BASE_URL}/reservations/user/${userId}`;

            try {
                const response = await fetch(url);
                if (!response.ok) throw new Error(`Error al cargar entradas: ${response.status}`);
                
                const data = await response.json();
                setTickets(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchTickets();
    }, [user]);

    if (loading) return <><Header /><Loader /><Footer /></>;

    return (
        <div className="bg-dark min-vh-100 d-flex flex-column">
            <Header />
            <div className="container flex-grow-1 py-5">
                <h2 className="text-white fw-bold mb-4 text-uppercase ls-2 text-center">Mis Entradas Compradas</h2>
                
                {tickets.length === 0 ? (
                    <div className="text-center text-white-50 mt-5">
                        <i className="bi bi-ticket-perforated display-1 mb-4 d-block"></i>
                        <h4>Aún no tienes entradas compradas</h4>
                        <p>Cuando realices un pago, tus entradas aparecerán aquí.</p>
                        <button onClick={() => navigate('/')} className="btn btn-primary mt-3 fw-bold text-uppercase small">
                            Ver Eventos
                        </button>
                    </div>
                ) : (
                    <div className="row g-4">
                        {tickets.map((ticket) => (
                            <div key={ticket.reservationId} className="col-md-6 col-lg-4">
                                <div className="ticket-card shadow-lg overflow-hidden">
                                    <div className="ticket-header p-3 text-center">
                                        <h5 className="mb-0 fw-bold">{ticket.eventName}</h5>
                                        <small className="text-white-50">{ticket.venue}</small>
                                    </div>
                                    <div className="ticket-body p-4 bg-white text-dark">
                                        <div className="d-flex justify-content-between mb-3 border-bottom pb-2">
                                            <div>
                                                <small className="text-muted d-block text-uppercase small fw-bold">Fila</small>
                                                <span className="h4 mb-0 fw-bold">{ticket.rowIdentifier}</span>
                                            </div>
                                            <div className="text-end">
                                                <small className="text-muted d-block text-uppercase small fw-bold">Asiento</small>
                                                <span className="h4 mb-0 fw-bold ">{ticket.seatNumber}</span>
                                            </div>
                                        </div>
                                        <div className="mb-3">
                                            <small className="text-muted d-block text-uppercase small fw-bold">Sector</small>
                                            <span className="fw-bold text-white">{ticket.sectorName}</span>
                                        </div>
                                        <div className="qr-placeholder d-flex align-items-center justify-content-center p-3">
                                            <div className="text-center">
                                                <i className="bi bi-qr-code display-4 qr-icon"></i>
                                                <small className="d-block text-muted mt-2">ID: {ticket.reservationId.slice(0, 8)}</small>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="ticket-footer p-2 text-center text-white-50 small fw-bold text-uppercase">
                                        {ticket.status}
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
            <Footer />
        </div>
    );
}
