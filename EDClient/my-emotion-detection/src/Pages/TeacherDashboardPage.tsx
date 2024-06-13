import {
  Box,
  Button,
  Grid,
  ThemeProvider,
  Typography
} from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from "../Components/Navbar";
import { pathClassesDashboard, pathCreateClass, pathHome, pathStudentsHistory } from "../Paths";
import { serverLogout } from "../Services/ClientService";
import { mainTheme, squaresColor } from "../Utils";


function TeacherDashboard() {
  const navigate = useNavigate();

  const handleLogout = () => {
    serverLogout().then((message: string) => {
      navigate(pathHome);
    }).catch((e) => alert(e));
  };

  return (
    <ThemeProvider theme={mainTheme}>
      <Box>
        <Navbar />
      </Box>
      <Grid
        container
        component="main"
        sx={{
          height: "90vh",
        }}
      >
        <Grid item xs={false} sm={6} md={7} />
        <Grid
          sx={{
            height: "90vh",
          }}
        >
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
              Teacher Dashboard
            </Typography>
            <Button
              fullWidth
              variant="contained"
              onClick={() => navigate(pathCreateClass)}
              sx={{
                mt: 3,
                mb: 2,
                color: "black",
                backgroundColor: "#fff",
                "&:hover": { backgroundColor: squaresColor },
              }}
            >
              Create Class
            </Button>
            <Button
              fullWidth
              variant="contained"
              onClick={() => navigate(pathStudentsHistory)}
              sx={{
                mt: 3,
                mb: 2,
                color: "black",
                backgroundColor: "#fff",
                "&:hover": { backgroundColor: squaresColor },
              }}
            >
              Students Dashboard
            </Button>
            <Button
              fullWidth
              variant="contained"
              onClick={() => navigate(pathClassesDashboard)}
              sx={{
                mt: 3,
                mb: 2,
                color: "black",
                backgroundColor: "#fff",
                "&:hover": { backgroundColor: squaresColor },
              }}
            >
              Classes Dashboard
            </Button>
            <Button
              fullWidth
              variant="contained"
              onClick={() => handleLogout()}
              sx={{
                mt: 3,
                mb: 2,
                color: "black",
                backgroundColor: "#fff",
                "&:hover": { backgroundColor: squaresColor },
              }}
            >
              Log Out
            </Button>
          </Box>
        </Grid>
      </Grid>
    </ThemeProvider>
  );
}

export default TeacherDashboard;
