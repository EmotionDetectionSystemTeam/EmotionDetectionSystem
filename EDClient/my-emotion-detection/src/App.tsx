import Cookies from 'js-cookie';
import React from 'react';
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import './App.css'; // Assuming you have a CSS file for styling
import ClassesDashboard from './Pages/ClasseHistory';
import CreateClass from './Pages/CreateClassPage';
import HomePage from './Pages/HomePage';
import Register from './Pages/RegisterPage';
import StudentDashboard from './Pages/StudentDashboardPage';
import StudentLesson from './Pages/StudentLessonPage';
import StudentLogin from './Pages/StudentLoginPage';
import StudentsHistory from './Pages/StudentsHistory';
import TeacherDashboard from './Pages/TeacherDashboardPage';
import TeacherLesson from './Pages/TeacherLessonPage';
import TeacherLogin from './Pages/TeacherLoginPage';
import * as Path from "./Paths";
import './styles.css';


const App = () => {
  const cookieNames = Object.keys(Cookies.get());
  cookieNames.forEach(cookieName => {
    Cookies.remove(cookieName);
});
  return (
    <Router>
      <Routes>
        <Route path={Path.pathHome} element={<HomePage />} />
        <Route path={Path.pathRegister} element={<Register />} />
        <Route path={Path.pathStudentLogin} element={<StudentLogin />} />
        <Route path={Path.pathTeacherLogin} element={<TeacherLogin />} />
        <Route path={Path.pathStudentDashBoard} element={<StudentDashboard />} />
        <Route path={Path.pathTeacherDashBoard} element={<TeacherDashboard />} />
        <Route path={Path.pathTeacherLesson} element={<TeacherLesson />} />
        <Route path={Path.pathStudentLesson} element={<StudentLesson />} />
        <Route path={Path.pathCreateClass} element={<CreateClass />} />
        <Route path={Path.pathClassesDashboard} element={<ClassesDashboard />} />
        <Route path={Path.pathStudentsHistory} element={<StudentsHistory />} />




      </Routes>
    </Router>
  );
}

export default App;
