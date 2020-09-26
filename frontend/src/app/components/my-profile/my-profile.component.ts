import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/user';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AlertService } from 'src/app/services/alert.service';
import { EMPTY_STRING, INVALID_USER_MESSAGE, UPDATED_PROFILE_MESSAGE } from 'src/app/utils/constants';
import { Router } from '@angular/router';
import { API_URL } from 'src/app/utils/config';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {
  private _user: User = null;
  public response: { 'dbPath': '' };
  private _checkoutForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private router: Router,
    private alertService: AlertService,
  ) {
    this._checkoutForm = this.formBuilder.group({
      username: EMPTY_STRING,
      email: EMPTY_STRING,
      password: EMPTY_STRING
    });
  }

  ngOnInit(): void {
    let userId = localStorage.getItem("userId");
    if (userId !== null) {
      this.userService.getUser(parseInt(userId)).subscribe(res => {
        if (res !== undefined) {
          this._user = res;
        }
      });
    }
  }

  public getCheckoutForm() {
    return this._checkoutForm;
  }

  private _getUserDataFromForm(formData): User {
    if (formData.username !== EMPTY_STRING && formData.email !== EMPTY_STRING) {
      let user = new User(
        formData.username,
        formData.email,
        this.response.dbPath,
        formData.password);
      user.id = this._user.id;
      return user;
    }
    else {
      this.alertService.alertError(INVALID_USER_MESSAGE);
    }
    return null;
  }

  public onSubmit() {
    let formData = this.getCheckoutForm().value;
    let user = this._getUserDataFromForm(formData);

    if (user) {
      this.userService.updateUser(user)
        .subscribe(res => {
          this.alertService.alertLoading();
          this.alertService.alertSuccess(UPDATED_PROFILE_MESSAGE);
          this.router.navigate(['/main']);
        });
    }
  }

  public createImgPath = (serverPath: string) => {
    return `${API_URL}${serverPath}`;
  }

  public uploadFinished = (event) => {
    this.response = event;
  }
}
