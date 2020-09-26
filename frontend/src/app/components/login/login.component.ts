import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EMPTY_STRING, FAILED_LOGIN_MESSAGE, INVALID_USER_MESSAGE } from 'src/app/utils/constants';
import { User } from 'src/app/models/user';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private checkoutForm: FormGroup;
  public user: User = null;
  public error: string = EMPTY_STRING;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
  ) {
    this.checkoutForm = this.formBuilder.group({
      email: EMPTY_STRING,
      password: EMPTY_STRING
    });
  }

  ngOnInit(): void {
  }

  getCheckoutForm() {
    return this.checkoutForm;
  }

  public login() {
    let formData = this.getCheckoutForm().value;

    if (formData.email !== EMPTY_STRING && formData.password !== EMPTY_STRING) {
      this.authenticationService.login(formData.email, formData.password)
        .pipe(first())
        .subscribe(
          () => {
            this.router.navigate(['/main']);
          },
          error => {
            this.error = error;
            this.alertService.alertError(FAILED_LOGIN_MESSAGE);
          });
    }
    else this.alertService.alertError(INVALID_USER_MESSAGE);
  }
}
