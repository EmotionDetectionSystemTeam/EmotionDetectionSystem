import { Button, ButtonGroup, ThemeProvider, createTheme } from "@mui/material";
import React from 'react';
import { NotificationMessage } from "../Components/NotificationMessage";
import * as Paths from "../Paths";
import { serverEnterAsGuest } from "../Services/ClientService";
import { getIsInitOccured, initSession } from "../Services/SessionService";
import { formatMessage, textColor } from "../Utils";
import logo from "../assets/FrontLogo.png";

const createButton = (name: string, path: string) => {
  return (
    <Button
      href={path}
      style={{ height: 100, width: 500 }}
      key="name"
      variant="outlined"
      size="large"
      color="primary"
      sx={{
        background: "#E8F0FE",
        m: 1,
        color: textColor,
        "&:hover": {
          borderRadius: 5,
          color: textColor,
        },
      }}
    >
      {name}
    </Button>
  );
};

const mainTheme = createTheme({
  palette: {
    mode: "dark",
    background: {
      default: '"#E8F0FE"', // Set the default background color
    },
  },
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

const buttons = [
    createButton("Enter as Teacher", Paths.pathTeacherLogin),
    createButton("Enter as Student", Paths.pathStudentLogin),
    createButton("Register", Paths.pathRegister),
];

function HomePage() {
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


  getIsInitOccured() ? null : serverEnterAsGuest().then((sessionId: string) => {
    initSession(sessionId);
    //initializeCookie(sessionId);
    displayMessage("Connected to server successfuly!", 'success');
  })
  .catch((e) => {
    displayMessage(e, 'error');
  });


  return (
    <ThemeProvider theme={mainTheme}>
      <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
          <img src={logo} alt="Logo" style={{ marginBottom: 40 }} />
          <ButtonGroup
            orientation="vertical"
            aria-label="vertical contained button group"
            variant="text"
          >
            {buttons}
          </ButtonGroup>
        </div>
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


export default HomePage;
