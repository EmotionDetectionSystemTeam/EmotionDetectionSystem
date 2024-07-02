import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import ClassIcon from '@mui/icons-material/Class';
import PeopleIcon from '@mui/icons-material/People';
import { Box, Button, Container, Paper, ThemeProvider, Typography } from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from '../Components/Navbar';
import { pathClassesDashboard, pathCreateClass, pathHome, pathStudentsHistory } from "../Paths";
import { serverLogout } from "../Services/ClientService";
import { mainTheme, squaresColor } from "../Utils";

function TeacherDashboard() {
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await serverLogout();
      navigate(pathHome);
    } catch (e) {
      alert(e);
    }
  };

  const buttonStyle = {

    mt: 2,
    py: 1.5,
    color: "black",
    backgroundColor: "#fff",
    "&:hover": { backgroundColor: squaresColor },
  };

  return (
    <ThemeProvider theme={mainTheme}>
            <Box>
        <Navbar />
      </Box>
      <Container component="main" maxWidth="sm" sx={{ mt: 8 }}>
        <Paper elevation={3} sx={{ p: 4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
          <Typography component="h1" variant="h4" gutterBottom>
            Welcome, Teacher!
          </Typography>
          <Typography variant="body1" sx={{ mb: 3 }}>
            What would you like to do today?
          </Typography>
          <Box sx={{ width: '100%' }}>
            <Button
              fullWidth
              variant="contained"
              onClick={() => navigate(pathCreateClass)}
              startIcon={<AddCircleOutlineIcon />}
              sx={buttonStyle}
            >
              Create Class
            </Button>
            <Button
              fullWidth
              variant="contained"
              onClick={() => navigate(pathStudentsHistory)}
              startIcon={<PeopleIcon />}
              sx={buttonStyle}
            >
              Students Dashboard
            </Button>
            <Button
              fullWidth
              variant="contained"
              onClick={() => navigate(pathClassesDashboard)}
              startIcon={<ClassIcon />}
              sx={buttonStyle}
            >
              Classes Dashboard
            </Button>
          </Box>
        </Paper>
      </Container>
    </ThemeProvider>
  );
}

export default TeacherDashboard;