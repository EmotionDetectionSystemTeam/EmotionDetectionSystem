// StudentCard.tsx
import { Card, CardContent, Typography } from "@mui/material";
import React from 'react';
import Student from "../Objects/Student";
import StudentDisplay from "../Objects/StudentDisplay";

interface StudentCardProps {
  student: Student | StudentDisplay;
  onClick: () => void;
}

const StudentCard: React.FC<StudentCardProps> = ({ student, onClick }) => {
  return (
    <Card sx={{
      width: 200,
      borderRadius: 2,
      marginBottom: 10,
      background: "#ede5e5",
      cursor: "pointer" // Indicate that the card is clickable

    }}
    onClick={onClick}>
      <CardContent>
        <Typography variant="h6">{student.name + '\n' + student.email}</Typography>

      </CardContent>
    </Card>
  );
};

export default StudentCard;
