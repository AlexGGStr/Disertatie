import React, { FC } from "react";
import { Outlet, useLoaderData } from "react-router-dom";
import AppNavbar from "../Components/Navbar";

interface MainRootProps {}

const MainRoot: FC<MainRootProps> = () => {
    return (
        <>
            <AppNavbar />
            <main>
                <Outlet />
            </main>
        </>
    );
};

export default MainRoot;