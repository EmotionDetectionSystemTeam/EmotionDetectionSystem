import {
    CategoryScale,
    Chart as ChartJS,
    Legend,
    LineElement,
    LinearScale,
    PointElement,
    TimeScale,
    TimeSeriesScale,
    Title,
    Tooltip
} from 'chart.js';
import 'chartjs-adapter-date-fns';
import React from 'react';
import { Line } from 'react-chartjs-2';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  TimeScale,
  TimeSeriesScale
);

interface TimeSeriesChartProps {
  data: { [key: string]: { x: Date; y: number }[] };
}

const TimeSeriesChart: React.FC<TimeSeriesChartProps> = ({ data }) => {
  const chartData = {
    datasets: Object.entries(data).map(([emotion, points]) => ({
      label: emotion,
      data: points,
      fill: false,
      borderColor: getRandomColor(),
      backgroundColor: getRandomColor(),
    }))
  };

  const options = {
    scales: {
      x: {
        type: 'timeseries',
        time: {
          unit: 'minute',
        },
        title: {
          display: true,
          text: 'Time'
        }
      },
      y: {
        beginAtZero: true,
        title: {
          display: true,
          text: 'Count'
        }
      }
    }
  };

  return <Line data={chartData} options={options} />;
};

// Utility function to generate random colors for lines
const getRandomColor = () => {
  const letters = '0123456789ABCDEF';
  let color = '#';
  for (let i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
};

export default TimeSeriesChart;
