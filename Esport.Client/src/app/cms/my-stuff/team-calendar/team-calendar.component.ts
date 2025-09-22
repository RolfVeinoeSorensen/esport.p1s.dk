import { Component } from '@angular/core';
import { CalendarComponent } from '@shared/calendar/calendar.component';

@Component({
  selector: 'app-team-calendar',
  imports: [CalendarComponent],
  templateUrl: './team-calendar.component.html',
  styleUrl: './team-calendar.component.css',
})
export class TeamCalendarComponent {}
