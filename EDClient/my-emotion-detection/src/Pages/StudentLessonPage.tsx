import { Box, Button, Typography, createTheme } from "@mui/material";
import * as React from "react";
import { ThemeProvider } from "styled-components";
import Navbar from "../Components/Navbar";
import ExpressionProcessor from "../Services/ModelService";

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

  React.useEffect(() => {
    const processor = new ExpressionProcessor();
    processor.processExpressions();

    return () => {
      processor.stopProcessing();
    };
  }, []);

  const handleExitLesson = () => {
    // Logic to handle exiting the lesson
    // You can add your logic here, such as redirecting to another page or displaying a confirmation dialog
    console.log("Exiting lesson...");
  };

  return (
    <ThemeProvider theme={theme}>
      <Box>
        <Navbar />
      </Box>
        <video ref={videoRef} autoPlay muted playsInline />
        <canvas ref={canvasRef} style={{ position: 'absolute', top: 0, left: 0, zIndex: 1 }} />
        <Typography variant="h5" style={{ position: 'absolute', top: '50%', left: '50%', transform: 'translate(-50%, -50%)', zIndex: 2 }}>
          Proccessing Data...
        </Typography>
        <Button variant="contained" color="primary" style={{ position: 'absolute', bottom: 20, left: '50%', transform: 'translateX(-50%)', zIndex: 2 }} onClick={handleExitLesson}>
          Exit Lesson
        </Button>
    </ThemeProvider>



  );
}

export default StudentLesson;
