export class ServiceEmotionData {
    time;
    neutral;
    happy;
    sad;
    angry;
    surprised;
    disgusted;
    fearful;

    constructor(neutral, happy, sad, angry, surprised, disgusted, fearful) {
        this.Time = new Date();
        this.Neutral = neutral;
        this.Happy = happy;
        this.Sad = sad;
        this.Angry = angry;
        this.Surprised = surprised;
        this.Disgusted = disgusted;
        this.Fearful = fearful;
    }
}