import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { EditorType } from '@models/editor-type';
import { SimpleId } from '@models/simple-id';
import { AuthUser, EventsService, EventSubmitRequest, Team, UsersService } from '@services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { DatePickerModule } from 'primeng/datepicker';
import { AutoCompleteCompleteEvent, AutoCompleteModule } from 'primeng/autocomplete';
import { Event } from '@services/client/model/event';
import { Subscription } from 'rxjs';
import { AuthenticationService } from '@services/authentication.service';
import { User } from '@models/user';

export interface UserExtended extends AuthUser {
  fullName?: string;
}

@Component({
  selector: 'app-event-editor',
  imports: [
    InputTextModule,
    ButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MessageModule,
    DatePickerModule,
    AutoCompleteModule
  ],
  templateUrl: './event-editor.component.html',
  styleUrl: './event-editor.component.css'
})
export class EventEditorComponent implements OnInit, OnChanges, OnDestroy {
  @Input() public id: SimpleId | undefined;
  @Output() closeHandler = new EventEmitter<EditorType>();
  private es = inject(EventsService);
  private us = inject(UsersService);
  private formBuilder = inject(UntypedFormBuilder);
  private its = inject(InternalToastService);
  private as: AuthenticationService = inject(AuthenticationService);

  userSubscription!: Subscription;
  user!: User;
  userInfo: AuthUser | undefined;
  currentDate = new Date();
  isAdmin: boolean = false;
  isEditor: boolean = false;

  allUsers: UserExtended[] = [];
  allTeams: Team[] = [];
  users: UserExtended[] = [];
  teams: Team[] = [];
  selectedUser: UserExtended | undefined;
  selectedTeam: Team | undefined;
  selectedEvent: Event | undefined;
  eventForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  startDateTime: Date | null = null;
  endDateTime: Date | null = null;

  constructor() {
    this.eventForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]]
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    console.log('Changes detected:', changes);
    this.reset();
  }

  ngOnInit(): void {
    this.reset();
  }
  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.eventForm.controls;
  }

  reset() {
    this.eventForm.reset();
    this.userSubscription = this.as.userSubject.subscribe(user => {
      this.user = user;
      this.isAdmin = this.as.isAdmin();
      this.isEditor = this.as.isEditor();
      if (this.user?.id) this.getUser();
    });
    if (this.id?.id && (this.selectedEvent === undefined || this.id.id !== this.selectedEvent?.id)) {
      this.es.eventsGetById(this.id.id).subscribe(event => {
        this.selectedEvent = event;
        this.eventForm.patchValue({
          name: event.name,
          description: event.description
        });
        this.startDateTime = new Date(event.startDateTime);
        this.endDateTime = new Date(event.endDateTime);
      });
      if (this.isAdmin === true) {
        if (this.allUsers.length === 0)
          this.us
            .usersGetAllUsers()
            .subscribe(users => (this.allUsers = users.map(u => ({ ...u, fullName: `${u.firstName} ${u.lastName}` }))));
        if (this.allTeams.length === 0) this.us.usersGetAllTeams().subscribe(teams => (this.allTeams = teams));
      }
    }
  }

  getUser() {
    this.us.usersGetUserById(this.user.id).subscribe(usr => {
      this.userInfo = usr;
    });
  }

  isInvalid(controlName: string) {
    const control = this.eventForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }
  onSubmit() {
    this.formSubmitted = true;
    if (this.eventForm.valid) {
      let request: EventSubmitRequest = {
        id: this.selectedEvent ? this.selectedEvent.id : 0,
        name: this.eventForm.value.name,
        description: this.eventForm.value.description,
        startDateTime: this.startDateTime?.toISOString() || '',
        endDateTime: this.endDateTime?.toISOString() || ''
      };
      this.es.eventsCreateOrUpdateEvent(request).subscribe(res => {
        this.selectedEvent = res;
        this.formSubmitted = false;
        this.its.addMessage({
          id: 'EventSaved',
          icon: 'pi pi-check-circle',
          summary: 'Event blev gemt',
          detail: 'Event blev gemt succesfuldt.',
          severity: 'success'
        });
      });
    } else {
      console.log('submit failed', this.eventForm.errors);
      this.formSubmitted = false;
    }
  }

  addTeam() {
    this.es.eventsAddTeamToEvent({ eventId: this.selectedEvent!.id, teamId: this.selectedTeam!.id }).subscribe(() => {
      this.reset();
    });
  }
  addUser() {}
  removeTeam(team: Team) {}
  removeUser(user: UserExtended) {}

  searchUsers(event: AutoCompleteCompleteEvent) {
    this.users = [...this.allUsers].filter(
      t =>
        t.firstName?.toLowerCase().includes(event.query.toLowerCase()) ||
        t.lastName?.toLowerCase().includes(event.query.toLowerCase())
    );
  }
  searchTeams(event: AutoCompleteCompleteEvent) {
    this.teams = [...this.allTeams].filter(t => t.name?.toLowerCase().includes(event.query.toLowerCase()));
  }
}
