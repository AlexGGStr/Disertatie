import React, { useState, useEffect } from "react";
import { Form, Button, Row, Col, Spinner, Alert, ListGroup } from "react-bootstrap";
import Property from "../Models/Property";
import Response from "../Models/Response";
import { PropertyServiceUrl } from "../config";
import PropertyList from "../Components/Property/PropertyList";
import { useDebounce } from "../utils/hooks/useDebounce";
import {useNavigate} from "react-router-dom";

type Suggestion = {
    description: string;
    placeId: string;
};

const HomePage: React.FC = () => {
    const navigate = useNavigate();
    const [inputValue, setInputValue] = useState(""); // what the user types
    const [selectedPlaceId, setSelectedPlaceId] = useState<string | null>(null); // actual location to search
    const debouncedInput = useDebounce(inputValue, 500);

    const [guests, setGuests] = useState(1);
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");

    const [properties, setProperties] = useState<Property[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const [suggestions, setSuggestions] = useState<Suggestion[]>([]);
    const [suggestionsLoading, setSuggestionsLoading] = useState(false);
    const [suggestionsError, setSuggestionsError] = useState<string | null>(null);
    const [justSelected, setJustSelected] = useState(false);

    // Fetch autocomplete suggestions
    useEffect(() => {
        if (!debouncedInput.trim() || justSelected) {
            setJustSelected(false);
            return;
        }

        const fetchSuggestions = async () => {
            setSuggestionsLoading(true);
            setSuggestionsError(null);

            try {
                const response = await fetch(`${PropertyServiceUrl}/properties/search?city=${encodeURIComponent(debouncedInput)}`);
                if (!response.ok) throw new Error("Failed to fetch suggestions.");
                const data: Response<Suggestion[]> = await response.json();

                setSuggestions(data.data || []);
            } catch (err: any) {
                setSuggestionsError(err.message || "Error fetching location suggestions.");
            } finally {
                setSuggestionsLoading(false);
            }
        };

        fetchSuggestions();
    }, [debouncedInput]);

    const handleSuggestionClick = (suggestion: Suggestion) => {
        setInputValue(suggestion.description);
        setSelectedPlaceId(suggestion.placeId);
        setSuggestions([]);
        setJustSelected(true);

        console.log(selectedPlaceId)
    };

    const handleSearch = async () => {
        setLoading(true);
        setError(null);

        try {
            const query = new URLSearchParams({
                placeId: selectedPlaceId || "", // send actual place_id
                noOfPeople: guests.toString(),
                from: startDate,
                to: endDate,
            });

            const response = await fetch(`${PropertyServiceUrl}/properties/searchProperties?${query.toString()}`);
            if (!response.ok) throw new Error("Failed to fetch properties.");

            const data: Response<Property[]> = await response.json();
            if (!data.success) throw new Error(data.message || "Failed to fetch properties.");

            setProperties(data.data);
        } catch (err: any) {
            setError(err.message || "Something went wrong.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container py-4">
            <h1 className="mb-4">Find Your Perfect Stay</h1>
            <Form className="mb-4" onSubmit={e => { e.preventDefault(); handleSearch(); }}>
                <Row className="gy-3 position-relative">
                    <Col md={3}>
                        <Form.Control
                            type="text"
                            placeholder="Location"
                            value={inputValue}
                            onChange={(e) => {
                                setInputValue(e.target.value);
                                setSelectedPlaceId(null); // reset on manual typing
                            }}
                            autoComplete="off"
                        />
                        {suggestionsLoading && <Spinner animation="border" size="sm" />}
                        {suggestions.length > 0 && (
                            <ListGroup className="position-absolute w-100 zindex-tooltip" style={{ maxHeight: "200px", overflowY: "auto" }}>
                                {suggestions.map((s, idx) => (
                                    <ListGroup.Item key={idx} action onClick={() => handleSuggestionClick(s)}>
                                        {s.description}
                                    </ListGroup.Item>
                                ))}
                            </ListGroup>
                        )}
                        {suggestionsError && <div className="text-danger">{suggestionsError}</div>}
                    </Col>
                    <Col md={2}>
                        <Form.Control
                            type="date"
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                        />
                    </Col>
                    <Col md={2}>
                        <Form.Control
                            type="date"
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                        />
                    </Col>
                    <Col md={2}>
                        <Form.Control
                            type="number"
                            min={1}
                            placeholder="Guests"
                            value={guests}
                            onChange={(e) => setGuests(parseInt(e.target.value) || 1)}
                        />
                    </Col>
                    <Col md={3}>
                        <Button className="w-100" type="submit" disabled={loading || !selectedPlaceId}>
                            {loading ? <Spinner animation="border" size="sm" /> : "Search"}
                        </Button>
                    </Col>
                </Row>
            </Form>

            {error && <Alert variant="danger">{error}</Alert>}

            <PropertyList properties={properties} actionLabel="View" onAction={(property) =>
                navigate(`/properties/${property.propertyId}?startDate=${startDate}&endDate=${endDate}`)}
            />
        </div>
    );
};

export default HomePage;
