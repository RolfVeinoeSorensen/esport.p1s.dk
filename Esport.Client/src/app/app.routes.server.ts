import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'news/read-article/:url_slug',
    renderMode: RenderMode.Server,
  },
  {
    path: 'news/edit/:url_slug',
    renderMode: RenderMode.Server,
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender,
  },
];
