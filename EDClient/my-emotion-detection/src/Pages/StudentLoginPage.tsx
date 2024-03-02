import {
  Box,
  Button,
  Grid,
  Link,
  TextField,
  ThemeProvider,
  Typography,
  createTheme,
} from "@mui/material";
import React from 'react';
import { useNavigate } from "react-router-dom";
import { pathHome, pathStudentDashBoard } from "../Paths";
import { serverLogin } from "../Services/ClientService";
import { initWebSocket } from "../Services/NotificationService";
import { squaresColor } from "../Utils";



  
  function StudentLogin() {
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

      const serverResponse = await serverLogin(email, password).then(() => {
        const address = `ws://127.0.0.1:4560/${email}-notifications`;
        initWebSocket(address);
        navigate(pathStudentDashBoard);
        alert(`${email} DashBoard is not ready yet!`);
        }).catch((e) => alert(e));
    };
  
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
      </ThemeProvider>
    );
  }
  
  export default StudentLogin;
  