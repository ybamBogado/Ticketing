import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import './Header.css';

export default function Header() {
    const { user, logout } = useAuth();
    const { cartItems, timeLeft, formatTime } = useCart();
    const navigate = useNavigate();

    const handleAuthClick = () => {
        if (user) {
            logout();
            navigate('/');
        } else {
            navigate('/login');
        }
    };

    return (
        <header className="header-main mb-4 sticky-top shadow-lg">
            <div className="container-fluid d-flex flex-column flex-md-row justify-content-between align-items-center p-3 p-md-4">
                
                <div className="text-center text-md-start mb-3 mb-md-0">
                    <Link to="/" className="text-decoration-none d-flex align-items-center justify-content-center justify-content-md-start">
                        <img
                            src="/Ticketinador.png"
                            alt="Logo"
                            className='logo me-2'
                        />
                        <h1 className="h3 text-white mb-0 fw-bold">Ticketinator 2000</h1>
                    </Link>
                </div>

                <div className="d-flex align-items-center gap-3">
                    
                    {cartItems.length > 0 && (
                        <div className="d-flex align-items-center gap-3 p-2 border border-secondary px-3 timer-container animate__animated animate__fadeIn">
                            <div className="text-center border-end border-secondary pe-3">
                                <small className="text-white-50 d-block text-uppercase fw-bold" >Expira en</small>
                                <span className={`fw-bold font-monospace ${timeLeft < 60 ? 'text-danger animate__animated animate__flash animate__infinite' : 'text-warning'}`}>
                                    {formatTime(timeLeft)}
                                </span>
                            </div>
                            <button 
                                onClick={() => navigate('/cart')} 
                                className="btn btn-primary rounded-circle position-relative shadow cart-btn"
                                title="Ver Carrito"
                            >
                                <i className="bi bi-cart-plus fs-4"></i>
                                <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger shadow-sm cart-badge">
                                    {cartItems.length}
                                </span>
                            </button>
                        </div>
                    )}

                    <div className="text-center text-md-end">
                        <button onClick={handleAuthClick} className="btn btn-outline-light px-4  fw-bold text-uppercase small">
                            {user ? `Salir (${user.name})` : "Entrar"}
                        </button>
                    </div>
                </div>

            </div>
        </header>
    );
}