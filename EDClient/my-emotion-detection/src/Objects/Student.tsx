import Emotion from "./Emotion";

class Student {
  public email: string;
    public name: string;
    public className: string;
    public emotions: Emotion[];
    public approaches: string[]
  
    constructor(email: string, name: string, classname: string, emotions: Emotion[]) {
      this.email = email;
      this.name = name;
      this.className = classname;
      this.emotions = emotions;
    }
    public setApproaches(Approaches : string[]){
      this.approaches = Approaches;
    }
  }
  
  export default Student;
  