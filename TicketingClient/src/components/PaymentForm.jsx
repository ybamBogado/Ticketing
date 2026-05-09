import React, { useState } from 'react';

const PaymentForm = ({ reservationId, userId, onSuccess }) => {
    const [cardNumber, setCardNumber] = useState('');
    const [cardHolderName, setCardHolderName] = useState('');
    const [status, setStatus] = useState('');
    const [loading, setLoading] = useState(false);

    const handlePayment = async (e) => {
        e.preventDefault();
        setLoading(true);
        setStatus('');

        try {
            const response = await fetch(`https://localhost:7285/api/v1/payments/${reservationId}/pay`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userId: parseInt(userId, 10),
                    cardNumber: cardNumber,
                    cardHolderName: cardHolderName
                }),
            });

            if (response.ok) {
                const message = await response.text();
                setStatus(`✅ Éxito: ${message}`);
                if (onSuccess) {
                    setTimeout(() => onSuccess(), 1500);
                }
            } else {
                const errorMessage = await response.text();
                setStatus(`❌ Error: ${errorMessage}`);
            }
        } catch (error) {
            setStatus(`❌ Error de red: ${error.message}`);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ maxWidth: '400px', margin: '20px auto', padding: '20px', backgroundColor: '#222', border: '1px solid #444', borderRadius: '8px', color: '#fff' }}>
            <h3 style={{ borderBottom: '1px solid #444', paddingBottom: '10px', marginBottom: '15px' }}>Procesar Pago</h3>
            <form onSubmit={handlePayment}>
                <div style={{ marginBottom: '15px' }}>
                    <label style={{ display: 'block', marginBottom: '5px', color: '#ccc' }}>
                        Número de Tarjeta:
                        <input
                            type="text"
                            value={cardNumber}
                            onChange={(e) => setCardNumber(e.target.value)}
                            placeholder="1234 5678 9101 1121"
                            required
                            style={{ width: '100%', padding: '10px', marginTop: '5px', backgroundColor: '#333', color: '#fff', border: '1px solid #555', borderRadius: '4px' }}
                        />
                    </label>
                </div>

                <div style={{ marginBottom: '20px' }}>
                    <label style={{ display: 'block', marginBottom: '5px', color: '#ccc' }}>
                        Titular de la Tarjeta:
                        <input
                            type="text"
                            value={cardHolderName}
                            onChange={(e) => setCardHolderName(e.target.value)}
                            placeholder="Juan Perez"
                            required
                            style={{ width: '100%', padding: '10px', marginTop: '5px', backgroundColor: '#333', color: '#fff', border: '1px solid #555', borderRadius: '4px' }}
                        />
                    </label>
                </div>

                <button 
                    type="submit" 
                    disabled={loading}
                    style={{ width: '100%', padding: '12px', backgroundColor: '#0d6efd', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
                >
                    {loading ? 'Procesando...' : 'Pagar ahora'}
                </button>
            </form>

            {status && (
                <div style={{ marginTop: '15px', padding: '10px', backgroundColor: status.includes('✅') ? '#198754' : '#dc3545', color: '#fff', borderRadius: '4px', textAlign: 'center' }}>
                    {status}
                </div>
            )}
        </div>
    );
};

export default PaymentForm;
