import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import {
  AttendEventRequest,
  AuthUser,
  EventsService,
  EventUserDto,
  GameServerDto,
  GamesService,
  UsersService
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
import { SimpleId } from '@models/simple-id';

export interface EventParticipants {
  invited: number;
  rejected: number;
  accepted: number;
}

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
    EventEditorComponent
  ],
  templateUrl: './my-stuff.component.html',
  styleUrl: './my-stuff.component.css'
})
export class MyStuffComponent implements OnInit, OnDestroy {
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
  selectedId: SimpleId | undefined;

  eventAttendOptions: any[] = [
    { icon: 'fal fa-check', label: 'Deltager', value: true },
    { icon: 'fal fa-times', label: 'Deltager ikke', value: false }
  ];

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

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

  getUser() {
    this.us.usersGetUserById(this.user.id).subscribe(usr => {
      this.userInfo = usr;
    });
  }

  getMyEvents() {
    return this.es.eventsGetUpcomingUserEventsByUserId().subscribe(ue => {
      this.userEvents = ue;
    });
  }

  getParticipantsCount(eventUser: EventUserDto): EventParticipants {
    let res: EventParticipants = { invited: 0, rejected: 0, accepted: 0 };
    eventUser.event?.eventsUsers?.forEach(eu => {
      if (eu.accepted != null) res.accepted++;
      else if (eu.declined != null) res.rejected++;
      else if (eu.invited != null) res.invited++;
    });
    return res;
  }

  getGameServers() {
    this.gs.gamesGetAllGameServers().subscribe(gs => {
      this.gameServers = gs;
    });
  }

  removeGameServer(gameServer: GameServerDto) {
    this.gs.gamesRemoveGameServer(gameServer.gameServer.id).subscribe(() => {
      this.getGameServers();
    });
  }

  setEventAttendance(eventUser: EventUserDto, attend: boolean) {
    if (attend === true && eventUser.eventsUser.accepted != null) return;
    if (attend === false && eventUser.eventsUser.declined != null) return;
    var request: AttendEventRequest = {
      eventId: eventUser.event.id,
      attend: attend
    };
    this.es.eventsSaveEventAttendance(request).subscribe(() => {
      this.getMyEvents();
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
    this.selectedId = { ...{ id: id } };
    const isNew = id === undefined;
    switch (editorType) {
      case EditorType.Event:
        this.editorText = isNew ? 'Opret aktivitet' : 'Rediger aktivitet';
        break;
      case EditorType.Game:
        this.editorText = 'Rediger spil';
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
