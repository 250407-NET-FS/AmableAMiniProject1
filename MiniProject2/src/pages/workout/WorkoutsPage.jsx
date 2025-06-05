import React, { useState, useEffect } from "react";
import {
  Box,
  Button,
  TextField,
  Select,
  MenuItem,
  InputLabel,
  FormControl,
  Typography,
  Divider,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import RemoveCircleOutlineIcon from "@mui/icons-material/RemoveCircleOutline";
import { useTheme } from "@mui/material/styles";
import { api } from "../../services/api";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function WorkoutsPage() {
  const { user, loading } = useAuth();
  const navigate = useNavigate();
  const theme = useTheme();


  const [exercisesList, setExercisesList] = useState([]);


  const [workouts, setWorkouts] = useState([]);


  const [workoutName, setWorkoutName] = useState("");
  const todayISO = new Date().toISOString().split("T")[0];
  const [workoutDate, setWorkoutDate] = useState(todayISO);


  const [workoutExercises, setWorkoutExercises] = useState([
    { exerciseId: "", sets: "", reps: "", weight: "" },
  ]);


  const [formError, setFormError] = useState("");


  const [newExerciseDialogOpen, setNewExerciseDialogOpen] = useState(false);
  const [newExerciseName, setNewExerciseName] = useState("");
  const [creatingForRow, setCreatingForRow] = useState(null);


  useEffect(() => {
    if (!loading && !user) {
      navigate("/signin", { replace: true });
    }
  }, [loading, user, navigate]);


  useEffect(() => {

    if (loading || !user || !user.id) return;


    api
      .get("exercises")
      .then((res) => {
        const data = Array.isArray(res.data) ? res.data : [];
        setExercisesList(data);
      })
      .catch((err) => {
        console.error("Error fetching exercises:", err);
        setExercisesList([]);
      });


    api
      .get(`workouts/user/${user.id}`)
      .then((res) => {
        const data = Array.isArray(res.data) ? res.data : [];
        setWorkouts(data);
      })
      .catch((err) => {
        console.error("Error fetching workouts:", err);
        setWorkouts([]);
      });
  }, [loading, user]);


  const handleExerciseChange = (index, field, value) => {
    setWorkoutExercises((prev) => {
      const updated = [...prev];
      updated[index] = {
        ...updated[index],
        [field]: value,
      };
      return updated;
    });
  };

  const handleAddExerciseRow = () => {
    setWorkoutExercises((prev) => [
      ...prev,
      { exerciseId: "", sets: "", reps: "", weight: "" },
    ]);
  };

  const handleRemoveExerciseRow = (index) => {
    setWorkoutExercises((prev) => prev.filter((_, i) => i !== index));
  };


  const handleExerciseSelect = (index, value) => {
    if (value === "__new__") {
      setCreatingForRow(index);
      setNewExerciseName("");
      setNewExerciseDialogOpen(true);
    } else {
      handleExerciseChange(index, "exerciseId", value);
    }
  };


  const handleSaveNewExercise = async () => {
    const nameTrimmed = newExerciseName.trim();
    if (!nameTrimmed) return;

    try {
      const res = await api.post("exercises", { name: nameTrimmed });

      const newId = res.data;


      setExercisesList((prev) => [
        ...prev,
        { id: newId, name: nameTrimmed },
      ]);
      if (creatingForRow !== null) {
        handleExerciseChange(creatingForRow, "exerciseId", newId);
      }
      setNewExerciseDialogOpen(false);
      setCreatingForRow(null);
      setNewExerciseName("");
    } catch (err) {
      console.error("Error creating new exercise:", err);
    }
  };

  const handleCancelNewExercise = () => {
    setNewExerciseDialogOpen(false);
    setCreatingForRow(null);
    setNewExerciseName("");
  };


  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormError("");

    if (!workoutName.trim()) {
      setFormError("Workout name is required.");
      return;
    }

    for (let i = 0; i < workoutExercises.length; i++) {
      const row = workoutExercises[i];
      if (!row.exerciseId) {
        setFormError(`Please select an exercise for row ${i + 1}.`);
        return;
      }
      if (!row.sets || isNaN(row.sets) || Number(row.sets) <= 0) {
        setFormError(`Please enter valid “sets” for row ${i + 1}.`);
        return;
      }
      if (!row.reps || isNaN(row.reps) || Number(row.reps) <= 0) {
        setFormError(`Please enter valid “reps” for row ${i + 1}.`);
        return;
      }
      if (row.weight === "" || isNaN(row.weight) || Number(row.weight) < 0) {
        setFormError(`Please enter valid “weight” (≥ 0) for row ${i + 1}.`);
        return;
      }
    }

    const payload = {
      name: workoutName.trim(),
      date: new Date(workoutDate).toISOString(),
      workoutExercises: workoutExercises.map((row) => ({
        exerciseId: row.exerciseId,
        sets: Number(row.sets),
        reps: Number(row.reps),
        weight: Number(row.weight),
      })),
    };

    try {
      const response = await api.post("workouts", payload);
      if (response.status >= 200 && response.status < 300) {
        setWorkoutName("");
        setWorkoutDate(todayISO);
        setWorkoutExercises([
          { exerciseId: "", sets: "", reps: "", weight: "" },
        ]);


        if (user && user.id) {
          const res2 = await api.get(
            `workouts/user/${user.id}`
          );
          setWorkouts(Array.isArray(res2.data) ? res2.data : []);
        }
      } else {
        setFormError("Failed to save workout. Try again.");
      }
    } catch (err) {
      console.error("Error saving workout:", err);
      setFormError("An error occurred. Please try again.");
    }
  };


  const getExerciseName = (we) => {
    if (we.exercise && we.exercise.name) return we.exercise.name;
    const found = exercisesList.find((ex) => ex.id === we.exerciseId);
    return found ? found.name : "Unknown";
  };


  if (loading) {
    return null;
  }


  if (!user) {
    return null;
  }


  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="h4" gutterBottom>
        Create New Workout
      </Typography>

      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{
          display: "flex",
          flexDirection: "column",
          gap: 2,
          mb: 4,
        }}
      >
        {/* Workout Name */}
        <TextField
          label="Workout Name"
          value={workoutName}
          onChange={(e) => setWorkoutName(e.target.value)}
          required
        />

        {/* Workout Date */}
        <TextField
          label="Date"
          type="date"
          value={workoutDate}
          onChange={(e) => setWorkoutDate(e.target.value)}
          InputLabelProps={{ shrink: true }}
          required
        />

        <Divider />

        <Typography variant="h6">Exercises</Typography>

        {workoutExercises.map((row, idx) => (
          <Box
            key={idx}
            sx={{
              display: "grid",
              gridTemplateColumns: "1fr 0.8fr 0.8fr 0.8fr auto",
              gap: 1,
              alignItems: "center",
            }}
          >
            {/*Exercise Select */}
            <FormControl fullWidth>
              <InputLabel id={`exercise-label-${idx}`}>Exercise</InputLabel>
              <Select
                labelId={`exercise-label-${idx}`}
                label="Exercise"
                value={row.exerciseId}
                onChange={(e) =>
                  handleExerciseSelect(idx, e.target.value)
                }
                required
              >
                <MenuItem value="">
                  <em>Select…</em>
                </MenuItem>
                {exercisesList.map((ex) => (
                  <MenuItem key={ex.id} value={ex.id}>
                    {ex.name}
                  </MenuItem>
                ))}
                <Divider />
                <MenuItem value="__new__">
                  <em>Create new exercise…</em>
                </MenuItem>
              </Select>
            </FormControl>

            {/*Sets */}
            <TextField
              label="Sets"
              type="number"
              value={row.sets}
              onChange={(e) =>
                handleExerciseChange(idx, "sets", e.target.value)
              }
              inputProps={{ min: 1 }}
              required
            />

            {/*Reps */}
            <TextField
              label="Reps"
              type="number"
              value={row.reps}
              onChange={(e) =>
                handleExerciseChange(idx, "reps", e.target.value)
              }
              inputProps={{ min: 1 }}
              required
            />

            {/*Weight */}
            <TextField
              label="Weight (lbs)"
              type="number"
              value={row.weight}
              onChange={(e) =>
                handleExerciseChange(idx, "weight", e.target.value)
              }
              inputProps={{ min: 0 }}
              required
            />

            {/*Remove Row Button */}
            <IconButton
              color="error"
              onClick={() => handleRemoveExerciseRow(idx)}
              disabled={workoutExercises.length === 1}
            >
              <RemoveCircleOutlineIcon />
            </IconButton>
          </Box>
        ))}

        {/* Add Another Exercise */}
        <Box>
          <Button
            startIcon={<AddCircleOutlineIcon />}
            onClick={handleAddExerciseRow}
          >
            Add Another Exercise
          </Button>
        </Box>

        {formError && (
          <Typography color="error" variant="body2">
            {formError}
          </Typography>
        )}

        <Button type="submit" variant="contained">
          Save Workout
        </Button>
      </Box>

      <Divider sx={{ my: 4 }} />


      {/* EXISTING WORKOUTS BOTTOM OF PAGE */}
      <Typography variant="h4" gutterBottom>
        Existing Workouts
      </Typography>

      {workouts.length === 0 ? (
        <Typography>No workouts saved yet.</Typography>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead >
              <TableRow>
                <TableCell padding="">Workout Name</TableCell>
                <TableCell>Date</TableCell>
                <TableCell>Exercises (sets X reps @ weight)</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {workouts.map((w) => (
                <TableRow key={w.id}>
                  <TableCell>{w.name}</TableCell>
                  <TableCell>
                    {new Date(w.date).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    {w.workoutExercises && w.workoutExercises.length ? (
                      w.workoutExercises.map((we, i) => {
                        const exName = getExerciseName(we);
                        return (
                          <Typography key={i} variant="body2">
                            {exName}: {we.sets}×{we.reps} @ {we.weight} lbs
                          </Typography>
                        );
                      })
                    ) : (
                      "—"
                    )}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <Dialog
        open={newExerciseDialogOpen}
        onClose={handleCancelNewExercise}
      >
        <DialogTitle>Create a New Exercise</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Exercise Name"
            fullWidth
            value={newExerciseName}
            onChange={(e) => setNewExerciseName(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancelNewExercise}>Cancel</Button>
          <Button
            variant="contained"
            onClick={handleSaveNewExercise}
            disabled={!newExerciseName.trim()}
          >
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
