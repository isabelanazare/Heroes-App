export class Power {
  public name: string;
  public mainTrait: string;
  public strength: number;
  public element: string;
  public details: string;
  public id?: number;

  constructor(name: string, mainTrait: string, strength: number, element: string, details: string) {
    this.name = name;
    this.mainTrait = mainTrait;
    this.strength = strength;
    this.element = element;
    this.details = details;
  }
}