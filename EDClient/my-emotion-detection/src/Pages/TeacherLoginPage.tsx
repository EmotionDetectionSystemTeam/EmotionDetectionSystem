import {
  Box,
  Button,
  Grid,
  Link,
  TextField,
  ThemeProvider,
  Typography
} from "@mui/material";
import Cookies from 'js-cookie';
import React from 'react';
import { useNavigate } from "react-router-dom";
import { NotificationMessage } from "../Components/NotificationMessage";
import { Lesson } from "../Objects/Lesson";
import { pathHome, pathTeacherDashBoard } from "../Paths";
import { serverLogin } from "../Services/ClientService";
import { setUsername } from "../Services/SessionService";
import { formatMessage, mainTheme, squaresColor } from "../Utils";





  
  function TeacherLogin() {
    const [showMessage, setShowMessage] = React.useState(false);
    const [messageContent, setMessageContent] = React.useState('');
    const [messageType, setMessageType] = React.useState<'error' | 'success'>('error');
  
  
    const handleClose = () => {
      setShowMessage(false);
    };
  
    const displayMessage = (message: string, type: 'error' | 'success') => {
      setMessageContent(formatMessage(message));
      setMessageType(type);
      setShowMessage(true);
    };

    const lessonCookie = Cookies.get('TeacherLesson');
    const lesson : Lesson = lessonCookie ? JSON.parse(lessonCookie) : null;
    //alert(lesson.LessonId);
  
    const navigate = useNavigate();


  
    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        /*
      if (!sessionService.getIsGuest()) {
        alert("You are already logged in!\n");
        return;
      }
      */
      event.preventDefault();
      const data = new FormData(event.currentTarget);
      const email = data.get("email")?.toString();
      const password = data.get("password")?.toString();

      serverLogin(email, password).then(() => {
        // const address = `ws://127.0.0.1:4560/${email}-notifications`;
        //initWebSocket(address);
        setUsername(String(email));
        navigate(pathTeacherDashBoard);
        }).catch((e) => displayMessage(e,'error'));
    };
  
    return (
      <ThemeProvider theme={mainTheme}>
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
                Login To Your Account
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
                  id="email"
                  label="email"
                  name="email"
                  autoComplete="email"
                  autoFocus
                  variant="outlined"
                />
                <TextField
                  margin="normal"
                  required
                  fullWidth
                  name="password"
                  label="password"
                  type="password"
                  id="password"
                  autoComplete="current-password"
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
                  Sign In
                </Button>
                <Button
                  fullWidth
                  variant="contained"
                  href={pathHome}
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
                <Grid item>
                  <Link href="/register" variant="body2">
                    {"Don't have an account? Sign Up"}
                  </Link>
                </Grid>
              </Box>
            </Box>
          </Grid>
        </Grid>
        {showMessage && (
        <NotificationMessage
          message={messageContent}
          onClose={handleClose}
          type={messageType}
        />
      )}
      </ThemeProvider>
    );
  }
  
  export default TeacherLogin;
  