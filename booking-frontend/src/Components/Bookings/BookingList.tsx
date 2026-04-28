import React from "react";
import BookingCard, { BookingCardProps } from "./BookingCard";

interface BookingListProps {
    bookings: BookingCardProps[];
    onCancel: (bookingId: string) => void;
    onPay: (bookingId: string, amount: number) => void;
}

const BookingList: React.FC<BookingListProps> = ({ bookings, onCancel, onPay }) => {
    return (
        <div>
            {bookings.map((booking) => (
                <BookingCard
                    key={booking.id}
                    {...booking}
                    onCancel={onCancel}
                    onPay={onPay}
                />
            ))}
        </div>
    );
};

export default BookingList;
