import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-events',
  imports: [CommonModule],
  templateUrl: './events.component.html',
  styleUrl: './events.component.css'
})
export class EventsComponent {
    @Input() events!: string[];
    @Output() removeEvent = new EventEmitter<string>();

    onRemoveEvent(event: string) {
        this.removeEvent.emit(event);
    }
}
