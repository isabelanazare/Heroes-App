import { AgGridAngular } from 'ag-grid-angular';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Power } from '../../models/power';
import { PowerService } from '../../services/power.service';
import { AlertService } from 'src/app/services/alert.service';
import { DeletePowerCellComponent } from '../delete-cell/delete-power-cell.component';
import { GridOptions, Module, AllCommunityModules } from "@ag-grid-community/all-modules";
import { DELETED_POWER_MESSAGE, SELECT_POWER_MESSAGE, INVALID_POWER_MESSAGE, UPDATED_POWER_MESSAGE, ADDED_POWER_MESSAGE } from 'src/app/utils/constants';
import { TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-powers',
  templateUrl: './powers.component.html',
  styleUrls: ['./powers.component.scss']
})
export class PowersComponent implements OnInit {
  @ViewChild('agGrid') agGrid: AgGridAngular;

  private _powers: Power[] = [];
  private _checkoutForm: FormGroup;

  public gridOptions: GridOptions;
  public modules: Module[] = AllCommunityModules;

  public modalRef: BsModalRef;

  private _config = {
    backdrop: false,
    ignoreBackdropClick: true
  };

  public columnDefs = [
    { headerName: 'Name', field: 'name', sortable: true, filter: true, checkboxSelection: true },
    { headerName: 'Main Trait', field: 'mainTrait', sortable: true, filter: true },
    { headerName: 'Element', field: 'element', sortable: true, filter: true },
    { headerName: 'Details', field: 'details', sortable: true, filter: true, width: 700 },
    {
      headerName: 'Delete',
      field: 'value',
      cellRendererFramework: DeletePowerCellComponent,
      colId: 'params',
      width: 180,
    }
  ];

  constructor(
    private modalService: BsModalService,
    private powerService: PowerService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {
    this._checkoutForm = this.formBuilder.group({
      id: '',
      name: '',
      mainTrait: '',
      strength: '',
      element: '',
      details: ''
    });

    this.gridOptions = <GridOptions>{
      context: {
        componentParent: this
      },
      rowHeight: 50
    };
  }

  ngOnInit(): void {
    this.powerService.getPowers().subscribe(res => {
      this._powers = res;
    })
  }

  public openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, this._config);
  }

  public getPowersArray() {
    return this._powers;
  }

  public getCheckoutForm() {
    return this._checkoutForm;
  }

  public exportDataAsCsv() {
    this.agGrid.api.exportDataAsCsv({ allColumns: true });
  }

  private _getSelectedPower() {
    return this.agGrid.api.getSelectedNodes().map(node => node.data)[0];
  }

  public resetForm() {
    this._checkoutForm.reset();
  }

  private _isPowerSelected() {
    if (this._getSelectedPower() === undefined) {
      this.alertService.alertError(SELECT_POWER_MESSAGE);
      this.resetForm();
      return false;
    }
    return true;
  }

  private _getPowerFromFormData(powerData): Power {
    return new Power(
      powerData.name,
      powerData.mainTrait,
      powerData.strength,
      powerData.element,
      powerData.details);
  }

  public addPower(powerData): void {
    let power = this._getPowerFromFormData(powerData);

    if (this.powerService.isPowerValid(powerData)) {
      this.powerService.addPower(power as Power)
        .subscribe(power => {
          this._powers.push(power);
          this.agGrid.api.updateRowData({ add: [this._powers[this._powers.length - 1]] });
        });

      this.alertService.alertSuccess(ADDED_POWER_MESSAGE);
      this.resetForm();
    }
    else {
      this.alertService.alertError(INVALID_POWER_MESSAGE);
      this.resetForm();
    }
  }

  public editPower(powerData) {
    if (this._isPowerSelected()) {
      let power = this._getPowerFromFormData(powerData);

      if (this.powerService.isPowerValid(powerData)) {
        power.id = this._getSelectedPower().id,

          this.powerService.updatePower(power as Power)
            .subscribe(power => {
              for (let i = 0; i < this._powers.length; i++) {
                if (this._powers[i].id === power.id) {
                  this._powers[i] = power;
                  break;
                }
              }
              this.agGrid.api.setRowData(this._powers);
              this.alertService.alertSuccess(UPDATED_POWER_MESSAGE);
              this.resetForm();
            });
      }
      else {
        this.alertService.alertError(INVALID_POWER_MESSAGE);
        this.resetForm();
      }
    }
  }

  public delete() {
    if (this._isPowerSelected()) {
      this.alertService.alertConfirm().then((result) => {
        if (result.value) {
          this._deletePower(this._getSelectedPower());
          this.alertService.alertSuccess(DELETED_POWER_MESSAGE);
        }
      })
    }
    else { this.alertService.alertError(SELECT_POWER_MESSAGE); }
  }

  private _deletePower(power) {
    this.powerService.deletePower(power).subscribe(() => {
      this.agGrid.api.updateRowData({ remove: [power] });
      this.alertService.alertSuccess("Power deleted");
    }
    );
  }
}