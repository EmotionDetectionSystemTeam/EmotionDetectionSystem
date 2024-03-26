import { Logout } from "@mui/icons-material";
import AccountCircle from "@mui/icons-material/AccountCircle";
import NotificationsActiveIcon from "@mui/icons-material/NotificationsActive";
import { BadgeProps, Tooltip } from "@mui/material";
import AppBar from "@mui/material/AppBar";
import Badge from "@mui/material/Badge";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Stack from "@mui/material/Stack";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import { ThemeProvider, createTheme, styled } from "@mui/material/styles";
import * as React from "react";
import { useNavigate } from "react-router-dom";
import { pathHome } from "../Paths";
import { serverLogout } from "../Services/ClientService";
import {
  clearSession,
  getIsGuest,
  initSession,
} from "../Services/SessionService";
import logo from "../assets/Logo.png";


const StyledBadge = styled(Badge)<BadgeProps>(({ theme }) => ({
  "& .MuiBadge-badge": {
    right: -3,
    top: 13,
    border: `2px solid ${theme.palette.background.paper}`,
    padding: "0 4px",
  },
}));

export default function Navbar() {
  const navigate = useNavigate();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [numOfNotifications, setNumOfNotifications] = React.useState<number>(0);


  const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };


  const handleClickDashboard = () => {
    //navigate(pathStudentDashBoard);
    //navigate(pathTeacherDashBoard)
  };
  /*
  const handleMyAccountClick = () => {
    setAnchorEl(null);
    if (getIsGuest()) navigate(`${pathLogin}`);
    else navigate(`${pathProfile}`);
  };

  const menuId = "primary-search-account-menu";
  const renderMenuAccount = (
    <Menu
      anchorEl={anchorEl}
      anchorOrigin={{
        vertical: "top",
        horizontal: "right",
      }}
      id={menuId}
      keepMounted
      transformOrigin={{
        vertical: "top",
        horizontal: "right",
      }}
      open={Boolean(anchorEl)}
      onClose={handleMenuClose}
    >
      {getIsGuest() ? (
        <>
          <MenuItem onClick={handleLogin}>Login</MenuItem>
          <MenuItem onClick={handleRegister}>Register</MenuItem>
        </>
      ) : (
        <MenuItem onClick={handleMyAccountClick}>Profile</MenuItem>
      )}
    </Menu>
  );
  */
  /*
  React.useEffect(() => {                               //TODO: Notifcations
    if (!getIsGuest()) {
      getMessagesNumber()
        .then((notifications: number) => {
          setNumOfNotifications(notifications);
        })
        .catch((e: any) => alert(e));
    }
  }, []);
  */

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

  const handleLogout = () => {
    if (!getIsGuest()) {
      serverLogout();
      clearSession();
      initSession();
      navigate(pathHome);
    }
  };

  return (
    <ThemeProvider theme={theme}>
      <AppBar position="sticky">
        <Toolbar
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            position: "fixed",
            top: 0,
            left: "50%",
            background: "#ede5e5",
            width: "100%",
            transform: "translateX(-50%)",
          }}
        >
          <div>
            <Stack direction="row" marginLeft={5} spacing={2}>
            <img src={logo} alt="Logo" style={{ 
                  width: "190px", // Adjust the width as needed
                  height: "auto", // Let height adjust proportionally
                  marginBottom: "5px", // Adjust margin if necessary
                  marginRight: "10px", // Adjust margin if necessary
             }} />
              <Typography
                onClick={handleClickDashboard}
                variant="h6"
                noWrap
                component="div"
                sx={{
                  color: "black",
                  display: { xs: "none", sm: "block" },
                  "&:hover": {
                    cursor: "pointer",
                  },
                }}
              >
                Emotion Detection Beta v1
              </Typography>
            </Stack>

            <Box sx={{}} />
          </div>
          {/* <div>
            <Box
              component="form"
              noValidate
              onSubmit={(e: any) => {
                handleSearch();
              }}
            ></Box>
          </div> */}
          <div>
            <Box marginRight={5} sx={{ display: { xs: "none", md: "flex" } }}>
              {getIsGuest() ? null : (
                <Tooltip title="Notifications">
                  <IconButton
                    aria-label="notification"
                    size="small"
                    color="default"
                    //component={Link}           //TODO: notfication impl
                    //to={pathNotifications}
                  >
                    <StyledBadge
                      badgeContent={numOfNotifications}
                      color="secondary"
                    >
                      <NotificationsActiveIcon />
                    </StyledBadge>
                  </IconButton>
                </Tooltip>
              )}
              <IconButton
                size="large"
                edge="end"
                aria-label="account of current user"
                //aria-controls={menuId}
                aria-haspopup="true"
                onClick={handleProfileMenuOpen}
                color="default"
              >
                <AccountCircle />
              </IconButton>
              {getIsGuest() ? null : (
                <Tooltip title="Logout">
                  <IconButton
                    size="large"
                    edge="end"
                    aria-label="account of current user"
                    //aria-controls={menuId}
                    aria-haspopup="true"
                    onClick={handleLogout}
                    color="default"
                  >
                    <Logout />
                  </IconButton>
                </Tooltip>
              )}
            </Box>
          </div>
        </Toolbar>
      </AppBar>
    </ThemeProvider>
  );
}
