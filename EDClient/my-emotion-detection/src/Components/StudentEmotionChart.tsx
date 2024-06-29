import React from "react";
import { Bar } from "react-chartjs-2";
import StudentOverview from "../Objects/StudentOverview";
import { emotionColors } from "../Utils";

interface StudentEmotionChartProps {
  studentOverview: StudentOverview;
}

const StudentEmotionChart: React.FC<StudentEmotionChartProps> = ({ studentOverview }) => {
  const allEmotions = studentOverview.classes.flatMap((classData) => classData.emotions);
  const uniqueEmotions = Array.from(new Set(allEmotions.map((emotion) => emotion.emotion)));

  const chartData = {
    labels: studentOverview.classes.map((classData) => classData.className),
    datasets: uniqueEmotions.map((emotion) => ({
      label: emotion,
      data: studentOverview.classes.map((classData) =>
        classData.emotions.filter((e) => e.emotion === emotion).length
      ),
      backgroundColor: emotionColors[emotion] || "#ff6384", // Use the color map or fall back to red
    })),
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

  return <Bar data={chartData} options={chartOptions} />;
};


export default StudentEmotionChart;