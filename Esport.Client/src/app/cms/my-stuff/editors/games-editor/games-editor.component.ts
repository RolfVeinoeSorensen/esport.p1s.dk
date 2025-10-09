import { Component, EventEmitter, inject, OnChanges, OnInit, Output } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule } from '@angular/forms';
import { EditorType } from '@models/editor-type';
import { AuthUser, Game, GamesService } from '@services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-games-editor',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule, SelectModule, FormsModule, TableModule],
  templateUrl: './games-editor.component.html',
  styleUrl: './games-editor.component.css'
})
export class GamesEditorComponent implements OnInit, OnChanges {
  @Output() closeHandler = new EventEmitter<EditorType>();
  private gs = inject(GamesService);
  private formBuilder = inject(UntypedFormBuilder);
  private its = inject(InternalToastService);
  gameForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  userInfo: AuthUser | undefined;
  games: Game[] = [];
  selectedGame: Game | undefined;

  constructor() {
    this.gameForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      logo: ['', [Validators.required]]
    });
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.gameForm.controls;
  }

  ngOnInit(): void {
    this.reset();
  }
  ngOnChanges() {
    this.reset();
  }

  reset() {
    this.gameForm.reset();
    this.gs.gamesGetAllGames().subscribe(games => {
      this.games = games;
    });
  }

  selectGame(game: Game) {
    this.selectedGame = game;
    this.f['description'].setValue(this.selectedGame.description);
    this.f['logo'].setValue(this.selectedGame.logo);
    this.f['name'].setValue(this.selectedGame.name);
  }

  isInvalid(controlName: string) {
    const control = this.gameForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }
  onRemove(id: number) {
    this.gs.gamesRemoveGame(id).subscribe(response => {
      this.its.addMessage({
        id: 'gameServerSaved',
        icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
        summary: 'Spillet blev slettet',
        detail: response.message,
        severity: response.ok === true ? 'success' : 'error'
      });
      this.reset();
    });
  }
  onSubmit() {
    this.formSubmitted = true;
    if (this.gameForm.valid && this.selectedGame) {
      this.gs
        .gamesCreateOrUpdateGame({
          name: this.gameForm.value.name,
          description: this.gameForm.value.description,
          logo: this.gameForm.value.logo,
          id: this.selectedGame?.id ?? -1
        })
        .subscribe(response => {
          this.its.addMessage({
            id: 'gameServerSaved',
            icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
            summary: 'Spil blev gemt',
            detail: response.message,
            severity: response.ok === true ? 'success' : 'error'
          });
          this.closeHandler.emit(EditorType.Game);
        });
    } else {
      console.log('submit failed', this.gameForm.errors);
      this.formSubmitted = false;
    }
  }
}
