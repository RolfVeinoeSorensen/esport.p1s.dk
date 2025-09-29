import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import {
  AuthUser,
  EventsService,
  EventUserDto,
  GameServerDto,
  GamesService,
  UsersService,
} from '@app/_services/client';
import { User } from '@models/user';
import { AuthenticationService } from '@services/authentication.service';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ToggleSwitchModule } from 'primeng/toggleswitch';

@Component({
  selector: 'app-my-stuff',
  imports: [CardModule, DatePipe, ButtonModule, ToggleSwitchModule, ReactiveFormsModule, FormsModule, CommonModule],
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
  isAdmin = false;
  isEditor = false;
  year = this.currentDate.getFullYear();
  month = this.currentDate.getMonth();
  userEvents: EventUserDto[] = [];
  gameServers: GameServerDto[] = [];
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
}
