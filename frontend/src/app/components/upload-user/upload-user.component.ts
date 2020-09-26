import { Component, OnInit } from '@angular/core';
import { Output, EventEmitter } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { AlertService } from 'src/app/services/alert.service';
import { INVALID_FILE_MESSAGE, EMPTY_STRING } from 'src/app/utils/constants';

@Component({
  selector: 'app-upload-user',
  templateUrl: './upload-user.component.html',
  styleUrls: ['./upload-user.component.css']
})
export class UploadUserComponent implements OnInit {

  public fileName: string = EMPTY_STRING;

  @Output() public onUploadFinished = new EventEmitter();

  constructor(
    private alertService: AlertService,
    private userService: UserService) { }

  ngOnInit() {
  }
  
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append(fileToUpload.name, fileToUpload, fileToUpload.name);

    let userId = localStorage.getItem("userId");

    this.userService.uploadFile(formData, userId)
      .subscribe(result => {
        this.alertService.alertShowTimer();
        if (result != null) {
          this.fileName = fileToUpload.name;
          this.onUploadFinished.emit(result);
        }
        else {
          this.alertService.alertError(INVALID_FILE_MESSAGE);
        }
      }
      );
  }
}

