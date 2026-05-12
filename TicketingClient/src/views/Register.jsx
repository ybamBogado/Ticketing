import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import API_BASE_URL from '../config';
import './Login.css';

export default function Register() {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);
    const [loading, setLoading] = useState(false);

    const [validationErrors, setValidationErrors] = useState({});
    
    const navigate = useNavigate();

    const validateForm = () => {
        const errors = {};
        if (!name.trim()) errors.name = "El nombre es obligatorio";
        
        // Validación súper simple:
        if (!email.trim()) {
            errors.email = "El correo es obligatorio";
        } else if (!email.includes('@') || !email.includes('.')) {
            errors.email = "El correo debe tener un @ y un punto";
        }

        if (!password) {
            errors.password = "La contraseña es obligatoria";
        } else if (password.length < 4) {
            errors.password = "La contraseña es muy corta";
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
        setSuccess(false);

        try {
            const response = await fetch(`${API_BASE_URL}/auth/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name, email, password })
            });

            if (!response.ok) {
                const errorData = await response.text();
                throw new Error(errorData || 'Error al registrar usuario');
            }

            setSuccess(true);
            setTimeout(() => navigate('/login'), 3000);
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
                    <h2 className="mb-0 fw-bold">Crear Cuenta</h2>
                    <p className="text-secondary small">Únete a Ticketinador 2000</p>
                </div>

                {error && (
                    <div className="alert alert-danger-custom py-2 text-center small shadow-sm mb-3">
                        {error}
                    </div>
                )}
                
                {success && (
                    <div className="alert alert-success-custom py-3 text-center small shadow-sm mb-3 border-0">
                        <i className="bi bi-check-circle-fill me-2"></i>
                        ¡Registro exitoso!
                    </div>
                )}

                {!success ? (
                    <form onSubmit={handleSubmit} noValidate className="mb-4">
                        <div className="mb-3">
                            <label className="form-label text-white-50 small">Nombre Completo</label>
                            <input
                                type="text"
                                className={`form-control bg-dark text-white border-secondary ${validationErrors.name ? 'is-invalid-custom' : ''}`}
                                placeholder="Tu nombre"
                                value={name}
                                onChange={(e) => {
                                    setName(e.target.value);
                                    if (validationErrors.name) setValidationErrors({...validationErrors, name: null});
                                }}
                            />
                            {validationErrors.name && <span className="invalid-feedback-custom">{validationErrors.name}</span>}
                        </div>
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
                            className="btn btn-success w-100 fw-bold py-2 shadow-sm"
                            disabled={loading}
                        >
                            {loading ? 'Registrando...' : 'Registrarse'}
                        </button>
                    </form>
                ) : (
                    <div className="text-center mb-4">
                        <Link to="/login" className="btn btn-primary w-100 fw-bold py-2 shadow-sm">
                            Ir al Login ahora
                        </Link>
                    </div>
                )}

                <div className="text-center mb-4">
                    <p className="text-white-50 small">¿Ya tienes cuenta? <Link to="/login" className="text-primary text-decoration-none fw-bold">Inicia sesión aquí</Link></p>
                </div>

                <div className="text-center border-top pt-3">
                    <Link to="/" className="text-decoration-none text-secondary small back-to-home-link">
                        ← Volver al inicio
                    </Link>
                </div>
            </div>
        </div>
    );
}
