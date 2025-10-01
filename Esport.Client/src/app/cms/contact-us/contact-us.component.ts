import { Component, inject, OnInit } from '@angular/core';
import { EditorModule } from 'primeng/editor';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { CaptchaDto, CaptchaService, ContactRequest, ContactService } from '@app/_services/client';

@Component({
  selector: 'app-contact-us',
  imports: [EditorModule, InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './contact-us.component.html',
  styleUrl: './contact-us.component.css'
})
export class ContactUsComponent implements OnInit {
  private formBuilder = inject(UntypedFormBuilder);
  private contactService = inject(ContactService);
  private captchaService = inject(CaptchaService);
  contactForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  captcha!: CaptchaDto;
  // convenience getter for easy access to form fields
  get f() {
    return this.contactForm.controls;
  }
  constructor() {
    this.contactForm = this.formBuilder.group({
      body: ['', [Validators.required]],
      captchaCode: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
      contactFrom: ['', [Validators.required, Validators.maxLength(255)]],
      contactName: ['', [Validators.required, Validators.maxLength(255)]],
      subject: ['', [Validators.required, Validators.maxLength(255)]],
      contactMobile: ['', [Validators.maxLength(50)]]
    });
  }
  ngOnInit() {
    this.captchaService.captchaGenerateCaptcha().subscribe(captcha => {
      this.captcha = captcha;
    });
  }
  isInvalid(controlName: string) {
    const control = this.contactForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.contactForm.valid) {
      const req: ContactRequest = {
        body: this.contactForm.value.body,
        captchaCode: this.contactForm.value.captchaCode,
        contactFrom: this.contactForm.value.contactFrom,
        contactName: this.contactForm.value.contactName,
        subject: this.contactForm.value.subject,
        contactMobile: this.contactForm.value.contactMobile,
        captchaId: this.captcha.captchaId
      };
      this.contactService.contactCreateContact(req).subscribe(response => {
        if (response.ok === false && response.captcha) {
          this.captcha = response.captcha;
        }
        this.contactForm.reset();
      });
    } else {
      console.log('submit failed', this.contactForm.errors);
      this.formSubmitted = false;
    }
  }
}
