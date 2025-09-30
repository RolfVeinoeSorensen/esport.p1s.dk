import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import {
  AuthUser,
  EventsService,
  EventUserDto,
  Game,
  GameServerDto,
  GamesService,
  UsersService,
} from '@app/_services/client';
import { User } from '@models/user';
import { AuthenticationService } from '@services/authentication.service';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { DialogModule } from 'primeng/dialog';
import { EditorType } from '@models/editor-type';
import { UserEditorComponent } from './editors/user-editor/user-editor.component';
import { GamesEditorComponent } from './editors/games-editor/games-editor.component';
import { GameServersEditorComponent } from './editors/game-servers-editor/game-servers-editor.component';
import { EventEditorComponent } from './editors/event-editor/event-editor.component';

@Component({
  selector: 'app-my-stuff',
  imports: [
    CardModule,
    DatePipe,
    ButtonModule,
    ToggleSwitchModule,
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    DialogModule,
    UserEditorComponent,
    GamesEditorComponent,
    GameServersEditorComponent,
    EventEditorComponent,
  ],
  templateUrl: './my-stuff.component.html',
  styleUrl: './my-stuff.component.css',
})
export class MyStuffComponent implements OnInit {
  private es = inject(EventsService);
  private gs = inject(GamesService);
  private us = inject(UsersService);
  private as: AuthenticationService = inject(AuthenticationService);
  userSubscription!: Subscription;
  user!: User;
  userInfo: AuthUser | undefined;
  currentDate = new Date();
  isAdmin: boolean = false;
  isEditor: boolean = false;
  year = this.currentDate.getFullYear();
  month = this.currentDate.getMonth();
  userEvents: EventUserDto[] = [];
  gameServers: GameServerDto[] = [];
  visibleEditor: boolean = false;
  editorText: string = '';
  editorType = EditorType;
  selectedEditorType!: EditorType;
  selectedId: number | undefined;
  ngOnInit(): void {
    this.getMyEvents();
    this.getGameServers();
    this.userSubscription = this.as.userSubject.subscribe(user => {
      this.user = user;
      this.isAdmin = this.as.isAdmin();
      this.isEditor = this.as.isEditor();
      if (this.user?.id) this.getUser();
    });
  }

  getUser() {
    this.us.usersGetUserById(this.user.id).subscribe(usr => {
      this.userInfo = usr;
    });
  }

  getMyEvents() {
    return this.es.eventsGetUserEventsByUserId(this.month + 1, this.year).subscribe(ue => {
      this.userEvents = ue;
    });
  }

  getGameServers() {
    this.gs.gamesGetAllGameServers().subscribe(gs => {
      this.gameServers = gs;
    });
  }

  handleEditorSave(editorType: EditorType) {
    this.visibleEditor = false;
    switch (editorType) {
      case EditorType.Event:
        this.getMyEvents();
        break;
      case EditorType.Game:
        this.getGameServers();
        break;
      case EditorType.GameServer:
        this.getGameServers();
        break;
      case EditorType.User:
        this.getUser();
        break;
    }
  }

  openEditor(editorType: EditorType, id: number | undefined) {
    this.selectedEditorType = editorType;
    this.selectedId = id;
    const isNew = id === undefined;
    switch (editorType) {
      case EditorType.Event:
        this.editorText = isNew ? 'Opret aktivitet' : 'Rediger aktivitet';
        break;
      case EditorType.Game:
        this.editorText = isNew ? 'Opret spil' : 'Rediger spil';
        break;
      case EditorType.GameServer:
        this.editorText = isNew ? 'Opret spil server' : 'Rediger spil server';
        break;
      case EditorType.User:
        this.editorText = isNew ? 'Opret bruger' : 'Rediger bruger';
        break;
    }
    this.visibleEditor = true;
  }
}
