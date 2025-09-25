import { ErrorHandler, Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable()
export class MyErrorHandler implements ErrorHandler {
  constructor(@Inject(PLATFORM_ID) private platformId: any) {}

  handleError(error: any): void {
    if (isPlatformBrowser(this.platformId)) {
      this.handleBrowser(error);
    } else {
      this.handleServer(error);
    }
  }

  handleBrowser(error: any): void {
    console.log(error);
  }

  handleServer(error: any): void {
    console.log('ssr: ', error ?? 'not known');
  }
}
