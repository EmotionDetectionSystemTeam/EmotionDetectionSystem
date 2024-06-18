class ClassDisplay {
    public id: string;
    public name: string;
    public date: Date;
    public description: string;
  
    constructor(id: string, name: string, date: Date, description: string) {
      this.id = id;
      this.name = name;
      this.date = date;
      this.description = description;
    }
  }

  export default ClassDisplay;