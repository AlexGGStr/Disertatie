import { redirect } from "react-router-dom";

export function getAuthToken() {
    return localStorage.getItem("token");
}

export function tokenLoader() {
    return getAuthToken();
}

export function checkAuthLoader() {
    const token = getAuthToken();
    if (!token) {
        return redirect("/auth?login=true");
    }
    return null;
}

export function isLoggedIn() {
    const token = getAuthToken();
    return !!token;
}

export function logout() {
    localStorage.removeItem("token");
    return redirect("/");
}