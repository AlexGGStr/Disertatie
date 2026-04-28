// src/pages/AddNewPropertyPage.tsx
import React, { useState, useEffect } from "react";
import { Form, Button, Spinner, ListGroup, Row, Col } from "react-bootstrap";
import { PropertyServiceUrl } from "../config";
import { useDebounce } from "../utils/hooks/useDebounce";
import Response from "../Models/Response";

const AddNewPropertyPage: React.FC = () => {
  const [inputValue, setInputValue] = useState("");
  const [selectedPlaceId, setSelectedPlaceId] = useState<string | null>(null);
  const [suggestions, setSuggestions] = useState<any[]>([]);
  const [suggestionsLoading, setSuggestionsLoading] = useState(false);
  const [suggestionsError, setSuggestionsError] = useState<string | null>(null);
  const [justSelected, setJustSelected] = useState(false);

  const [propertyName, setPropertyName] = useState("");
  const [pricePerNight, setPricePerNight] = useState(0);
  const [description, setDescription] = useState("");
  const [rooms, setRooms] = useState(0);
  const [capacity, setCapacity] = useState(0);
  const [images, setImages] = useState<FileList | null>(null);

  const debouncedInput = useDebounce(inputValue, 500);

  useEffect(() => {
    if (!debouncedInput.trim() || justSelected) {
      setJustSelected(false);
      return;
    }

    const fetchSuggestions = async () => {
      setSuggestionsLoading(true);
      setSuggestionsError(null);

      try {
        const res = await fetch(`${PropertyServiceUrl}/properties/search?city=${encodeURIComponent(debouncedInput)}&onlyCity=false`);
        if (!res.ok) throw new Error("Failed to fetch suggestions");
        const data = await res.json();
        setSuggestions(data.data || []);
      } catch (err: any) {
        setSuggestionsError(err.message || "Error fetching suggestions");
      } finally {
        setSuggestionsLoading(false);
      }
    };

    fetchSuggestions();
  }, [debouncedInput]);

  const handleSuggestionClick = (s: any) => {
    setInputValue(s.description);
    setSelectedPlaceId(s.placeId);
    setSuggestions([]);
    setJustSelected(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedPlaceId) return;

    const formData = new FormData();
    formData.append("Name", propertyName);
    formData.append("PricePerNight", pricePerNight.toString());
    formData.append("Description", description);
    formData.append("Rooms", rooms.toString());
    formData.append("NoOfPeople", capacity.toString());
    formData.append("LocationId", selectedPlaceId);
    formData.append("LocationAdress", inputValue);

    if (images) {
      for (let i = 0; i < images.length; i++) {
        formData.append("Images", images[i]);
      }
    }

    const res = await fetch(`${PropertyServiceUrl}/properties/addproperty`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: formData,
    });

    if (!res.ok) {
      const errorData = await res.json();
      throw new Error(errorData.message || "Failed to add property");
    }

    const data: Response<string> = await res.json();
    alert("Property added successfully!" + data.data);
  };

  return (
      <div className="container py-4">
        <h1 className="mb-4">Add New Property</h1>
        <Form onSubmit={handleSubmit}>
          <Row className="mb-3">
            <Col md={6}>
              <Form.Label>Property Name</Form.Label>
              <Form.Control
                  type="text"
                  value={propertyName}
                  onChange={(e) => setPropertyName(e.target.value)}
                  required
              />
            </Col>
            <Col md={6}>
              <Form.Label>Price per Night ($)</Form.Label>
              <Form.Control
                  type="number"
                  value={pricePerNight}
                  onChange={(e) => setPricePerNight(Number(e.target.value))}
                  required
              />
            </Col>
          </Row>
          <Row className="mb-3">
            <Col md={6}>
              <Form.Label>Rooms</Form.Label>
              <Form.Control
                  type="number"
                  value={rooms}
                  onChange={(e) => setRooms(Number(e.target.value))}
                  required
              />
            </Col>
            <Col md={6}>
              <Form.Label>Capacity</Form.Label>
              <Form.Control
                  type="number"
                  value={capacity}
                  onChange={(e) => setCapacity(Number(e.target.value))}
                  required
              />
            </Col>
          </Row>
          <Row className="mb-3">
            <Col md={12}>
              <Form.Label>Description</Form.Label>
              <Form.Control
                  as="textarea"
                  rows={3}
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  required
              />
            </Col>
          </Row>
          <Row className="mb-3">
            <Col md={12}>
              <Form.Label>Location</Form.Label>
              <Form.Control
                  type="text"
                  value={inputValue}
                  onChange={(e) => {
                    setInputValue(e.target.value);
                    setSelectedPlaceId(null);
                  }}
                  autoComplete="off"
                  placeholder="Search address..."
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
          </Row>
          <Row className="mb-3">
            <Col>
              <Form.Label>Upload Images</Form.Label>
              <Form.Control
                  type="file"
                  multiple
                  accept="image/*"
                  onChange={(e) => setImages((e.target as HTMLInputElement).files)}
              />
            </Col>
          </Row>
          <Button variant="primary" type="submit" disabled={!selectedPlaceId}>
            Submit Property
          </Button>
        </Form>
      </div>
  );
};

export default AddNewPropertyPage;
