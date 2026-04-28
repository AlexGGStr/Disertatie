// src/components/Navbar.tsx
import React, {useEffect, useState} from 'react';
import { Navbar, Container, Nav, Button, NavDropdown } from 'react-bootstrap';
import {NavLink, useNavigate, useRouteLoaderData} from 'react-router-dom';
import {logout} from "../utils/auth";
import {useUser} from "../context/UserContext";

const AppNavbar: React.FC = () => {
    const navigate = useNavigate();
    const token = useRouteLoaderData("root");

    const { user, setUser } = useUser();

    const isLoggedIn = !!user;

    const handleLogout = () => {
        console.log(user);
        setUser(null);
        logout();
    };

    return (
        <Navbar bg="light" expand="lg" sticky="top" className="shadow-sm mb-4">
            <Container>
                <Navbar.Brand as={NavLink} to="/">HOLIBOOK</Navbar.Brand>
                <Navbar.Toggle aria-controls="main-navbar" />
                <Navbar.Collapse id="main-navbar">
                    <Nav className="me-auto">
                        <Nav.Link as={NavLink} to="/" end>Home</Nav.Link>
                        <Nav.Link as={NavLink} to="/bookings">My Bookings</Nav.Link>
                        {user?.role === "Admin" ? (
                            <Nav.Link as={NavLink} to="/manageProperties">
                               Manage Properties
                            </Nav.Link>
                        ) : (
                            <Nav.Link as={NavLink} to="/addProperty">
                                Add your first Property
                            </Nav.Link>
                        )}
                    </Nav>
                    <Nav>
                        {!isLoggedIn ? (
                            <Button variant="outline-primary" onClick={() => navigate('/auth?login=true')}>
                                Login
                            </Button>
                        ) : (
                            <NavDropdown title={user?.name ?? "Uof :("} align="end">
                                <NavDropdown.Item onClick={() => navigate('/profile')}>Profile</NavDropdown.Item>
                                <NavDropdown.Divider />
                                <NavDropdown.Item onClick={handleLogout}>Logout</NavDropdown.Item>
                            </NavDropdown>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default AppNavbar;
