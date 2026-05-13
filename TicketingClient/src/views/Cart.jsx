import { useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import Header from '../components/Header';
import Footer from '../components/Footer';

export default function Cart() {
    const { cartItems, removeFromCart, formatTime, timeLeft, totalAmount } = useCart();
    const navigate = useNavigate();

    if (cartItems.length === 0) {
        return (
            <>
                <Header />
                <div className="container mt-5 text-center" >
                    <h2 className="text-white-50">Tu carrito está vacío</h2>
                    <button className="btn btn-primary mt-3" onClick={() => navigate('/')}>
                        Volver al Catálogo
                    </button>
                </div>
                <Footer />
            </>
        );
    }

    return (
        <>
            <Header />
            <div className="container mt-5 flex-grow-1 pb-5 min-vh-70">
                <div className="row g-4">
                    <div className="col-lg-8">
                        <div className="card bg-secondary bg-opacity-10 text-white shadow-lg p-4 border-0 rounded-4">
                            <h2 className="fw-bold mb-4 d-flex align-items-center gap-3">
                                <i className="bi bi-cart3 text-primary"></i>
                                Tu Carrito
                            </h2>
                            <div className="table-responsive">
                                <table className="table table-dark table-hover align-middle">
                                    <thead>
                                        <tr>
                                            <th>Descripción</th>
                                            <th className="text-center">Precio</th>
                                            <th className="text-end">Acción</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {cartItems.map((item, index) => (
                                            <tr key={index}>
                                                <td>
                                                    <div className="fw-bold">Butaca Reservada</div>
                                                    <small className="text-white-50">
                                                        <i className="bi bi-geo-alt me-1 text-primary"></i>
                                                        Fila: <span className="text-white fw-bold">{item.row}</span> - 
                                                        Asiento: <span className="text-white fw-bold">{item.number}</span>
                                                    </small>
                                                </td>
                                                <td className="text-center border-secondary fw-bold text-success">${item.price}</td>
                                                <td className="text-end border-secondary">
                                                    <button 
                                                        className="btn btn-outline-danger btn-sm  px-3"
                                                        onClick={() => removeFromCart(item.reservationId)}
                                                    >
                                                        <i className="bi bi-trash me-1"></i> Eliminar
                                                    </button>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div className="col-lg-4">
                        <div className="card bg-dark text-white shadow-lg p-4 border-primary border-top border-5 rounded-4">
                            <h4 className="fw-bold mb-4 text-primary text-uppercase small ls-1">Resumen de Compra</h4>
                            <div className="d-flex justify-content-between mb-2 text-white-50 small">
                                <span>Subtotal</span>
                                <span>${totalAmount}</span>
                            </div>
                            <div className="d-flex justify-content-between mb-2 text-white-50 small">
                                <span>Cargos por servicio</span>
                                <span className="text-success">¡Gratis!</span>
                            </div>
                            <hr className="border-secondary" />
                            <div className="d-flex justify-content-between mb-4">
                                <h5 className="fw-bold h5">Total</h5>
                                <h5 className="fw-bold text-success h4">${totalAmount}</h5>
                            </div>

                            <div className="alert bg-transparent border border-secondary text-center py-3 mb-4 rounded-3 shadow-sm">
                                <small className="d-block text-uppercase fw-bold  mb-1 ls-1" >
                                    La reserva expira en:
                                </small>
                                <span className={`h4 fw-bold ${timeLeft < 60 ? 'text-danger' : ''}`}>
                                    {formatTime(timeLeft)}
                                </span>
                            </div>

                            <button 
                                className="btn btn-primary w-100 fw-bold py-3 shadow"
                                onClick={() => navigate('/payment')}
                            >
                                Proceder al Pago
                            </button>
                            <button 
                                className="btn btn-link text-white-50 w-100 text-decoration-none small text-uppercase fw-bold"
                                onClick={() => navigate(-1)}
                            >
                                <i className="bi bi-arrow-left me-2"></i> Seguir eligiendo
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <Footer />
        </>
    );
}
