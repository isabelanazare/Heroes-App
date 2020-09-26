import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { AlertService } from 'src/app/services/alert.service';
import { EMPTY_STRING, PASSWORD_FAILED_MATCH_MESSAGE, INVALID_USER_MESSAGE, ACCOUNT_CREATED_MESSAGE, DEFAULT_PICTURE_PATH } from 'src/app/utils/constants';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  private checkoutForm: FormGroup;
  public message: string;

  @Output() public onUploadFinished = new EventEmitter();

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {
    this.checkoutForm = this.formBuilder.group({
      username: EMPTY_STRING,
      email: EMPTY_STRING,
      password: EMPTY_STRING,
      confirmedPassword: EMPTY_STRING
    });
  }

  ngOnInit(): void {
  }

  public getCheckoutForm() {
    return this.checkoutForm;
  }

  private _getUserDataFromForm(formData): User {
    if (formData.username !== EMPTY_STRING && formData.email !== EMPTY_STRING && formData.password !== EMPTY_STRING && formData.confirmedPassword !== EMPTY_STRING) {
      if (formData.password === formData.confirmedPassword) {
        return new User(
          formData.username,
          formData.email,
          DEFAULT_PICTURE_PATH,
          formData.password
        );
      }
      else { this.alertService.alertError(PASSWORD_FAILED_MATCH_MESSAGE); }
    }
    else this.alertService.alertError(INVALID_USER_MESSAGE);
    return null;
  }

  public onCreate() {
    let formData = this.getCheckoutForm().value;
    let user = this._getUserDataFromForm(formData);

    if (user !== null) {
      this.userService.addUser(user)
        .subscribe(res => {
          this.alertService.alertLoading();
          this.alertService.alertSuccess(ACCOUNT_CREATED_MESSAGE);
        });
    }
  }
}