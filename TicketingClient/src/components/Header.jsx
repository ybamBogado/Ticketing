import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Header.css';

export default function Header() {

    const { user, logout } = useAuth();
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
        <header className="border-bottom border-secondary mb-4">
            <div className="container-fluid d-flex justify-content-between align-items-center p-4 px-3">


                <div className="flex-grow-1">
                    <Link to="/" className="text-decoration-none d-flex align-items-center">
                        <img 
                            src="https://copilot.microsoft.com/th/id/BCO.6bd8487f-b1cf-4e6a-846c-de1acd93d166.png" 
                            alt="Logo" 
                            className='logo'
                        />
                        <h1 className="h3 text-white mb-0 fw-bold">Ticketinator 2000</h1>
                    </Link>

                </div>

                

                <div className="flex-grow-1 ">
                    <button onClick={handleAuthClick} className="btn btn-outline-light">
                        {user ? `Cerrar Sesión (${user.name})` : "Iniciar Sesión"}
                    </button>
                </div>

            </div>
        </header>
    );

}