import { useSearchParams } from "react-router-dom";
import { Elements } from "@stripe/react-stripe-js";
import { stripePromise } from "../utils/stripePromise";
import CheckoutForm from "../Components/Checkout/CheckoutForm";
import { Container } from "react-bootstrap";

export default function CheckoutPage() {
    const [searchParams] = useSearchParams();

    const bookingId = searchParams.get("bookingId") || "";
    const amountParam = searchParams.get("amount");
    const amount = amountParam ? parseFloat(amountParam) : 0;

    if (!bookingId || !amount) {
        return (
            <Container className="py-5 text-center">
                <h3>❌ Invalid booking or amount</h3>
            </Container>
        );
    }

    return (
        <Container className="py-5 d-flex flex-column align-items-center">
            <h2 className="mb-4">Complete Your Payment</h2>
            <p className="mb-4">Amount: <strong>${amount.toFixed(2)}</strong></p>

            <div className="w-100" style={{ maxWidth: "400px" }}>
                <Elements stripe={stripePromise}>
                    <CheckoutForm amount={amount} bookingId={bookingId} />
                </Elements>
            </div>
        </Container>
    );
}
