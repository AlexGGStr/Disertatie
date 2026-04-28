import React from "react";
import { GoogleMap, LoadScript, Marker } from "@react-google-maps/api";

type MapWithMarkerProps = {
    lat: number;
    lng: number;
};

const containerStyle = {
    width: "100%",
    height: "400px",
};

const MapWithMarker: React.FC<MapWithMarkerProps> = ({ lat, lng }) => {
    const center = {
        lat,
        lng,
    };

    return (
        <LoadScript googleMapsApiKey={process.env.REACT_APP_GOOGLE_MAPS_API_KEY!}>
            <GoogleMap mapContainerStyle={containerStyle} center={center} zoom={14}>
                <Marker position={center} />
            </GoogleMap>
        </LoadScript>
    );
};

export default MapWithMarker;
