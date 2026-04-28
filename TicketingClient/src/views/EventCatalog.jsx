import { useState, useEffect } from 'react'
import Header from '../components/Header.jsx'
import Footer from '../components/Footer.jsx'
import Loader from '../components/Loader.jsx'
import { Link } from 'react-router-dom'
import './EventCatalog.css'

export default function EventCatalog() {

    const [events, setEvents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetch('https://localhost:7285/api/v1/events')
            .then(response => {
            if (!response.ok) throw new Error("No se pudieron cargar los eventos.");
            return response.json();
        })
            .then(data => setEvents(data))
            .catch(err => {
                console.error(err);
                setError(err.message);
            })
            .finally(() => setLoading(false));
    }, []);

    if (error) {
    return (
        <>
            <Header />
            <div className="container text-center mt-5">
                <div className="alert alert-danger shadow-sm py-4">
                    <h4 className="fw-bold">Error al cargar los eventos</h4>
                    <button className="btn btn-outline-danger mt-2" onClick={() => window.location.reload()}>
                        Reintentar
                    </button>
                </div>
            </div>
            <Footer />
        </>
    );
}
    
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
            <div className="container mt-4 event-container">
                <h1 className="text-center mb-4 event-title fw-bold">Catálogo de Eventos</h1>
                <div className="row justify-content-center">
                    {events.map(event => (
                        <div key={event.id} className="col-12 col-md-6 col-lg-4 mb-3">
                            <div className="card shadow-sm event-card">
                                <div className="card-body">
                                    <img src="https://tse4.mm.bing.net/th/id/OIP.bjBxphKfMiwFkmpipnQ2CAHaFj?rs=1&pid=ImgDetMain&o=7&rm=3" alt={event.name} className="card-img-top" />
                                    <h5 className="card-title event-title fw-bold">{event.name}</h5>
                                    <h5 className="card-title event-title fw-bold">{event.venue}</h5>
                                    <Link to={`/event/${event.id}`}>
                                        <button className="btn btn-dark btn-detail w-100">Ver Detalles</button>
                                    </Link>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
            <Footer />
        </>
    );
}
