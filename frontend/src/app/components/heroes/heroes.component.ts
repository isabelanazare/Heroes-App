import { AgGridAngular } from 'ag-grid-angular';
import { Component, OnInit, ViewChild, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Hero } from '../../models/hero';
import { HeroType } from '../../models/heroType';
import { HeroService } from '../../services/hero.service';
import { AlertService } from 'src/app/services/alert.service';
import { HeroImageRendererComponent } from '../hero-image-renderer/hero-image-renderer.component';
import { DeleteHeroCellComponent } from '../delete-cell/delete-hero-cell.component';
import { SharedService } from 'src/app/services/shared.service';
import { GridOptions, Module, AllCommunityModules } from "@ag-grid-community/all-modules";
import * as moment from 'moment';
import '@ag-grid-community/all-modules/dist/styles/ag-grid.css';
import { EMPTY_STRING, SELECT_HERO_MESSAGE, DELETED_HERO_MESSAGE, UPDATED_HERO_MESSAGE, ADDED_HERO_MESSAGE, DEFAULT_PICTURE_PATH, INVALID_HERO_MESSAGE } from 'src/app/utils/constants';
import { Power } from 'src/app/models/power';
import { PowerService } from 'src/app/services/power.service';
import { TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-heroes',
  templateUrl: './heroes.component.html',
  styleUrls: ['./heroes.component.scss']
})

export class HeroesComponent implements OnInit {
  @ViewChild('agGrid') agGrid: AgGridAngular;

  private _heroes: Hero[] = [];
  private _powers: Power[] = [];
  private _heroTypes: HeroType[] = [];
  private _checkoutForm: FormGroup;

  public response: { 'dbPath': '' };
  public currentHeroId: number;

  public gridOptions: GridOptions;
  public modules: Module[] = AllCommunityModules;

  public modalRef: BsModalRef;
  private _config = {
    backdrop: false,
    ignoreBackdropClick: true
  };

  public columnDefs = [
    { headerName: 'Name', field: 'name', sortable: true, filter: true, checkboxSelection: true },
    { headerName: 'Main Power', field: 'mainPower', sortable: true, filter: true },
    { headerName: 'Ally', field: 'ally', sortable: true, filter: true },
    { headerName: 'Other powers', field: 'powers', sortable: true, filter: true },
    { headerName: 'Birthday', field: 'birthday', sortable: true, filter: true, editable: true },
    {
      headerName: 'Image',
      field: 'value',
      cellRendererFramework: HeroImageRendererComponent,
      colId: 'params',
      width: 300,
    },
    {
      headerName: 'Delete',
      field: 'value',
      cellRendererFramework: DeleteHeroCellComponent,
      colId: 'params',
      width: 180,
    }
  ];

