import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportCvComponent } from './export-cv.component';

describe('ExportCvComponent', () => {
  let component: ExportCvComponent;
  let fixture: ComponentFixture<ExportCvComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExportCvComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExportCvComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
