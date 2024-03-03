import { Button, Card, CardContent, Grid } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import * as React from "react";
import ClientStudent from "../Objects/ClientStudent";
import { serverEndLesson } from "../Services/ClientService";

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
  const [studentList, setStudentList] = React.useState<ClientStudent[]>([]);

  React.useEffect(() => {
    // Fetch or initialize class code
    setClassCode("yihie beseder ya Galmuzz");

    // Fetch or initialize student list
    setStudentList([
      new ClientStudent("John", "Doe", "john@example.com", "Happy", "blue"),
      new ClientStudent("Jane", "Doe", "jane@example.com", "Excited", "green"),
      new ClientStudent("Alice", "Smith", "alice@example.com", "Sad", "red"),
      new ClientStudent("Bob", "Johnson", "bob@example.com", "Neutral", "yellow"),
    ]);

    const intervalId = setInterval(() => {
      HandleGetEmotions();
    }, 3000); // Run HandleGetEmotions every 30 seconds

    // Clean up interval to avoid memory leaks
    return () => clearInterval(intervalId);
  }, []);

  const HandleGetEmotions = () => {
    // Logic to handle getting emotions
    console.log("Getting emotions...");
  };

  const handleEndLesson = () => {
    // Logic to handle ending the lesson
    serverEndLesson().then((message: string) => alert(message)).catch((e) => alert(e))
  };

  return (
    <ThemeProvider theme={theme}>
      <Box>
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
