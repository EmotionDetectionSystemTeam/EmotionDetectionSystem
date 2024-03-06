export class ServiceRealTimeUser {
    public email: string;
    public firstName: string;
    public lastName: string;
    public winingEmotion: string;

    constructor(email: string, firstName: string, lastName: string, winingEmotion: string) {
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.winingEmotion = winingEmotion;
    }
}