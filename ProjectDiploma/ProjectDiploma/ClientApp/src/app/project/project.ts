export class Project {

  //public Name: string;
  //public Start: Date;
  //public Finish: Date;
  //public Cost: number;


  public Name: string;
  public Description: string;
  public Risks: string;
  public Start: Date;
  public Finish: Date;
  public Cost: number;
  public StartDate: Date;
  public FinishDate: Date;
  public CostCurrent: number;
  public CostFull: number;
  public Date: Date;
  public IsClosed: boolean;
  public Initializer: University;

  constructor() { }
}

export class University {
  public Name: string;
  public ContactInformation: string;

  constructor() { }
}
