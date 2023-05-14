import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserStoryComponent } from './add-user-story.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('AddUserStoryComponent', () => {
  let component: AddUserStoryComponent;
  let fixture: ComponentFixture<AddUserStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUserStoryComponent ],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MatCardModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        MatInputModule,
        BrowserAnimationsModule
      ],
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
