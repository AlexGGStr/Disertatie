import React, { FC, useState } from "react";
import { Form, Button, Alert } from "react-bootstrap";
import { userServiceUrl } from "../config";
import Response from "../Models/Response";
import { useNavigate } from "react-router-dom";

interface RegisterFormProps {}

const RegisterForm: FC<RegisterFormProps> = () => {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [response, setResponse] = useState<Response<string> | null>(null);
    const [validationError, setValidationError] = useState<string | null>(null);
    const navigate = useNavigate();

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault();

        if (password !== confirmPassword) {
            setValidationError("Passwords do not match");
            return;
        }

        try {
            const res = await fetch(`${userServiceUrl}/Auth/register`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ name, email, password }),
            });

            const dataJson: Response<string> = await res.json();
            setResponse(dataJson);

            if (dataJson.success) {
                alert("Registration successful!");
                navigate("/auth?login=true");
            }
        } catch (err) {
            let errorMessage = "Unknown error occurred";
            if (err instanceof Error) errorMessage = err.message;

            setResponse({ success: false, message: errorMessage, data: "" });
        }
    };

    return (
        <>
            {validationError && (
                <Alert variant="danger">{validationError}</Alert>
            )}

            {response && !response.success && (
                <Alert variant="danger">{response.message}</Alert>
            )}

            <Form onSubmit={submitHandler}>
                <Form.Group className="mb-3" controlId="name">
                    <Form.Label>Name</Form.Label>
                    <Form.Control
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                        placeholder="Enter full name"
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="email">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        placeholder="Enter email"
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="password">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        placeholder="Enter password"
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="confirmPassword">
                    <Form.Label>Confirm Password</Form.Label>
                    <Form.Control
                        type="password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        required
                        placeholder="Re-enter password"
                    />
                </Form.Group>

                <Button variant="primary" type="submit" className="w-100">
                    Register
                </Button>
            </Form>
        </>
    );
};

export default RegisterForm;
