class ClientStudent {
  public name: string;
  public lastName: string;
  public email: string;
  public emotion: string;
  public color: string;

  constructor(name: string, lastName: string, email: string, emotion: string, color: string) {
    this.name = name;
    this.lastName = lastName;
    this.email = email;
    this.emotion = emotion;
    this.color = color;
  }
}

export default ClientStudent;
