import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LanPartiesComponent } from './lan-parties.component';

describe('LanPartiesComponent', () => {
  let component: LanPartiesComponent;
  let fixture: ComponentFixture<LanPartiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LanPartiesComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(LanPartiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
