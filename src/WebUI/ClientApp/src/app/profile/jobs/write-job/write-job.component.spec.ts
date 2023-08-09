import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WriteJobComponent } from './write-job.component';

describe('WriteJobComponent', () => {
  let component: WriteJobComponent;
  let fixture: ComponentFixture<WriteJobComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WriteJobComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WriteJobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
