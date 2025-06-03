import * as React from "react";
import {
  Box,
  Button,
  Checkbox,
  CssBaseline,
  FormControl,
  FormControlLabel,
  FormLabel,
  Divider,
  Link,
  TextField,
  Typography,
  Stack,
  Card as MuiCard,
} from "@mui/material";
import { styled, useTheme } from "@mui/material/styles";
import { useAuth } from "./context/AuthContext";
import { useNavigate } from "react-router-dom";

// Instead of theme.applyStyles("dark", {...}),
// we'll do theme.palette.mode === "dark" ? darkStyles : {}
const Card = styled(MuiCard)(({ theme }) => {
  const isDark = theme.palette.mode === "dark";

  return {
    display: "flex",
    flexDirection: "column",
    alignSelf: "center",
    width: "100%",
    padding: theme.spacing(4),
    gap: theme.spacing(2),
    margin: "auto",
    [theme.breakpoints.up("sm")]: {
      maxWidth: "450px",
    },
    boxShadow: isDark
      ? "hsla(220, 30%, 5%, 0.5) 0px 5px 15px 0px, hsla(220, 25%, 10%, 0.08) 0px 15px 35px -5px"
      : "hsla(220, 30%, 5%, 0.05) 0px 5px 15px 0px, hsla(220, 25%, 10%, 0.05) 0px 15px 35px -5px",
  };
});

const SignInContainer = styled(Stack)(({ theme }) => {
  const isDark = theme.palette.mode === "dark";

  return {
    height: "calc((1 - var(--template-frame-height, 0)) * 100dvh)",
    minHeight: "100%",
    padding: theme.spacing(2),
    [theme.breakpoints.up("sm")]: {
      padding: theme.spacing(4),
    },
    position: "relative",
    "&::before": {
      content: '""',
      display: "block",
      position: "absolute",
      zIndex: -1,
      inset: 0,
      backgroundImage: isDark
        ? "radial-gradient(at 50% 50%, hsla(210, 100%, 16%, 0.5), hsl(220, 30%, 5%))"
        : "radial-gradient(ellipse at 50% 50%, hsl(210, 100%, 97%), hsl(0, 0%, 100%))",
      backgroundRepeat: "no-repeat",
    },
  };
});

export default function SignIn() {
  const theme = useTheme(); // you can still read palette.mode if you need it
  const { login } = useAuth();
  const [emailError, setEmailError] = React.useState(false);
  const [emailErrorMessage, setEmailErrorMessage] = React.useState("");
  const [passwordError, setPasswordError] = React.useState(false);
  const [passwordErrorMessage, setPasswordErrorMessage] = React.useState("");
  const [logInError, setLogInError] = React.useState(false);

  const handleSubmit = async (event) => {
    const navigate = useNavigate();
    event.preventDefault();
    if (!validateInputs()) return;

    const Email = event.currentTarget.email.value;
    const Password = event.currentTarget.password.value;
    const success = await login({ Email, Password });
    if (success){
      navigate("../signin")
    }
    else {
      setLogInError(true);
      logInError('Registed Failed');
    };
  };

  const validateInputs = () => {
    const email = document.getElementById("email");
    const password = document.getElementById("password");
    let isValid = true;

    if (!email.value || !/\S+@\S+\.\S+/.test(email.value)) {
      setEmailError(true);
      setEmailErrorMessage("Please enter a valid email address.");
      isValid = false;
    } else {
      setEmailError(false);
      setEmailErrorMessage("");
    }

    if (!password.value || password.value.length < 6) {
      setPasswordError(true);
      setPasswordErrorMessage("Password must be at least 6 characters long.");
      isValid = false;
    } else {
      setPasswordError(false);
      setPasswordErrorMessage("");
    }

    return isValid;
  };

  return (
    <>
      <SignInContainer direction="column" justifyContent="space-between">
        <Card variant="outlined">
          <Typography
            component="h1"
            variant="h4"
            sx={{ width: "100%", fontSize: "clamp(2rem, 10vw, 2.15rem)" }}
          >
            Sign in
          </Typography>

          <Box
            component="form"
            onSubmit={handleSubmit}
            noValidate
            sx={{
              display: "flex",
              flexDirection: "column",
              width: "100%",
              gap: 2,
            }}
          >
            <FormControl>
              <FormLabel htmlFor="email">Email</FormLabel>
              <TextField
                error={emailError}
                helperText={emailErrorMessage}
                id="email"
                type="email"
                name="Email"
                placeholder="your@email.com"
                autoComplete="email"
                autoFocus
                required
                fullWidth
                variant="outlined"
                color={emailError ? "error" : "primary"}
              />
            </FormControl>

            <FormControl>
              <FormLabel htmlFor="password">Password</FormLabel>
              <TextField
                error={passwordError}
                helperText={passwordErrorMessage}
                name="Password"
                placeholder="••••••"
                type="password"
                id="password"
                autoComplete="current-password"
                required
                fullWidth
                variant="outlined"
                color={passwordError ? "error" : "primary"}
              />
            </FormControl>

            <FormControlLabel
              control={<Checkbox value="remember" color="primary" />}
              label="Remember me"
            />

            <Button type="submit" fullWidth variant="contained" onClick={validateInputs}>
              Sign in
            </Button>

            {logInError && <Typography sx={{ mt: 1 }}>Sign in failed.</Typography>}
          </Box>

          <Divider>or</Divider>

          <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
            <Typography sx={{ textAlign: "center" }}>
              Don’t have an account?{" "}
              <Link href="/register" variant="body2" sx={{ alignSelf: "center" }}>
                Sign up
              </Link>
            </Typography>
          </Box>
        </Card>
      </SignInContainer>
    </>
  );
}
