import { Button, DialogActions, DialogContent } from '@mui/material';
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import * as React from "react";
import { Bar, BarChart, Cell, Legend, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';

export function EmotionsPopup({student, onClose}) {
    // Count occurrences of each emotion
    const emotionCount = student.previousEmotions.reduce((acc, emotion) => {
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

    return (
      <Dialog open={true} onClose={onClose} fullWidth maxWidth="xl">
        <DialogTitle>Emotions of {student.name}</DialogTitle>
        <DialogContent sx={{ height: '80vh' }}>
          {student.previousEmotions && (
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
          )}
        </DialogContent>
        <DialogActions>
          <Button variant='contained' onClick={onClose}>Close</Button>
        </DialogActions>
      </Dialog>
    );
}

export default EmotionsPopup;
