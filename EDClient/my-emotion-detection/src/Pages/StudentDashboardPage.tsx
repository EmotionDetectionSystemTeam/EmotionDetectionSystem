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
import { pathHome, pathStudentLesson } from "../Paths";
import { serverLogout } from "../Services/ClientService";
import { squaresColor } from "../Utils";

    
    function StudentDashboard() {
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
        const classCode = data.get("classCode")?.toString();
        navigate(pathStudentLesson);
        /*
        serverJoinLesson(classCode).then((message : string) => {
          navigate(pathStudentLesson);
          alert(message);
          }).catch((e) => alert(e));
          */
      };

      const handleLogout = () => {
        serverLogout().then((message : string) => {
            alert(message);
            navigate(pathHome);
        }).catch((e) => alert(e))
        
      }
    
      return (
        <ThemeProvider theme={theme}>
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
                  Student Dash Board
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
                    id="classCode"
                    label="classCode"
                    name="classCode"
                    autoComplete="classCode"
                    autoFocus
                    variant="outlined"
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
                    Enter Lesson Code
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
                    Back
                  </Button>
                </Box>
              </Box>
            </Grid>
          </Grid>
        </ThemeProvider>
      );
    }
    
    export default StudentDashboard;
    