import { EMPTY_STRING } from '../utils/constants';

export class User {
  public id?: number;
  public username: string = EMPTY_STRING;
  public email: string = EMPTY_STRING;
  public imgPath: string = EMPTY_STRING;
  public password: string = EMPTY_STRING;
  public token?: string;

  constructor(username: string, email: string, imgPath: string, password: string) {
    this.username = username;
    this.email = email;
    this.imgPath = imgPath;
    this.password = password;
  }
}