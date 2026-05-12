import React, { useState } from 'react';
import API_BASE_URL from '../config';

const PaymentForm = ({ reservationId, userId, onSuccess, onCancel }) => {
    const [cardNumber, setCardNumber] = useState('');
    const [cardHolderName, setCardHolderName] = useState('');
    const [status, setStatus] = useState('');
    const [loading, setLoading] = useState(false);

    const handlePayment = async (e) => {
        e.preventDefault();
        setLoading(true);
        setStatus('');

        try {
            const response = await fetch(`${API_BASE_URL}/payments/${reservationId}/pay`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    UserId: parseInt(userId, 10),
                    CardNumber: cardNumber,
                    CardHolderName: cardHolderName
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

    return (
        <div className="card shadow-lg border-0 bg-dark text-white p-4 mx-auto" style={{ maxWidth: '450px', borderRadius: '16px' }}>
            <div className="card-body">
                <h3 className="text-center mb-4 fw-bold border-bottom pb-3" style={{ borderColor: '#334155 !important' }}>Procesar Pago</h3>
                
                <form onSubmit={handlePayment}>
                    <div className="mb-3">
                        <label className="form-label text-secondary small text-uppercase fw-bold">Número de Tarjeta</label>
                        <input
                            type="text"
                            className="form-control bg-secondary bg-opacity-10 border-secondary text-white py-2"
                            value={cardNumber}
                            onChange={(e) => setCardNumber(e.target.value)}
                            placeholder="1234 5678 9101 1121"
                            required
                        />
                    </div>

                    <div className="mb-4">
                        <label className="form-label text-secondary small text-uppercase fw-bold">Titular de la Tarjeta</label>
                        <input
                            type="text"
                            className="form-control bg-secondary bg-opacity-10 border-secondary text-white py-2"
                            value={cardHolderName}
                            onChange={(e) => setCardHolderName(e.target.value)}
                            placeholder="Juan Perez"
                            required
                        />
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
                            className="btn btn-link text-secondary text-decoration-none mt-2"
                            onClick={onCancel}
                        >
                            Cancelar Pago
                        </button>
                    </div>
                </form>

                {status && (
                    <div className={`alert mt-4 text-center ${status.includes('Success') ? 'alert-success-custom' : 'alert-danger-custom'}`}>
                        {status.replace('Success:', '').replace('Error:', '')}
                    </div>
                )}
            </div>
        </div>
    );
};

export default PaymentForm;
