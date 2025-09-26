import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { News, NewsService } from '@app/_services/client';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-news-article',
  imports: [DatePipe, RouterLink],
  templateUrl: './news-article.component.html',
  styleUrl: './news-article.component.css',
})
export class NewsArticleComponent implements OnInit {
  private newsService = inject(NewsService);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  url_slug = '';
  news!: News;

  ngOnInit() {
    this.activatedRoute.url.subscribe(urlSegment => {
      this.url_slug = urlSegment
        .map(x => x.path)
        .join('/')
        .replace('read-article/', '');
      console.log(this.url_slug);
      if (this.url_slug === '') {
        this.router.navigate(['/404']);
      } else {
        this.newsService.newsGetNewsByUrl(this.url_slug).subscribe(news => (this.news = news));
      }
    });
  }
}
