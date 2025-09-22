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
    path: 'my-stuff',
    children: [
      {
        path: 'team-calendar',
        loadComponent: () =>
          import('./cms/my-stuff/team-calendar/team-calendar.component').then(
            (m) => m.TeamCalendarComponent
          ),
      },
      {
        path: '',
        loadComponent: () =>
          import('./cms/my-stuff/my-stuff.component').then(
            (m) => m.MyStuffComponent
          ),
      },
    ],
  },
];
