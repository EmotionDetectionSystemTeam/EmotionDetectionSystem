export class ServiceRealTimeUser {
    public email: string;
    public firstName: string;
    public lastName: string;
    public winingEmotion: string;
    public previousEmotions: string[];

    constructor(email: string, firstName: string, lastName: string, winingEmotion: string, previousEmotions: string[]) {
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.winingEmotion = winingEmotion;
        this.previousEmotions = previousEmotions;
    }
}