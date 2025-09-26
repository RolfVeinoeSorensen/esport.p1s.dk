import { Component, inject } from '@angular/core';
import { EditorModule } from 'primeng/editor';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-news-editor',
  imports: [EditorModule, InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './news-editor.component.html',
  styleUrl: './news-editor.component.css',
})
export class NewsEditorComponent {
  private formBuilder = inject(UntypedFormBuilder);
  newsForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  // convenience getter for easy access to form fields
  get f() {
    return this.newsForm.controls;
  }
  constructor() {
    this.newsForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(255)]],
      content: ['', [Validators.required]],
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
      this.newsForm.reset();
      this.formSubmitted = false;
    }
  }
}
