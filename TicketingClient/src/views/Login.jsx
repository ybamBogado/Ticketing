import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import API_BASE_URL from '../config';
import './Login.css';

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const [validationErrors, setValidationErrors] = useState({});
    
    const navigate = useNavigate();
    const { login } = useAuth();

    const validateForm = () => {
        const errors = {};
        if (!email.trim()) {
            errors.email = "El correo es obligatorio";
        } else if (!email.includes('@')) {
            errors.email = "Correo inválido (falta el @)";
        }
        if (!password) {
            errors.password = "La contraseña es obligatoria";
        }
        return errors;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const errors = validateForm();
        if (Object.keys(errors).length > 0) {
            setValidationErrors(errors);
            return;
        }

        setLoading(true);
        setError(null);
        setValidationErrors({});

        try {
            const response = await fetch(`${API_BASE_URL}/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });

            if (!response.ok) {
                throw new Error('Correo o contraseña incorrectos');
            }

            const userData = await response.json();
            
            login(userData); 
            navigate('/');
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="login-page">
            <div className="login-card shadow-lg">
                <div className="d-flex flex-column align-items-center justify-content-center mb-4">
                    <Link to="/" title="Ir al inicio">
                        <img src="/Ticketinador.png" alt="Logo" className="login-card-logo mb-2" />
                    </Link>
                    <h2 className="mb-0 fw-bold">Bienvenido</h2>
                    <p className="text-secondary small">Ingresa tus credenciales para continuar</p>
                </div>

                {error && (
                    <div className="alert alert-danger py-2 text-center small shadow-sm mb-3">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} noValidate className="mb-4">
                    <div className="mb-3">
                        <label className="form-label text-white-50 small">Correo Electrónico</label>
                        <input 
                            type="email" 
                            className={`form-control bg-dark text-white border-secondary ${validationErrors.email ? 'is-invalid-custom' : ''}`} 
                            placeholder="ejemplo@correo.com"
                            value={email}
                            onChange={(e) => {
                                setEmail(e.target.value);
                                if (validationErrors.email) setValidationErrors({...validationErrors, email: null});
                            }}
                        />
                        {validationErrors.email && <span className="invalid-feedback-custom">{validationErrors.email}</span>}
                    </div>
                    <div className="mb-4">
                        <label className="form-label text-white-50 small">Contraseña</label>
                        <input 
                            type="password" 
                            className={`form-control bg-dark text-white border-secondary ${validationErrors.password ? 'is-invalid-custom' : ''}`} 
                            placeholder="••••••••"
                            value={password}
                            onChange={(e) => {
                                setPassword(e.target.value);
                                if (validationErrors.password) setValidationErrors({...validationErrors, password: null});
                            }}
                        />
                        {validationErrors.password && <span className="invalid-feedback-custom">{validationErrors.password}</span>}
                    </div>
                    <button 
                        type="submit" 
                        className="btn btn-primary w-100 fw-bold py-2 shadow-sm"
                        disabled={loading}
                    >
                        {loading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
                    </button>
                </form>

                <div className="text-center mb-4">
                    <p className="text-white-50 small">¿No tienes cuenta? <Link to="/register" className="text-primary text-decoration-none fw-bold">Regístrate aquí</Link></p>
                </div>
                
                <div className="divider mb-4"><span>O</span></div>

                <button className="btn btn-outline-light w-100 mb-4 d-flex align-items-center justify-content-center gap-2 google-btn">
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
