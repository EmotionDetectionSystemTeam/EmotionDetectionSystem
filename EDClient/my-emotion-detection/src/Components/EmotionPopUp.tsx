import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Slider, Tab, Tabs, Typography } from '@mui/material';
import React, { useState } from 'react';
import { Bar, BarChart, Cell, Legend, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';
import Student from '../Objects/Student';
import StudentBarChart from './StudentBarChart'; // Assume you have this component

interface EmotionsPopupProps {
  student: any;
  onClose: () => void;
}

const EmotionsPopup = ({ student, onClose }) => {
  const [tabIndex, setTabIndex] = useState(0);
  const [intervalMinutes, setIntervalMinutes] = useState(1);

  // Count occurrences of each emotion
  const emotionCount = student.previousEmotions.map(emotion => emotion.emotion).reduce((acc, emotion) => {
    acc[emotion] = (acc[emotion] || 0) + 1;
    return acc;
  }, {});

  const emotionColorMap: { [key: string]: string } = {
    Neutral: 'gray',
    Happy: 'green',
    Surprised: 'yellow',
    Angry: 'red',
    Sad: 'red',
    Disgusted: 'red',
    Fearful: 'red',
  };

  // Transform emotionCount into an array of objects for BarChart
  const emotionData = Object.keys(emotionCount).map((emotion) => ({
    emotion,
    count: emotionCount[emotion],
    color: emotionColorMap[emotion],
  }));

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setTabIndex(newValue);
  };

  return (
    <Dialog open={true} onClose={onClose} fullWidth maxWidth="xl">
      <DialogTitle>Emotions of {student.name}</DialogTitle>
      <DialogContent sx={{ height: '80vh' }}>
        <Tabs value={tabIndex} onChange={handleTabChange}>
          <Tab label="Emotions Summary" />
          <Tab label="Emotions Overtime" />
        </Tabs>
        {tabIndex === 0 && (
          <Box sx={{ mt: 2, height: '90%', width: '100%' }}>
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={emotionData}>
                  <XAxis dataKey="emotion" />
                  <YAxis />
                  <Tooltip />
                  <Legend />
                  <Bar dataKey="count" barSize={50}>
                    {emotionData.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} /> // Dynamically assign fill color
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            
          </Box>
        )}
        {tabIndex === 1 && (
          <Box sx={{ mt: 2 }} maxWidth="lg">
            <Typography variant="h6" sx={{ mt: 4 }}>{student.name}'s Emotions Over Time</Typography>
            <Typography gutterBottom>Interval Duration (minutes):</Typography>
            <Slider
              value={intervalMinutes}
              onChange={(e, newValue) => setIntervalMinutes(newValue as number)}
              valueLabelDisplay="auto"
              min={1}
              max={60}
              step={1}
            />
            <StudentBarChart student={new Student(student.email ,student.name,"",student.previousEmotions)} intervalMinutes={intervalMinutes}  />
          </Box>
        )}
      </DialogContent>
      <DialogActions>
        <Button variant='contained' onClick={onClose}>Close</Button>
      </DialogActions>
    </Dialog>
  );
}

export default EmotionsPopup;
