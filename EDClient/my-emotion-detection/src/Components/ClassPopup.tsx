import { Button, Dialog, DialogContent, DialogTitle, Grid, Typography } from "@mui/material";
import { ArcElement, Chart, Legend, Tooltip } from "chart.js";
import ChartDataLabels from 'chartjs-plugin-datalabels';
import React, { useEffect, useRef, useState } from 'react';
import { Pie } from "react-chartjs-2";
import Class from "../Objects/Class";
import Student from "../Objects/Student";
import StudentBarChart from "./StudentBarChart";
import StudentCard from "./StudentCard";

Chart.register(ArcElement, ChartDataLabels, Tooltip, Legend);

interface ClassPopupProps {
  open: boolean;
  onClose: () => void;
  classLesson: Class | null;
}

const ClassPopup: React.FC<ClassPopupProps> = ({ open, onClose, classLesson }) => {
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);
  const [openStudentDialog, setOpenStudentDialog] = useState(false); // State for student dialog


  const chartRef = useRef<any>(null);


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
    datasets: [{
      data: Object.values(emotionCounts),
      backgroundColor: ['#ff6384', '#36a2eb', '#ffce56', '#4bc0c0', '#9966ff'],
      hoverBackgroundColor: ['#ff6384', '#36a2eb', '#ffce56', '#4bc0c0', '#9966ff']
    }]
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
    <>
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="md">
      <DialogTitle>{classLesson.name}</DialogTitle>
      <DialogContent>
        <Grid container spacing={2}>
          <Grid item xs={6}>
            {classLesson.students.map(student => (
              <StudentCard key={student.id} student={student} onClick={() => handleStudentClick(student)} />
            ))}
          </Grid>
          <Grid item xs={6}>
            <Typography variant="h6">Emotions Summary</Typography>
            <Pie id="chart-id" data={pieData} options={pieOptions} />
          </Grid>
        </Grid>
      </DialogContent>
    </Dialog>
          <Dialog open={openStudentDialog} onClose={handleCloseStudentDialog} fullWidth maxWidth="sm">
          <DialogContent>
            {selectedStudent && (
              <>
                {/* Add your content related to the selected student here */}
                <Typography variant="h6" sx={{ mt: 4 }}>{selectedStudent.name}'s Emotions Over Time</Typography>
                <StudentBarChart student={selectedStudent} />

              </>
            )}
            <Button variant="contained" color="primary" onClick={handleCloseStudentDialog}>
              Close
            </Button>
          </DialogContent>
        </Dialog>
      </>
  );
};

export default ClassPopup;
