import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UsersService } from '@app/_services/client';

@Component({
  selector: 'app-user-activate',
  imports: [],
  templateUrl: './user-activate.component.html',
  styleUrl: './user-activate.component.css',
})
export class UserActivateComponent implements OnInit {
  private userService = inject(UsersService);
  private activatedRoute = inject(ActivatedRoute);
  token!: string | null;
  isActivated = false;
  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.token = params.get('token');
      console.log(this.token);
      if (this.token)
        this.userService.usersActivateUser(this.token).subscribe(res => {
          console.log(res);
          if (res.ok === true) this.isActivated;
        });
    });
  }
}
