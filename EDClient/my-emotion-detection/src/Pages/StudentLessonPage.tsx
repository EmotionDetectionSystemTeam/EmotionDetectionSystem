import { Box, Button, Typography } from "@mui/material";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import Navbar from "../Components/Navbar";
import { pathStudentDashBoard } from "../Paths";
import { serverLeaveLesson } from "../Services/ClientService";
import ExpressionProcessor from "../Services/ModelService";
import { setLessonId, setLessonTeacher } from "../Services/SessionService";
import { mainTheme } from "../Utils";


function StudentLesson() {

  const navigate = useNavigate();
  const videoRef = React.useRef<HTMLVideoElement>(null);
  const canvasRef = React.useRef<HTMLCanvasElement>(null);
  const processor = new ExpressionProcessor();


  React.useEffect(() => {
    processor.processExpressions();

    return () => {
    };
  }, []);

  const handleExitLesson = () => {
    // Logic to handle exiting the lesson
    // You can add your logic here, such as redirecting to another page or displaying a confirmation dialog
    serverLeaveLesson().then(() => {
      // processor.stopProcessing();
      processor.stopProcessing();
      setLessonId("");
      setLessonTeacher("");
      navigate(pathStudentDashBoard);
      }).catch((e) => alert(e));
    
  };


  return (
    <ThemeProvider theme={mainTheme}>
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
