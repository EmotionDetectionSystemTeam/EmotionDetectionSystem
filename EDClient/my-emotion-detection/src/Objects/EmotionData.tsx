export class ServiceEmotionData {
    public Time: Date;
    public Neutral: number;
    public Happy: number;
    public Sad: number;
    public Angry: number;
    public Surprised: number;
    public Disgusted: number;
    public Fearful: number;

    constructor(neutral: number, happy: number, sad: number, angry: number, surprised: number,
                disgusted: number, fearful: number) {
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