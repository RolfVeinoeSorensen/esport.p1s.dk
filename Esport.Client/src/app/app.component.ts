import { Component, inject, LOCALE_ID, OnDestroy, OnInit } from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterOutlet,
  RouterLink
} from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { filter, first, map, mergeMap, Subscription } from 'rxjs';
import { animate, style, transition, trigger } from '@angular/animations';
import { slideInAnimation } from '@app/animations';
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
import { AuthenticationService } from '@services/authentication.service';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { User } from '@models/user';
import { UserRole } from './_models/userrole';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    MenubarModule,
    InputTextModule,
    Ripple,
    CommonModule,
    ScrollTop,
    ButtonModule,
    FooterComponent,
    DialogModule,
    ReactiveFormsModule,
    RouterLink
],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  animations: [
    slideInAnimation,
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateY(-100%)' }),
        animate('500ms ease-in', style({ transform: 'translateY(0%)' })),
      ]),
      transition(':leave', [
        animate('500ms ease-in', style({ transform: 'translateY(-100%)' })),
      ]),
    ]),
  ],
})
export class AppComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private ui = inject(UiService);
  private authenticationService: AuthenticationService = inject(
    AuthenticationService
  );
  userSubscription!: Subscription;
  title = 'esport.p1s.dk';
  items: MenuItem[] | undefined;
  private locale = inject(LOCALE_ID);
  private meta = inject(Meta);
  visibleLogin: boolean = false;
  loginForm: UntypedFormGroup;
  isLoggedIn = false;
  user!: User;
  // convenience getter for easy access to form fields
  get f() {
    return this.loginForm.controls;
  }

  constructor(private formBuilder: UntypedFormBuilder) {
    this.loginForm = this.formBuilder.group({
      username: [
        '',
        [Validators.required, Validators.email, Validators.maxLength(255)],
      ],
      password: ['', Validators.required],
    });
  }
  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }
  ngOnInit(): void {
    this.userSubscription = this.authenticationService.userSubject.subscribe(
      (user) => {
        this.user = user;
        if (user.username !== undefined) {
          this.isLoggedIn = true;
          this.refreshMenu();
        }
      }
    );
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
        filter((event) => event instanceof NavigationEnd),
        map(() => this.activatedRoute),
        map((route) => {
          while (route.firstChild) route = route.firstChild;
          return route;
        }),
        filter((route) => route.outlet === 'primary'),
        mergeMap((route) => route.data)
      )
      .subscribe((event) => {
        this.ui.setMeta({
          title: event['title'],
          description: event['description'],
          tags: event['tags'],
          url: event['ogUrl'] ?? this.router.url,
        });
      });
    this.loginForm = this.formBuilder.group({
      username: [
        '',
        [Validators.required, Validators.email, Validators.maxLength(255)],
      ],
      password: ['', Validators.required],
    });
    this.refreshMenu();
  }
  refreshMenu() {
    this.items = [
      {
        label: 'Nyheder',
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
        label: 'Klubben',
        items: [
          {
            label: "Om os"
          },
          {
            label: "Mød holdet"
          }
        ]
      },
    ];
    if (this.isLoggedIn) {
      this.items.push({
        label: 'Mine ting',
        items: [
          {
            label: "Overblik",
            routerLink: "my-stuff"
          },
          {
            label: 'Kalender',
            routerLink: "my-stuff/team-calendar",
          },
        ],
      });
    }
    if (this.user.role === UserRole.Admin) {
      this.items.push({
        label: 'Administration',
      });
    }
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
    if (element) element.classList.toggle('dark');
  }
  signIn() {
    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }
    this.visibleLogin = false;
    this.authenticationService
      .login(this.f['username'].value, this.f['password'].value)
      .pipe(first())
      .subscribe({
        next: () => {
          // get return url from query parameters or default to home page
          const returnUrl =
            this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigateByUrl(returnUrl);
        },
        error: (error) => {
          console.log(error);
          // this.error = error;
          // this.loading = false;
        },
      });
  }
}
