import React, { FC } from "react";
import { Link, useSearchParams } from "react-router-dom";
import LoginForm from "../Components/LoginForm";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import RegisterForm from "../Components/RegisterForm";

interface LoginPageProps {}

const LoginPage: FC<LoginPageProps> = () => {
    const [searchParams] = useSearchParams();
    const login = searchParams.get("login");

    return (
        <Container className="d-flex justify-content-center align-items-center" style={{ minHeight: "100vh" }}>
            <Row>
                <Col>
                    <Card className="shadow-sm p-4" style={{ width: "22rem" }}>
                        <Card.Title className="mb-4 text-center">
                            {login === "true" ? "Login" : "Register"}
                        </Card.Title>

                        {login === "true" ? (
                            <LoginForm />
                        ) : <RegisterForm />}

                        <Link
                            to={`/auth?login=${login === "true" ? "false" : "true"}`}
                            className="btn btn-outline-secondary w-100 mt-2"
                        >
                            {login === "true"
                                ? "No account? Register here"
                                : "Already have an account? Login here"}
                        </Link>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
};

export default LoginPage;
