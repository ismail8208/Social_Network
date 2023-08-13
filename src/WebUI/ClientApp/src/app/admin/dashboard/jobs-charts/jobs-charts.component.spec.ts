import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobsChartsComponent } from './jobs-charts.component';

describe('JobsChartsComponent', () => {
  let component: JobsChartsComponent;
  let fixture: ComponentFixture<JobsChartsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JobsChartsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JobsChartsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
