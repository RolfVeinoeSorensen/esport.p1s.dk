import { Component, inject } from '@angular/core';
import { UsersService } from '@app/_services/client';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-register',
  imports: [ButtonModule, InputTextModule, PasswordModule, ReactiveFormsModule],
  templateUrl: './user-register.component.html',
  styleUrl: './user-register.component.css',
})
export class UserRegisterComponent {
  private userService = inject(UsersService);
  private formBuilder = inject(UntypedFormBuilder);
  registerForm: UntypedFormGroup;
  // convenience getter for easy access to form fields
  get f() {
    return this.registerForm.controls;
  }
  constructor() {
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      password: ['', [Validators.required, Validators.maxLength(255)]],
      firstname: ['', [Validators.required, Validators.maxLength(255)]],
      lastname: ['', [Validators.required, Validators.maxLength(255)]],
    });
  }
  submitUser() {
    this.userService
      .usersRegisterUser({
        username: 'test',
        password: '',
        firstname: '',
        lastname: '',
      })
      .subscribe(response => {
        console.log(response);
      });
  }
}
