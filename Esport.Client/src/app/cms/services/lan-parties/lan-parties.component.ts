import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-lan-parties',
  imports: [CardModule, RouterLink],
  templateUrl: './lan-parties.component.html',
  styleUrl: './lan-parties.component.css'
})
export class LanPartiesComponent {}
