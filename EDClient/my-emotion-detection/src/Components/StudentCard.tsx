import { Card, CardContent, Typography } from "@mui/material";
import React from 'react';
import Student from "../Objects/Student";

interface StudentCardProps {
  student: Student;
}

const StudentCard: React.FC<StudentCardProps> = ({ student }) => {
  return (
    <Card
      sx={{
        width: 200,
        border: `2px solid #000`,
        borderRadius: 2,
        marginBottom: 10,
        background: "#f0f0f0",
      }}
    >
      <CardContent>
        <Typography variant="body1" gutterBottom>
          Name: {student.name}
        </Typography>
      </CardContent>
    </Card>
  );
};

export default StudentCard;
