import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonthViewComponent } from './month-view/month-view.component';
import { ButtonModule } from 'primeng/button';
@Component({
  selector: 'app-calendar',
  imports: [CommonModule, MonthViewComponent, ButtonModule],
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.css'
})
export class CalendarComponent {
    currentDate = new Date();

    changeMonth(offset: number) {
        this.currentDate = new Date(
            this.currentDate.setMonth(this.currentDate.getMonth() + offset)
        );
    }
}
