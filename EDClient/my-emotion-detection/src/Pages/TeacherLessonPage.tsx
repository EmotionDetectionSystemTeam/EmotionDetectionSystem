import InfoIcon from '@mui/icons-material/Info';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid, IconButton } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { ThemeProvider } from "@mui/material/styles";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import ActiveStudentCard from '../Components/ActiveStudentCard';
import EmotionsPopup from "../Components/EmotionPopUp";
import Navbar from "../Components/Navbar";
import StudentCard from "../Components/StudentCard";
import ClientStudent from "../Objects/ClientStudent";
import { Lesson } from "../Objects/Lesson";
import { ServiceRealTimeUser } from "../Objects/ServiceRealTimeUser";
import { pathTeacherDashBoard } from "../Paths";
import { serverAddTeacherApproach, serverEndLesson, serverGetLastEmotionsData, serverGetLesson } from "../Services/ClientService";
import { getLessonId } from "../Services/SessionService";
import { mainTheme } from "../Utils";

function TeacherLesson() {
  const [classCode, setClassCode] = React.useState<string | null>(null);
  const [className, setClassName] = React.useState<string | null>(null);
  const [date, setDate] = React.useState<string | null>(null);
  const [teacherName, setTeacherName] = React.useState<string | null>(null);
  const [showPopup, setShowPopup] = React.useState(false);
  const [selectedStudent, setSelectedStudent] = React.useState<ClientStudent | null>(null);
  const [studentList, setStudentList] = React.useState<ClientStudent[]>([]);
  const [activeStudentList, setActiveStudentList] = React.useState<ClientStudent[]>([]);
  const [infoDialogOpen, setInfoDialogOpen] = React.useState(false);





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

    const mappedStudents: ClientStudent[] = students.map((student: ServiceRealTimeUser) => {
      const color: string = emotionColorMap[student.winingEmotion];

      const clientStudent: ClientStudent = new ClientStudent(
        student.firstName,
        student.lastName,
        student.email,
        student.winingEmotion,
        student.previousEmotions,
        color
      );

      return clientStudent;
    });


    // mappedStudents.sort((a: ClientStudent, b: ClientStudent) => {
    //   return emotionSorMap[a.color] - emotionSorMap[b.color];
    // });

    return mappedStudents;
  };

  const filterInactiveStudents = (students: ClientStudent[]): ClientStudent[] => {
    const filtered = students.filter((student: ClientStudent) =>
      student.emotion !== "No Data" && student.emotion !== "Neutral" && student.emotion !== "1Happy");
    return filtered;

  }

  const handleStudentClick = (student) => {
    setSelectedStudent(student);
    setShowPopup(true);
  };

  const handleStudentCardClick = (student) => {
    serverAddTeacherApproach(student.email).catch((e) => alert(e))
  };

  React.useEffect(() => {
    serverGetLesson(getLessonId()).then((lesson: Lesson) => {
      setClassCode(lesson.EntryCode);
      setClassName(lesson.LessonName);
      setDate(lesson.Date.toString());
      setTeacherName(lesson.Teacher);
      setStudentList(MapEmotionsToStudents(lesson.StudentsEmotions));
      setActiveStudentList(filterInactiveStudents(studentList));

    })
    // Fetch or initialize class code

    const intervalId = setInterval(() => {
      HandleGetEmotions();
    }, 3000); // Run HandleGetEmotions every 30 seconds

    // Clean up interval to avoid memory leaks
    return () => clearInterval(intervalId);
  }, []);
  const HandleGetEmotions = () => {

    serverGetLastEmotionsData(getLessonId()).then((students: ServiceRealTimeUser[]) => {
      const clientStudents: ClientStudent[] = MapEmotionsToStudents(students);
      setStudentList(clientStudents)
      setActiveStudentList(filterInactiveStudents(clientStudents));
    });

    console.log("Getting emotions...");
  };



  const handleEndLesson = () => {
    // Logic to handle ending the lesson
    serverEndLesson().then((message: string) => {
      navigate(pathTeacherDashBoard);
    }).catch((e) => alert(e))
  };

  const handleInfoClick = () => {
    setInfoDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setInfoDialogOpen(false);
  };

  return (
    <ThemeProvider theme={mainTheme}>
      <Box>
        <Navbar />
      </Box>
      <IconButton sx={{
        position: 'fixed',
        right: 20,
        top: 70,
      }} onClick={handleInfoClick}>
        <InfoIcon fontSize="large" />
      </IconButton>
      {showPopup && selectedStudent && (
        <EmotionsPopup
          onClose={() => setShowPopup(false)}
          student={selectedStudent}
        />
      )}
      <Box sx={{ padding: 2 }}>
        <Grid container justifyContent="center" spacing={3}>
          <Grid sx={{
            position: 'fixed',
            right: 20,
            top: 20,
          }} item xs={12} container justifyContent="center" alignItems="center">

          </Grid>
        </Grid>
      </Box>
      <Box sx={{ padding: 2, }}>
        <Grid container justifyContent="center">
          <Grid item xs={12}>
            <Typography variant="h4" align="center" gutterBottom>
              Class Name: {className}
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
              {activeStudentList.map((student, index) => (
                <ActiveStudentCard key={index} student={student} onClick={() => handleStudentCardClick(student)} />

              ))}
            </Box>
          </Grid>
          <Box sx={{
            position: 'fixed',
            left: 320,
            bottom: 20,
          }}>
            <Button variant="contained" onClick={handleEndLesson}>
              End Lesson
            </Button>
          </Box>
          <Dialog open={infoDialogOpen} onClose={handleCloseDialog}>
            <DialogTitle>Class Information</DialogTitle>
            <DialogContent>
              <Typography variant="h6">
                Class Code: {classCode}
              </Typography>
              <Typography variant="h6">
                Class Name: {className}
              </Typography>
              <Typography variant="h6">
                Date: {date}
              </Typography>
              <Typography variant="h6">
                Teacher Name: {teacherName}
              </Typography>
            </DialogContent>
            <DialogActions>
              <Button variant='contained' onClick={handleCloseDialog}>Close</Button>
            </DialogActions>
          </Dialog>
        </Grid>
      </Box>
      <Box
        sx={{
          left: '0%',
          top: '10%',
          position: 'absolute',
          maxHeight: '100%',
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(240px, 1fr))",
          gap: 0,
          mt: 0,
          padding: 1,
          backgroundColor: '#f0f0f0',
          borderRadius: 3,
          boxShadow: "0 4px 12px rgba(0, 0, 0, 0.1)"
        }}
      >
        <Typography variant="h6" align="center" gutterBottom>
          Participants: {studentList.length}
        </Typography>
        <Grid container spacing={1} justifyContent="center"> {/* Updated Grid to a container with spacing and alignment */}
          {studentList != null ? studentList.map((student, index) => (
            <Grid item key={index} width={300}> {/* Each participant button is placed in a grid item */}
              <StudentCard key={student.name} student={student} onClick={() => handleStudentClick(student)} />
            </Grid>
          )) : null}
        </Grid>
      </Box>

    </ThemeProvider>
  );
}

export default TeacherLesson;
