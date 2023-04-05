import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserStoryComponent } from './add-user-story.component';

describe('AddUserStoryComponent', () => {
  let component: AddUserStoryComponent;
  let fixture: ComponentFixture<AddUserStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUserStoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddUserStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
