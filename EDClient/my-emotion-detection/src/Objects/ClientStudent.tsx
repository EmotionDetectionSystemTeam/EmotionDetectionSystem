import Emotion from "./Emotion";

class ClientStudent {
  public name: string;
  public lastName: string;
  public email: string;
  public emotion: string;
  public previousEmotions : Emotion[];
  public color: string;

  constructor(name: string, lastName: string, email: string, emotion: string, previousEmotions : Emotion[], color: string) {
    this.name = name;
    this.lastName = lastName;
    this.email = email;
    this.emotion = emotion;
    this.previousEmotions = previousEmotions;
    this.color = color;
  }
}

export default ClientStudent;
