import { FC, useState } from "react";
import { useNavigate } from "react-router-dom";
import Response from "../Models/Response";
import { userServiceUrl } from "../config";
import { Form, Button, Alert, Container, Row, Col, Card } from "react-bootstrap";
import {useUser} from "../context/UserContext";
import {jwtDecode} from "jwt-decode";

interface LoginFormProps {}

const LoginForm: FC<LoginFormProps> = () => {
    const [enteredEmail, setEnteredEmail] = useState("");
    const [enteredPassword, setEnteredPassword] = useState("");
    const [response, setResponse] = useState<Response<string> | null>(null);
    const navigate = useNavigate();
    const { setUser } = useUser();

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault();

        try {
            const res = await fetch(`${userServiceUrl}/Auth/login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: enteredEmail,
                    password: enteredPassword,
                }),
            });

            const dataJson: Response<string> = await res.json();
            setResponse(dataJson);

            if (dataJson.success) {
                localStorage.setItem("token", dataJson.data);

                const decoded: any = jwtDecode(dataJson.data);
                setUser({
                    id: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
                    name: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                    email: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
                    role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
                });

                alert("Login successful");
                navigate("/");
            }
        } catch (err) {
            let errorMessage = "Unknown error occurred";

            if (err instanceof Error) {
                errorMessage = err.message;
            }

            setResponse({ success: false, message: errorMessage, data: "" });
        }
    };

    return (
        <>
            {response && !response.success && (
                <Alert variant="danger">{response.message}</Alert>
            )}
            <Form onSubmit={submitHandler}>
                <Form.Group className="mb-3" controlId="email">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type="email"
                        value={enteredEmail}
                        onChange={(e) => setEnteredEmail(e.target.value)}
                        required
                        placeholder="Enter email"
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="password">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        value={enteredPassword}
                        onChange={(e) => setEnteredPassword(e.target.value)}
                        required
                        placeholder="Enter password"
                    />
                </Form.Group>

                <Button variant="primary" type="submit" className="w-100">
                    Log In
                </Button>
            </Form>
        </>
    );
};

export default LoginForm;
