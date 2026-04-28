import {
    CardElement,
    useStripe,
    useElements
} from "@stripe/react-stripe-js";
import axios from "axios";
import { useState } from "react";
import { paymentServiceUrl } from "../../config";
import { Spinner, Alert, Button, Form } from "react-bootstrap";
import {useNavigate} from "react-router-dom";

type Props = {
    amount: number;
    bookingId: string;
};

export default function CheckoutForm({ amount, bookingId }: Props) {
    const navigate = useNavigate();
    const stripe = useStripe();
    const elements = useElements();

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        setSuccess(false);

        if (!stripe || !elements) {
            setError("Stripe not loaded");
            return;
        }

        try {
            const response = await axios.post(`${paymentServiceUrl}/payments/create-payment-intent`, {
                amount,
                bookingId
            });

            const clientSecret = response.data.clientSecret;

            const result = await stripe.confirmCardPayment(clientSecret, {
                payment_method: {
                    card: elements.getElement(CardElement)!
                }
            });

            if (result.error) {
                setError(result.error.message || "Payment failed");
            } else if (result.paymentIntent?.status === "succeeded") {
                setSuccess(true);
                navigate('/bookings', { replace: true });
            }
        } catch (err: any) {
            setError("Server error: " + err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Form onSubmit={handleSubmit}>
            {error && <Alert variant="danger">{error}</Alert>}
            {success && <Alert variant="success">✅ Payment successful!</Alert>}

            <div className="mb-3">
                <CardElement options={{
                    style: {
                        base: {
                            fontSize: '16px',
                            color: '#424770',
                            '::placeholder': {
                                color: '#aab7c4',
                            },
                        },
                        invalid: {
                            color: '#9e2146',
                        },
                    },
                }} />
            </div>

            <Button
                type="submit"
                variant="primary"
                disabled={!stripe || loading}
                className="w-100"
            >
                {loading ? <Spinner animation="border" size="sm" /> : `Pay $${amount.toFixed(2)}`}
            </Button>
        </Form>
    );
}
