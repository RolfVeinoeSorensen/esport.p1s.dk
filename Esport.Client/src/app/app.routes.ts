import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('@cms/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: 'gdpr',
    loadComponent: () =>
      import('@cms/gdpr/gdpr.component').then((m) => m.GdprComponent),
  },
  {
    path: 'forbidden',
    loadComponent: () => import('@shared/forbidden/forbidden.component').then(m => m.ForbiddenComponent),
  },
  {
    path: '404',
    loadComponent: () => import('@shared/page-not-found/page-not-found.component').then(m => m.PageNotFoundComponent),
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
    // otherwise redirect to page not found
  { path: '**', redirectTo: '/404' },
];
