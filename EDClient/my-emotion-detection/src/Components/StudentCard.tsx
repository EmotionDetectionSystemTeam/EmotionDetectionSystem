// StudentCard.tsx
import { Avatar, Box, Card, CardActionArea, CardContent, Typography } from "@mui/material";
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
      width: 220,
      borderRadius: 2,
      marginBottom: 2,
      background: "#f5f5f5",
      boxShadow: "0 4px 8px rgba(0,0,0,0.2)",
      '&:hover': {
        boxShadow: "0 8px 16px rgba(0,0,0,0.2)",
        transform: 'translateY(-4px)',
        transition: '0.3s',
      },
    }}>
      <CardActionArea onClick={onClick}>
        <CardContent>
          <Box display="flex" alignItems="center">
            <Avatar sx={{ marginRight: 2 }}>{student.name.charAt(0)}</Avatar>
            <Typography variant="h6">{student.name}</Typography>
          </Box>
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default StudentCard;
