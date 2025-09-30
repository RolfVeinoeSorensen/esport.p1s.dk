import { Component, inject, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '@services/authentication.service';
import { AuthUser, FileService, UsersService } from '@services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-user-editor',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule],
  templateUrl: './user-editor.component.html',
  styleUrl: './user-editor.component.css',
})
export class UserEditorComponent implements OnInit {
  @Input() public id: number | undefined;
  private us = inject(UsersService);
  private as: AuthenticationService = inject(AuthenticationService);
  private fs = inject(FileService);
  private formBuilder = inject(UntypedFormBuilder);
  private its = inject(InternalToastService);
  userForm: UntypedFormGroup;
  formSubmitted: boolean = false;
  userInfo: AuthUser | undefined;

  // convenience getter for easy access to form fields
  get f() {
    return this.userForm.controls;
  }
  // firstName?: string;
  // lastName?: string;
  // username?: string;
  // addressStreet?: string;
  // addressStreetNumber?: number;
  // addressFloor?: string;
  // addressSide?: string;
  // addressPostalCode?: string;
  // addressCity?: string;
  // mobile?: string;
  // consentShowImages: boolean;
  // canBringLaptop: boolean;
  // canBringStationaryPc: boolean;
  // canBringPlaystation: boolean;
  // imageId?: number;
  // image?: any;
  constructor() {
    this.userForm = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      username: ['', [Validators.required]],
      addressStreet: ['', [Validators.required]],
      addressStreetNumber: ['', [Validators.required]],
      addressFloor: ['', [Validators.required]],
      addressSide: ['', [Validators.required]],
      addressPostalCode: ['', [Validators.required]],
      addressCity: ['', [Validators.required]],
      mobile: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    console.log('ngInit', this.id);
    if (this.id)
      this.us.usersGetUserById(this.id).subscribe(userInfo => {
        this.userInfo = userInfo;
        this.f['username'].setValue(this.userInfo.username);
        this.f['firstName'].setValue(this.userInfo.firstName);
        this.f['lastName'].setValue(this.userInfo.lastName);
      });
  }
  isInvalid(controlName: string) {
    const control = this.userForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit() {
    // this.formSubmitted = true;
    // if (this.userForm.valid && this.userInfo) {
    //   this.userInfo.username = this.userForm.value.username;
    //   this.us.(this.userInfo).subscribe(response => {
    //     this.its.addMessage({
    //       id: 'userRegistered',
    //       icon: response.ok === true ? 'pi pi-check-circle' : 'pi pi-exclamation-triangle',
    //       summary: 'Bruger registrering',
    //       detail: response.message,
    //       severity: response.ok === true ? 'success' : 'error',
    //     });
    //     this.userForm.reset();
    //     this.formSubmitted = false;
    //   });
    // } else {
    //   console.log('submit failed', this.userForm.errors);
    //   this.formSubmitted = false;
    // }
  }
}
