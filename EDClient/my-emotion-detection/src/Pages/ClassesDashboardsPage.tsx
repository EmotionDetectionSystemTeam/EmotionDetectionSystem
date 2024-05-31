import { Box, Button, Grid, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";
import React, { useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import ClassDisplayCard from "../Components/ClassDisplayCard";
import ClassPopup from "../Components/ClassPopup";
import Navbar from "../Components/Navbar";
import Class from "../Objects/Class";
import ClassDisplay from "../Objects/ClassDisplay";
import Emotion from "../Objects/Emotion";
import Student from "../Objects/Student";
import { pathTeacherDashBoard } from "../Paths";
import { squaresColor } from "../Utils";

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

export const ServerGetClasses = async (): Promise<ClassDisplay[]> => {
  // Mock data for class displays
  return [
    new ClassDisplay(1, "Math 101", "2023-05-25", "Introduction to Algebra"),
    new ClassDisplay(2, "History 201", "2023-05-26", "World War II Overview"),
    new ClassDisplay(3, "Science 301", "2023-05-27", "Basics of Physics")
  ];
};

export const ServerGetClass = async (id: number): Promise<Class> => {
  // Mock data for a specific class lesson

  // Generate mock students with emotions over time
  const students: Student[] = [
    new Student(1, "Alice", [
      new Emotion("Happy", new Date("2023-05-25T10:03:00")),
      new Emotion("Happy", new Date("2023-05-25T10:03:00")),
      new Emotion("Happy", new Date("2023-05-25T10:03:00")),
      new Emotion("Happy", new Date("2023-05-25T10:03:00")),
      new Emotion("Happy", new Date("2023-05-25T10:03:00")),

      new Emotion("Happy", new Date("2023-05-25T10:00:00")),

      new Emotion("Sad", new Date("2023-05-25T10:05:00")),
      new Emotion("Happy", new Date("2023-05-25T10:10:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:15:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:20:00")),
      new Emotion("Sad", new Date("2023-05-25T10:25:00")),
      new Emotion("Happy", new Date("2023-05-25T10:30:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:35:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:40:00")),
      new Emotion("Sad", new Date("2023-05-25T10:45:00")),
    ]),
    new Student(2, "Bob", [
      new Emotion("Happy", new Date("2023-05-25T10:00:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:05:00")),
      new Emotion("Sad", new Date("2023-05-25T10:10:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:15:00")),
      new Emotion("Happy", new Date("2023-05-25T10:20:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:25:00")),
      new Emotion("Sad", new Date("2023-05-25T10:30:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:35:00")),
      new Emotion("Happy", new Date("2023-05-25T10:40:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:45:00")),
    ]),
    new Student(3, "Charlie", [
      new Emotion("Happy", new Date("2023-05-25T10:00:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:05:00")),
      new Emotion("Happy", new Date("2023-05-25T10:10:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:15:00")),
      new Emotion("Sad", new Date("2023-05-25T10:20:00")),
      new Emotion("Happy", new Date("2023-05-25T10:25:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:30:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:35:00")),
      new Emotion("Sad", new Date("2023-05-25T10:40:00")),
      new Emotion("Happy", new Date("2023-05-25T10:45:00")),
    ]),
    new Student(4, "David", [
      new Emotion("Happy", new Date("2023-05-25T10:00:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:05:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:10:00")),
      new Emotion("Sad", new Date("2023-05-25T10:15:00")),
      new Emotion("Happy", new Date("2023-05-25T10:20:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:25:00")),
      new Emotion("Neutral", new Date("2023-05-25T10:30:00")),
      new Emotion("Sad", new Date("2023-05-25T10:35:00")),
      new Emotion("Happy", new Date("2023-05-25T10:40:00")),
      new Emotion("Surprised", new Date("2023-05-25T10:45:00")),
    ]),
  ];

  return new Class(
    id,
    "Math 101",
    "Introduction to Algebra",
    new Date("2023-05-25T10:00:00"),
    students
  );
};



function ClassesDashboard() {
  const navigate = useNavigate();
  const [classes, setClasses] = useState<ClassDisplay[]>([]);
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [selectedClass, setSelectedClass] = useState<Class | null>(null);
  const [popupOpen, setPopupOpen] = useState<boolean>(false);


  useEffect(() => {
    const fetchClasses = async (): Promise<void> => {
      const data: ClassDisplay[] = await ServerGetClasses();
      setClasses(data);
    };
    fetchClasses();
  }, []);

  const handleCardClick = async (id: number) => {
    const classData = await ServerGetClass(id);
    setSelectedClass(classData);
    setPopupOpen(true);
  };

  const handleClosePopup = () => {
    setPopupOpen(false);
    setSelectedClass(null);
  };
  
  const filteredClasses = classes.filter((classItem) =>
    classItem.name.toLowerCase().includes(searchTerm.toLowerCase())
  );


  return (
    <ThemeProvider theme={theme}>
      <Box>
        <Navbar />
      </Box>
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
          Classes Dashboard
        </Typography>
        <TextField
          label="Search Classes"
          variant="outlined"
          fullWidth
          sx={{ mb: 3 }}
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <Grid container spacing={2} justifyContent="center">
          {filteredClasses.map((classItem) => (
            <Grid item key={classItem.id}>
              <ClassDisplayCard
                id ={classItem.id}
                name={classItem.name}
                date={classItem.date}
                description={classItem.description}
                onClick={handleCardClick}

              />
            </Grid>
          ))}
        </Grid>
        <Button
          fullWidth
          variant="contained"
          onClick={() => navigate(pathTeacherDashBoard)}
          sx={{
            mt: 3,
            mb: 2,
            color: "black",
            backgroundColor: "#fff",
            "&:hover": { backgroundColor: squaresColor },
          }}
        >
          Back to Dashboard
        </Button>
        <ClassPopup
          open={popupOpen}
          onClose={handleClosePopup}
          classLesson={selectedClass}
        />
      </Box>
    </ThemeProvider>
  );
}

export default ClassesDashboard;
