import { useState, useEffect } from 'react'
import Header from '../components/Header.jsx'
import { Link } from 'react-router-dom'
import './EventCatalog.css'

export default function EventCatalog() {

    const [events, setEvents] = useState([]);
    useEffect(() => {
        fetch('https://localhost:7285/api/v1/events')
            .then(response => response.json())
            .then(data => setEvents(data))
    }, []);

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
                                    <h5 className="card-title event-title fw-bold">{event.name}</h5>
                                    <Link to={`/event/${event.id}`}>
                                        <button className="btn btn-dark btn-detail w-100">Ver Detalles</button>
                                    </Link>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </>
    );
}
