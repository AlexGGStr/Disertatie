interface Response<T> {
    success: boolean;
    message: string;
    data: T;
}

export default Response;
