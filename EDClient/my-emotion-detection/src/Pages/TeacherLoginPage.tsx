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
import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { NotificationMessage } from "../Components/NotificationMessage";
import { Lesson } from "../Objects/Lesson";
import { pathHome, pathTeacherDashBoard } from "../Paths";
import { serverLogin } from "../Services/ClientService";
import { setUsername } from "../Services/SessionService";
import { formatMessage, mainTheme, squaresColor } from "../Utils";

const TeacherLogin: React.FC = () => {
  const [showMessage, setShowMessage] = useState(false);
  const [messageContent, setMessageContent] = useState('');
  const [messageType, setMessageType] = useState<'error' | 'success'>('error');

  const handleClose = () => {
    setShowMessage(false);
  };

  const displayMessage = (message: string, type: 'error' | 'success') => {
    setMessageContent(formatMessage(message));
    setMessageType(type);
    setShowMessage(true);
  };

  const lessonCookie = Cookies.get('TeacherLesson');
  const lesson: Lesson | null = lessonCookie ? JSON.parse(lessonCookie) : null;

  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const email = data.get("email")?.toString();
    const password = data.get("password")?.toString();

    serverLogin(email, password).then(() => {
      setUsername(String(email));
      navigate(pathTeacherDashBoard);
    }).catch((e) => displayMessage(e, 'error'));
  };

  return (
    <ThemeProvider theme={mainTheme}>

      
        <Grid           item
          xs={false}
          sm={6}
          md={7} />
        <Grid
          item
          xs={12}
          sm={6}
          md={5}
          component={Box}
          display="flex"
          alignItems="center"
          justifyContent="center"
          sx={{
            height: "90vh",
            p: 4,
          }}
        >
          <Box
            sx={{
              width: "100%",
              maxWidth: 400,
              my: 8,
              mx: 4,
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              boxShadow: 3,
              p: 4,
              backgroundColor: "white",
              borderRadius: 2,
            }}
          >
            <Typography component="h1" variant="h5">
              Login To Your Teacher Account
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
                label="Email"
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
                label="Password"
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
                  color: "white",
                  backgroundColor: "#1976d2",
                  "&:hover": { backgroundColor: "#1565c0" },
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
                  backgroundColor: "#fff",
                  "&:hover": { backgroundColor: squaresColor },
                }}
              >
                Back
              </Button>
              <Grid container justifyContent="center">
                <Grid item>
                  <Link href="/register" variant="body2">
                    {"Don't have an account? Sign Up"}
                  </Link>
                </Grid>
              </Grid>
            </Box>
          </Box>
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
};

export default TeacherLogin;
