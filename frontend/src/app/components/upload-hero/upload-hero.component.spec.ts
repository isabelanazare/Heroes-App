import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadHeroComponent } from './upload-hero.component';

describe('UploadHeroComponent', () => {
  let component: UploadHeroComponent;
  let fixture: ComponentFixture<UploadHeroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadHeroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadHeroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
