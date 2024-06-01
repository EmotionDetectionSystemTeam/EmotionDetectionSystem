import { Card, CardContent, Typography } from "@mui/material";
import React from 'react';
import ClassDisplay from "../Objects/ClassDisplay";

interface ClassDisplayCardProps extends ClassDisplay {
  onClick: (id: number) => void;
}

const ClassDisplayCard: React.FC<ClassDisplayCardProps> = ({ id, name, date, description, onClick }) => {
  return (
    <Card
      sx={{
        width: 200,
        border: `5px solid #000`, // Adjust the color as needed
        borderRadius: 2,
        marginBottom: 10,
        background: "#ede5e5",
        cursor: "pointer" // Indicate that the card is clickable

      }}
      onClick={() => onClick(id)} // Add onClick handler
    >
      <CardContent>
        <div>
          <Typography variant="body1" gutterBottom>
            Name: {name}
          </Typography>
          <Typography variant="body1" gutterBottom>
            Date: {date}
          </Typography>
          <Typography variant="body1" gutterBottom>
            Description: {description}
          </Typography>
        </div>
      </CardContent>
    </Card>
  );
};

export default ClassDisplayCard;
