import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateEducationComponent } from './update-education.component';

describe('UpdateEducationComponent', () => {
  let component: UpdateEducationComponent;
  let fixture: ComponentFixture<UpdateEducationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateEducationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateEducationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
