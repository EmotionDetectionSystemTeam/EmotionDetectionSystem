import React from 'react';
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import './App.css'; // Assuming you have a CSS file for styling
import HomePage from './Pages/HomePage';
import Register from './Pages/RegisterPage';
import StudentLogin from './Pages/StudentLoginPage';
import TeacherLogin from './Pages/TeacherLoginPage';
import * as Path from "./Paths";

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path={Path.pathHome} element={<HomePage />} />
        <Route path={Path.pathRegister} element={<Register />} />
        <Route path={Path.pathStudentLogin} element={<StudentLogin />} />
        <Route path={Path.pathTeacherLogin} element={<TeacherLogin />} />
      </Routes>
    </Router>
  );
}

export default App;
