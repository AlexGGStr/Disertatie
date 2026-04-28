import React, { useEffect, useState } from "react";
import { Container, Spinner, Alert } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import {BookingCardProps} from "../Components/Bookings/BookingCard";
import {bookingServiceUrl} from "../config";
import {getAuthToken} from "../utils/auth";
import BookingList from "../Components/Bookings/BookingList";

const MyBookingsPage: React.FC = () => {
    const [bookings, setBookings] = useState<BookingCardProps[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchBookings = async () => {
            try {
                const token = getAuthToken();
                if (!token) return navigate("/auth?login=true");

                const response = await fetch(`${bookingServiceUrl}/booking/myBookings`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (!response.ok) throw new Error("Failed to fetch bookings");

                const data = await response.json();
                setBookings(data.data || []);
            } catch (err: any) {
                setError(err.message || "Something went wrong.");
            } finally {
                setLoading(false);
            }
        };

        fetchBookings();
    }, [navigate]);

    const cancelBooking = async (bookingId: string) => {
        try {
            const token = getAuthToken();
            const response = await fetch(`${bookingServiceUrl}/booking/cancel/${bookingId}`, {
                method: "PUT",
                headers: { Authorization: `Bearer ${token}` },
            });

            if (!response.ok) throw new Error("Failed to cancel booking");

            setBookings((prev) =>
                prev.map((b) =>
                    b.id === bookingId ? { ...b, status: "Cancelled" } : b
                )
            );
        } catch (err: any) {
            alert(err.message);
        }
    };

    const payForBooking = (bookingId: string, amount: number) => {
        navigate(`/checkout?bookingId=${bookingId}&amount=${amount}`);
    };

    return (
        <Container className="py-4">
            <h2 className="mb-4">My Bookings</h2>
            {loading ? (
                <Spinner animation="border" className="m-5" />
            ) : error ? (
                <Alert variant="danger">{error}</Alert>
            ) : bookings.length === 0 ? (
                <p>No bookings found.</p>
            ) : (
                <BookingList
                    bookings={bookings}
                    onCancel={cancelBooking}
                    onPay={payForBooking}
                />
            )}
        </Container>
    );
};

export default MyBookingsPage;
