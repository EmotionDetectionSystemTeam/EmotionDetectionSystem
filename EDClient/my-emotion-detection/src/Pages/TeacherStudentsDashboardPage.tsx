
import { Box, Button, ThemeProvider, Typography, createTheme } from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from "../Components/Navbar";
import { pathTeacherDashBoard } from "../Paths";
import { squaresColor } from "../Utils";

const theme = createTheme({
  typography: {
    fontFamily: [
      "-apple-system",
      "BlinkMacSystemFont",
      '"Segoe UI"',
      "Roboto",
      '"Helvetica Neue"',
      "Arial",
      "sans-serif",
      '"Apple Color Emoji"',
      '"Segoe UI Emoji"',
      '"Segoe UI Symbol"',
    ].join(","),
  },
});

function StudentsHistory() {
  const navigate = useNavigate();

  return (
    <ThemeProvider theme={theme}>
      <Box>
        <Navbar />
      </Box>
      <Box
        sx={{
          my: 8,
          mx: 4,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Typography component="h1" variant="h5">
          Students Dashboard
        </Typography>
        <Button
          fullWidth
          variant="contained"
          onClick={() => navigate(pathTeacherDashBoard)}
          sx={{
            mt: 3,
            mb: 2,
            color: "black",
            backgroundColor: "#fff",
            "&:hover": { backgroundColor: squaresColor },
          }}
        >
          Back to Dashboard
        </Button>
      </Box>
    </ThemeProvider>
  );
}

export default StudentsHistory;
