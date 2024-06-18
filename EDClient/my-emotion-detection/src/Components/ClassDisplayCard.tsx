import { Card, CardContent, Typography } from "@mui/material";
import React from 'react';
import ClassDisplay from "../Objects/ClassDisplay";

interface ClassDisplayCardProps {
  Class: ClassDisplay;
  onClick: (c: ClassDisplay) => void;
}

const ClassDisplayCard: React.FC<ClassDisplayCardProps> = ({ Class, onClick }) => {
  return (
    <Card
      sx={{
        width: 200,
        borderRadius: 2,
        marginBottom: 10,
        background: "#ede5e5",
        cursor: "pointer" // Indicate that the card is clickable

      }}
      onClick={() => onClick(Class)} // Add onClick handler
    >
      <CardContent>
        <div>
          <Typography variant="body1" gutterBottom>
            Name: {Class.name}
          </Typography>
          <Typography variant="body1" gutterBottom>
            Date: {String(Class.date)}
          </Typography>
          <Typography variant="body1" gutterBottom>
            Description: {Class.description}
          </Typography>
        </div>
      </CardContent>
    </Card>
  );
};

export default ClassDisplayCard;
