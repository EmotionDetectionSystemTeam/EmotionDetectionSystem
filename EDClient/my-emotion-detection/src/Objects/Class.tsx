import Student from "./Student";

class Class {
  public id: number;
  public name: string;
  public description: string;
  public date: Date;
  public students: Student[];

  constructor(id: number, name: string, description: string, date: Date, students: Student[]) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.date = date;
    this.students = students;
  }
}

export default Class;
