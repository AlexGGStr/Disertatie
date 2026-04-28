import { useEffect, useState } from "react";
import { Button, Spinner, Alert } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import PropertyList from "../Components/Property/PropertyList";
import Property from "../Models/Property";
import Response from "../Models/Response";
import { PropertyServiceUrl } from "../config";

const ManagePropertiesPage = () => {
    const navigate = useNavigate();
    const [properties, setProperties] = useState<Property[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchProperties = async () => {
            setLoading(true);
            setError(null);
            try {
                const token = localStorage.getItem("token"); // or sessionStorage or a context/store

                const response = await fetch(`${PropertyServiceUrl}/properties/getMyProperties`, {
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`, // Include token in headers
                    },
                });
                if (!response.ok) throw new Error("Failed to fetch properties.");

                const data: Response<Property[]> = await response.json();
                if (!data.success) throw new Error(data.message || "Error fetching properties.");

                setProperties(data.data);
            } catch (err: any) {
                setError(err.message || "Something went wrong.");
            } finally {
                setLoading(false);
            }
        };

        fetchProperties();
    }, []);

    return (
        <div className="container py-4">
            <div>
                <h2 className="mb-4">Manage Your Properties</h2>
                <Button variant={"primary"} className="mb-3" onClick={() => navigate("/addProperty")}>Add New Property</Button>
            </div>

            {loading && <Spinner animation="border" />}
            {error && <Alert variant="danger">{error}</Alert>}

            {!loading && !error && (
                <PropertyList
                    properties={properties}
                    actionLabel="Edit"
                    onAction={(property) =>
                        navigate(`edit/${property.propertyId}`)
                    }
                />
            )}
        </div>
    );
};

export default ManagePropertiesPage;
