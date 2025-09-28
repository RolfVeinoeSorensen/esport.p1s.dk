import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import Validation from '@app/_helpers/validation';
import { ChangePasswordRequest, UsersService } from '@app/_services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-user-password-change',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './user-password-change.component.html',
  styleUrl: './user-password-change.component.css',
})
export class UserPasswordChangeComponent implements OnInit {
  private userService = inject(UsersService);
  private formBuilder = inject(UntypedFormBuilder);
  private activatedRoute = inject(ActivatedRoute);
  private its = inject(InternalToastService);
  changePasswordForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  token!: string | null;
  // convenience getter for easy access to form fields
  get f() {
    return this.changePasswordForm.controls;
  }
  constructor() {
    this.changePasswordForm = this.formBuilder.group(
      {
        password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(255)]],
        passwordRepeat: ['', [Validators.required]],
      },
      {
        validators: [Validation.match('password', 'passwordRepeat')],
      }
    );
  }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.token = params.get('token');
      console.log(this.token);
    });
  }

  isInvalid(controlName: string) {
    const control = this.changePasswordForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.changePasswordForm.valid && this.token) {
      const req: ChangePasswordRequest = { password: this.changePasswordForm.value.password, token: this.token };
      this.userService.usersChangePassword(req).subscribe(response => {
        this.its.addMessage({
          id: 'usersChangePassword',
          icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
          summary: 'Password ændring',
          detail: response.message,
          severity: response.ok === true ? 'success' : 'error',
        });
        this.changePasswordForm.reset();
        this.formSubmitted = false;
      });
    } else {
      console.log('submit failed', this.changePasswordForm.errors);
      this.formSubmitted = false;
    }
  }
}
