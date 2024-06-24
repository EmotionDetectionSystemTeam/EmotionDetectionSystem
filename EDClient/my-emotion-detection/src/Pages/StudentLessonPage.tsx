import { Box, Button, Typography, createTheme } from "@mui/material";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import Navbar from "../Components/Navbar";
import { pathStudentDashBoard } from "../Paths";
import { serverLeaveLesson } from "../Services/ClientService";
import ExpressionProcessor from "../Services/ModelService";
import { initWebSocket } from "../Services/NotificationService";
import { getUserName } from "../Services/SessionService";

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

function StudentLesson() {
  const videoRef = React.useRef<HTMLVideoElement>(null);
  const canvasRef = React.useRef<HTMLCanvasElement>(null);
  const navigate = useNavigate();
  const processorRef = React.useRef<ExpressionProcessor | null>(null);

  React.useEffect(() => {
    const processor = new ExpressionProcessor();
    processorRef.current = processor;
    processor.processExpressions();

    return () => {
      if (processorRef.current) {
        processorRef.current.stopProcessing();
      }
      window.location.reload();

    };
  }, []);

  const handleExitLesson = () => {
    if (processorRef.current) {
      processorRef.current.stopProcessing();
    }

    serverLeaveLesson().then((response: string) => {
      alert(response);
      navigate(pathStudentDashBoard);
    }).catch((e) => alert(e));
  };

  const handleWebSocketMessage = (data) => {
    alert(data);
    handleExitLesson();
  };
  const address = `ws://127.0.0.1:4560/${getUserName()}-notifications`;
  initWebSocket(address, handleWebSocketMessage); // Initialize WebSocket with the message handler

  return (
    <ThemeProvider theme={theme}>
      <Box>
        <Navbar />
      </Box>
      <video ref={videoRef} autoPlay muted playsInline />
      <canvas ref={canvasRef} style={{ position: 'absolute', top: 0, left: 0, zIndex: 1 }} />
      <Typography variant="h5" style={{ position: 'absolute', top: '50%', left: '50%', transform: 'translate(-50%, -50%)', zIndex: 2 }}>
        Processing Data...
      </Typography>
      <Button variant="contained" color="primary" style={{ position: 'absolute', bottom: 20, left: '50%', transform: 'translateX(-50%)', zIndex: 2 }} onClick={handleExitLesson}>
        Exit Lesson
      </Button>
    </ThemeProvider>
  );
}

export default StudentLesson;
