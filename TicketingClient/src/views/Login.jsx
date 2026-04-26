import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Login.css';

export default function Login() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
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
        alert(`¡Bienvenido ${user.name}!`);
        navigate(-1); // Regresar a la página anterior
    };

    const handleGoogleLogin = () => {
        alert("Integración con Google: Próximamente...");
    };

    return (
        <div className="login-page">
            <div className="login-card shadow-lg">
                <h2 className="text-center mb-4 fw-bold">Bienvenido</h2>
                <p className="text-muted text-center mb-4">Selecciona una cuenta para continuar</p>

                {/* Botón de Google (Preparado para el futuro) */}
                <button className="btn btn-outline-light w-100 mb-4 d-flex align-items-center justify-content-center gap-2 google-btn" onClick={handleGoogleLogin}>
                    <img src="https://fonts.gstatic.com/s/i/productlogos/googleg/v6/24px.svg" alt="Google" />
                    Continuar con Google
                </button>

                <div className="divider mb-4"><span>O selecciona un usuario local</span></div>

                <div className="user-list">
                    {users.map(u => (
                        <div key={u.id} className="user-item p-3 mb-2 rounded d-flex align-items-center justify-content-between" onClick={() => handleSelectUser(u)}>
                            <div>
                                <div className="fw-bold text-white">{u.name}</div>
                                <div className="small text-muted">{u.email}</div>
                            </div>
                            <i className="bi bi-chevron-right text-muted"></i>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}
