import { Avatar, Box, Card, CardActionArea, CardContent, Typography } from "@mui/material";
import React from 'react';
import ClientStudent from "../Objects/ClientStudent";
import { emotionColors, getEmotionEmoji } from "../Utils";

interface StudentCardProps {
  student: ClientStudent;
  onClick: () => void;
}



const ActiveStudentCard: React.FC<StudentCardProps> = ({ student, onClick }) => {

return (
    <Card
    sx={{
      width: 220,
      borderRadius: 2,
      border: `5px solid ${emotionColors[student.emotion]}`,

      marginBottom: 2,
      background: "#f5f5f5",
      boxShadow: "0 4px 8px rgba(0,0,0,0.2)",
      '&:hover': {
        boxShadow: "0 8px 16px rgba(0,0,0,0.2)",
        transform: 'translateY(-4px)',
        transition: '0.3s',
      },
    }}
  >
    <CardActionArea onClick={onClick}>
      <CardContent>

        <Box display="flex" alignItems="center">
          <Avatar sx={{ marginRight: 2 }}>{student.name.charAt(0)}</Avatar>

          <Typography variant="h6" gutterBottom>
            {student.name + " " + student.lastName}
          </Typography>
          <Typography variant="h6" sx={{ marginLeft: 1 }}>{getEmotionEmoji(student.emotion)}</Typography>

        </Box>

      </CardContent>
    </CardActionArea>

  </Card>
)}

export default ActiveStudentCard;