import { Component, OnInit } from '@angular/core';
import { Hero } from 'src/app/models/hero';
import { HeroService } from 'src/app/services/hero.service';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';

@Component({
  selector: 'app-hero-chart',
  templateUrl: './hero-chart.component.html',
  styleUrls: ['./hero-chart.component.css']
})
export class HeroChartComponent implements OnInit {
  private heroes: Hero[] = [];

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
    private heroService: HeroService) { }

  ngOnInit(): void {
    this.heroService.getHeroes().subscribe(res => {
      this.heroes = res;
      this.heroService.getHeroChartData().subscribe(res => {
        this._addLabels(this._getHeroNames());
        this._addData(res.powers);
      })
    })
  }

  private _getHeroNames() {
    return this.heroes.map(hero => hero.name);
  }

  private _addLabels(labels) {
    this.barChartLabels = labels;
  }

  private _addData(data) {
    this.barChartData = data;
  }
}