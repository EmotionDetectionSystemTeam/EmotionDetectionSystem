import { Dialog } from "@mui/material";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Checkbox from "@mui/material/Checkbox";
import FormControlLabel from "@mui/material/FormControlLabel";
import Grid from "@mui/material/Grid";
import Link from "@mui/material/Link";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import FailureSnackbar from "../Components/FailureSnackbar";
import SuccessSnackbar from "../Components/SuccessSnackbar";
import { pathHome, pathStudentLogin, pathTeacherLogin } from "../Paths";
import { serverRegister } from "../Services/ClientService";
import { getCookie, getSessionId, setCookie, setUsername } from "../Services/SessionService";
import { squaresColor } from "../Utils";



function Register() {
  const theme = createTheme({
    
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
  const navigate = useNavigate();

  const [isStudent, setIsStudent] = React.useState(false);
  const [openFailSnack, setOpenFailSnack] = React.useState<boolean>(false);
  const [openSuccSnack, setOpenSuccSnack] = React.useState<boolean>(false);
  const [failuretMsg, setFailuretMsg] = React.useState<string>("");
  const [successtMsg, setSuccesstMsg] = React.useState<string>("");





  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const email = data.get("email")?.toString();
    const firstName = data.get("firstName")?.toString();
    const lastName = data.get("lastName")?.toString();
    const password = data.get("password")?.toString();
    const confirmPassword = data.get("confirmPassword")?.toString();

    if (password !== confirmPassword) {
      alert("Passwords do not match!");
      return; // Exit the function if passwords don't match
  }
  
    const isStudentValue = isStudent ? 0 : 1;
    serverRegister(email, firstName, lastName, password, confirmPassword,isStudentValue)
    .then((response : string) => {
      //setOpenSuccSnack(true);
      //setSuccesstMsg(response);
      isStudent ? navigate(pathStudentLogin) : navigate(pathTeacherLogin);

      setUsername(String(email));
      setCookie(getSessionId(), "email", email);
      alert(getCookie(getSessionId(),"email"));
      //alert(getUserName());
      alert(response)})
      .catch((e) => {
        setOpenFailSnack(true);
        setFailuretMsg(String(e));
        alert(e);
      })

  };

  const handleCloseSuccessSnack = () => {
    setOpenSuccSnack(false);
    isStudent ? navigate(pathStudentLogin) : navigate(pathTeacherLogin);
  };
  

  return (
    <ThemeProvider theme={theme}>
      <Grid
        container
        component="main"
        sx={{
          height: "90vh",
        }}
      >
        <Grid item xs={false} sm={6} md={7} />
        <Grid
          sx={{
            height: "90vh",
          }}
        >
          <Box
            sx={{
              my: 8,
              mx: 4,
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}
          >
            <Typography component="h1" variant="h5">
              Register
            </Typography>
            <Box
              component="form"
              noValidate
              onSubmit={handleSubmit}
              sx={{
                mt: 1,
              }}
            >
              <TextField
                margin="normal"
                required
                fullWidth
                id="email"
                label="Email"
                name="email"
                autoComplete="email"
                autoFocus
                variant="outlined"
                type="email"
              />
                <TextField
                margin="normal"
                required
                fullWidth
                id="firstName"
                label="First Name"
                name="firstName"
                autoComplete="firstName"
                variant="outlined"
              />
              <TextField
                margin="normal"
                required
                fullWidth
                name="lastName"
                label="Last Name"
                type="lastName"
                id="lastName"
                autoComplete="current-password"
                variant="outlined"
              />
                <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Password"
                type="password"
                id="password"
                autoComplete="current-password"
                variant="outlined"
              />
                <TextField
                margin="normal"
                required
                fullWidth
                name="confirmPassword"
                label="Confirm Password"
                type="confirmPassword"
                id="confirmPassword"
                autoComplete="current-password"
                variant="outlined"
              />
              <FormControlLabel
                control={<Checkbox checked={isStudent} onChange={(e) => setIsStudent(e.target.checked)} color="primary" name="isStudent" />}
                label="Are you a student?"
              />
              <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{
                  mt: 3,
                  mb: 2,
                  color: "black",
                  backgroundColor: "#fff",
                  "&:hover": { backgroundColor: "#E8F0FE" },
                }}
              >
                Register
              </Button>
              <Button
                fullWidth
                variant="contained"
                href={pathHome}
                sx={{
                  mt: 3,
                  mb: 2,
                  color: "black",
                  backgroundColor: "#fff",
                  "&:hover": { backgroundColor: squaresColor },
                }}
              >
                Back
              </Button>
              <Grid item>
                <Link href="/" variant="body2">
                  Already have an account? Sign in
                </Link>
              </Grid>
            </Box>
          </Box>
        </Grid>
      </Grid>
      <Dialog open={openFailSnack}>
      {FailureSnackbar(failuretMsg, openFailSnack, () =>
          setOpenFailSnack(false)
        )}
      </Dialog>
      <Dialog open={openSuccSnack}>
      {SuccessSnackbar(successtMsg, openSuccSnack, handleCloseSuccessSnack)}
      </Dialog>


    </ThemeProvider>
  );
}

export default Register;