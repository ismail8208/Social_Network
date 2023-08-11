import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BriefPostsComponent } from './brief-posts.component';

describe('BriefPostsComponent', () => {
  let component: BriefPostsComponent;
  let fixture: ComponentFixture<BriefPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BriefPostsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BriefPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
