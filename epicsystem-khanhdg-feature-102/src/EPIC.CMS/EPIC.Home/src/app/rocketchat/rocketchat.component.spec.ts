import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RocketchatComponent } from './rocketchat.component';

describe('RocketchatComponent', () => {
  let component: RocketchatComponent;
  let fixture: ComponentFixture<RocketchatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RocketchatComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RocketchatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
