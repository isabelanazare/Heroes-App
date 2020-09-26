import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { User } from 'src/app/models/user';
import { EMPTY_STRING } from 'src/app/utils/constants';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Router } from '@angular/router';
import { URL } from 'src/app/utils/config';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  private _checkoutForm: FormGroup;
  public user: User = null;
  public response: { 'dbPath': '' };

  @Output() public onUploadFinished = new EventEmitter();

  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService
  ) {
    this._checkoutForm = this.formBuilder.group({
      username: EMPTY_STRING
    });
  }

  ngOnInit(): void {
    this._getUser();
  }

  private _getUser() {
    let userId = localStorage.getItem("userId");

    if (userId !== null) {
      this.userService.getUser(parseInt(userId)).subscribe(res => {
        if (res !== undefined) {
          this.user = res;
        }
      });
    }
  }

  public getCheckoutForm() {
    return this._checkoutForm;
  }

  public logOut() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

  public createImgPath = (serverPath: string) => {
    return `${URL}/${serverPath}`;
  }
}
