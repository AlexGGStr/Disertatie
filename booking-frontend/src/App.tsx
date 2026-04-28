import React from 'react';
import logo from './logo.svg';
import './App.css';
import { Button } from "react-bootstrap";
import AppNavbar from "./Components/Navbar";
import {createBrowserRouter, Route, RouterProvider, Routes} from "react-router-dom";
import MainRoot from "./Pages/MainRoot";
import {checkAuthLoader, tokenLoader} from "./utils/auth";
import LoginPage from "./Pages/LoginPage";
import HomePage from "./Pages/HomePage";
import PropertyDetailPage from "./Pages/PropertyDetailPage";
import ManagePropertiesPage from "./Pages/ManagePropertiesPage";
import AddPropertyPage from "./Pages/AddPropertyPage";
import BookingsPage from "./Pages/BookingsPage";
import CheckoutPage from "./Pages/CheckoutPage";
import EditPropertyPage from "./Pages/EditPropertyPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <MainRoot />,
    loader: tokenLoader,
    id: "root",
    children: [
      { index: true, element: <HomePage /> },
      {
        path: "/properties/:propertyId",
        children: [
          { index: true, element: <PropertyDetailPage /> }
        ],
      },
      {
        path: "/manageProperties",
        loader: checkAuthLoader,
        children: [
          { index: true, element: <ManagePropertiesPage />},
          { path: "edit/:propertyId", element: <EditPropertyPage /> },
        ]
      },
      {
        path: "/auth",
        element: <LoginPage />
      },
      {
        path: "/addProperty",
        loader: checkAuthLoader,
        element: <AddPropertyPage />
      },
      {
        path: "/bookings",
        loader: checkAuthLoader,
        element: <BookingsPage />,
      },
      {
        path: "/account",
        // loader: checkAuthLoader,
        element: <h1>Account</h1>,
      },
      {
        path: "logout",
        // action: logoutAction,
      },
      {
        path: "/checkout",
        loader: checkAuthLoader,
        element: <CheckoutPage />
      }
    ],
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
