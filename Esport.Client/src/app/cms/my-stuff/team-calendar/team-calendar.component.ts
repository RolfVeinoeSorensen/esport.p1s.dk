import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { EventDto, EventsService } from '@app/_services/client';

@Component({
  selector: 'app-team-calendar',
  imports: [CommonModule, ButtonModule, DialogModule],
  templateUrl: './team-calendar.component.html',
  styleUrl: './team-calendar.component.css',
})
export class TeamCalendarComponent implements OnInit {
  currentDate = new Date();

  changeMonth(offset: number) {
    this.currentDate = new Date(this.currentDate.setMonth(this.currentDate.getMonth() + offset));
    this.loadEvents();
  }
  diff: string[] = [];
  events: { [key: string]: EventDto } = {};
  selectedDate: Date | null = null;
  visibleDay = false;

  constructor(private eventsService: EventsService) {}

  ngOnInit() {
    this.loadEvents();
  }

  selectDate(event: EventDto) {
    this.selectedDate = new Date(event.weekendWorkday.date);
    if (event.events.length > 0) this.visibleDay = true;
  }
  getDaysInMonth(): Date[] {
    const days = [];
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();
    const numDays = new Date(year, month + 1, 0).getDate();

    for (let i = 1; i <= numDays; i++) {
      days.push(new Date(year, month, i));
    }
    console.log(days);
    return days;
  }

  loadEvents() {
    const y = this.currentDate != null ? this.currentDate.getFullYear() : new Date().getFullYear();
    const m = this.currentDate != null ? this.currentDate.getMonth() : new Date().getMonth();
    this.eventsService.eventsGetAllEvents(m + 1, y).subscribe(events => {
      const firstDay = new Date(y, m, 1);
      const day = firstDay.getDay();
      let diff = (day === 0 ? -6 : 1) - day;
      if (diff !== 0) diff = diff * -1;
      const diffArr = [];
      for (let i = 0; i < Math.abs(diff); i++) {
        diffArr.push(this.addDays(firstDay, -1 - i).toDateString());
      }
      this.diff = diffArr.sort();
      console.log('diff', new Date(y, m, 1), diff, day);
      this.events = events;
      console.log(events);
    });
  }
  addDays(date: Date, days: number): Date {
    const result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }
  addEvent(day: string) {
    const event = prompt('Enter event:');
    if (event) {
      const dateString = new Date(day).toISOString().split('T')[0];
      console.log(dateString, event);
      this.loadEvents();
    }
  }
}
