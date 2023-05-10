import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EndPageComponent } from './end-page.component';
import { MatFormFieldModule } from '@angular/material/form-field';

describe('EndPageComponent', () => {
  let component: EndPageComponent;
  let fixture: ComponentFixture<EndPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EndPageComponent ],
      imports: [ MatFormFieldModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EndPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