  constructor(
    private modalService: BsModalService,
    private sharedService: SharedService,
    private heroService: HeroService,
    private powerService: PowerService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {
    this._checkoutForm = this.formBuilder.group({
      name: EMPTY_STRING,
      mainPower: EMPTY_STRING,
      ally: EMPTY_STRING,
      powers: EMPTY_STRING,
      type: EMPTY_STRING,
      birthday: EMPTY_STRING
    });

    this.gridOptions = <GridOptions>{
      context: {
        componentParent: this
      },
      rowHeight: 50,
      rowClassRules: {
        'hero-birthday': function (params) {
          return (moment(params.data.birthday).isoWeek() === moment().add(1, 'weeks').startOf('isoWeek').isoWeek());
        }
      }
    };
  }

  ngOnInit(): void {
    this.heroService.getHeroes().subscribe(res => {
      this._heroes = res;
    })
    this.heroService.getHeroTypes().subscribe(res => {
      this._heroTypes = res;
    })
    this.powerService.getPowers().subscribe(res => {
      this._powers = res;
    })
  }

  public openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, this._config);
  }

  public getHeroesArray() {
    return this._heroes;
  }

  public getHeroTypes() {
    return this._heroTypes;
  }

  public getPowersArray() {
    return this._powers;
  }

  public getCheckoutForm() {
    return this._checkoutForm;
  }

  public resetForm() {
    this._checkoutForm.reset();
  }

  public exportDataAsCsv() {
    this.agGrid.api.exportDataAsCsv({ allColumns: true });
  }

  private _getSelectedHero() {
    return this.agGrid.api
      .getSelectedNodes()
      .map(node => node.data)[0];
  }

  private _isHeroSelected() {
    if (this._getSelectedHero() === undefined) {
      this.alertService.alertError(SELECT_HERO_MESSAGE);
      return false;
    }
    return true;
  }

  private _getHeroFromFormData(heroData): Hero {
    let powers = [];

    if (heroData.powers) {
      powers = heroData.powers.map((powerName) => powerName);
      powers.filter(e => e !== heroData.mainPower);
    }
    else {
      powers = null;
    }

    let hero = new Hero(heroData.name, heroData.ally, heroData.mainPower, powers, heroData.type, heroData.birthday);

    if (this.response !== undefined) {
      if (this.response.dbPath) {
        hero.imgPath = this.response.dbPath;
      }
      else {
        hero.imgPath = DEFAULT_PICTURE_PATH;
      }
    }
    else {
      hero.imgPath = DEFAULT_PICTURE_PATH;
    }
    return hero;
  }

  public addHero(): void {
    let formData = this.getCheckoutForm().value;
    let hero = this._getHeroFromFormData(formData);
    if (this.heroService.isHeroValid(hero)) {
      this.heroService.addHero(hero as Hero)
        .subscribe(hero => {
          this._heroes.push(hero);
          this.agGrid.api.updateRowData({ add: [this._heroes[this._heroes.length - 1]] });
          this.response = { 'dbPath': EMPTY_STRING };
          this.alertService.alertSuccess(ADDED_HERO_MESSAGE);
          this.resetForm();
        });
    }
    else {
      this.alertService.alertError(INVALID_HERO_MESSAGE);
      this.resetForm();
    }
  }

  public selectHero() {
    if (this._isHeroSelected()) {
      this.currentHeroId = this._getSelectedHero().id;
      this.sharedService.changeCurrentHeroId(this._getSelectedHero().id);
    }
    else {
      this.alertService.alertError(SELECT_HERO_MESSAGE);
    }
  }

  public editHero() {
    let formData = this.getCheckoutForm().value;
    let hero = this._getHeroFromFormData(formData);
    hero.id = this._getSelectedHero().id;

    if (this.heroService.isHeroValid(hero)) {
      this.heroService.updateHero(hero as Hero)
        .subscribe(hero => {
          for (let i = 0; i < this._heroes.length; i++) {
            if (this._heroes[i].id === hero.id) {
              this._heroes[i] = hero;
              break;
            }
          }
          this.agGrid.api.setRowData(this._heroes);
          this.alertService.alertSuccess(UPDATED_HERO_MESSAGE);
          this.response = { 'dbPath': EMPTY_STRING };
          this.resetForm();
        });
    }
    else {
      this.alertService.alertError(INVALID_HERO_MESSAGE);
      this.resetForm();
    }
    this.resetForm();
  }

  public delete() {
    if (this._isHeroSelected()) {
      this.alertService.alertConfirm().then((result) => {
        if (result.value) {
          this.deleteHero(this._getSelectedHero());
          this.alertService.alertSuccess(DELETED_HERO_MESSAGE);
        }
      })
    }
    else { this.alertService.alertError(SELECT_HERO_MESSAGE); }
  }

  public deleteHero(hero) {
    this.heroService.deleteHero(hero.id).subscribe(() => {
      this.agGrid.api.updateRowData({ remove: [hero] });
      this.alertService.alertSuccess("Hero deleted");
    });
  }

  public uploadFinished = (event) => {
    this.response = event;
  }
}
