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
import { createTheme, ThemeProvider, CssBaseline, Box } from "@mui/material";
import WorkoutsPage from "./pages/workout/WorkoutsPage.jsx";
import RequireAdmin from "./pages/admin/RequireAdmin.jsx";
import Dashboard from "./pages/admin/Dashboard.jsx";
import UserList from "./pages/admin/UserList.jsx";


function Root() {
  const [mode, setMode] = useState("light");

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
    <AuthProvider>
      <ThemeProvider theme={theme}>
        <CssBaseline enableColorScheme />
        <BrowserRouter>
        <Box 
        sx={{
          bgcolor: "background.default",
        minHeight: "100vh",
         }}
       >
        {/* Allows me to get rid of padding issues all over the place */}

          <NavBar mode={mode} toggleColorMode={toggleColorMode} />
          <Routes>
            <Route path="/" element={<WorkoutsPage />} />
            <Route path="/signin" element={<SignIn />} />
            <Route path="/register" element={<Register />} />


            <Route
              path="/admin/Dashboard"
              element={
                <RequireAdmin>
                  <Dashboard />
                </RequireAdmin>
              }
            >
              <Route index element={<UserList />} />

              <Route path="UserList" element={<UserList />} />
            </Route>
          </Routes>
          </Box>
        </BrowserRouter>
      </ThemeProvider>
    </AuthProvider>
  );
}

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <Root />
  </StrictMode>
);
