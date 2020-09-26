import {Component} from "@angular/core";
import {ICellRendererAngularComp} from "@ag-grid-community/angular";
import { URL } from 'src/app/utils/config';

@Component({
    selector: 'hero-image-cell',
    template: `
    <img [src]="this.getImagePath()" alt="profile picture" class="dot">
    `,
    styleUrls: ['./hero-image-renderer.component.scss']
})
export class HeroImageRendererComponent implements ICellRendererAngularComp {
    private params: any;
    public cell: any;

    agInit(params: any): void {
        this.params = params;
        this.cell = {row: params.value, col: params.colDef.headerName};
    }

    public refresh(): boolean {
        return false;
    }

    public getImagePath(): string {
        return `${URL}/${this.params.data.imgPath}`;
    }
}