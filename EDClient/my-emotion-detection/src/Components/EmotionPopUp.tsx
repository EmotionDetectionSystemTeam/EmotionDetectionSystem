import { Button, DialogActions, DialogContent } from '@mui/material';
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import * as React from "react";
import { Bar, BarChart, Legend, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';

export function EmotionsPopup({ studentName, onClose, emotions }) {
    // Count occurrences of each emotion
    const emotionCount = emotions.reduce((acc, emotion) => {
        acc[emotion] = (acc[emotion] || 0) + 1;
        return acc;
    }, {});

    // Transform emotionCount into an array of objects for BarChart
    const emotionData = Object.keys(emotionCount).map((emotion) => ({
        emotion,
        count: emotionCount[emotion],
    }));

    return (
      <Dialog open={true} onClose={onClose} fullWidth maxWidth="xl">
        <DialogTitle>Emotions</DialogTitle>
        <DialogContent sx={{ height: '80vh' }}>
          {emotions && (
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={emotionData}>
                <XAxis dataKey="emotion" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar dataKey="count" fill="#8884d8" barSize={50}/>
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
