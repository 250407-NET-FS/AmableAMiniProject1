// shared/navbar.jsx
import React from "react";
import { useTheme } from "@mui/material/styles";
import {
  AppBar,
  Container,
  Toolbar,
  Box,
  Button,
  IconButton,
  Typography,
} from "@mui/material";
import { Link } from "react-router-dom";
import logo from "../assets/logo.png";
import { useAuth } from "../context/AuthContext";

// two MUI icons for sun and moon
import Brightness4Icon from "@mui/icons-material/Brightness4";
import Brightness7Icon from "@mui/icons-material/Brightness7";

const NavBar = ({ mode, toggleColorMode }) => {
  const theme = useTheme();
  const { user, logout } = useAuth();

  return (
    <AppBar
      position="static"
      color="transparent"
      sx={{
        backgroundColor:
          theme.palette.mode === "dark"
            ? theme.palette.primary.dark
            : theme.palette.primary.main,
      }}
    >
      <Container maxWidth={false} disableGutters>
        <Toolbar
          sx={{
            px: "1.5rem",
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <IconButton component={Link} to="/" disableRipple>
              <img src={logo} alt="App Logo" className="logo-image" />
            </IconButton>
            <Typography
              variant="h4"
              component={Link}
              to="/"
              sx={{ color: "#fff", fontWeight: "bold", textDecoration: "none" }}
            >
              Workout Tracker
            </Typography>
          </Box>

          <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
            <Typography sx={{ color: "#fff" }}>
              {user ? `Welcome, ${user.email.split("@")[0]}` : "Welcome, Guest"}
            </Typography>

            {/* If Admin, show Dashboard link */}
            {user?.role === "Admin" && (
              <Button
                component={Link}
                to="/admin/Dashboard"
                sx={{ color: "#fff", textTransform: "none" }}
              >
                Admin Dashboard
              </Button>
            )}

            {user ? (
              <Button
                onClick={logout}
                sx={{ color: "#fff", textTransform: "none" }}
              >
                Logout
              </Button>
            ) : (
              <Button
                component={Link}
                to="/signin"
                sx={{ color: "#fff", textTransform: "none" }}
              >
                Log in
              </Button>
            )}

            {/* Theme Toggle */}
            <IconButton
              onClick={toggleColorMode}
              sx={{ color: "#fff" }}
              size="large"
            >
              {theme.palette.mode === "dark" ? (
                <Brightness7Icon />
              ) : (
                <Brightness4Icon />
              )}
            </IconButton>
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};

export default NavBar;
