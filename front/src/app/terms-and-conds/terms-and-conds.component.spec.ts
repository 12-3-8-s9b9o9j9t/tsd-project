import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermsAndCondsComponent } from './terms-and-conds.component';

describe('TermsAndCondsComponent', () => {
  let component: TermsAndCondsComponent;
  let fixture: ComponentFixture<TermsAndCondsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TermsAndCondsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TermsAndCondsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
