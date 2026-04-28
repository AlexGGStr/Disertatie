// src/context/UserContext.tsx
import { createContext, useContext, useEffect, useState, ReactNode } from "react";
import {jwtDecode} from "jwt-decode";

interface User {
    id: string;
    name: string;
    email: string;
    role: string;
}

interface UserContextType {
    user: User | null;
    setUser: (user: User | null) => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) return;

        try {
            const decoded: any = jwtDecode(token);
            const user: User = {
                id: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
                name: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                email: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
                role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
            };
            setUser(user);
            console.log(user);
        } catch (err) {
            console.error("Invalid token", err);
        }
    }, []);

    return (
        <UserContext.Provider value={{ user, setUser }}>
            {children}
        </UserContext.Provider>
    );
};

// Export custom hook
export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) throw new Error("useUser must be used within UserProvider");
    return context;
};
