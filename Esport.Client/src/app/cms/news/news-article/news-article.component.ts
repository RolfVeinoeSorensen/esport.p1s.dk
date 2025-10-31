import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthUser, News, NewsService, UsersService } from '@app/_services/client';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { SafeHtmlPipe } from '@pipes/safehtml.pipe';
import { User } from '@models/user';
import { AuthenticationService } from '@services/authentication.service';
import { Subscription } from 'rxjs';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-news-article',
  imports: [DatePipe, RouterLink, SafeHtmlPipe, ButtonModule],
  templateUrl: './news-article.component.html',
  styleUrl: './news-article.component.css'
})
export class NewsArticleComponent implements OnInit, OnDestroy {
  private newsService = inject(NewsService);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private as: AuthenticationService = inject(AuthenticationService);
  private us = inject(UsersService);
  userSubscription!: Subscription;
  user!: User;
  userInfo: AuthUser | undefined;
  currentDate = new Date();
  isAdmin: boolean = false;
  isEditor: boolean = false;
  url_slug = '';
  news!: News;

  ngOnInit() {
    this.userSubscription = this.as.userSubject.subscribe(user => {
      this.user = user;
      this.isAdmin = this.as.isAdmin();
      this.isEditor = this.as.isEditor();
      if (this.user?.id) this.getUser();
      console.log('Is Admin:', this.isAdmin, 'Is Editor:', this.isEditor);
    });
    this.activatedRoute.url.subscribe(urlSegment => {
      this.url_slug = urlSegment
        .map(x => x.path)
        .join('/')
        .replace('read-article/', '');
      if (this.url_slug === '') {
        this.router.navigate(['/404']);
      } else {
        this.newsService.newsGetNewsByUrl(this.url_slug).subscribe(news => (this.news = news));
      }
    });
  }
  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }
  getUser() {
    this.us.usersGetUserById(this.user.id).subscribe(usr => {
      this.userInfo = usr;
    });
  }
}
