interface Property {
    propertyId: string;
    propertyName: string;
    description: string;
    location: Location;
    pricePerNight: number;
    noOfRooms: number;
    capacity: number;
    images: string[];
}

interface Location {
    address: string;
    city: string;
    country: string;
    latitude: number;
    longitude: number;
}

export default Property;