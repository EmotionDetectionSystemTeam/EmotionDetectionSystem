// StudentCard.tsx
import { Card, CardContent, Typography } from "@mui/material";
import React from 'react';
import Student from "../Objects/Student";

interface StudentCardProps {
  student: Student;
  onClick: () => void;
}

const StudentCard: React.FC<StudentCardProps> = ({ student, onClick }) => {
  return (
    <Card onClick={onClick}>
      <CardContent>
        <Typography variant="h6">{student.name}</Typography>
      </CardContent>
    </Card>
  );
};

export default StudentCard;
