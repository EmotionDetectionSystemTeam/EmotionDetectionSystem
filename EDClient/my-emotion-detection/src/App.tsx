import React from 'react';
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import './App.css'; // Assuming you have a CSS file for styling
import HomePage from './Pages/HomePage';
import Register from './Pages/RegisterPage';
import StudentDashboard from './Pages/StudentDashboard';
import StudentLogin from './Pages/StudentLoginPage';
import TeacherDashboard from './Pages/TeacherDashboard';
import TeacherLogin from './Pages/TeacherLoginPage';
import * as Path from "./Paths";
import { initSession } from "./Services/SessionService";


const App = () => {
  initSession();
  return (
    <Router>
      <Routes>
        <Route path={Path.pathHome} element={<HomePage />} />
        <Route path={Path.pathRegister} element={<Register />} />
        <Route path={Path.pathStudentLogin} element={<StudentLogin />} />
        <Route path={Path.pathTeacherLogin} element={<TeacherLogin />} />
        <Route path={Path.pathStudentDashBoard} element={<StudentDashboard />} />
        <Route path={Path.pathTeacherDashBoard} element={<TeacherDashboard />} />

      </Routes>
    </Router>
  );
}

export default App;
