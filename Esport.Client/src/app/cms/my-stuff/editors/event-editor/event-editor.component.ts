import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { EditorType } from '@models/editor-type';
import { FileService } from '@services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-event-editor',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './event-editor.component.html',
  styleUrl: './event-editor.component.css'
})
export class EventEditorComponent implements OnInit {
  @Input() public id: number | undefined;
  @Output() closeHandler = new EventEmitter<EditorType>();
  private fs = inject(FileService);
  private formBuilder = inject(UntypedFormBuilder);
  private its = inject(InternalToastService);
  eventForm: UntypedFormGroup;
  formSubmitted: boolean = false;

  constructor() {
    this.eventForm = this.formBuilder.group({
      server: ['', [Validators.required]],
      port: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
}
