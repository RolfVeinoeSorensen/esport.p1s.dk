import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthUser, News, NewsService, UsersService } from '@app/_services/client';
import { User } from '@models/user';
import { AuthenticationService } from '@services/authentication.service';
import { Subscription } from 'rxjs';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-news',
  imports: [DatePipe, RouterLink, ButtonModule],
  templateUrl: './news.component.html',
  styleUrl: './news.component.css'
})
export class NewsComponent implements OnInit {
  private newsService = inject(NewsService);
  private as: AuthenticationService = inject(AuthenticationService);
  private us = inject(UsersService);
  news: News[] = [];
  page = 1;
  pageSize = 10;
  userSubscription!: Subscription;
  user!: User;
  userInfo: AuthUser | undefined;
  currentDate = new Date();
  isAdmin: boolean = false;
  isEditor: boolean = false;
  ngOnInit(): void {
    this.userSubscription = this.as.userSubject.subscribe(user => {
      this.user = user;
      this.isAdmin = this.as.isAdmin();
      this.isEditor = this.as.isEditor();
      if (this.user?.id) this.getUser();
      console.log('Is Admin:', this.isAdmin, 'Is Editor:', this.isEditor);
    });
    this.getNews();
  }
  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }
  getNews() {
    this.newsService.newsGetNews(this.page, this.pageSize).subscribe(news => {
      this.news = news;
    });
  }
  getUser() {
    this.us.usersGetUserById(this.user.id).subscribe(usr => {
      this.userInfo = usr;
    });
  }
}
