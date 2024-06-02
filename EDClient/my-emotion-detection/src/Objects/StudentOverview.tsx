// StudentOverview.tsx
import Student from "./Student";

class StudentOverview {
  public email: string;
  public name: string;
  public classes: Student[];

  constructor(email: string, name: string, classes: Student[]) {
    this.email = email;
    this.name = name;
    this.classes = classes;
  }
}

export default StudentOverview;