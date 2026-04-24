import { useState, useEffect } from 'react'

export default function EventCatalog() {

    const [events, setEvents] = useState([]);
    useEffect(() => {
        fetch('https://localhost:7285/api/event')
            .then(response => response.json())
            .then(data => setEvents(data))
    }, []);
    return (
        <div>
            <h1>Catalogo de Eventos</h1>
            <ul>
                {events.map(event => (
                    <li key={event.id}>{event.name}</li>
                ))}
            </ul>
        </div>
    );
}
