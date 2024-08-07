import ClassDisplay from "../Objects/ClassDisplay";
import { ServiceEmotionData } from "../Objects/EmotionData";
import { Lesson } from "../Objects/Lesson";
import { ServiceRealTimeUser } from "../Objects/ServiceRealTimeUser";
import Student from "../Objects/Student";
import StudentDisplay from "../Objects/StudentDisplay";
import StudentOverview from "../Objects/StudentOverview";
import checkInput, { serverPort } from "../Utils";
import ClientResponse from "./Response";
import { getLessonId, getSessionId, getUserName, setIsGuest } from "./SessionService";

export async function serverEnterAsGuest(): Promise<string> {
  const uri = serverPort + "/api/eds/enter-as-guest";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*",
      },
      body: JSON.stringify({
        sessionID: "dummySession", // Pass the appropriate session ID value here
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      alert(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverRegister(
  email: string | undefined | null,
  firstName: string | undefined | null,
  lastName: string | undefined | null,
  password: string | undefined | null,
  confirmPassword: string | undefined | null,
  isStudent: number | undefined | null,
): Promise<string> {
  const fields: any[] = [email, firstName, lastName, password, isStudent];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/register";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: email,
        firstName: firstName,
        lastName: lastName,
        password: password,
        confirmPassword: confirmPassword,
        isStudent: isStudent,
      }),
    });
    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    //const response = JSON.parse(responseText);
    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverLogin(
  email: string | undefined | null,
  password: string | undefined | null
): Promise<string> {
  const fields: any[] = [email, password];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/login";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      // body: '{\n  "userName": "string",\n  "password": "string"\n}',
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: email,
        Password: password,
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    setIsGuest(false);
    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverJoinLesson(
  classCode: string | undefined | null,
): Promise<Lesson> {
  const fields: any[] = [classCode];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/join-lesson";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      // body: '{\n  "userName": "string",\n  "password": "string"\n}',
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
        EntryCode: classCode
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    const lesson: Lesson = new Lesson(
      response.value.lessonId,
      response.value.lessonName,
      response.value.teacher,
      response.value.date,
      response.value.isActive,
      response.value.entryCode,
      response.value.studentsQuantity,
      response.value.studentsEmotions
    )


    return lesson;
  } catch (e) {
    return Promise.reject(e);
  }
}
export async function serverCreateLesson(
  title: string | undefined | null,
  description: string | undefined | null,
  tags: string[] | undefined | null
): Promise<Lesson> {
  const fields: any[] = [title, description];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/create-lesson";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      // body: '{\n  "userName": "string",\n  "password": "string"\n}',
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
        Title: title,
        Description: description,
        Tags: tags
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    const lesson: Lesson = new Lesson(
      response.value.lessonId,
      response.value.lessonName,
      response.value.teacher,
      response.value.date,
      response.value.isActive,
      response.value.entryCode,
      response.value.studentsQuantity,
      response.value.studentsEmotions

    )

    return lesson;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverLogout(
): Promise<string> {
  const uri = serverPort + "/api/eds/logout";
  console.log(getSessionId());
  console.log(getUserName());
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      // body: '{\n  "userName": "string",\n  "password": "string"\n}',
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }


    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverEndLesson(
): Promise<string> {
  const uri = serverPort + "/api/eds/end-lesson"; // Adjust the URI endpoint accordingly
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName()
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverGetLastEmotionsData(
  lessonId: string | undefined | null
): Promise<ServiceRealTimeUser[]> {
  const fields: any[] = [lessonId];
  if (!checkInput(fields)) return Promise.reject();
  const uri = `${serverPort}/api/eds/get-last-emotions-data`;
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
        LessonId: lessonId
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    const students: ServiceRealTimeUser[] = response.value.map((student: any) => {
      return new ServiceRealTimeUser(
        student.email,
        student.firstName,
        student.lastName,
        student.winingEmotion,
        student.previousEmotions
      )
    })

    return students;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverPushEmotionData(
  lessonId: string | undefined | null,
  emotionData: ServiceEmotionData
): Promise<string> {
  const fields: any[] = [lessonId, emotionData];
  if (!checkInput(fields)) return Promise.reject();
  const uri = `${serverPort}/api/eds/push-emotion-data`;
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
        LessonId: lessonId,
        EmotionData: emotionData
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverGetLesson(
  lessonId: string | undefined | null
): Promise<Lesson> {
  const fields: any[] = [lessonId];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/get-lesson"; // Update the endpoint
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
        LessonId: lessonId
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    const lesson: Lesson = new Lesson(
      response.value.lessonId,
      response.value.lessonName,
      response.value.teacher,
      response.value.date,
      response.value.isActive,
      response.value.entryCode,
      response.value.studentsQuantity,
      response.value.studentsEmotions
    )

    return lesson;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverNotifyEmotion(
  teacherEmail: string | undefined | null
): Promise<string> {
  const fields: any[] = [teacherEmail];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/emotion-notification"; // Update the endpoint
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        TeacherEmail: teacherEmail,
        StudentEmail: getUserName()
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverGetEnrolledLessons(
): Promise<ClassDisplay[]> {
  const uri = serverPort + "/api/eds/get-enrolled-lessons"; // Update the endpoint
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        TeacherEmail: getUserName(),
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    const myClasses: ClassDisplay[] = response.value.map((lesson: any) => {
      return new ClassDisplay(
        lesson.lessonId,
        lesson.lessonName,
        lesson.date,
        lesson.description,
      );
    });

    return myClasses;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverGetStudentDataByLesson(
  lessonId: string | undefined | null
): Promise<Student[]> {
  const fields: any[] = [lessonId];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/get-students-data-by-lesson";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
        LessonId: lessonId,
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    const myClasses: Student[] = response.value.map((student: any) => {
      let s = new Student(
        student.email,
        student.firstName + " " + student.lastName,
        student.className,
        student.emotions,

      );
      s.setApproaches(student.teacherApproach);
      return s;
    });

    return myClasses;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverGetAllStudentData(
): Promise<StudentDisplay[]> {
  const uri = serverPort + "/api/eds/get-all-student-data";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    const students: StudentDisplay[] = response.value.map((studentOverview: any) => {
      return new StudentDisplay(
        studentOverview.email,
        studentOverview.firstName + " " + studentOverview.lastName,
      );
    });

    return students;
  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverGetStudentData(
  email: string | undefined | null
): Promise<StudentOverview> {
  const fields: any[] = [email];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/get-student-data";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        TeacherEmail: getUserName(),
        StudentEmail: email,
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    let students: Student[] = response.value.lessons.map((student: any) => {
      return new Student(
        student.email,
        student.firstName + " " + student.lastName,
        student.className,
        student.emotions,
      )
    })
    return new StudentOverview(
      response.value.email,
      response.value.firstName + " " + response.value.lastName,
      students,
    );
    ;

  } catch (e) {
    return Promise.reject(e);
  }
}

export async function serverLeaveLesson(
): Promise<string> {
  const uri = serverPort + "/api/eds/leave-lesson";
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      // body: '{\n  "userName": "string",\n  "password": "string"\n}',
      body: JSON.stringify({
        SessionId: getSessionId(),
        Email: getUserName(),
        LessonId: getLessonId(),
      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }

}

export async function serverAddTeacherApproach(
  studentEmail: string | undefined | null
): Promise<string> {
  const fields: any[] = [studentEmail];
  if (!checkInput(fields)) return Promise.reject();
  const uri = serverPort + "/api/eds/teacher-approach"
  try {
    const jsonResponse = await fetch(uri, {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        SessionId: getSessionId(),
        TeacherEmail: getUserName(),
        LessonId: getLessonId(),
        StudentEmail: studentEmail,

      }),
    });

    if (!jsonResponse.ok) {
      const errorResponse: ClientResponse<string> = await jsonResponse.json();
      throw new Error(errorResponse.errorMessage);
    }

    const response: ClientResponse<string> = await jsonResponse.json();
    // Handle empty response
    if (!response) {
      throw new Error("Empty response received");
    }

    return response.value;
  } catch (e) {
    return Promise.reject(e);
  }
}


