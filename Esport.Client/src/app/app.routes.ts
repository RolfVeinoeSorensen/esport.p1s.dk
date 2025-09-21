import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./cms/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: 'gdpr',
    loadComponent: () =>
      import('./cms/gdpr/gdpr.component').then((m) => m.GdprComponent),
  },
  {
    path: 'team-calendar',
    loadComponent: () =>
      import('./cms/team-calendar/team-calendar.component').then((m) => m.TeamCalendarComponent),
  },
];
