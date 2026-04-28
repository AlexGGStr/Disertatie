// src/components/PropertyCard.tsx
import React from "react";
import { Card, Button } from "react-bootstrap";
import Property from "../../Models/Property";

interface PropertyCardProps {
    property: Property;
    actionLabel?: string;
    onAction?: (property: Property) => void;
    actions?: React.ReactNode;
}

const PropertyCard: React.FC<PropertyCardProps> = ({
                                                       property,
                                                       actionLabel = "View",
                                                       onAction,
                                                       actions,
                                                   }) => {
    return (
        <Card className="mb-4 shadow-sm">
            <Card.Img
                variant="top"
                src={property.images?.[0] || ""}
                style={{ height: "200px", objectFit: "cover" }}
            />
            <Card.Body>
                <Card.Title>{property.propertyName}</Card.Title>
                <Card.Subtitle className="mb-2 text-muted">
                    {property.location.address}, {property.location.city}, {property.location.country}
                </Card.Subtitle>
                <Card.Text>{property.description}</Card.Text>
                <div className="d-flex justify-content-between align-items-center">
                    <span className="fw-bold">${property.pricePerNight}/night</span>
                    <div className="d-flex gap-2">
                        <Button variant="primary" onClick={() => onAction?.(property)}>
                            {actionLabel}
                        </Button>
                        {actions}
                    </div>
                </div>
            </Card.Body>
        </Card>
    );
};

export default PropertyCard;
