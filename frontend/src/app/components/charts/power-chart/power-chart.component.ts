import { Component, OnInit } from '@angular/core';
import { PowerService } from 'src/app/services/power.service';
import { Power } from 'src/app/models/power';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';
import { HeroService } from 'src/app/services/hero.service';

@Component({
  selector: 'app-power-chart',
  templateUrl: './power-chart.component.html',
  styleUrls: ['./power-chart.component.css']
})
export class PowerChartComponent implements OnInit {
  private powers: Power[] = [];

  public barChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false
  };
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [];
  public barChartLabels: Label[] = [];
  public barChartData: ChartDataSets[] = [{ data: [] }];

  constructor(
    private heroService: HeroService,
    private powerService: PowerService) { }

  ngOnInit(): void {
    this.powerService.getPowers().subscribe(res => {
      this.powers = res;
      this._populateChart();
    })
  }

  private _addData(labels, data) {
    this.barChartLabels = labels;

    this.barChartData = [
      { data: data, label: 'Power frequency' }
    ];
  }

  private _populateChart() {
    let data = [];

    this.heroService.getPowerFrequencies().subscribe(result => {
      for (const res in result) {
        data.push(result[res]);
      }
      this._addData(this._getPowerNames(), data);
    }
    );
  }

  private _getPowerNames() {
    return this.powers.map(power => power.name);
  }
}