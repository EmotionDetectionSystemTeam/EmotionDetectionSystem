import { Dialog, DialogContent, DialogTitle, Grid, Typography } from "@mui/material";
import { ArcElement, Chart, Legend } from "chart.js";
import ChartDataLabels from 'chartjs-plugin-datalabels';
import React, { useEffect } from 'react';
import { Pie } from "react-chartjs-2";
import Class from "../Objects/Class";

import { Tooltip } from "chart.js";
import StudentCard from "./StudentCard";
import TimeSeriesChart from "./TimeSeriesChart";

Chart.register(ArcElement,ChartDataLabels,Tooltip, Legend);

interface ClassPopupProps {
  open: boolean;
  onClose: () => void;
  classLesson: Class | null;
}

const ClassPopup: React.FC<ClassPopupProps> = ({ open, onClose, classLesson }) => {
  if (!classLesson) return null;

  const emotionCounts = classLesson.students.flatMap(student => student.emotions.map(emotion => emotion.emotion)).reduce((acc, emotion) => {
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

  const timeSeriesData = classLesson.students.flatMap(student => student.emotions)
  .reduce((acc, emotion) => {
    const timePassed = Math.floor((new Date(emotion.date).getTime() - new Date(classLesson.date).getTime()) / (5 * 60 * 1000)) * 5;
    if (!acc[emotion.emotion]) {
      acc[emotion.emotion] = [];
    }
    const timeEntry = acc[emotion.emotion].find(entry => entry.x.getTime() === timePassed);
    if (timeEntry) {
      timeEntry.y += 1;
    } else {
      acc[emotion.emotion].push({ x: new Date(classLesson.date.getTime() + timePassed * 60 * 1000), y: 1 });
    }
    return acc;
  }, {} as { [key: string]: { x: Date; y: number }[] });
  
  useEffect(() => {
    return () => {
      // Ensure chart instance is properly destroyed when the component unmounts
      Chart.getChart("chart-id")?.destroy();
    };
  }, []);

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="md">
      <DialogTitle>{classLesson.name}</DialogTitle>
      <DialogContent>
        <Grid container spacing={2}>
          <Grid item xs={6}>
            {classLesson.students.map(student => (
              <StudentCard key={student.id} student={student} />
            ))}
          </Grid>
          <Grid item xs={6}>
            <Typography variant="h6">Emotions Summary</Typography>
            <Pie id="chart-id" data={pieData} options={pieOptions} />
            <Typography variant="h6" sx={{ mt: 4 }}>Emotions Over Time</Typography>
            <TimeSeriesChart data={timeSeriesData} />
          </Grid>
        </Grid>
      </DialogContent>
    </Dialog>
  );
};

export default ClassPopup;
