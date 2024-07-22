import Emotion from "./Emotion";

export class ServiceRealTimeUser {
    public email: string;
    public firstName: string;
    public lastName: string;
    public winingEmotion: string;
    public previousEmotions: Emotion[];

    constructor(email: string, firstName: string, lastName: string, winingEmotion: string, previousEmotions: Emotion[]) {
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.winingEmotion = winingEmotion;
        this.previousEmotions = previousEmotions;
    }
}