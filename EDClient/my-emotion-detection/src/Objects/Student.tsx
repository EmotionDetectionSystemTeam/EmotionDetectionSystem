import Emotion from "./Emotion";

class Student {
  public email: string;
    public name: string;
    public className: string;
    public emotions: Emotion[];
  
    constructor(email: string, name: string, classname: string, emotions: Emotion[]) {
      this.email = email;
      this.name = name;
      this.className = classname;
      this.emotions = emotions;
    }
  }
  
  export default Student;
  