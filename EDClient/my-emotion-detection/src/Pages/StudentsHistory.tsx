
import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid, TextField, ThemeProvider, Typography } from "@mui/material";
import React, { useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import Navbar from "../Components/Navbar";
import StudentCard from "../Components/StudentCard";
import StudentEmotionChart from "../Components/StudentEmotionChart";
import StudentDisplay from "../Objects/StudentDisplay";
import StudentOverview from "../Objects/StudentOverview";
import { pathTeacherDashBoard } from "../Paths";
import { serverGetAllStudentData, serverGetStudentData } from "../Services/ClientService";
import { mainTheme, squaresColor } from "../Utils";



function StudentsHistory() {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [selectedStudent, setSelectedStudent] = useState<StudentDisplay | null>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [fetchedOverview, setFetchedOverview] = useState<StudentOverview | null>(null);


  const [students, setStudentsData] = useState<StudentDisplay[]>([]);

  useEffect(() => {
    serverGetAllStudentData().then((students : StudentDisplay[]) => {
      setStudentsData(students);
    }).catch((e) =>alert(e));

    //ServerMockGetStudentsData().then((students :StudentDisplay[]) =>
    //setStudentsData(students));

  },[]);

  const filteredStudents = students.filter((student) =>
    student.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    student.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleStudentClick = async (student: StudentDisplay) => {
    serverGetStudentData(student.email).then((studentOverview : StudentOverview) => {
      setDialogOpen(true);
      setSelectedStudent(studentOverview);
      setFetchedOverview(studentOverview);

    }).catch((e) => alert(e));

    // try {
    //   const overview = await ServerMockStudentOverview(student.email);
    //   // Handle the fetched overview data
    //   setFetchedOverview(overview);
    //   console.log(overview);
    // } catch (error) {
    //   console.error("Failed to fetch student overview:", error);
    // }
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
    setSelectedStudent(null);
    setFetchedOverview(null);
  };

  
  
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
          Students Dashboard
        </Typography>
        <TextField
          label="Search Students"
          variant="outlined"
          fullWidth
          sx={{ mb: 3 }}
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <Grid container spacing={2} justifyContent="center">
          {filteredStudents.map((student) => (
            <Grid item key={student.email}>
              <StudentCard student={student} onClick={() => handleStudentClick(student)}
 />
            </Grid>
          ))}
        </Grid>
        {/* ... */}
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
      </Box>
      <Dialog open={dialogOpen} onClose={handleCloseDialog} fullWidth maxWidth="md">
        <DialogTitle>{selectedStudent ? `${selectedStudent.name}'s Profile` : ''}</DialogTitle>
        <DialogContent>
          {fetchedOverview && (
            <>
              <Typography variant="h6" gutterBottom>
                Student Details
              </Typography>
              <Typography>Email: {fetchedOverview.email}</Typography>
              <Typography variant="h6" gutterBottom style={{ marginTop: '1rem' }}>
                All Class Emotions
              </Typography>
              <StudentEmotionChart studentOverview={fetchedOverview} />
            </>
          )}
        </DialogContent>
        <DialogActions>
        <Button variant="contained" color="primary" onClick={handleCloseDialog}>
      Close
      </Button>        </DialogActions>
      </Dialog>
    </ThemeProvider>
  );
}

export default StudentsHistory;
