import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withInMemoryScrolling } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { providePrimeNG } from 'primeng/config';
import ThemePreset from '@app/app-theme';
import { environment } from '@environments/environment';

import { ScrollerModule } from 'primeng/scroller';
import { ScrollTopModule } from 'primeng/scrolltop';

import { routes } from './app.routes';
import {
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { BASE_PATH } from '@services/client';
import { JwtInterceptor } from '@helpers/jwt.interceptor';
import { ErrorInterceptor } from '@helpers/error.interceptor';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeDa from '@angular/common/locales/da';

registerLocaleData(localeDa);
export function getBaseUrl(): string {
  return environment.apiUrl;
}

export const appConfig: ApplicationConfig = {
  providers: [
    BrowserModule,
    BrowserAnimationsModule,
    provideAnimations(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(
      routes,
      withInMemoryScrolling({
        scrollPositionRestoration: 'top',
      })
    ),
    provideClientHydration(withEventReplay()),
    providePrimeNG({
      theme: {
        preset: ThemePreset,
        options: {
          prefix: 'p',
          darkModeSelector: '.dark',
          cssLayer: false,
        },
      },
    }),
    provideHttpClient(),
    { provide: LOCALE_ID, useValue: 'da-DK' },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: BASE_PATH, useValue: environment.apiUrl },
    ScrollerModule,
    ScrollTopModule,
  ],
};
