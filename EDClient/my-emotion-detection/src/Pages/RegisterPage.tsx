import { Checkbox, FormControlLabel, FormGroup } from "@mui/material";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Grid from "@mui/material/Grid";
import Link from "@mui/material/Link";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import { ThemeProvider } from "@mui/material/styles";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import { NotificationMessage } from "../Components/NotificationMessage";
import { pathHome, pathStudentLogin, pathTeacherLogin } from "../Paths";
import { serverRegister } from "../Services/ClientService";
import { getSessionId, setCookie } from "../Services/SessionService";
import { formatMessage, mainTheme, squaresColor } from "../Utils";

function Register() {
  const navigate = useNavigate();

  const [isStudent, setIsStudent] = React.useState(true);
  const [isTeacher, setIsTeacher] = React.useState(false);
  const [showMessage, setShowMessage] = React.useState(false);
  const [messageContent, setMessageContent] = React.useState('');
  const [messageType, setMessageType] = React.useState<'error' | 'success'>('error');

  const handleClose = () => {
    setShowMessage(false);
  };

  const displayMessage = (message: string, type: 'error' | 'success') => {
    setMessageContent(formatMessage(message));
    setMessageType(type);
    setShowMessage(true);
  };

  const handleStudentChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.checked) {
      setIsStudent(true);
      setIsTeacher(false);
    }
  };

  const handleTeacherChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.checked) {
      setIsTeacher(true);
      setIsStudent(false);
    }
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const email = data.get("email")?.toString();
    const firstName = data.get("firstName")?.toString();
    const lastName = data.get("lastName")?.toString();
    const password = data.get("password")?.toString();
    const confirmPassword = data.get("confirmPassword")?.toString();

    if (password !== confirmPassword) {
      displayMessage("Passwords do not match!", 'error');
      return; // Exit the function if passwords don't match
    }

    const isStudentValue = isStudent ? 0 : 1;
    serverRegister(email, firstName, lastName, password, confirmPassword, isStudentValue)
      .then((response: string) => {
        isStudent ? navigate(pathStudentLogin) : navigate(pathTeacherLogin);

        setCookie(getSessionId(), "email", email);
      })
      .catch((e) => {
        displayMessage(e, 'error');
      });
  };

  return (
    <ThemeProvider theme={mainTheme}>

      <Grid
        item
        xs={false}
        sm={6}
        md={7}
      />
      <Grid
        item
        xs={12}
        sm={6}
        md={5}
        component={Box}
        display="flex"
        alignItems="center"
        justifyContent="center"
        sx={{
          height: "90vh",
          p: 4,
        }}
      >
        <Box
          sx={{
            width: "100%",
            maxWidth: 400,
            my: 8,
            mx: 4,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            boxShadow: 3,
            p: 4,
            backgroundColor: "white",
            borderRadius: 2,
          }}
        >
          <Typography component="h1" variant="h5" gutterBottom>
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
            <FormGroup row sx={{ mb: 2 }}>
              <Typography variant="body1" sx={{ mr: 2, alignSelf: 'center' }}>I am a</Typography>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={isStudent}
                    onChange={handleStudentChange}
                    color="primary"
                    name="isStudent"
                  />
                }
                label="Student"
              />
              <FormControlLabel
                control={
                  <Checkbox
                    checked={isTeacher}
                    onChange={handleTeacherChange}
                    color="primary"
                    name="isTeacher"
                  />
                }
                label="Teacher"
              />
            </FormGroup>
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label={isStudent ? "User Name" : "Email"}
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
              type="password"
              id="confirmPassword"
              autoComplete="current-password"
              variant="outlined"
            />
            <Button
              type="submit"
              fullWidth
              sx={{
                mt: 3,
                mb: 2,
                color: "white",
                backgroundColor: "#1976d2",
                "&:hover": { backgroundColor: "#1565c0" },
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
            <Link href="/" align="center" variant="body2">
              Already have an account? Sign in
            </Link>
          </Box>
        </Box>
      </Grid>
      {showMessage && (
        <NotificationMessage
          message={messageContent}
          onClose={handleClose}
          type={messageType}
        />
      )}
    </ThemeProvider>
  );
}

export default Register;
