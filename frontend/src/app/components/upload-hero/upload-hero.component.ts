import { Component, OnInit, Input } from '@angular/core';
import { Output, EventEmitter } from '@angular/core';
import { HeroService } from '../../services/hero.service';
import { AlertService } from 'src/app/services/alert.service';
import { SharedService } from 'src/app/services/shared.service';
import { INVALID_FILE_MESSAGE, EMPTY_STRING } from 'src/app/utils/constants';
import { Subject, Subscription } from 'rxjs';

@Component({
  selector: 'app-upload-hero',
  templateUrl: './upload-hero.component.html',
  styleUrls: ['./upload-hero.component.css']
})
export class UploadHeroComponent implements OnInit {

  public fileName: string = EMPTY_STRING;
  @Output() public onUploadFinished = new EventEmitter();
  @Input() public selectedHeroId: number;
  private _subscription: Subscription;

  constructor(
    private sharedService: SharedService,
    private alertService: AlertService,
    private heroService: HeroService) { }

  private _files$ = new Subject<any>();

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  ngOnInit() {
    this._subscription = this._files$
      .subscribe(data => {
        const file = data[0];

        if (!file) {
          return;
        }

        if (file.length === 0) {
          return;
        }

        let fileToUpload = <File>file;
        const formData = new FormData();
        formData.append(fileToUpload.name, fileToUpload, fileToUpload.name);

        this.heroService.uploadFile(formData, this.selectedHeroId)
          .subscribe(result => {
            this.alertService.alertShowTimer();
            if (result != null) {
              this.fileName = fileToUpload.name;
              this.onUploadFinished.emit(result);
            }
            else {
              this.alertService.alertError(INVALID_FILE_MESSAGE);
            }
          });
      });
  }

  public uploadFile = (files) => {
    this._files$.next(files);
  }
}
