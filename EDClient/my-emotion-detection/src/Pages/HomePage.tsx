import { Box, Button, Container, Paper, ThemeProvider, Typography, createTheme } from "@mui/material";
import React, { useEffect, useState } from 'react';
import { NotificationMessage } from "../Components/NotificationMessage";
import * as Paths from "../Paths";
import { serverEnterAsGuest } from "../Services/ClientService";
import { getIsInitOccured, initSession } from "../Services/SessionService";
import { formatMessage } from "../Utils";
import logo from "../assets/FrontLogo.png";

const mainTheme = createTheme({
  palette: {
    
    primary: {
      main: '#1976d2',
    },
    background: {
      default: '#f5f5f5',
    },
  },
  
  typography: {
    fontFamily: [
      'Roboto',
      'Arial',
      'sans-serif',
    ].join(','),
  },
});

const buttonStyle = {
  width: '100%',
  py: 2,
  mb: 2,
  fontSize: '1.1rem',
  fontWeight: 'bold',
  borderRadius: 2,
  textTransform: 'none',
};

function HomePage() {
  const [showMessage, setShowMessage] = useState(false);
  const [messageContent, setMessageContent] = useState('');
  const [messageType, setMessageType] = useState('error');

  useEffect(() => {
    if (!getIsInitOccured()) {
      serverEnterAsGuest()
        .then((sessionId) => {
          initSession(sessionId);
          displayMessage("Connected to server successfully!", 'success');
        })
        .catch((e) => {
          displayMessage(e, 'error');
        });
    }
  }, []);

  const handleClose = () => setShowMessage(false);

  const displayMessage = (message, type) => {
    setMessageContent(formatMessage(message));
    setMessageType(type);
    setShowMessage(true);
  };

  return (
    <ThemeProvider theme={mainTheme}>
      
      <Box
        sx={{
          minHeight: '100vh',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
        }}
      >
        <Container maxWidth="sm">
          <Paper
            elevation={3}
            sx={{
              p: 4,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              bgcolor: 'white',
              borderRadius: 4,
            }}
          >
            <Box
              component="img"
              src={logo}
              alt="Logo"
              sx={{
                width: '70%',
                maxWidth: 300,
                mb: 4,
                bgcolor: 'white',
                p: 2,
                borderRadius: 2,
              }}
            />
            <Typography variant="h4" component="h1" gutterBottom sx={{ fontWeight: 'bold', color: 'primary.main' }}>
              Welcome to Our Platform
            </Typography>
            <Typography variant="body1" gutterBottom sx={{ mb: 3, textAlign: 'center' }}>
              Choose your role to get started with our class management system.
            </Typography>
            <Box sx={{ width: '100%' }}>
              <Button
                href={Paths.pathTeacherLogin}
                variant="outlined"
                color="primary"
                sx={buttonStyle}
              >
                Enter as Teacher
              </Button>
              <Button
                href={Paths.pathStudentLogin}
                variant="outlined"
                color="primary"
                sx={buttonStyle}
              >
                Enter as Student
              </Button>
              <Button
                href={Paths.pathRegister}
                variant="outlined"
                color="primary"
                sx={buttonStyle}
              >
                Register
              </Button>
            </Box>
          </Paper>
        </Container>
        {showMessage && (
          <NotificationMessage
            message={messageContent}
            onClose={handleClose}
            type={messageType}
          />
        )}
      </Box>
    </ThemeProvider>
  );
}

export default HomePage;