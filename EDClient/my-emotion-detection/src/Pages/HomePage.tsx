import { Button, ButtonGroup, Typography, createTheme } from "@mui/material";
import React from 'react';
import * as Paths from "../Paths";
import { textColor } from "../Utils";


const createButton = (name: string, path: string) => {
  return (
    <Button
      href={path}
      style={{ height: 100, width: 500 }}
      key="name"
      variant="outlined"
      size="large"
      color="primary"
      sx={{
        m: 1,
        color: textColor,
        "&:hover": {
          borderRadius: 5,
          color: textColor,
        },
      }}
    >
      {name}
    </Button>
  );
};

const mainTheme = createTheme({
  palette: {
    mode: "light",
  },
  typography: {
    fontFamily: [
      "-apple-system",
      "BlinkMacSystemFont",
      '"Segoe UI"',
      "Roboto",
      '"Helvetica Neue"',
      "Arial",
      "sans-serif",
      '"Apple Color Emoji"',
      '"Segoe UI Emoji"',
      '"Segoe UI Symbol"',
    ].join(","),
  },
});

const buttons = [
    createButton("Enter as Teacher", Paths.pathTeacherLogin),
    createButton("Enter as Student", Paths.pathStudentLogin),
    createButton("Register", Paths.pathRegister),
];

function HomePage() {
  return (
    <>
      <div className={`container ${mainTheme}`}>
        <main>
          <Typography
            color="inherit"
            align="center"
            variant="h2"
            sx={{ mb: 4, mt: { sx: 4, sm: 2 } }}
          >
            Welcome to EMS 
          </Typography>
          <ButtonGroup
            orientation="vertical"
            aria-label="vertical contained button group"
            variant="text"
          >
            {buttons}
          </ButtonGroup>
        </main>
      </div>
    </>
  );
}

export default HomePage;
