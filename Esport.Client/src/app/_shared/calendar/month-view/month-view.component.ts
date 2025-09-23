import { Component, Input, OnInit } from '@angular/core';
import { EventsComponent } from '../events/events.component';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { EventDto, EventsService } from '@app/_services/client';
import { EventService } from '../events/events.service';

@Component({
  selector: 'app-month-view',
  imports: [CommonModule, EventsComponent, ButtonModule, DialogModule],
  templateUrl: './month-view.component.html',
  styleUrl: './month-view.component.css'
})
export class MonthViewComponent implements OnInit {
    @Input() date!: Date;
    diff: string[] = [];
    events: { [key: string]: EventDto } = {};
    selectedDate: Date | null = null;
    visibleDay = false;

    constructor(private eventsService: EventsService, private eventService: EventService) { }

    ngOnInit() {

    }

    ngOnChanges() {
        this.loadEvents();
    }
    selectDate(date: string) {
        this.selectedDate = new Date(date);
        this.visibleDay = true;
    }
    getDaysInMonth(): Date[] {
        const days = [];
        const year = this.date.getFullYear();
        const month = this.date.getMonth();
        const numDays = new Date(year, month + 1, 0).getDate();

        for (let i = 1; i <= numDays; i++) {
            days.push(new Date(year, month, i));
        }
        console.log(days);
        return days;
    }
    

    loadEvents() {
        const y = this.date != null ? this.date.getFullYear() : new Date().getFullYear();
        const m = this.date != null ? this.date.getMonth() : new Date().getMonth();
        this.eventsService.eventsGetAllEvents(m+1, y).subscribe(events =>{
            const firstDay = new Date(y, m, 1);
            const day = firstDay.getDay();
            let diff = ((day === 0 ? -6 : 1) - day);
            if (diff !== 0) diff = diff * -1;
            const diffArr = [];
            for (let i = 0; i < Math.abs(diff); i++) {
                diffArr.push(this.addDays(firstDay,(-1-i)).toDateString());
            }
            this.diff = diffArr.sort();
            console.log('diff',new Date(y, m, 1), diff, day);
            this.events = events;
            console.log(events)
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
            this.eventService.addEvent(dateString, event);
            this.loadEvents();
        }
    }

    removeEvent(day: Date, event: string) {
        const dateString = day.toISOString().split('T')[0];
        this.eventService.removeEvent(dateString, event);
        this.loadEvents();
    }
    checkEvents(day: string):boolean {
        const dateString = new Date(day).toISOString().split('T')[0];
        const arr = this.eventService.getEvents(dateString);
        return arr.length > 0;
    }
    getEvents(day: string) {
        const dateString = new Date(day).toISOString().split('T')[0];
        const arr = this.eventService.getEvents(dateString);
        return arr;
    }
}