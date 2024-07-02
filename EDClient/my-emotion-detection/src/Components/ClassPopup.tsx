import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid, Slider, TextField, ThemeProvider, Typography } from "@mui/material";
import { ArcElement, Chart, Legend, Tooltip } from "chart.js";
import ChartDataLabels from 'chartjs-plugin-datalabels';
import React, { useEffect, useRef, useState } from 'react';
import { Pie } from "react-chartjs-2";
import Class from "../Objects/Class";
import Student from "../Objects/Student";
import { mainTheme } from "../Utils";
import StudentBarChart from "./StudentBarChart";
import StudentCard from "./StudentCard";

Chart.register(ArcElement, ChartDataLabels, Tooltip, Legend);

interface ClassPopupProps {
  open: boolean;
  onClose: () => void;
  classLesson: Class | null;
}

const emotionColors = {
  'Happy': '#4caf50', // Green
  'Surprised': '#f9a201', // Yellow
  'Neutral': '#9e9e9e', // Gray
  'Sad' : '#ff6384',
  'Angry' : '#ff6384',
  'Fearful' : '#ff6384',
  'Disgusted' : '#ff6384',

};

const ClassPopup: React.FC<ClassPopupProps> = ({ open, onClose, classLesson }) => {
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);
  const [openStudentDialog, setOpenStudentDialog] = useState(false);
  const [intervalMinutes, setIntervalMinutes] = useState(5);
  const [searchTerm, setSearchTerm] = useState<string>("");





  const chartRef = useRef<any>(null);

  const filteredStudents = classLesson !== null ? classLesson.students.filter((student) =>
    student.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    student.email.toLowerCase().includes(searchTerm.toLowerCase())
  ) : null;

  useEffect(() => {
    return () => {
      if (chartRef.current) {
        chartRef.current.destroy(); // Ensure chart is destroyed on unmount
      }
    };
  }, []);

  if (!classLesson) return null;

  const handleStudentClick = (student: Student) => {
    setSelectedStudent(student);
    setOpenStudentDialog(true);
  };

  const handleCloseStudentDialog = () => {
    setOpenStudentDialog(false);
    setSelectedStudent(null); // Reset selected student when closing dialog
  };

  const emotionCounts = classLesson.students
    .flatMap(student => student.emotions.map(emotion => emotion.emotion))
    .reduce((acc, emotion) => {
      acc[emotion] = (acc[emotion] || 0) + 1;
      return acc;
    }, {} as Record<string, number>);

  const pieData = {
    labels: Object.keys(emotionCounts),
    datasets: [
      {
        data: Object.values(emotionCounts),
        backgroundColor: Object.keys(emotionCounts).map(
          (emotion) => emotionColors[emotion] || '#ff6384'
        ),
        hoverBackgroundColor: Object.keys(emotionCounts).map(
          (emotion) => emotionColors[emotion] || '#ff6384'
        ),
      },
    ],
  };

  const pieOptions = {
    plugins: {
      datalabels: {
        color: '#fff',
        formatter: (value: number, context: any) => {
          return `${context.chart.data.labels[context.dataIndex]}: ${value}`;
        }
      }
    }
  };


  return (
    <ThemeProvider theme={mainTheme}>
      <Box>
        <Dialog open={open} onClose={onClose} fullScreen={true} fullWidth={true}>
          <DialogTitle>{ }</DialogTitle>
          <DialogContent>
            <Box>
              <Typography align="center" variant="h4" > {classLesson.name + ' - ' + classLesson.description}</Typography>
              <Typography align="center" variant="h6" gutterBottom> Class Date: {classLesson.date.toString()}</Typography>



              <Grid container spacing={2}>
                <Grid item xs={6}>
                  <Typography align="center" variant="h5">Students Attendants</Typography>

                  <Box

                    sx={{
                      width: "100%",
                      display: "flex",
                      flexWrap: "wrap",
                      justifyContent: "flex-start",
                      gap: 2,
                      mt: 3,
                    }}
                  >
                    <TextField
                      label="Search Students"
                      variant="outlined"
                      fullWidth
                      sx={{ mb: 3 }}
                      value={searchTerm}
                      onChange={(e) => setSearchTerm(e.target.value)}
                    />

                    {filteredStudents?.map(student => (
                      <StudentCard key={student.name} student={student} onClick={() => handleStudentClick(student)} />
                    ))}
                  </Box>

                </Grid>
                <Grid item xs={6}>


                  <Box

                    sx={{
                      overflow: "false",
                      width: "75%",
                      display: "flex",
                      flexWrap: "wrap",
                      justifyContent: "flex-start",
                      gap: 2,
                      mt: 3,
                    }}
                  >

                    <Pie id="chart-id" data={pieData} options={pieOptions} />


                  </Box>

                </Grid>
              </Grid>
            </Box>
          </DialogContent>
          <DialogActions>
            <Button sx={{
              position: 'fixed',
              left: 20,
              bottom: 20,
            }} variant="contained" color="primary" onClick={onClose}>Close</Button>
          </DialogActions>

        </Dialog>
        <Dialog open={openStudentDialog} onClose={handleCloseStudentDialog} fullWidth maxWidth="lg">
          <DialogContent>
            {selectedStudent && (
              <>
                {/* Add your content related to the selected student here */}
                <Typography variant="h6" sx={{ mt: 4 }}>{selectedStudent.name}'s Emotions Over Time</Typography>
                <Typography gutterBottom>Interval Duration (minutes):</Typography>
                {/* Range Slider */}
                <Slider
                  value={intervalMinutes}
                  onChange={(e, newValue) => setIntervalMinutes(newValue as number)}
                  valueLabelDisplay="auto"
                  min={1}
                  max={60}
                  step={1}
                />
                <StudentBarChart student={selectedStudent} intervalMinutes={intervalMinutes} />
                <Typography variant="h6" sx={{ mt: 4 }}>Teacher Approaches to {selectedStudent.name}  </Typography>


              </>
            )}
            {/* { selectedStudent?.emotions != null ? selectedStudent?.emotions.map(approach => (
              <Typography fontSize="h6"> {approach.emotion}  </Typography>

            ))

              : null} */}
            <Button variant="contained" sx={{ mt: 4 }} color="primary" onClick={handleCloseStudentDialog}>
              Close
            </Button>
          </DialogContent>
        </Dialog>


      </Box>
    </ThemeProvider>
  );
};

export default ClassPopup;
