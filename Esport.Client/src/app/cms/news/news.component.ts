import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { News, NewsService } from '@app/_services/client';

@Component({
  selector: 'app-news',
  imports: [DatePipe],
  templateUrl: './news.component.html',
  styleUrl: './news.component.css',
})
export class NewsComponent implements OnInit {
  private newsService = inject(NewsService);
  news: News[] = [];
  page = 1;
  pageSize = 10;
  ngOnInit(): void {
    this.getNews();
  }
  getNews() {
    this.newsService.newsGetNews(this.page, this.pageSize).subscribe(news => {
      this.news = news;
    });
  }
}
