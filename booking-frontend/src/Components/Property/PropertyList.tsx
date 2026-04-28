// src/components/PropertyList.tsx
import React from "react";
import PropertyCard from "./PropertyCard";
import Property from "../../Models/Property";

interface PropertyListProps {
    properties: Property[];
    actionLabel?: string;
    onAction?: (property: Property) => void;
    renderActions?: (property: Property) => React.ReactNode;
}

const PropertyList: React.FC<PropertyListProps> = ({
   properties,
   actionLabel,
   onAction,
   renderActions,
}) => {
    if (properties.length === 0) {
        return <p></p>;
    }

    return (
        <div className="row">
            {properties.map((property) => (
                <div className="col-md-4" key={property.propertyId}>
                    <PropertyCard
                        property={property}
                        actionLabel={actionLabel}
                        onAction={onAction}
                        actions={renderActions?.(property)}
                    />
                </div>
            ))}
        </div>
    );
};

export default PropertyList;
