import { createTheme } from "@mui/material";

export const textColor = "black";
export const serverPort = "https://localhost:7069";
export const squaresColor = "#E8F0FE";


export const makeSetStateFromEvent = (setState: any) => {
    return (event: React.ChangeEvent<HTMLInputElement>) => {
      setState(event.target.value);
    };
  };

export const zip = <T, Z>(arr: T[], arr2: Z[]): [T, Z][] =>
  arr.map((a, i) => [a, arr2[i]]);
export const notificationsPort = 4560;
export const logsPort = 4560;

export default function checkInput(fields: any[]): boolean {
    for (const field of fields) {
      if (field === undefined) {
        return false;
      }
    }
    return true;
  }

export const mainTheme = createTheme({
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