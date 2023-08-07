import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BriefUserComponent } from './brief-user.component';

describe('BriefUserComponent', () => {
  let component: BriefUserComponent;
  let fixture: ComponentFixture<BriefUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BriefUserComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BriefUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
