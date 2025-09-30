import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameServersEditorComponent } from './game-servers-editor.component';

describe('GameServersEditorComponent', () => {
  let component: GameServersEditorComponent;
  let fixture: ComponentFixture<GameServersEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameServersEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameServersEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
