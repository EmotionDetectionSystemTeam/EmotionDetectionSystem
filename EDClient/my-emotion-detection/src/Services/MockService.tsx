import Class from "../Objects/Class";
import ClassDisplay from "../Objects/ClassDisplay";
import Emotion from "../Objects/Emotion";
import Student from "../Objects/Student";
import StudentDisplay from "../Objects/StudentDisplay";
import StudentOverview from "../Objects/StudentOverview";

export const ServerMockStudentOverview = async (studentId) => {
    // Mock data for demonstration purposes
    return new StudentOverview(
      "alice@example.com",
      "Alice Johnson",
      [
        new Student(
          "alice@example.com",
          "Alice Johnson",
          "Math 101",
          [
            new Emotion("Happy", new Date("2023-05-25T10:00:00")),
            new Emotion("Sad", new Date("2023-05-25T10:05:00")),
            new Emotion("Surprised", new Date("2023-05-25T10:10:00")),
            new Emotion("Neutral", new Date("2023-05-25T10:15:00")),
            new Emotion("Happy", new Date("2023-05-25T10:20:00")),
          ]
        ),
        new Student(
          "alice@example.com",
          "Alice Johnson",
          "History 201",
          [
            new Emotion("Surprised", new Date("2023-05-26T10:00:00")),
            new Emotion("Surprised", new Date("2023-05-26T10:05:00")),
            new Emotion("Neutral", new Date("2023-05-26T10:10:00")),
            new Emotion("Neutral", new Date("2023-05-26T10:15:00")),
            new Emotion("Neutral", new Date("2023-05-26T10:20:00")),
          ]
        ),
        new Student(
          "alice@example.com",
          "Alice Johnson",
          "Algebra 301",
          [
            new Emotion("Happy", new Date("2023-05-26T10:00:00")),
            new Emotion("Happy", new Date("2023-05-26T10:05:00")),
            new Emotion("Sad", new Date("2023-05-26T10:10:00")),
            new Emotion("Neutral", new Date("2023-05-26T10:15:00")),
            new Emotion("Happy", new Date("2023-05-26T10:20:00")),
          ]
        ),
        new Student(
          "alice@example.com",
          "Alice Johnson",
          "Physics 999",
          [
            new Emotion("Surprised", new Date("2023-05-26T10:00:00")),
            new Emotion("Sad", new Date("2023-05-26T10:05:00")),
            new Emotion("Sad", new Date("2023-05-26T10:10:00")),
            new Emotion("Neutral", new Date("2023-05-26T10:15:00")),
            new Emotion("Sad", new Date("2023-05-26T10:20:00")),
          ]
        ),
        
      ]
    );
  }

  export const ServerMockGetClass = async (id: number): Promise<Class> => {
    // Mock data for a specific class lesson
  
    // Generate mock students with emotions over time
    const students: Student[] = [
      new Student("alice@example.com", "Alice","Math 101", [
        new Emotion("Happy", new Date("2023-05-25T10:03:00")),
        new Emotion("Happy", new Date("2023-05-25T10:03:00")),
        new Emotion("Happy", new Date("2023-05-25T10:03:00")),
        new Emotion("Happy", new Date("2023-05-25T10:03:00")),
        new Emotion("Happy", new Date("2023-05-25T10:03:00")),
  
        new Emotion("Happy", new Date("2023-05-25T10:00:00")),
  
        new Emotion("Sad", new Date("2023-05-25T10:05:00")),
        new Emotion("Happy", new Date("2023-05-25T10:10:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:15:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:20:00")),
        new Emotion("Sad", new Date("2023-05-25T10:25:00")),
        new Emotion("Happy", new Date("2023-05-25T10:30:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:35:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:40:00")),
        new Emotion("Sad", new Date("2023-05-25T10:45:00")),
      ]),
      new Student("bob@example.com", "Bob","Math 101", [
        new Emotion("Happy", new Date("2023-05-25T10:00:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:05:00")),
        new Emotion("Sad", new Date("2023-05-25T10:10:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:15:00")),
        new Emotion("Happy", new Date("2023-05-25T10:20:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:25:00")),
        new Emotion("Sad", new Date("2023-05-25T10:30:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:35:00")),
        new Emotion("Happy", new Date("2023-05-25T10:40:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:45:00")),
      ]),
      new Student("charlie@example.com", "Charlie","Math 101", [
        new Emotion("Happy", new Date("2023-05-25T10:00:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:05:00")),
        new Emotion("Happy", new Date("2023-05-25T10:10:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:15:00")),
        new Emotion("Sad", new Date("2023-05-25T10:20:00")),
        new Emotion("Happy", new Date("2023-05-25T10:25:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:30:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:35:00")),
        new Emotion("Sad", new Date("2023-05-25T10:40:00")),
        new Emotion("Happy", new Date("2023-05-25T10:45:00")),
      ]),
      new Student("david@example.com", "David","Math 101", [
        new Emotion("Happy", new Date("2023-05-25T10:00:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:05:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:10:00")),
        new Emotion("Sad", new Date("2023-05-25T10:15:00")),
        new Emotion("Happy", new Date("2023-05-25T10:20:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:25:00")),
        new Emotion("Neutral", new Date("2023-05-25T10:30:00")),
        new Emotion("Sad", new Date("2023-05-25T10:35:00")),
        new Emotion("Happy", new Date("2023-05-25T10:40:00")),
        new Emotion("Surprised", new Date("2023-05-25T10:45:00")),
      ]),
    ];
  
  
    return new Class(
      id,
      "Math 101",
      "Introduction to Algebra",
      new Date("2023-05-25T10:00:00"),
      students
    );
  };

  export const ServerMockGetClasses = async (): Promise<ClassDisplay[]> => {
    // Mock data for class displays
    return [
      new ClassDisplay(1, "Math 101", "2023-05-25", "Introduction to Algebra"),
      new ClassDisplay(2, "History 201", "2023-05-26", "World War II Overview"),
      new ClassDisplay(3, "Science 301", "2023-05-27", "Basics of Physics")
    ];
  };

  export const ServerMockGetStudentsData = async (): Promise<StudentDisplay[]> => {
    return [
        new StudentDisplay('john.doe@example.com', 'John Doe'),
        new StudentDisplay('jane.smith@example.com','Jane Smith'),
        new StudentDisplay('bob.johnson@example.com', 'Bob Johnson'),
        new StudentDisplay( 'sarah.lee@example.com', 'Sarah Lee'),
        new StudentDisplay('mike.brown@example.com', 'Mike Brown'),
      ]
  }