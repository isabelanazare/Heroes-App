export class Hero {
  public id?: number;
  public name: string;
  public ally: string;
  public mainPower: string;
  public powers: string[];
  public type: string;
  public birthday: Date;
  public imgPath?: string;

  constructor(name: string, ally: string, mainPower: string, powers: string[], type: string, birthday: Date) {
    this.name = name;
    this.ally = ally;
    this.mainPower = mainPower;
    this.powers = powers;
    this.type = type;
    this.birthday = new Date(birthday);
  }
}