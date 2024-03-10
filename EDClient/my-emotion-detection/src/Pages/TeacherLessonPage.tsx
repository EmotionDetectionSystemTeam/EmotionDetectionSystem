import { Button, Card, CardContent, Grid } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import EmotionsPopup from "../Components/EmotionPopUp";
import Navbar from "../Components/Navbar";
import ClientStudent from "../Objects/ClientStudent";
import { Lesson } from "../Objects/Lesson";
import { ServiceRealTimeUser } from "../Objects/ServiceRealTimeUser";
import { pathTeacherDashBoard } from "../Paths";
import { serverEndLesson, serverGetLastEmotionsData, serverGetLesson } from "../Services/ClientService";
import { getLessonId } from "../Services/SessionService";


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
  const [className, setClassName] = React.useState<string | null>(null);
  const [date, setDate] = React.useState<string | null>(null);
  const [participants, setParticipants] = React.useState<number>(0);
  const [teacherName, setTeacherName] = React.useState<string | null>(null);
  const [les, setLesson] = React.useState<Lesson | null>(null);
  const [showPopup, setShowPopup] = React.useState(false);
  const [selectedStudent, setSelectedStudent] = React.useState(null);
  const [selectedEmotions, setSelectedEmotions] = React.useState([]);
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

  const handleStudentClick = (studentName,emotions) => {
    setSelectedStudent(studentName);
    setSelectedEmotions(emotions);
    setShowPopup(true);
  };
  
  React.useEffect(() => {
    serverGetLesson(getLessonId()).then((lesson: Lesson) => {
      setClassCode(lesson.EntryCode);
      setClassName(lesson.LessonName);
      setDate(lesson.Date.toString());
      setTeacherName(lesson.Teacher);
      setLesson(lesson);
    })
    // Fetch or initialize class code

    const intervalId = setInterval(() => {
      HandleGetEmotions();
    }, 12000); // Run HandleGetEmotions every 30 seconds

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
        {showPopup && selectedStudent && selectedEmotions && (
        <EmotionsPopup
          studentName={selectedStudent}
          onClose={() => setShowPopup(false)}
          emotions={selectedEmotions}
        />
      )}
      <Box sx={{ padding: 2, }}>
        <Grid container justifyContent="center">
          <Grid item xs={12}>
            <Typography variant="h4" align="center" sx={{ mt: 3 }}>
              Class Code: {classCode}
            </Typography>
            <Typography variant="h5" align="center" gutterBottom>
              Class Name: {className}
            </Typography>
            <Typography variant="h6" align="center" gutterBottom>
              Date: {date}
            </Typography>
            <Typography variant="h6" align="center" gutterBottom>
              Teacher Name: {teacherName}
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
                    background: "#ede5e5",
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
      <Box sx={{
        left: '0%',
        top: '10%',
        position: 'absolute', 
        maxHeight: '100%',
         overflowY: 'auto', 
         maxWidth: '200px',
         background: "#ede5e5",   
         }}>
        {les && les.StudentsEmotions && typeof les.StudentsEmotions === 'object' && (
          <Typography variant="h6" align="center" gutterBottom>
            Participants: {Object.keys(les.StudentsEmotions).length}
          </Typography>
        )}
        <Grid>
          {les && Object.keys(les.StudentsEmotions).map((studentName, index) => (
          <Button sx={{border: '1px solid black',width: '100%'}} key={index} onClick={() => handleStudentClick(studentName,les.StudentsEmotions[studentName])}>
            {studentName}
          </Button>
          ))}
        </Grid>
      </Box>

    </ThemeProvider>
  );
}

export default TeacherLesson;
