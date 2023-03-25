import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerDeckComponent } from './player-deck.component';

describe('PlayerDeckComponent', () => {
  let component: PlayerDeckComponent;
  let fixture: ComponentFixture<PlayerDeckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlayerDeckComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlayerDeckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
