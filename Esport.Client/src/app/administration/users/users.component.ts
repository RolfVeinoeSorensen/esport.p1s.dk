import { Component, inject, OnInit } from '@angular/core';
import { AuthUser, UsersService } from '@services/client';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-users',
  imports: [TableModule, ButtonModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  private us = inject(UsersService);
  users: AuthUser[] = [];

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers() {
    this.us.usersGetAllUsers().subscribe(users => (this.users = users));
  }
}
