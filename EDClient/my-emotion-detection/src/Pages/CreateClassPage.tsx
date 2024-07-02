import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import {
  Box,
  Button,
  Container,
  Paper,
  TextField,
  ThemeProvider,
  Typography
} from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from '../Components/Navbar';
import { pathTeacherDashBoard, pathTeacherLesson } from "../Paths";
import { serverCreateLesson } from "../Services/ClientService";
import { getSessionId, setCookie, setLessonId } from "../Services/SessionService";
import { mainTheme, squaresColor } from "../Utils";

function CreateClass() {
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const title = data.get("title")?.toString();
    const description = data.get("description")?.toString();
    const tags = data.get("tags")?.toString().split(',').map(tag => tag.trim());

    try {
      const lesson = await serverCreateLesson(title, description, tags);
      setLessonId(lesson.LessonId);
      setCookie(getSessionId(), 'lesson', lesson);
      navigate(pathTeacherLesson);
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
            Create New Lesson
          </Typography>
          <Typography variant="body1" sx={{ mb: 3 }}>
            Fill in the details for your new lesson.
          </Typography>
          <Box component="form" noValidate onSubmit={handleSubmit} sx={{ width: '100%' }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="title"
              label="Title"
              name="title"
              autoComplete="off"
              variant="outlined"
            />
            <TextField
              margin="normal"
              required
              fullWidth
              id="description"
              label="Description"
              name="description"
              autoComplete="off"
              variant="outlined"
              multiline
              rows={4}
            />
            <TextField
              margin="normal"
              required
              fullWidth
              id="tags"
              label="Tags (comma-separated)"
              name="tags"
              autoComplete="off"
              variant="outlined"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              startIcon={<AddCircleOutlineIcon />}
              sx={{
                mt: 3,
                mb: 2,
                color: "white",
                backgroundColor: mainTheme.palette.primary.main,
                "&:hover": { backgroundColor: mainTheme.palette.primary.dark },
              }}
            >
              Create New Lesson
            </Button>
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
        </Paper>
      </Container>
    </ThemeProvider>
  );
}

export default CreateClass;