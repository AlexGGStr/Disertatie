import React, { useState, useCallback } from "react";
import { GoogleMap, useJsApiLoader, Marker } from "@react-google-maps/api";

const containerStyle = {
    width: "100%",
    height: "500px",
};

const center = {
    lat: 44.4268, // Bucharest
    lng: 26.1025,
};

const MapWithClickPin = () => {
    const { isLoaded } = useJsApiLoader({
        googleMapsApiKey: process.env.REACT_APP_GOOGLE_MAPS_API_KEY ??"", // Replace with your actual key
    });

    const [markerPosition, setMarkerPosition] = useState<google.maps.LatLngLiteral | null>(null);

    const onMapClick = useCallback((e: google.maps.MapMouseEvent) => {
        if (e.latLng) {
            setMarkerPosition({
                lat: e.latLng.lat(),
                lng: e.latLng.lng(),
            });
        }
    }, []);

    if (!isLoaded) return <div>Loading Map...</div>;

    return (
        <div>
            <GoogleMap
                mapContainerStyle={containerStyle}
                center={center}
                zoom={13}
                onClick={onMapClick}
            >
                {markerPosition && <Marker position={markerPosition} />}
            </GoogleMap>

            {markerPosition && (
                <div className="mt-4">
                    <p><strong>Latitude:</strong> {markerPosition.lat.toFixed(6)}</p>
                    <p><strong>Longitude:</strong> {markerPosition.lng.toFixed(6)}</p>
                </div>
            )}
        </div>
    );
};

export default MapWithClickPin;
