import {
  Box,
  Button,
  Grid,
  TextField,
  ThemeProvider,
  Typography,
  createTheme
} from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from "../Components/Navbar";
import { Lesson } from "../Objects/Lesson";
import { pathHome, pathTeacherLesson } from "../Paths";
import { serverCreateLesson, serverLogout } from "../Services/ClientService";
import { getSessionId, setCookie, setLessonId } from "../Services/SessionService";
import { squaresColor } from "../Utils";


  
  function TeacherDashboard() {
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
  
    const navigate = useNavigate();
  
    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {

      event.preventDefault();
      const data = new FormData(event.currentTarget);
      const title = data.get("title")?.toString();
      const description = data.get("description")?.toString();

      serverCreateLesson(title, description,[]).then((lesson : Lesson) => { //TODO: tags impl 
        //Cookies.set('TeacherLesson', JSON.stringify(lesson));
        setLessonId(lesson.LessonId);
        setCookie(getSessionId(), 'lesson', lesson);
        alert(lesson.EntryCode);
        navigate(pathTeacherLesson);
        }).catch((e) => alert(e));
    };

    const handleLogout = () => {
      serverLogout().then((message : string) => {
          alert(message);
          navigate(pathHome);
      }).catch((e) => alert(e))
      
    }
  
    return (
      <ThemeProvider theme={theme}>
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
                Teacher Dash Board
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
                    backgroundColor: "#fff	",
                    "&:hover": { backgroundColor: squaresColor },
                  }}
                >
                  Create New Lesson
                </Button>
                <Button
                  fullWidth
                  variant="contained"
                  onClick={() => {
                      handleLogout();
                    }}
                  sx={{
                    mt: 3,
                    mb: 2,
                    color: "black",
                    backgroundColor: "#fff	",
                    "&:hover": { backgroundColor: squaresColor },

                  }}
                >
                  Log Out
                </Button>
              </Box>
            </Box>
          </Grid>
        </Grid>
      </ThemeProvider>
    );
  }
  
  export default TeacherDashboard;
  