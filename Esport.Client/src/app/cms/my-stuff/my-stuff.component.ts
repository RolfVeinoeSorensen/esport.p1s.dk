import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { EventsService, EventUserDto, GameServerDto, GamesService } from '@app/_services/client';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-my-stuff',
  imports: [CardModule, DatePipe, ButtonModule],
  templateUrl: './my-stuff.component.html',
  styleUrl: './my-stuff.component.css',
})
export class MyStuffComponent implements OnInit {
  private eventService = inject(EventsService);
  private gamesService = inject(GamesService);
  currentDate = new Date();
  year = this.currentDate.getFullYear();
  month = this.currentDate.getMonth();
  userEvents: EventUserDto[] = [];
  gameServers: GameServerDto[] = [];
  ngOnInit(): void {
    this.getMyEvents();
    this.getGameServers();
  }

  getMyEvents() {
    return this.eventService.eventsGetUserEventsByUserId(this.month + 1, this.year).subscribe(ue => {
      this.userEvents = ue;
    });
  }

  getGameServers() {
    this.gamesService.gamesGetAllGameServers().subscribe(gs => {
      this.gameServers = gs;
    });
  }
}
