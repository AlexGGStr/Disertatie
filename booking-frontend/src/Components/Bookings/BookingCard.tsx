import React from "react";
import { Card, Button, Badge, Row, Col } from "react-bootstrap";

export interface BookingCardProps {
    id: string;
    propertyName: string;
    location: string,
    from: string;
    to: string;
    status: string;
    onCancel: (id: string) => void;
    onPay: (id: string, amount: number) => void;
    totalPrice?: number;
}

const statusVariant = (status: string) => {
    switch (status.toLowerCase()) {
        case "confirmed":
            return "success";
        case "pending":
            return "warning";
        case "cancelled":
            return "secondary";
        default:
            return "primary";
    }
};

const BookingCard: React.FC<BookingCardProps> = ({
                                                     id,
                                                     propertyName,
                                                     location,
                                                     from,
                                                     to,
                                                     status,
                                                     onCancel,
                                                     onPay,
                                                    totalPrice
                                                 }) => {
    return (
        <Card className="mb-4 shadow-sm w-100">
            <Card.Body>
                <Row className="align-items-center">
                    <Col md={3}>
                        <h5 className="mb-1">{propertyName}</h5>
                        <p className="text-muted mb-0">{location}</p>
                    </Col>

                    <Col md={3}>
                        <div><strong>From:</strong> {new Date(from).toLocaleDateString()}</div>
                        <div><strong>To:</strong> {new Date(to).toLocaleDateString()}</div>
                    </Col>

                    <Col md={2} className="text-md-center mt-2 mt-md-0">
                        <Badge bg={statusVariant(status)}>{status}</Badge>
                    </Col>

                    <Col md={4} className="d-flex justify-content-end gap-2 mt-3 mt-md-0">
                        <Button
                            variant="danger"
                            size="sm"
                            onClick={() => onCancel(id)}
                            disabled={status.toLowerCase() === "cancelled"}
                        >
                            Cancel
                        </Button>
                        {status.toLowerCase() !== "confirmed" && status.toLowerCase() !== "cancelled" && (
                            <Button
                                variant="success"
                                size="sm"
                                onClick={() => onPay(id, totalPrice || 1)}
                            >
                                Pay
                            </Button>
                        )}
                    </Col>
                </Row>

                <hr className="my-3" />

                <Row>
                    <Col md={6} className="text-muted">
                        Booking ID: <strong>{id}</strong>
                    </Col>
                    <Col md={6} className="text-md-end fw-semibold">
                        Total Amount: ${totalPrice}
                    </Col>
                </Row>
            </Card.Body>
        </Card>
    );
};

export default BookingCard;
