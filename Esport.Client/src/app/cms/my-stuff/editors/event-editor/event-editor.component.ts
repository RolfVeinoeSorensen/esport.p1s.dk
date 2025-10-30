import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { EditorType } from '@models/editor-type';
import { SimpleId } from '@models/simple-id';
import { EventsService, EventSubmitRequest } from '@services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { Event } from '@services/client/model/event';

@Component({
  selector: 'app-event-editor',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './event-editor.component.html',
  styleUrl: './event-editor.component.css'
})
export class EventEditorComponent implements OnInit, OnChanges {
  @Input() public id: SimpleId | undefined;
  @Output() closeHandler = new EventEmitter<EditorType>();
  private es = inject(EventsService);
  private formBuilder = inject(UntypedFormBuilder);
  private its = inject(InternalToastService);
  selectedEvent: Event | undefined;
  eventForm: UntypedFormGroup;
  formSubmitted: boolean = false;

  constructor() {
    this.eventForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      startDateTime: ['', [Validators.required]],
      endDateTime: ['', [Validators.required]]
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    console.log('Changes detected:', changes);
    this.reset();
  }

  ngOnInit(): void {
    this.reset();
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.eventForm.controls;
  }

  reset() {
    this.eventForm.reset();
    if (this.id?.id) {
      this.es.eventsGetById(this.id.id).subscribe(event => {
        this.selectedEvent = event;
        this.eventForm.patchValue({
          name: event.name,
          description: event.description,
          startDateTime: event.startDateTime,
          endDateTime: event.endDateTime
          // Map other event properties to form fields as needed
        });
      });
    }
  }
  saveOrCreateEvent() {
    let request: EventSubmitRequest = {
      id: this.selectedEvent ? this.selectedEvent.id : 0,
      name: this.eventForm.value.name,
      description: this.eventForm.value.description,
      startDateTime: this.eventForm.value.startDateTime,
      endDateTime: this.eventForm.value.endDateTime
    };
    this.es.eventsCreateOrUpdateEvent(request).subscribe(res => {
      console.log('Event saved successfully:', res);
    });
  }
}
