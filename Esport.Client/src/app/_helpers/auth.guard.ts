import { Injectable, inject } from '@angular/core';
import { ActivatedRouteSnapshot, Route, Router, RouterStateSnapshot } from '@angular/router';
import { User } from '@app/_models/user';

import { AuthenticationService } from '@services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard {
  private router = inject(Router);
  private authenticationService = inject(AuthenticationService);

  constructor() {}

  canLoad(route: Route, state: RouterStateSnapshot): boolean {
    const user: User = this.authenticationService.userValue;
    // console.log('canLoad', route?.data, user, route.data?.['requiredRoles']);
    if (user && user.token && user.roles) {
      let isAuthorized = true;
      // check if route is restricted by role
      if (route.data?.['requiredRoles']?.length > 0) {
        const anyUserRoles = user.roles.length > 0;
        if (!anyUserRoles) {
          isAuthorized = false;
        } else {
          const found: boolean = route.data?.['requiredRoles'].some(
            (r: string) => user && user.token && user.roles && user.roles.some(ur => ur.role === r) === true
          );
          isAuthorized = found;
          if (!found) {
            //user is authenticated but not allowed this resource so redirect them to forbidden
            this.router.navigate(['/forbidden'], { queryParams: { returnUrl: state.url, logIn: true } });
            return false;
          }
        }
      }
      if (!isAuthorized) {
        this.authenticationService.logout();
        // role not authorized so redirect to home page
        this.router.navigate(['/'], { queryParams: { returnUrl: state.url, logIn: true } });
        return false;
      }
      // authorized so return true
      return isAuthorized;
    }
    this.router.navigate(['/'], { queryParams: { returnUrl: state.url, logIn: true } });
    return false;
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const user: User = this.authenticationService.userValue;
    // console.log('canActivate', route.data['requiredRoles'], user, route.data?.['requiredRoles']);
    if (user && user.token && user.roles) {
      let isAuthorized = true;
      // check if route is restricted by role
      if (route.data?.['requiredRoles']?.length > 0) {
        const anyUserRoles = user.roles.length > 0;
        if (!anyUserRoles) {
          isAuthorized = false;
        } else {
          const found: boolean = route.data?.['requiredRoles'].some(
            (r: string) => user && user.token && user.roles && user.roles.some(ur => ur.role === r) === true
          );
          isAuthorized = found;
          if (!found) {
            //user is authenticated but not allowed this resource so redirect them to forbidden
            this.router.navigate(['/forbidden'], { queryParams: { returnUrl: state.url, logIn: true } });
            return false;
          }
        }
      }
      if (!isAuthorized) {
        this.authenticationService.logout();
        // role not authorized so redirect to home page
        this.router.navigate(['/'], { queryParams: { returnUrl: state.url, logIn: true } });
        return false;
      }
      // authorized so return true
      return isAuthorized;
    }
    this.router.navigate(['/'], { queryParams: { returnUrl: state.url, logIn: true } });
    return false;
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const user = this.authenticationService.userValue;
    // console.log('canActivateChild', route.data['requiredRoles'], user);
    if (user && user.token && user.roles) {
      let isAuthorized = true;
      // check if route is restricted by role
      if (route.data?.['requiredRoles']?.length > 0) {
        const anyUserRoles = user.roles.length > 0;
        if (!anyUserRoles) {
          isAuthorized = false;
        } else {
          const found: boolean = route.data?.['requiredRoles'].some(
            (r: string) => user && user.token && user.roles && user.roles.some(ur => ur.role === r) === true
          );
          isAuthorized = found;
          if (!found) {
            //user is authenticated but not allowed this resource so redirect them to forbidden
            this.router.navigate(['/forbidden'], { queryParams: { returnUrl: state.url, logIn: true } });
            return false;
          }
        }
      }
      if (!isAuthorized) {
        this.authenticationService.logout();
        // role not authorized so redirect to home page
        this.router.navigate(['/'], { queryParams: { returnUrl: state.url, logIn: true } });
        return false;
      }
      // authorized so return true
      return isAuthorized;
    }
    this.router.navigate(['/'], { queryParams: { returnUrl: state.url, logIn: true } });
    return false;
  }
}
