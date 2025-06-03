import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from "./context/AuthContext";
import { useNavigate } from "react-router-dom";

function Login() {
  const { login } = useAuth();
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    email: "",
    password: "",
  });

  const handleSubmit = async (e) => {
    e.preventDefault();

    // logging and user messages
    if (credentials.email === '' || credentials.password === '') {
      setErrorMessage('Please fill in all fields');
      setSuccessMessage('');
      credentials.email = "";
      credentials.password = "";
    } else {
      // logging
      console.log('Logging in with:', {
            email: credentials.email,
            password: credentials.password
        });

        // Attempt to log in the user with the provided credentials
        try {
            const success = await login(credentials);
          if (success) {
              // user message
              setSuccessMessage('Login successful!');
              setErrorMessage(null);
              navigate("/");
          } else {
            setErrorMessage('Login failed. Please try again.');
            credentials.email = "";
            credentials.password = "";
          }
        } catch (errorMessage) {
            setErrorMessage("Invalid credentials. Please try again.");
            return;
        }
    }

  };

  return (
    <>
      <h1>User Login</h1>

      {/* Alert Messages */}
      {errorMessage && (
        <div className="alert alert-danger" role="alert">
          {errorMessage}
        </div>
      )}

      {successMessage && (
        <div className="alert alert-success" role="alert">
          {successMessage}
        </div>
      )}

      <hr />
      <div className="row">
        <div className="col-md-4">
          <form onSubmit={handleSubmit}>
            {/* Email */}
            <div className="form-group">
              <p>Email</p>
              <input
                type="email"
                value={credentials.email}
                onChange={(e) => 
                setCredentials({ ...credentials, email: e.target.value })
                }
            required
              />
            </div>

            {/* Password */}
            <div className="form-group">
              <p>Password</p>
              <input
                type="password"
                value={credentials.password}
                onChange={(e) =>
                setCredentials({ ...credentials, password: e.target.value })
                }
                required
              />
            </div>

            {/* Submit Button */}
            <div className="form-group">
              <input
                type="submit"
                value="Login"
                className="btn btn-outline-dark"
              />
            </div>
          </form>
        </div>
      </div>

      <div>
        <p>
            Don't have an account?{' '}
            <Link to="/register" className="btn btn-outline-dark">Register</Link>
        </p>
      </div>
    </>
  );
}

export default Login;