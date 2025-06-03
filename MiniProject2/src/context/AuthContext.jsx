
import React, {
  createContext,
  useContext,
  useReducer,
  useEffect,
  useState,
} from "react";
import {jwtDecode} from "jwt-decode";
import { api } from "../services/api";

const AuthContext = createContext(null);

// cleans up claims to neat strings
const normalizeClaims = (decoded) => ({
  email:
    decoded[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
    ],
  id:
    decoded[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
    ],
  role:
    decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
});

// finite state machine for authContext
export const authReducer = (state, action) => {
  switch (action.type) {
    case "LOGIN":
      return {
        ...state,
        user: action.payload,
        isAuthenticated: true,
      };
    case "REGISTER":
      return {
        ...state,
        user: action.payload,
        isAuthenticated: false,
      };
    case "LOGOUT":
      return {
        ...state,
        user: null,
        isAuthenticated: false,
      };
    default:
      return state;
  }
};

export const AuthProvider = ({ children }) => {
 
  const [state, dispatch] = useReducer(authReducer, {
    user: null,
    isAuthenticated: false,
  });


  const [loading, setLoading] = useState(true);


  const checkTokenValidity = () => {
    const token = localStorage.getItem("jwt");
    if (!token) return false;
    try {
      const decoded = jwtDecode(token);
      return decoded.exp * 1000 > Date.now();
    } catch (error) {
      return false;
    }
  };


  useEffect(() => {
    const init = async () => {
      const valid = checkTokenValidity();
      if (valid) {
        const token = localStorage.getItem("jwt");
        const decoded = jwtDecode(token);
        dispatch({ type: "LOGIN", payload: normalizeClaims(decoded) });
      }

      setLoading(false);
    };
    init();
  }, []);


  const login = async (credentials) => {
    try {
      const response = await api.post("users/login", credentials);
      const token = response.data.token;
      localStorage.setItem("jwt", token);
      const decoded = jwtDecode(token);
      dispatch({ type: "LOGIN", payload: normalizeClaims(decoded) });
      return true;
    } catch (err) {
      if (err.response && err.response.status === 401) {
        return false;
      }
      console.error("Login error:", err);
      return false;
    }
  };

  const logout = () => {
    localStorage.removeItem("jwt");
    dispatch({ type: "LOGOUT" });
  };

  const register = async (credentials) => {
    try {
      const response = await api.post("users/register", credentials);
      if (response.status >= 200 && response.status < 300) {
        return true;
      } else {
        console.error("Failed to register:", response.status, response.data);
        return false;
      }
    } catch (error) {
      console.error("Registration Error:", error.response ?? error.message);
      return false;
    }
  };

  return (
    <AuthContext.Provider
      value={{
        user: state.user,
        isAuthenticated: state.isAuthenticated,
        loading,  
        login,
        logout,
        register,
        checkTokenValidity,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
