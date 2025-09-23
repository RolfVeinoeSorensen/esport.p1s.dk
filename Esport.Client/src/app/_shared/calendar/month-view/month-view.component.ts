import { Component, Input, OnInit } from '@angular/core';
import { EventsComponent } from '../events/events.component';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { EventsService } from '@app/_services/client';
import { EventService } from '../events/events.service';

@Component({
  selector: 'app-month-view',
  imports: [CommonModule, EventsComponent, ButtonModule, DialogModule],
  templateUrl: './month-view.component.html',
  styleUrl: './month-view.component.css'
})
export class MonthViewComponent implements OnInit {
    @Input() date!: Date;
    days: Date[] = [];
    events: { [key: string]: string[] } = {};
    selectedDate: Date | null = null;
    visibleDay = false;

    constructor(private eventsService: EventsService, private eventService: EventService) { }

    ngOnInit() {
        this.days = this.getDaysInMonth();
        this.loadEvents();
    }

    ngOnChanges() {
        this.days = this.getDaysInMonth();
        this.loadEvents();
    }
    selectDate(day: Date) {
        this.selectedDate = day;
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

        return days;
    }

    loadEvents() {
        const y = this.selectedDate != null ? this.selectedDate.getFullYear() : new Date().getFullYear();
        const m = this.selectedDate != null ? this.selectedDate.getMonth() : new Date().getMonth();
        this.eventsService.eventsGetAllEvents(m, y).subscribe(events =>{
            console.log(events)
        });
        this.days.forEach((day) => {
            const dateString = day.toISOString().split('T')[0];
            this.events[dateString] = this.eventService.getEvents(dateString);
        });
    }

    addEvent(day: Date) {
        const event = prompt('Enter event:');
        if (event) {
            const dateString = day.toISOString().split('T')[0];
            this.eventService.addEvent(dateString, event);
            this.loadEvents();
        }
    }

    removeEvent(day: Date, event: string) {
        const dateString = day.toISOString().split('T')[0];
        this.eventService.removeEvent(dateString, event);
        this.loadEvents();
    }
    checkEvents(day: Date):boolean {
        const dateString = day.toISOString().split('T')[0];
        const arr = this.eventService.getEvents(dateString);
        return arr.length > 0;
    }
    getEvents(day: Date) {
        const dateString = day.toISOString().split('T')[0];
        const arr = this.eventService.getEvents(dateString);
        return arr;
    }
}