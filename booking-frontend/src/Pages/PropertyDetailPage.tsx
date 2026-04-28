// src/pages/PropertyDetailPage/PropertyDetailPage.tsx
import React, { useEffect, useState } from "react";
import { useParams, useSearchParams, useNavigate } from "react-router-dom";
import {
    Container,
    Row,
    Col,
    Spinner,
    Alert,
    Carousel,
    Card,
    Button,
    Form,
} from "react-bootstrap";
import Property from "../Models/Property";
import { bookingServiceUrl, PropertyServiceUrl } from "../config";
import Response from "../Models/Response";
import MapWithMarker from "../Components/Maps/MapWithMarker";

const PropertyDetailPage: React.FC = () => {
    const { propertyId } = useParams();
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();

    const [property, setProperty] = useState<Property | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [startDate, setStartDate] = useState(searchParams.get("startDate") || "");
    const [endDate, setEndDate] = useState(searchParams.get("endDate") || "");
    const [noOfPeople, setNoOfPeople] = useState<number>(1);
    const [isAvailable, setIsAvailable] = useState<boolean | null>(null);
    const [availabilityError, setAvailabilityError] = useState<string | null>(null);
    const [checkingAvailability, setCheckingAvailability] = useState(false);

    useEffect(() => {
        const fetchProperty = async () => {
            try {
                const response = await fetch(`${PropertyServiceUrl}/properties/getById/${propertyId}`);
                if (!response.ok) throw new Error("Failed to fetch property details.");
                const data: Response<Property> = await response.json();
                setProperty(data.data);
            } catch (err: any) {
                setError(err.message || "Something went wrong.");
            } finally {
                setLoading(false);
            }
        };

        fetchProperty();
    }, [propertyId]);

    useEffect(() => {
        const checkAvailability = async () => {
            if (!startDate || !endDate || !propertyId) {
                setIsAvailable(null);
                return;
            }

            const start = new Date(startDate);
            const end = new Date(endDate);
            if (end <= start) {
                setIsAvailable(false);
                return;
            }

            try {
                setCheckingAvailability(true);
                setAvailabilityError(null);

                const response = await fetch(
                    `${bookingServiceUrl}/Booking/areDatesAvailable?propertyId=${property?.propertyId}&startDate=${startDate}&endDate=${endDate}`
                );
                if (!response.ok) throw new Error("Failed to check availability.");

                const data: Response<boolean> = await response.json();
                setIsAvailable(data.data);
            } catch (err: any) {
                setAvailabilityError(err.message || "Error checking availability.");
                setIsAvailable(null);
            } finally {
                setCheckingAvailability(false);
            }
        };

        checkAvailability();
    }, [startDate, endDate, propertyId]);

    const calculateTotal = () => {
        if (!startDate || !endDate) return 0;
        const start = new Date(startDate);
        const end = new Date(endDate);
        const nights = Math.max(0, Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24)));
        return nights * (property?.pricePerNight || 0);
    };

    const submitBooking = async () => {
        const token = localStorage.getItem("token");
        if (!token) {
            navigate("/auth?login=true");
            return;
        }

        try {
            const response = await fetch(`${bookingServiceUrl}/booking/addBooking`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify({
                    propertyId: property?.propertyId,
                    from: startDate,
                    to: endDate,
                    noOfPeople: noOfPeople
                })
            });

            const data: Response<string> = await response.json();
            if (!response.ok || !data.success) {
                throw new Error(data.message || "Booking failed.");
            }

            alert("Booking successful!");
            navigate("/bookings");
        } catch (err: any) {
            setError(err.message || "Something went wrong.");
        }
    };

    if (loading) return <Spinner animation="border" className="m-5" />;
    if (error) return <Alert variant="danger" className="m-5">{error}</Alert>;
    if (!property) return <p className="m-5">No property found.</p>;

    return (
        <Container className="py-4">
            <h2>{property.propertyName}</h2>
            <p className="text-muted">{property.location.address}, {property.location.city}, {property.location.country}</p>

            <Row className="my-4">
                <Col md={7}>
                    <Carousel controls indicators interval={null}>
                        {property?.images?.map((img, index) => (
                            <Carousel.Item key={index}>
                                <img
                                    className="d-block w-100"
                                    src={img}
                                    alt={`Slide ${index}`}
                                    style={{
                                        maxHeight: "300px",
                                        objectFit: "contain",
                                        borderRadius: "8px"
                                    }}
                                />
                            </Carousel.Item>
                        ))}
                    </Carousel>
                </Col>
                <Col md={5}>
                    <Card>
                        <Card.Body>
                            <Card.Title>${property.pricePerNight}/night</Card.Title>
                            <Card.Text>{property.description}</Card.Text>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>

            <Row className="my-4 align-items-end">
                <Col md={3}>
                    <Form.Control
                        type="date"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                    />
                </Col>
                <Col md={3}>
                    <Form.Control
                        type="date"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                    />
                </Col>
                <Col md={2}>
                    <Form.Control
                        type="number"
                        value={noOfPeople}
                        min={1}
                        max={property.capacity}
                        onChange={(e) => setNoOfPeople(parseInt(e.target.value))}
                        placeholder="Guests"
                    />
                </Col>
                <Col md={2}>
                    <div><strong>Total:</strong> ${calculateTotal().toFixed(2)}</div>
                </Col>
                <Col>
                    <Button
                        disabled={
                            !startDate ||
                            !endDate ||
                            isAvailable === false ||
                            checkingAvailability ||
                            calculateTotal() <= 0
                        }
                        onClick={submitBooking}
                    >
                        {checkingAvailability ? "Checking..." : "Book Now"}
                    </Button>
                </Col>
            </Row>

            {availabilityError && <Alert variant="danger">{availabilityError}</Alert>}
            {(isAvailable === false && !availabilityError) && (
                <Alert variant="warning">This property is not available for the selected dates.</Alert>
            )}

            <Row>
                <MapWithMarker lat={property.location.latitude} lng={property.location.longitude} />
            </Row>
        </Container>
    );
};

export default PropertyDetailPage;
