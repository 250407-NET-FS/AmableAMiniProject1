import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
const RequireAdmin = ({ children }) => {

  const { user } = useAuth();
  console.log("User in RequireAdmin:", user);


    if (user === null) {
    return <div>Loading</div>; //Otherwise returns early in case of reload
  }

  if (!user || user.role !== "Admin") {
    return <Navigate to="/" replace />; //https://stackoverflow.com/questions/72794430/what-does-usenavigate-replace-option-do
  }

  return children;
};

export default RequireAdmin;
