import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withInMemoryScrolling } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { providePrimeNG } from 'primeng/config';
import ThemePreset from '@app/app-theme';
import { environment } from '@environments/environment';

import { ScrollerModule } from 'primeng/scroller';
import { ScrollTopModule } from 'primeng/scrolltop';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { BASE_PATH as ClientBasePath, ApiModule as ClientModule } from '@services/client';
import { jwtInterceptorProviders } from '@helpers/jwt.interceptor';
import { errorInterceptorProviders } from '@helpers/error.interceptor';
import { LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeDa from '@angular/common/locales/da';
import { loadingInterceptorService } from './_shared/loading';
import { MessageService } from 'primeng/api';
import { InternalToastService } from './_services/internaltoast.service';

registerLocaleData(localeDa);
export function getBaseUrl(): string {
  return environment.apiUrl;
}

export const appConfig: ApplicationConfig = {
  providers: [
    BrowserModule,
    ClientModule,
    MessageService,
    InternalToastService,
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
    jwtInterceptorProviders,
    errorInterceptorProviders,
    loadingInterceptorService,
    provideHttpClient(withInterceptorsFromDi(), withFetch()),
    { provide: LOCALE_ID, useValue: 'da-DK' },
    { provide: ClientBasePath, useValue: environment.apiUrl },
    ScrollerModule,
    ScrollTopModule,
  ],
};
