import { ErrorHandler, inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable()
export class MyErrorHandler implements ErrorHandler {
  private platformId = inject(PLATFORM_ID);
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  handleError(error: any): void {
    if (isPlatformBrowser(this.platformId)) {
      this.handleBrowser(error);
    } else {
      this.handleServer(error);
    }
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  handleBrowser(error: any): void {
    console.log(error);
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  handleServer(error: any): void {
    console.log('ssr: ', error ?? 'not known');
  }
}
