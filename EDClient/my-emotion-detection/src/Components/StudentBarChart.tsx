import { BarElement, CategoryScale, Chart, Legend, LinearScale, Tooltip } from 'chart.js';
import moment, { Moment } from 'moment';
import React from 'react';
import { Bar } from 'react-chartjs-2';
import Student from '../Objects/Student';

Chart.register(BarElement, CategoryScale, LinearScale, Tooltip, Legend);

interface ChartData {
  labels: string[];
  datasets: {
    label: string;
    data: number[];
    backgroundColor: string[];
  }[];
}

interface Emotion {
  emotion: string;
  date: Date;
}

interface StudentBarChartProps {
  student: Student;
}

const emotionColors = {
  'Happy': '#4caf50', // Green
  'Surprised': '#ffeb3b', // Yellow
  'Neutral': '#9e9e9e', // Gray
};

const StudentBarChart: React.FC<StudentBarChartProps> = ({ student }) => {
  const getStudentBarData = (): ChartData => {
    const emotions: Emotion[] = student.emotions;
    const startTime = moment.min(emotions.map(e => moment(e.date))).startOf('minute');
    const endTime = moment.max(emotions.map(e => moment(e.date))).endOf('minute');

    const uniqueEmotions = Array.from(new Set(emotions.map(e => e.emotion)));


    const data: ChartData = {
      labels: [],
      datasets: uniqueEmotions.map(emotion => ({
        label: emotion,
        data: [],
        backgroundColor: emotionColors[emotion] || '#ff6384', // Red as a fallback color
      })),
    };

    let currentTime: Moment = startTime.clone();
    while (currentTime.isBefore(endTime)) {
      data.labels.push(currentTime.format('HH:mm'));
  
      data.datasets.forEach(dataset => {
        const emotionCounts: Record<string, number> = {};
        emotions.forEach(emotion => {
          const emotionTime = moment(emotion.date);
          if (
            emotionTime.isSameOrAfter(currentTime) &&
            emotionTime.isBefore(currentTime.clone().add(5, 'minutes')) &&
            emotion.emotion === dataset.label
          ) {
            emotionCounts[emotion.emotion] = (emotionCounts[emotion.emotion] || 0) + 1;
          }
        });
        dataset.data.push(emotionCounts[dataset.label] || 0);
      });
  
      currentTime.add(5, 'minutes');
    }
  
    return data;
  };
  const chartData = getStudentBarData();

  return <Bar data={chartData} />;
};

export default StudentBarChart;