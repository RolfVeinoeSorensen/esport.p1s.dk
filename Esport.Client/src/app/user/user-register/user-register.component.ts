import { Component, inject, OnInit } from '@angular/core';
import { CaptchaDto, CaptchaService, RegisterRequest, UsersService } from '@app/_services/client';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MessageModule } from 'primeng/message';
import Validation from '@helpers/validation';
import { InternalToastService } from '@services/internaltoast.service';

@Component({
  selector: 'app-user-register',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './user-register.component.html',
  styleUrl: './user-register.component.css',
})
export class UserRegisterComponent implements OnInit {
  private userService = inject(UsersService);
  private formBuilder = inject(UntypedFormBuilder);
  private captchaService = inject(CaptchaService);
  private its = inject(InternalToastService);
  registerForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  captcha!: CaptchaDto;
  // convenience getter for easy access to form fields
  get f() {
    return this.registerForm.controls;
  }
  constructor() {
    this.registerForm = this.formBuilder.group(
      {
        username: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
        password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(255)]],
        passwordRepeat: ['', [Validators.required]],
        firstname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(255)]],
        lastname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(255)]],
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
    const control = this.registerForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.registerForm.valid) {
      const req: RegisterRequest = {
        username: this.registerForm.value.username,
        firstname: this.registerForm.value.firstname,
        lastname: this.registerForm.value.lastname,
        password: this.registerForm.value.password,
        passwordRepeat: this.registerForm.value.passwordRepeat,
        captchaId: this.captcha.captchaId,
        captchaCode: this.registerForm.value.captchaCode,
      };
      this.userService.usersRegisterUser(req).subscribe(response => {
        if (response.ok === false && response.captcha) {
          this.captcha = response.captcha;
        }
        this.its.addMessage({
          id: 'userRegistered',
          icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
          summary: 'Bruger registrering',
          detail: response.message,
          severity: response.ok === true ? 'success' : 'error',
        });
        this.registerForm.reset();
      });
    } else {
      console.log('submit failed', this.registerForm.errors);
      this.formSubmitted = false;
    }
  }
}
