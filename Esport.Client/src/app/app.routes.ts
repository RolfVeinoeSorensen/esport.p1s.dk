import { Routes } from '@angular/router';
import { AuthGuard } from '@helpers/auth.guard';
import { UserRole } from '@services/client';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('@cms/home/home.component').then(m => m.HomeComponent),
  },
  {
    path: 'gdpr',
    loadComponent: () => import('@cms/gdpr/gdpr.component').then(m => m.GdprComponent),
  },
  {
    path: 'about',
    loadComponent: () => import('@cms/about/about.component').then(m => m.AboutComponent),
  },
  {
    path: 'faqs',
    loadComponent: () => import('@cms/faqs/faqs.component').then(m => m.FaqsComponent),
  },
  {
    path: 'meet-the-team',
    loadComponent: () => import('@cms/meet-the-team/meet-the-team.component').then(m => m.MeetTheTeamComponent),
  },
  {
    path: 'contact-us',
    loadComponent: () => import('@cms/contact-us/contact-us.component').then(m => m.ContactUsComponent),
  },
  {
    path: 'services',
    children: [
      {
        path: 'e-sports',
        loadComponent: () => import('@cms/services/e-sports/e-sports.component').then(m => m.ESportsComponent),
      },
      {
        path: 'lan-parties',
        loadComponent: () => import('@cms/services/lan-parties/lan-parties.component').then(m => m.LanPartiesComponent),
      },
    ],
  },
  {
    path: 'news',
    loadComponent: () => import('@cms/news/news.component').then(m => m.NewsComponent),
    children: [
      {
        path: ':url_slug',
        loadComponent: () => import('@cms/news/news-article/news-article.component').then(m => m.NewsArticleComponent),
      },
    ],
  },
  {
    canLoad: [AuthGuard],
    canActivate: [AuthGuard],
    data: { requiredRoles: [UserRole.Admin, UserRole.Editor] },
    path: 'admin',
    children: [
      {
        path: '',
        pathMatch: 'full',
        loadComponent: () => import('@admin/administration.component').then(m => m.AdministrationComponent),
      },
    ],
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
    canLoad: [AuthGuard],
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    data: { requiredRoles: [UserRole.Admin, UserRole.Editor] },
    path: 'my-stuff',
    children: [
      {
        path: 'team-calendar',
        loadComponent: () =>
          import('./cms/my-stuff/team-calendar/team-calendar.component').then(m => m.TeamCalendarComponent),
      },
      {
        path: '',
        loadComponent: () => import('./cms/my-stuff/my-stuff.component').then(m => m.MyStuffComponent),
      },
    ],
  },
  // otherwise redirect to page not found
  { path: '**', redirectTo: '/404' },
];
