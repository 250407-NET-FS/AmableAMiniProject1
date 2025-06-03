import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
import Register from "./Register.jsx";
import { AuthProvider } from "./context/AuthContext.jsx";
import SignIn from "./SignIn.jsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <AuthProvider>
      <SignIn />
    </AuthProvider>
  </StrictMode>
);
