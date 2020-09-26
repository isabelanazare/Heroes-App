import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeroImageRendererComponent } from './hero-image-renderer.component';

describe('HeroImageRendererComponent', () => {
  let component: HeroImageRendererComponent;
  let fixture: ComponentFixture<HeroImageRendererComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeroImageRendererComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeroImageRendererComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
