import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StateJobListComponent } from './state-job-list.component';

describe('StateJobListComponent', () => {
  let component: StateJobListComponent;
  let fixture: ComponentFixture<StateJobListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StateJobListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StateJobListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
