import { Link } from 'react-router-dom';

export default function Header() {
    return (
        <header className="text-center p-4 border-bottom border-secondary mb-4">
            <Link to="/" className="text-decoration-none">
                <h1 className="h2 text-white mb-0 fw-bold">Ticketinator 2000</h1>
            </Link>
            <p className="text-muted small mt-2">El mejor sistema de venta de boletos</p>
        </header>
    );
}