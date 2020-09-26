import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EMPTY_STRING, TEMPORARY_PASSWORD_MESSAGE } from 'src/app/utils/constants';
import { AlertService } from 'src/app/services/alert.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  private checkoutForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private alertService: AlertService,
    private usersService: UserService
  ) {
    this.checkoutForm = this.formBuilder.group({
      email: EMPTY_STRING
    });
  }

  ngOnInit(): void {
  }

  getCheckoutForm() {
    return this.checkoutForm;
  }

  resetPassword() {
    let formData = this.getCheckoutForm().value;

    if (formData.email !== EMPTY_STRING) {
      this.usersService.resetPassword(formData.email).subscribe(() => {
        this.alertService.alertSuccess(TEMPORARY_PASSWORD_MESSAGE);
      });
    }
  }
}
