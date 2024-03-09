import { Button, Card, CardContent, Grid } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import Navbar from "../Components/Navbar";
import ClientStudent from "../Objects/ClientStudent";
import { Lesson } from "../Objects/Lesson";
import { ServiceRealTimeUser } from "../Objects/ServiceRealTimeUser";
import { pathTeacherDashBoard } from "../Paths";
import { serverEndLesson, serverGetLastEmotionsData } from "../Services/ClientService";
import { getCookie, getLessonId, getSessionId } from "../Services/SessionService";


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

function TeacherLesson() {
  const [classCode, setClassCode] = React.useState<string | null>(null);
  const [studentList, setStudentList] = React.useState<ClientStudent[]>([
    new ClientStudent("John", "Doe", "john@example.com", "Happy", "gray"),
    new ClientStudent("Jane", "Doe", "jane@example.com", "Excited", "green"),
    new ClientStudent("Alice", "Smith", "alice@example.com", "Sad", "red"),
    new ClientStudent("Bob", "Johnson", "bob@example.com", "Neutral", "yellow"),
  ]);

  //const lessonCookie = Cookies.get('TeacherLesson');
  //const lesson : Lesson = lessonCookie ? JSON.parse(lessonCookie) : null;
  //alert(lesson.LessonId);
  const navigate = useNavigate();


  const MapEmotionsToStudents = (students: ServiceRealTimeUser[]): ClientStudent[] => {
    // Define a mapping of emotions to colors
    const emotionColorMap: { [key: string]: string } = {
      Natural: 'gray',
      Happy: 'green',
      Surprised: 'yellow',
      Angry: 'red',
      Sad: 'red',
      Disgusted: 'red',
      Fearful: 'red',
    };

    const emotionSorMap: { [key: string]: number } = {
      yellow: 1,
      red: 2,
      green: 3,
      gray: 4,
    };
  
    // Map ServiceRealTimeUser objects to ClientStudent objects
    const mappedStudents: ClientStudent[] = students.map((student: ServiceRealTimeUser) => {
      // Determine the color based on the emotion
      const color: string = emotionColorMap[student.winingEmotion];
  
      // Create a new ClientStudent object
      const clientStudent: ClientStudent = new ClientStudent(
        student.firstName,
        student.lastName,
        student.email,
        student.winingEmotion,
        color
      );
  
      return clientStudent;
    });

    mappedStudents.sort((a: ClientStudent, b: ClientStudent) => {
      return emotionSorMap[a.color] - emotionSorMap[b.color];
    });
  
    return mappedStudents;
  };
  
  React.useEffect(() => {
    // Fetch or initialize class code
    const lesson : Lesson=  getCookie(getSessionId(), 'lesson')
    setClassCode(lesson.EntryCode);

    const intervalId = setInterval(() => {
      HandleGetEmotions();
    }, 30000); // Run HandleGetEmotions every 30 seconds

    // Clean up interval to avoid memory leaks
    return () => clearInterval(intervalId);
  }, []);
  const HandleGetEmotions = () => {
    
    serverGetLastEmotionsData(getLessonId()).then((students : ServiceRealTimeUser[]) => {
      const clientStudents : ClientStudent[]=  MapEmotionsToStudents(students);
      setStudentList(clientStudents)
    });
    
    console.log("Getting emotions...");
  };

  const handleEndLesson = () => {
    // Logic to handle ending the lesson
    serverEndLesson().then((message: string) => {
      alert(message);
      navigate(pathTeacherDashBoard);
    }).catch((e) => alert(e))
  };

  return (
    <ThemeProvider theme={theme}>
        <Box>
          <Navbar />
        </Box>
      <Box sx={{ border: "5px solid black", padding: 2 }}>
        <Grid container justifyContent="center">
          <Grid item xs={12}>
            <Typography variant="h4" align="center" sx={{ mt: 3 }}>
              Class Code: {classCode}
            </Typography>
          </Grid>
          <Grid item xs={12}>
            <Box
              sx={{
                display: "flex",
                flexWrap: "wrap",
                justifyContent: "flex-start",
                gap: 2,
                mt: 3,
              }}
            >
              {studentList.map((student, index) => (
                <Card
                  key={index}
                  sx={{
                    width: 200,
                    border: `5px solid ${student.color}`,
                    borderRadius: 2,
                    marginBottom: 10,
                  }}
                >
                  <CardContent>
                    <div>
                      <Typography variant="body1" gutterBottom>
                        Name: {student.name}
                      </Typography>
                      <Typography variant="body1" gutterBottom>
                        Emotion: {student.emotion}
                      </Typography>
                    </div>
                  </CardContent>
                </Card>
              ))}
            </Box>
          </Grid>
          <Grid item xs={12} sx={{ mt: 3 }}>
            <Button variant="contained" onClick={handleEndLesson}>
              End Lesson
            </Button>
          </Grid>
        </Grid>
      </Box>
    </ThemeProvider>
  );
}

export default TeacherLesson;
