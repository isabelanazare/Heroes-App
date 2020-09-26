import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeroChartComponent } from './hero-chart.component';

describe('HeroChartComponent', () => {
  let component: HeroChartComponent;
  let fixture: ComponentFixture<HeroChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeroChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeroChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
