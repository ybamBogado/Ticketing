import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Login.css';

export default function Login() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [infoMsg, setInfoMsg] = useState(null);
    const navigate = useNavigate();
    const { login } = useAuth();

    useEffect(() => {
        fetch('https://localhost:7285/api/v1/users')
            .then(res => res.json())
            .then(data => {
                setUsers(data);
                setLoading(false);
            })
            .catch(err => {
                console.error("usuarios de prueba", err);
                setUsers([
                    { id: 2, name: 'Juan Román Riquelme', email: 'test1@test.com' },
                    { id: 5, name: 'Diego Maradona', email: 'test5@test.com' }
                ]);
                setLoading(false);
            });
    }, []);

    const handleSelectUser = (user) => {
        login(user);
        navigate(-1);
    };

    const handleGoogleLogin = () => {
        setInfoMsg("Integración con Google: Próximamente...");
        setTimeout(() => setInfoMsg(null), 3000)
    };

    return (
        <div className="login-page">
            <div className="login-card shadow-lg">
                <div className="d-flex flex-column align-items-center justify-content-center mb-4">
                    <Link to="/" title="Ir al inicio">
                        <img src="/Ticketinador.png" alt="Logo" className="login-card-logo mb-2" />
                    </Link>
                    <h2 className="mb-0 fw-bold">Bienvenido</h2>
                </div>

                {infoMsg && (
                    <div className="alert alert-info py-2 text-center small shadow-sm">
                        {infoMsg}
                    </div>
                )}

                <div className="divider mb-4"><span>Selecciona un usuario local</span></div>

                <div className="user-list">
                    {users.map(u => (
                        <div key={u.id} className="user-item p-3 mb-2 rounded d-flex align-items-center justify-content-between" onClick={() => handleSelectUser(u)}>
                            <div>
                                <div className="fw-bold text-white">{u.name}</div>
                                <div className="small text-secondary">{u.email}</div>
                            </div>
                            <i className="bi bi-chevron-right text-muted"></i>
                        </div>
                    ))}
                </div>
                
                <p className="text-white text-center mb-4">O prueba con una cuenta de Google (Próximamente)</p>
                <button className="btn btn-outline-light w-100 mb-4 d-flex align-items-center justify-content-center gap-2 google-btn" onClick={handleGoogleLogin}>
                    <img src="https://fonts.gstatic.com/s/i/productlogos/googleg/v6/24px.svg" alt="Google" />
                    Continuar con Google
                </button>

                <div className="text-center border-top pt-3">
                    <Link to="/" className="text-decoration-none text-secondary small back-to-home-link">
                        ← Volver al inicio
                    </Link>
                </div>
            </div>
        </div>
    );
}
