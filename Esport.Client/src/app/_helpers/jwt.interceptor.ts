import { inject, Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from './../../environments/environment';
import { AuthenticationService } from '@app/_services/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private authenticationService = inject(AuthenticationService);
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add auth header with jwt if user is logged in and request is to api url
    const user = this.authenticationService.userValue;
    const isLoggedIn = user && user.token;
    const isApiUrl = request.url.startsWith(environment.apiUrl);
    // console.log(request.url, isLoggedIn, isApiUrl, user);
    if (isLoggedIn && isApiUrl) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${user.token}`,
        },
      });
    }

    return next.handle(request);
  }
}

export const jwtInterceptorProviders = [{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }];
