import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingHandlerService } from './loading-handler.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  private loadingHandlerService = inject(LoadingHandlerService);

  skipUrls = [];

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    for (const skipUrl of this.skipUrls) {
      if (new RegExp(skipUrl).test(request.url)) {
        this.loadingHandlerService.handleRequest();
        return next.handle(request).pipe(finalize(this.finalize.bind(this)));
      }
    }

    this.loadingHandlerService.handleRequest('plus');
    return next.handle(request).pipe(finalize(this.finalize.bind(this)));
  }
  finalize = (): void => this.loadingHandlerService.handleRequest();
}
export const loadingInterceptorService = [{ provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }];
