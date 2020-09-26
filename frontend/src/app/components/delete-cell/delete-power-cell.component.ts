import { Component } from "@angular/core";
import { ICellRendererAngularComp } from "@ag-grid-community/angular";

@Component({
    selector: 'delete-power-cell',
    template: `
        <button style="height: 25px" (click)="clicked()" class="btn btn-danger">x</button>
    `,
    styles: [
        `.btn {
            line-height: 0.5;
            width: 25%;
        }`
    ]
})
export class DeletePowerCellComponent implements ICellRendererAngularComp {
    private _params: any;
    public _cell: any;

    agInit(params: any): void {
        this._params = params;
        this._cell = { row: params.value, col: params.colDef.headerName };
    }

    public clicked(): void {
        this._params.context.componentParent._deletePower(this._params.data);
    }

    public refresh(): boolean {
        return false;
    }
}