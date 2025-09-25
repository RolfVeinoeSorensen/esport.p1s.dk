import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ESportsComponent } from './e-sports.component';

describe('ESportsComponent', () => {
  let component: ESportsComponent;
  let fixture: ComponentFixture<ESportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ESportsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ESportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
