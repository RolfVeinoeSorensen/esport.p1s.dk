import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-e-sports',
  imports: [CardModule, RouterLink],
  templateUrl: './e-sports.component.html',
  styleUrl: './e-sports.component.css'
})
export class ESportsComponent {}
