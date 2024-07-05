import { Box, Button, Grid, TextField, ThemeProvider, Typography } from "@mui/material";
import React, { useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import ClassDisplayCard from "../Components/ClassDisplayCard";
import ClassPopup from "../Components/ClassPopup";
import Navbar from "../Components/Navbar";
import Class from "../Objects/Class";
import ClassDisplay from "../Objects/ClassDisplay";
import Student from "../Objects/Student";
import { pathTeacherDashBoard } from "../Paths";
import { serverGetEnrolledLessons, serverGetStudentDataByLesson } from "../Services/ClientService";
import { mainTheme, squaresColor } from "../Utils";


function ClassesDashboard() {
  const navigate = useNavigate();
  const [classes, setClasses] = useState<ClassDisplay[]>([]);
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [selectedClass, setSelectedClass] = useState<Class | null>(null);
  const [popupOpen, setPopupOpen] = useState<boolean>(false);


  useEffect(() => {
    serverGetEnrolledLessons().then((classes: ClassDisplay[]) => {
      setClasses(classes);
    }).catch((e) => alert(e));
    //ServerMockGetClasses().then((data: ClassDisplay[]) => setClasses(data));
  }, []);

  const handleCardClick = async (SelectedClassDisplay: ClassDisplay) => {
    serverGetStudentDataByLesson(SelectedClassDisplay.id).then((students: Student[]) => {
      setSelectedClass(new Class(
        SelectedClassDisplay.id,
        SelectedClassDisplay.name,
        SelectedClassDisplay.description,
        SelectedClassDisplay.date,
        students
      ))
    }).catch((e) => alert(e));



    // const classData = await ServerMockGetClass(SelectedClassDisplay.id);
    // setSelectedClass(classData);
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
    <ThemeProvider theme={mainTheme}>
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
                Class={classItem}
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
