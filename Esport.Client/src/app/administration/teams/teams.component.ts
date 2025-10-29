import { Component, inject, OnInit } from '@angular/core';
import { AuthUser, Team, UsersService } from '@services/client';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-teams',
  imports: [TableModule, ButtonModule, DatePipe],
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.css'
})
export class TeamsComponent implements OnInit {
  private us = inject(UsersService);
  teams: Team[] = [];

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers() {
    this.us.usersGetAllTeams().subscribe(teams => (this.teams = teams));
  }
}
