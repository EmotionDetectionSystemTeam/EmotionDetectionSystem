// StudentEmotionChart.tsx
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

  return <Bar data={chartData} />;
};

// Helper function to generate random colors (you can use a library like `randomcolor` instead)
function getRandomColor() {
  const letters = "0123456789ABCDEF";
  let color = "#";
  for (let i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
}

export default StudentEmotionChart;