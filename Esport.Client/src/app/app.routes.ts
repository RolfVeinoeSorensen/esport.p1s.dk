import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./cms/home/home.component').then((m) => m.HomeComponent),
  },
];
