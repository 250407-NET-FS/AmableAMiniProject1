
import React from "react";
import "@testing-library/jest-dom";
import { render, screen, fireEvent, waitFor, act } from "@testing-library/react";
import WorkoutsPage from "../src/pages/workout/WorkoutsPage"; 
import { api } from "../src/services/api";
import { useAuth } from "../src/context/AuthContext";
import { useNavigate } from "react-router-dom";


jest.mock("../src/services/api");
jest.mock("../src/context/AuthContext", () => ({
  useAuth: jest.fn(),
}));
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(),
}));

describe("WorkoutsPage – basic behaviors", () => {
  let mockNavigate;
  
  beforeEach(() => {

    jest.clearAllMocks();
    mockNavigate = jest.fn();
    useNavigate.mockReturnValue(mockNavigate);
  });

  it("1. redirects to /signin when user is null and loading is false", async () => {

    useAuth.mockReturnValue({ user: null, loading: false });
    
     //Act
    await act(async () => {
      render(<WorkoutsPage />);
    });
    
     //Assert: 
    expect(mockNavigate).toHaveBeenCalledWith("/signin", { replace: true });
  });

  it("2. shows “No workouts saved yet.” when there are no workouts for a valid user", async () => {
    // Arrange

    useAuth.mockReturnValue({ user: { id: "user-1", fullName: "Test User" }, loading: false });
    
     - api.get("exercises")
    api.get.mockImplementation((url) => {
      if (url === "exercises") {
        return Promise.resolve({ data: [] });
      }
      if (url === `workouts/user/user-1`) {
        return Promise.resolve({ data: [] });
      }
      return Promise.resolve({ data: [] });
    });

     //Act
    await act(async () => {
      render(<WorkoutsPage />);
    });

    await waitFor(() => {
      expect(screen.getByText("Existing Workouts")).toBeInTheDocument();
    });

    //Assert
    expect(screen.getByText("No workouts saved yet.")).toBeInTheDocument();
  });

  it("3. clicking “Add Another Exercise” adds a new exercise row", async () => {
     //Arrange

    useAuth.mockReturnValue({ user: { id: "user-1" }, loading: false });
     - api.get
    api.get.mockResolvedValue({ data: [] });

     //Act
    await act(async () => {
      render(<WorkoutsPage />);
    });

    const selectsBefore = screen.getAllByLabelText("Exercise");
    expect(selectsBefore.length).toBe(1);

    const addButton = screen.getByRole("button", { name: /add another exercise/i });
    fireEvent.click(addButton);

    const selectsAfter = screen.getAllByLabelText("Exercise");
    expect(selectsAfter.length).toBe(2);
  });
});
