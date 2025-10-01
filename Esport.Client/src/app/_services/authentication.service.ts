import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthRole, UserRole, UsersService } from '@services/client';
import { User } from '../_models/user';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  public userSubject: BehaviorSubject<User> = new BehaviorSubject<User>(new User());
  public user!: Observable<User>;

  constructor(
    private router: Router,
    private service: UsersService
  ) {
    let lUser = typeof window !== 'undefined' ? localStorage.getItem('user') : null;
    if (lUser) {
      this.userSubject.next(JSON.parse(lUser));
      this.user = this.userSubject.asObservable();
    }
  }

  public get userValue(): User {
    return this.userSubject.value;
  }

  hasRole(role: UserRole): boolean {
    return this.userValue.roles.some(x => x.role === role);
  }

  isAdmin(): boolean {
    return this.userValue.roles.some(x => x.role === UserRole.Admin);
  }

  isEditor(): boolean {
    return this.userValue.roles.some(x => x.role === UserRole.Editor);
  }

  login(username: string, password: string) {
    return this.service.usersAuthenticate({ username: username, password: password }).pipe(
      map(res => {
        let user: User = new User();
        user.id = res.id;
        user.firstName = res.firstName ?? '';
        user.lastName = res.lastName ?? '';
        user.roles = (res.roles as AuthRole[]) ?? [];
        user.token = res.token ?? '';
        user.username = res.username ?? '';
        localStorage.setItem('user', JSON.stringify(user));
        this.userSubject.next(user);
        return user;
      })
    );
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
    this.userSubject.next(new User());
    this.router.navigate(['/log-in']);
  }
}
