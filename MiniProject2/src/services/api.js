// Importing axios for making HTTP requests
import axios from "axios";

// Importing toast from react-toastify for displaying notifications
// This library is used to show toast notifications in the application
// It provides a simple way to display messages to the user
// https://www.npmjs.com/package/react-toastify
import { toast } from "react-toastify";

// Create Axios instance with base URL
// This instance will be used to make API calls to the .NET backend
const baseURL = import.meta.env.DEV
  ? "http://localhost:5136/api" // Local .NET backend
  : "https://jazaback-gndrbbftb8fchyf2.canadaeast-01.azurewebsites.net/api";
export const api = axios.create({
  baseURL, // .NET API base URL
});

// Request interceptor to attach JWT token to every request
// This interceptor will run before every request made with the Axios instance
// It checks if a JWT token is present in local storage
// and attaches it to the Authorization header of the request
api.interceptors.request.use((config) => {

  // Get JWT from localStorage
  const token = localStorage.getItem("jwt");

  if (token) {
    // Attach to Authorization header as Bearer token
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});


// Response interceptor to handle errors globally
api.interceptors.response.use(
  (response) => response, // Pass through successful responses
  (error) => {
    // Show error toast using react-toastify
    toast.error(error.response?.data?.message || "An error occurred");
    return Promise.reject(error);
  },
);