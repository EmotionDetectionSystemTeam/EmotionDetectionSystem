// ClassEmotionChart.tsx
import React from "react";
import { Bar } from "react-chartjs-2";
import Student from "../Objects/Student";

interface ClassEmotionChartProps {
  student: Student;
}

const ClassEmotionChart: React.FC<ClassEmotionChartProps> = ({ student }) => {
  const emotionCounts = student.emotions.reduce((acc, emotion) => {
    acc[emotion.emotion] = (acc[emotion.emotion] || 0) + 1;
    return acc;
  }, {} as Record<string, number>);

  const chartData = {
    labels: Object.keys(emotionCounts),
    datasets: [
      {
        label: `Emotions in ${student.className}`,
        data: Object.values(emotionCounts),
        backgroundColor: ["#ff6384", "#36a2eb", "#ffce56", "#4bc0c0", "#9966ff"],
      },
    ],
  };

  return <Bar data={chartData} />;
};

export default ClassEmotionChart;