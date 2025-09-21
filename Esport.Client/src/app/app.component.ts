import { Component, inject, LOCALE_ID, OnInit } from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterOutlet,
  RouterLink,
} from '@angular/router';
import { animate, style, transition, trigger } from '@angular/animations';
import { slideInAnimation } from '@app/animations';
import { BehaviorSubject, Subscription, filter, map, mergeMap } from 'rxjs';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule, formatDate } from '@angular/common';
import { Ripple } from 'primeng/ripple';
import { ScrollTop } from 'primeng/scrolltop';
import { ButtonModule } from 'primeng/button';
import { Meta } from '@angular/platform-browser';
import { FooterComponent } from './cms/footer/footer.component';
import { UiService } from '@services/ui.service';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-root',
  animations: [
    slideInAnimation,
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateY(-100%)' }),
        animate('500ms ease-in', style({ transform: 'translateY(0%)' })),
      ]),
      transition(':leave', [animate('500ms ease-in', style({ transform: 'translateY(-100%)' }))]),
    ]),
  ],
  imports: [
    RouterOutlet,
    MenubarModule,
    InputTextModule,
    Ripple,
    CommonModule,
    ScrollTop,
    ButtonModule,
    FooterComponent,
    DialogModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private ui = inject(UiService);
  title = 'esport.p1s.dk';
  items: MenuItem[] | undefined;
  private locale = inject(LOCALE_ID);
  private meta = inject(Meta);
  visibleLogin: boolean = false;
  ngOnInit(): void {
    if (
      typeof window !== 'undefined' &&
      window.matchMedia &&
      window.matchMedia('(prefers-color-scheme: dark)').matches
    ) {
      this.toggleDarkMode();
    }
    //base tags are handled by router events
    this.meta.addTags([
      { name: 'robots', content: 'index. follow' },
      { name: 'author', content: 'Hourplanner APS' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      {
        name: 'date',
        content: formatDate(Date.now(), 'yyyy-MM-dd', this.locale),
        scheme: 'yyyy-MM-dd',
      },
      { charset: 'UTF-8' },
    ]);
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map(() => this.activatedRoute),
        map(route => {
          while (route.firstChild) route = route.firstChild;
          return route;
        }),
        filter(route => route.outlet === 'primary'),
        mergeMap(route => route.data)
      )
      .subscribe(event => {
        this.ui.setMeta({
          title: event['title'],
          description: event['description'],
          tags: event['tags'],
          url: event['ogUrl'] ?? this.router.url,
        });
      });
    this.items = [
      {
        label: "Nyheder"
      },
      {
        label: 'Ydelser',
        items: [
          {
            label: 'Træning',
            shortcut: '⌘+S',
          },
          {
            label: 'LAN',
            shortcut: '⌘+B',
          },
          {
            separator: true,
          },
        ],
      },
      {
        label: "Om os"
      },
    ];
  }
  getAnimationData(outlet: RouterOutlet) {
    return (
      outlet &&
      outlet.activatedRouteData &&
      outlet.activatedRouteData['animation']
    );
  }
  toggleDarkMode() {
    const element = document.querySelector('html');
    if (element) element.classList.toggle('app-dark');
  }
  signIn() {}
}
