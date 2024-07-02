import ClassIcon from '@mui/icons-material/Class';
import { Box, Button, Container, Paper, TextField, ThemeProvider, Typography } from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from '../Components/Navbar';
import { pathHome, pathStudentLesson } from "../Paths";
import { serverJoinLesson, serverLogout } from "../Services/ClientService";
import { setLessonId, setLessonTeacher } from "../Services/SessionService";
import { mainTheme, squaresColor } from "../Utils";

function StudentDashboard() {
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const classCode = data.get("classCode")?.toString();
    
    try {
      const lesson = await serverJoinLesson(classCode);
      setLessonId(lesson.LessonId);
      setLessonTeacher(lesson.Teacher);
      navigate(pathStudentLesson);
    } catch (e) {
      alert(e);
    }
  };

  const handleLogout = async () => {
    try {
      await serverLogout();
      navigate(pathHome);
    } catch (e) {
      alert(e);
    }
  };

  return (
    <ThemeProvider theme={mainTheme}>
            <Box>
        <Navbar />
      </Box>
      <Container component="main" maxWidth="sm" sx={{ mt: 8 }}>
        <Paper elevation={3} sx={{ p: 4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
          <Typography component="h1" variant="h4" gutterBottom>
            Welcome, Student!
          </Typography>
          <Typography variant="body1" sx={{ mb: 3 }}>
            Enter your class code to join a lesson.
          </Typography>
          <Box component="form" onSubmit={handleSubmit} noValidate sx={{ width: '100%' }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="classCode"
              label="Class Code"
              name="classCode"
              autoComplete="off"
              autoFocus
              variant="outlined"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{
                mt: 2,
                py: 1.5,
                color: "black",
                backgroundColor: "#fff",
                "&:hover": { backgroundColor: squaresColor },
              }}
              startIcon={<ClassIcon />}
            >
              Join Lesson
            </Button>
          </Box>
        </Paper>
      </Container>
    </ThemeProvider>
  );
}

export default StudentDashboard;