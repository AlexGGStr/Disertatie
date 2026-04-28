// src/pages/EditPropertyPage.tsx
import {
    Calendar,
    momentLocalizer,
    Event,
    Views,
} from "react-big-calendar";
import 'react-big-calendar/lib/css/react-big-calendar.css';
import { Modal, Button } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { PropertyServiceUrl } from "../config";
import moment from "moment";
import 'moment-timezone';

const localizer = momentLocalizer(moment);

interface Booking {
    startDate: string;
    endDate: string;
    bookingUserId: string;
    bookingUserName: string;
    numberOfPeople: number;
    totalPrice: number;
}

interface BookingEvent extends Event {
    booking: Booking;
}

const EditPropertyPage = () => {
    const { propertyId } = useParams(); // propertyId
    const [bookings, setBookings] = useState<BookingEvent[]>([]);
    const [selectedBooking, setSelectedBooking] = useState<BookingEvent | null>(null);
    const [showModal, setShowModal] = useState(false);

    useEffect(() => {
        const fetchBookings = async () => {
            const token = localStorage.getItem("token");
            const response = await fetch(
                `${PropertyServiceUrl}/Properties/bookings?propertyId=${propertyId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
            const data = await response.json();
            if (data.success) {
                const events = data.data.map((booking: Booking) => ({
                    title: `Booking - ${booking.bookingUserName}`,
                    start: new Date(booking.startDate),
                    end: new Date(booking.endDate),
                    booking,
                }));
                setBookings(events);
            }
        };

        fetchBookings();
    }, [propertyId]);

    return (
        <div className="container py-4">
            <h2 className="mb-4">Edit Property</h2>

            {/* Property form would go here */}

            <h4 className="mt-5">Bookings Calendar</h4>
            <div style={{ height: 500 }}>
                <Calendar
                    localizer={localizer}
                    events={bookings}
                    startAccessor="start"
                    endAccessor="end"
                    onSelectEvent={(event) => {
                        setSelectedBooking(event);
                        setShowModal(true);
                    }}
                />
            </div>

            <Modal show={showModal} onHide={() => setShowModal(false)} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Booking Details</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {selectedBooking && (
                        <>
                            <p>
                                <strong>Guest:</strong> {selectedBooking.booking.bookingUserName}
                            </p>
                            <p>
                                <strong>Dates:</strong>{" "}
                                {moment(selectedBooking.booking.startDate).format("LL")} –{" "}
                                {moment(selectedBooking.booking.endDate).format("LL")}
                            </p>
                            <p>
                                <strong>People:</strong> {selectedBooking.booking.numberOfPeople}
                            </p>
                            <p>
                                <strong>Total Price:</strong> ${selectedBooking.booking.totalPrice}
                            </p>
                        </>
                    )}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default EditPropertyPage;
