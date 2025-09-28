import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import Validation from '@app/_helpers/validation';
import { CaptchaDto, CaptchaService, ForgotPasswordRequest, UsersService } from '@app/_services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-user-password-reset',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './user-password-reset.component.html',
  styleUrl: './user-password-reset.component.css',
})
export class UserPasswordResetComponent implements OnInit {
  private userService = inject(UsersService);
  private formBuilder = inject(UntypedFormBuilder);
  private captchaService = inject(CaptchaService);
  private its = inject(InternalToastService);
  resetForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  captcha!: CaptchaDto;
  // convenience getter for easy access to form fields
  get f() {
    return this.resetForm.controls;
  }
  constructor() {
    this.resetForm = this.formBuilder.group(
      {
        username: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
        captchaCode: ['', [Validators.required, , Validators.minLength(6), Validators.maxLength(6)]],
      },
      {
        validators: [Validation.match('password', 'passwordRepeat')],
      }
    );
  }

  ngOnInit() {
    this.captchaService.captchaGenerateCaptcha().subscribe(captcha => {
      this.captcha = captcha;
    });
  }

  isInvalid(controlName: string) {
    const control = this.resetForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.resetForm.valid) {
      const req: ForgotPasswordRequest = {
        email: this.resetForm.value.username,
        captchaId: this.captcha.captchaId,
        captchaCode: this.resetForm.value.captchaCode,
      };
      this.userService.usersForgotPassword(req).subscribe(response => {
        if (response.ok === false && response.captcha) {
          this.captcha = response.captcha;
        }
        this.its.addMessage({
          id: 'resetPassword',
          icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
          summary: 'Password nulstilling',
          detail: response.message,
          severity: response.ok === true ? 'success' : 'error',
        });
        this.resetForm.reset();
      });
    } else {
      console.log('submit failed', this.resetForm.errors);
      this.formSubmitted = false;
    }
  }
}
