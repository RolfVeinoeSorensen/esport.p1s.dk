import { Component, inject, LOCALE_ID, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { filter, first, map, mergeMap, Subscription } from 'rxjs';
import { animate, style, transition, trigger } from '@angular/animations';
import { slideInAnimation } from '@app/animations';
import { MenuItem, MessageService } from 'primeng/api';
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
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { User } from '@models/user';
import { UserRole } from './_services/client';
import { InternalToastService } from './_services/internaltoast.service';
import { ToastCloseEvent, ToastModule } from 'primeng/toast';

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
    RouterLink,
    ToastModule,
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
      transition(':leave', [animate('500ms ease-in', style({ transform: 'translateY(-100%)' }))]),
    ]),
  ],
})
export class AppComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private ui = inject(UiService);
  private as: AuthenticationService = inject(AuthenticationService);
  private its = inject(InternalToastService);
  private ms = inject(MessageService);
  private formBuilder = inject(UntypedFormBuilder);
  title = 'p1s.dk';
  items: MenuItem[] | undefined;
  private locale = inject(LOCALE_ID);
  private meta = inject(Meta);
  visibleLogin: boolean = false;
  loginForm: UntypedFormGroup;
  isLoggedIn = false;
  isAdmin = false;
  isEditor = false;
  userSubscription!: Subscription;
  user!: User;
  returnUrl: string = '/';
  notificationSubscription!: Subscription;
  // convenience getter for easy access to form fields
  get f() {
    return this.loginForm.controls;
  }

  constructor() {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      password: ['', Validators.required],
    });
  }
  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }
  ngOnInit(): void {
    this.userSubscription = this.as.userSubject.subscribe(user => {
      this.user = user;
      if (user.username !== undefined) {
        this.isLoggedIn = true;
        this.isAdmin = this.as.isAdmin();
        this.isEditor = this.as.isEditor();
        this.refreshMenu();
      }
    });
    this.notificationSubscription = this.its.messages$.subscribe(messages => {
      this.ms.clear();
      this.ms.addAll(messages);
    });
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
      { name: 'author', content: 'Ea APS' },
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
        console.log('router.events', this.activatedRoute.snapshot.queryParams['logIn']);
        this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] ?? '/';
        this.visibleLogin = (this.activatedRoute.snapshot.queryParams['logIn'] as boolean) ?? false;
        this.ui.setMeta({
          title: event['title'],
          description: event['description'],
          tags: event['tags'],
          url: event['ogUrl'] ?? this.router.url,
        });
      });
    this.refreshMenu();
  }
  refreshMenu() {
    const items: MenuItem[] | undefined = [];
    if (this.isAdmin === true) {
      items.push({
        label: 'Administration',
        routerLink: 'admin',
      });
    }
    if (this.isLoggedIn === true) {
      items.push({
        label: 'Mine ting',
        items: [
          {
            label: 'Overblik',
            routerLink: 'my-stuff',
          },
          {
            label: 'Kalender',
            routerLink: 'my-stuff/team-calendar',
          },
        ],
      });
    }
    const itemsPublic: MenuItem[] | undefined = [
      {
        label: 'Nyheder',
        routerLink: 'news',
      },
      {
        label: 'Ydelser',
        items: [
          {
            label: 'E-sport træning',
            routerLink: 'services/e-sports',
          },
          {
            label: 'LAN parties',
            routerLink: 'services/lan-parties',
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
            label: 'Om os',
            routerLink: 'about',
          },
          {
            label: 'Mød holdet',
            routerLink: 'meet-the-team',
          },
        ],
      },
    ];
    this.items = items.concat(itemsPublic);
  }
  getAnimationData(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData['animation'];
  }
  toggleDarkMode() {
    const element = document.querySelector('html');
    if (element) element.classList.toggle('dark');
  }
  onSigninOrSignoutClick() {
    if (this.isLoggedIn) {
      this.as.logout();
      this.isLoggedIn = false;
      this.refreshMenu();
      this.router.navigate(['/']);
    } else {
      this.visibleLogin = true;
    }
  }
  signIn() {
    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }
    this.visibleLogin = false;
    this.as
      .login(this.f['username'].value, this.f['password'].value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.router.navigateByUrl(this.returnUrl);
        },
        error: error => {
          console.log(error);
          // this.error = error;
          // this.loading = false;
        },
      });
  }
  onToastClose(event: ToastCloseEvent) {
    this.its.removeMessage(event.message);
  }
}
