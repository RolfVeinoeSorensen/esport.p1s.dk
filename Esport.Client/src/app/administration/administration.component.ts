import { Component } from '@angular/core';
import { CardModule } from "primeng/card";
import { ButtonModule } from "primeng/button";

@Component({
  selector: 'app-administration',
  imports: [CardModule, ButtonModule],
  templateUrl: './administration.component.html',
  styleUrl: './administration.component.css'
})
export class AdministrationComponent {}
