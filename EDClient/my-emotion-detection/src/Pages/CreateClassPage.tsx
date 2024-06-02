import {
  Box,
  Button,
  TextField,
  ThemeProvider,
  Typography
} from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from "../Components/Navbar";
import { Lesson } from "../Objects/Lesson";
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

    serverCreateLesson(title, description, []).then((lesson: Lesson) => { // TODO: tags implementation
      setLessonId(lesson.LessonId);
      setCookie(getSessionId(), 'lesson', lesson);
      alert(lesson.EntryCode);
      navigate(pathTeacherLesson);
    }).catch((e) => alert(e));
  };

  return (
    <ThemeProvider theme={mainTheme}>
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
          Create New Lesson
        </Typography>
        <Box
          component="form"
          noValidate
          onSubmit={handleSubmit}
          sx={{
            mt: 1,
          }}
        >
          <TextField
            margin="normal"
            required
            fullWidth
            id="title"
            label="Title"
            name="title"
            autoComplete="title"
            variant="outlined"
            type="text"
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="description"
            label="Description"
            name="description"
            autoComplete="description"
            variant="outlined"
            type="text"
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="tags"
            label="Tags"
            name="tags"
            autoComplete="tags"
            variant="outlined"
            type="text"
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{
              mt: 3,
              mb: 2,
              color: "black",
              backgroundColor: "#fff",
              "&:hover": { backgroundColor: squaresColor },
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
      </Box>
    </ThemeProvider>
  );
}

export default CreateClass;
