import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { EventsService, EventUserDto, GameServerDto, GamesService } from '@app/_services/client';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-my-stuff',
  imports: [CardModule, DatePipe],
  templateUrl: './my-stuff.component.html',
  styleUrl: './my-stuff.component.css',
})
export class MyStuffComponent implements OnInit {
  currentDate = new Date();
  year = this.currentDate.getFullYear();
  month = this.currentDate.getMonth();
  userEvents: EventUserDto[] = [];
  gameServers: GameServerDto[] = [];
  constructor(
    private eventService: EventsService,
    private gamesService: GamesService
  ) {}
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
      console.log(this.gameServers);
    });
  }
}
