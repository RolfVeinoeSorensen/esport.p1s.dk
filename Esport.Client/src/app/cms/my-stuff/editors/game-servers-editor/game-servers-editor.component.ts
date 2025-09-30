import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule } from '@angular/forms';
import { EditorType } from '@models/editor-type';
import { AuthUser, Game, GameServer, GamesService } from '@services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select';

@Component({
  selector: 'app-game-servers-editor',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule, SelectModule, FormsModule],
  templateUrl: './game-servers-editor.component.html',
  styleUrl: './game-servers-editor.component.css',
})
export class GameServersEditorComponent implements OnInit, OnChanges {
  @Input() public id: number | undefined;
  @Output() close = new EventEmitter<EditorType>();
  private gs = inject(GamesService);
  private formBuilder = inject(UntypedFormBuilder);
  private its = inject(InternalToastService);
  gameServerForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  userInfo: AuthUser | undefined;
  games: Game[] = [];
  gameServer: GameServer | undefined;
  selectedGame: Game | undefined;

  constructor() {
    this.gameServerForm = this.formBuilder.group({
      server: ['', [Validators.required]],
      port: ['', [Validators.required]],
    });
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.gameServerForm.controls;
  }

  ngOnInit(): void {
    this.reset();
  }
  ngOnChanges(changes: SimpleChanges) {
    this.reset();
  }

  reset() {
    this.gameServerForm.reset();
    this.gs.gamesGetAllGames().subscribe(games => {
      this.games = games;
      if (this.id)
        this.gs.gamesGetGameServerById(this.id).subscribe(gameServer => {
          this.gameServer = gameServer;
          this.selectedGame = this.games.find(g => g.id === gameServer.gameId);
          this.f['server'].setValue(this.gameServer.server);
          this.f['port'].setValue(this.gameServer.port);
        });
    });
  }

  isInvalid(controlName: string) {
    const control = this.gameServerForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }
  onRemove(id: number) {
    this.gs.gamesRemoveGameServer(id).subscribe(response => {
      this.its.addMessage({
        id: 'gameServerSaved',
        icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
        summary: 'Spilserveren blev slettet',
        detail: response.message,
        severity: response.ok === true ? 'success' : 'error',
      });
      this.close.emit(EditorType.GameServer);
    });
  }
  onSubmit() {
    this.formSubmitted = true;
    if (this.gameServerForm.valid && this.selectedGame) {
      this.gs
        .gamesCreateOrUpdateGameServer({
          gameId: this.selectedGame.id,
          port: this.gameServerForm.value.port,
          server: this.gameServerForm.value.server,
          id: this.gameServer?.id ?? -1,
        })
        .subscribe(response => {
          this.its.addMessage({
            id: 'gameServerSaved',
            icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
            summary: 'Spilserveren blev gemt',
            detail: response.message,
            severity: response.ok === true ? 'success' : 'error',
          });
          this.close.emit(EditorType.GameServer);
        });
    } else {
      console.log('submit failed', this.gameServerForm.errors);
      this.formSubmitted = false;
    }
  }
}
