import Emotion from "./Emotion";

class Student {
    public id: number;
    public name: string;
    public emotions: Emotion[];
  
    constructor(id: number, name: string, emotions: Emotion[]) {
      this.id = id;
      this.name = name;
      this.emotions = emotions;
    }
  }
  
  export default Student;
  