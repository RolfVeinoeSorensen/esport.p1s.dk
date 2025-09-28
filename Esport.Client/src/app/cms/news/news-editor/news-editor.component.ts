import { Component, inject, OnInit } from '@angular/core';
import { EditorModule } from 'primeng/editor';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { News, NewsService } from '@app/_services/client';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-news-editor',
  imports: [EditorModule, InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './news-editor.component.html',
  styleUrl: './news-editor.component.css',
})
export class NewsEditorComponent implements OnInit {
  private formBuilder = inject(UntypedFormBuilder);
  private newsService = inject(NewsService);
  private activatedRoute = inject(ActivatedRoute);
  isNewPage = true;
  url_slug: string = '';
  newsForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  news!: News;
  // convenience getter for easy access to form fields
  get f() {
    return this.newsForm.controls;
  }
  constructor() {
    this.newsForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(255)]],
      content: ['', [Validators.required]],
      url_slug: ['', [Validators.required, Validators.maxLength(255)]],
    });
  }
  ngOnInit() {
    this.activatedRoute.url.subscribe(urlSegment => {
      this.url_slug = urlSegment.map(x => x.path).join('/');
      if (this.url_slug === '') {
        this.isNewPage = true;
      } else {
        this.isNewPage = false;
        this.getData();
      }
    });
  }
  getData() {
    this.newsService.newsGetNewsByUrl(this.url_slug).subscribe(news => {
      this.news = news;
      if (this.news) {
        this.isNewPage = false;
        this.newsForm.patchValue({
          title: this.news.title,
          content: this.news.content,
          url_slug: this.news.urlSlug,
        });
      }
      console.log(news);
    });
  }
  isInvalid(controlName: string) {
    const control = this.newsForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.newsForm.valid) {
      console.log(this.newsForm.value);
      this.newsService
        .newsCreateOrUpdateNews({
          title: this.newsForm.value.title,
          content: this.newsForm.value.content,
          id: this.news ? this.news.id : 0,
          isPublished: this.news ? this.news.isPublished : false,
          createdAt: this.news ? this.news.createdAt : new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          createdBy: this.news ? this.news.createdBy : 1,
          updatedBy: 1,
          urlSlug: this.newsForm.value.url_slug,
        })
        .subscribe(news => {
          console.log(news);
          this.news = news;
          this.isNewPage = false;
          this.url_slug = news.urlSlug != undefined ? news.urlSlug : this.url_slug;
        });
    }
    this.newsForm.reset();
    this.formSubmitted = false;
  }
}
