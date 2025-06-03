// index.jsx
import { StrictMode, useState, useMemo } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
import Register from "./Register.jsx";
import SignIn from "./SignIn.jsx";
import NavBar from "./shared/navbar.jsx";
import { AuthProvider } from "./context/AuthContext.jsx";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { createTheme, ThemeProvider, CssBaseline } from "@mui/material";

function Root() {
  const [mode, setMode] = useState("light");
  const { user } = useAuth();

  const theme = useMemo(
    () =>
      createTheme({
        palette: {
          mode,
        },
      }),
    [mode]
  );

  const toggleColorMode = () =>
    setMode((prev) => (prev === "light" ? "dark" : "light"));

  return (
    <Routes>
      <Route path="/" element={<App />} />
      <Route
        path="/signin"
        element={!user ? <SignIn /> : <Navigate to="/" replace />}
      />
      <Route
        path="/register"
        element={!user ? <Register /> : <Navigate to="/" replace />}
      />
    </Routes>
  );
}

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <Root />
  </StrictMode>
);
