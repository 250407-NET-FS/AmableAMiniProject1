// src/pages/admin/UserList.jsx
import React, { useEffect, useState } from "react";
import { api } from "../../services/api";
import { useAuth } from "../../context/AuthContext";
import {
  Box,
  Grid,
  Card,
  CardContent,
  CardActions,
  Typography,
  Button,
  CircularProgress,
} from "@mui/material";

export default function UserList() {
  const [users, setUsers] = useState([]);
  const { user: admin } = useAuth();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api
      .get("admin")
      .then((res) => {
        setUsers(res.data);
      })
      .catch((err) => console.error("Error fetching user list:", err))
      .finally(() => setLoading(false));
  }, []);

  const banHandler = (userId) => {
    api
      .post(`admin/ban/${userId}`)
      .then((res) => {
        const updatedUser = res.data;
        setUsers((prev) =>
          prev.map((u) =>
            u.id === updatedUser.id ? { ...u, isLockedOut: true } : u
          )
        );
      })
      .catch((err) => console.error("Ban failed:", err));
  };

  const unbanHandler = (userId) => {
    api
      .post(`admin/unban/${userId}`)
      .then((res) => {
        const updatedUser = res.data;
        setUsers((prev) =>
          prev.map((u) =>
            u.id === updatedUser.id ? { ...u, isLockedOut: false } : u
          )
        );
      })
      .catch((err) => console.error("Unban failed:", err));
  };

  const deleteHandler = (userId) => {
    api
      .delete(`admin/${userId}`)
      .then(() => {
        setUsers((prev) => prev.filter((u) => u.id !== userId));
      })
      .catch((err) => console.error("Delete failed:", err));
  };

  if (loading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="h4" align="center" gutterBottom>
        User List
      </Typography>

      {users.length === 0 ? (
        <Typography align="center">No users found.</Typography>
      ) : (
        <Grid container spacing={2}>
          {users.map((u) => (
            <Grid key={u.id}>
              <Card variant="outlined" sx={{ height: "100%", display: "flex", flexDirection: "column" }}>
                <CardContent>
                  <Typography variant="h6" gutterBottom noWrap>
                    {u.userName || u.email}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    ID: {u.id}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Email: {u.email}
                  </Typography>
                  <Typography variant="body2" sx={{ mt: 1 }}>
                    Status:{" "}
                    {u.isLockedOut ? (
                      <Typography component="span" color="error">
                        Banned
                      </Typography>
                    ) : (
                      <Typography component="span" color="success.main">
                        Active
                      </Typography>
                    )}
                  </Typography>
                </CardContent>

                <Box sx={{ flexGrow: 1 }} />

                <CardActions>
                  {!admin || u.id !== admin.id ? (
                    <>
                      {u.isLockedOut ? (
                        <Button size="small" onClick={() => unbanHandler(u.id)}>
                          Unban
                        </Button>
                      ) : (
                        <Button size="small" onClick={() => banHandler(u.id)}>
                          Ban
                        </Button>
                      )}
                      <Button
                        size="small"
                        color="error"
                        onClick={() => deleteHandler(u.id)}
                      >
                        Delete
                      </Button>
                    </>
                  ) : (
                    <Typography
                      variant="caption"
                      color="text.secondary"
                      sx={{ ml: 1 }}
                    >
                      (You)
                    </Typography>
                  )}
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}
    </Box>
  );
}
