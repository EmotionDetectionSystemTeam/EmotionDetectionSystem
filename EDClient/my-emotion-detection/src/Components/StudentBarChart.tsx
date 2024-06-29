import { BarElement, CategoryScale, Chart, Legend, LinearScale, Tooltip } from 'chart.js';
import moment, { Moment } from 'moment';
import React, { useEffect, useState } from 'react';
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
  intervalMinutes: number;
}

const emotionColors = {
  'Happy': '#4caf50', // Green
  'Surprised': '#f9a201', // Yellow
  'Neutral': '#9e9e9e', // Gray
};

const StudentBarChart: React.FC<StudentBarChartProps> = ({ student, intervalMinutes }) => {

  const [chartData, setChartData] = useState<ChartData | null>(null);
  useEffect(() => {
    // Update the chart data when the intervalMinutes or student.emotions changes
    setChartData(getStudentBarData(intervalMinutes));
  }, [intervalMinutes, student.emotions]);

  const getStudentBarData = (intervalMinutes: number): ChartData => {
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
            emotionTime.isBefore(currentTime.clone().add(intervalMinutes, 'minutes')) &&
            emotion.emotion === dataset.label
          ) {
            emotionCounts[emotion.emotion] = (emotionCounts[emotion.emotion] || 0) + 1;
          }
        });
        dataset.data.push(emotionCounts[dataset.label] || 0);
      });

      currentTime.add(intervalMinutes, 'minutes');
    }

    return data;
  };

  const chartOptions = {
    scales: {
      x: {
        title: {
          display: true,
          text: 'Time of the Emotion',
        },
      },
      y: {
        title: {
          display: true,
          text: 'Amount of Emotion',
        },
        ticks: {
          stepSize: 1, // Ensure the y-axis shows only natural numbers
        },

      },
    },
    plugins: {
      tooltip: {
        callbacks: {
          label: function(context) {
            let label = context.dataset.label || '';
            if (label) {
              label += ': ';
            }
            label += context.raw;
            return label;
          }
        }
      },
      legend: {
        display: true,
      },
    },
  };

  return chartData ? <Bar data={chartData} options={chartOptions} /> : null;
};

export default StudentBarChart;
