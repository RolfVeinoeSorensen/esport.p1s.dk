import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'news',
    renderMode: RenderMode.Server
  },
  {
    path: 'news/read-article/:url_slug',
    renderMode: RenderMode.Server
  },
  {
    path: 'news/edit/:url_slug',
    renderMode: RenderMode.Client
  },
  {
    path: 'news/create',
    renderMode: RenderMode.Client
  },
  {
    path: 'news/edit/:url_slug',
    renderMode: RenderMode.Server
  },
  {
    path: 'user/change-password/:token',
    renderMode: RenderMode.Client
  },
  {
    path: 'user/activate-user/:token',
    renderMode: RenderMode.Client
  },
  {
    path: 'my-stuff',
    renderMode: RenderMode.Client
  },
  {
    path: 'my-stuff/team-calendar',
    renderMode: RenderMode.Client
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];
