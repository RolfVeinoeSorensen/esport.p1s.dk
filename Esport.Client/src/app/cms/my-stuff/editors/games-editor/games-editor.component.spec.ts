import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesEditorComponent } from './games-editor.component';

describe('GamesEditorComponent', () => {
  let component: GamesEditorComponent;
  let fixture: ComponentFixture<GamesEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GamesEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GamesEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
