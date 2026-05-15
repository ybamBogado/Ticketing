import React, { useState } from 'react';
import { useCart } from '../context/CartContext';
import API_BASE_URL from '../config';

const PaymentForm = ({ reservationId, userId, onSuccess, onCancel }) => {
    const { removeFromCart } = useCart();
    const [cardNumber, setCardNumber] = useState('');
    const [cardHolderName, setCardHolderName] = useState('');
    const [expiryDate, setExpiryDate] = useState('');
    const [cvv, setCvv] = useState('');
    const [status, setStatus] = useState('');
    const [loading, setLoading] = useState(false);
    const [validationErrors, setValidationErrors] = useState({});

    const validateForm = () => {
        const errors = {};
        if (!cardNumber || cardNumber.length !== 16) {
            errors.cardNumber = "El número de tarjeta debe tener 16 dígitos";
        }
        if (!expiryDate || !/^(0[1-9]|1[0-2])\/\d{2}$/.test(expiryDate)) {
            errors.expiryDate = "Formato inválido (MM/YY)";
        }
        if (!cvv || cvv.length !== 3) {
            errors.cvv = "El CVV debe tener 3 dígitos";
        }
        if (!cardHolderName.trim()) {
            errors.cardHolderName = "El nombre del titular es obligatorio";
        }
        return errors;
    };

    const handlePayment = async (e) => {
        e.preventDefault();
        
        const errors = validateForm();
        if (Object.keys(errors).length > 0) {
            setValidationErrors(errors);
            return;
        }

        setLoading(true);
        setStatus('');
        setValidationErrors({});

        try {
            const response = await fetch(`${API_BASE_URL}/reservations/${reservationId}/payments`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    UserId: parseInt(userId, 10),
                    CardNumber: cardNumber,
                    CardHolderName: cardHolderName,
                    ExpiryDate: expiryDate,
                    CVV: cvv
                }),
            });

            if (response.ok) {
                const message = await response.text();
                setStatus(`Success: ${message}`);
                if (onSuccess) {
                    setTimeout(() => onSuccess(), 1500);
                }
            } else {
                setStatus('Error: No se pudo procesar el pago. Revisa los datos.');
            }
        } catch (error) {
            setStatus(`Error de red: ${error.message}`);
        } finally {
            setLoading(false);
        }
    };

    const handleCancel = async () => {
        await removeFromCart(reservationId);
        if (onCancel) onCancel();
    };

    const handleFieldChange = (field, value, setter) => {
        setter(value);
        if (validationErrors[field]) {
            setValidationErrors({ ...validationErrors, [field]: null });
        }
    };

    return (
        <div className="card shadow-lg border-0 bg-dark text-white p-4 mx-auto" >
            <div className="card-body">
                <h3 className="text-center mb-4 fw-bold border-bottom pb-3" >Procesar Pago</h3>
                
                <form onSubmit={handlePayment} noValidate>
                    <div className="mb-3">
                        <label className="form-label text-secondary small text-uppercase fw-bold">Número de Tarjeta</label>
                        <input
                            type="text"
                            className={`form-control bg-secondary bg-opacity-10 border-secondary text-white py-2 ${validationErrors.cardNumber ? 'is-invalid-custom' : ''}`}
                            value={cardNumber}
                            onChange={(e) => handleFieldChange('cardNumber', e.target.value.replace(/\D/g, '').slice(0, 16), setCardNumber)}
                            placeholder="16 dígitos de tu tarjeta"
                            inputMode="numeric"
                        />
                        {validationErrors.cardNumber && <span className="invalid-feedback-custom">{validationErrors.cardNumber}</span>}
                    </div>

                    <div className="row">
                        <div className="col-md-6 mb-3">
                            <label className="form-label text-secondary small text-uppercase fw-bold">Vencimiento</label>
                            <input 
                                type="text" 
                                className={`form-control bg-secondary bg-opacity-10 border-secondary text-white py-2 ${validationErrors.expiryDate ? 'is-invalid-custom' : ''}`}
                                value={expiryDate}
                                onChange={(e) => {
                                    let val = e.target.value.replace(/\D/g, '');    
                                    if (val.length > 2) {
                                        val = val.slice(0, 2) + '/' + val.slice(2, 4);
                                    } else {
                                        val = val.slice(0, 2);
                                    }
                                    handleFieldChange('expiryDate', val, setExpiryDate);
                                }}
                                placeholder="MM/YY"
                                inputMode="numeric"
                                maxLength="5"
                            />
                            {validationErrors.expiryDate && <span className="invalid-feedback-custom">{validationErrors.expiryDate}</span>}
                        </div>
                        <div className="col-md-6 mb-3">
                            <label className="form-label text-secondary small text-uppercase fw-bold">CVV</label>
                            <input 
                                type="password" 
                                className={`form-control bg-secondary bg-opacity-10 border-secondary text-white py-2 ${validationErrors.cvv ? 'is-invalid-custom' : ''}`}
                                value={cvv}
                                onChange={(e) => handleFieldChange('cvv', e.target.value.replace(/\D/g, '').slice(0, 3), setCvv)}
                                placeholder="123"
                                inputMode="numeric"
                            />
                            {validationErrors.cvv && <span className="invalid-feedback-custom">{validationErrors.cvv}</span>}
                        </div>
                    </div>

                    <div className="mb-4">
                        <label className="form-label text-secondary small text-uppercase fw-bold">Titular de la Tarjeta</label>
                        <input
                            type="text"
                            className={`form-control bg-secondary bg-opacity-10 border-secondary text-white py-2 ${validationErrors.cardHolderName ? 'is-invalid-custom' : ''}`}
                            value={cardHolderName}
                            onChange={(e) => handleFieldChange('cardHolderName', e.target.value, setCardHolderName)}
                            placeholder="Juan Perez"
                        />
                        {validationErrors.cardHolderName && <span className="invalid-feedback-custom">{validationErrors.cardHolderName}</span>}
                    </div>

                    <div className="d-grid gap-2">
                        <button 
                            type="submit" 
                            className="btn btn-primary btn-lg fw-bold py-3 shadow-sm"
                            disabled={loading}
                        >
                            {loading ? (
                                <><span className="spinner-border spinner-border-sm me-2"></span>Procesando...</>
                            ) : 'Pagar ahora'}
                        </button>
                        
                        <button
                            type="button"
                            className="btn btn-danger btn-sm border-0 mt-2 fw-bold text-uppercase"
                            onClick={handleCancel}
                        >
                            <i className="bi bi-x-circle me-2"></i>
                            Cancelar Pago
                        </button>
                    </div>
                </form>

                {status && (
                    <div className={`alert mt-4 text-center ${status.includes('Success') ? 'alert-success' : 'alert-danger'}`}>
                        {status.replace('Success:', '').replace('Error:', '')}
                    </div>
                )}
            </div>
        </div>

    );
};

export default PaymentForm;
