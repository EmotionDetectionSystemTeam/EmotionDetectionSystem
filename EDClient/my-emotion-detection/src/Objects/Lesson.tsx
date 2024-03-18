import { ServiceRealTimeUser } from "./ServiceRealTimeUser";

export class Lesson {
    public LessonId: string;
    public LessonName: string;
    public Teacher: string;
    public Date: Date;
    public IsActive: boolean;
    public EntryCode: string;
    public studentsQuantity: number;
    public StudentsEmotions : ServiceRealTimeUser[];

    constructor(
        lessonId: string,
        lessonName: string,
        teacher: string,
        date: Date,
        isActive: boolean,
        entryCode: string,
        studentsQuantity: number,
        studentsEmotions : ServiceRealTimeUser[]
    ) {
        this.LessonId = lessonId;
        this.LessonName = lessonName;
        this.Teacher = teacher;
        this.Date = date;
        this.IsActive = isActive;
        this.EntryCode = entryCode;
        this.studentsQuantity = studentsQuantity;
        this.StudentsEmotions = studentsEmotions;
    }

}
