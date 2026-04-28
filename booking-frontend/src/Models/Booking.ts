interface Booking {
    from: string;
    to: string;
    success: boolean;
    totalPrice: number;
    noOfPeople: number;
    status: string;
}

export default Booking;