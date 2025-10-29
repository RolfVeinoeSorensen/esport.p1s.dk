import { Component, inject, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '@services/authentication.service';
import { AuthUser, FileService, UsersService } from '@services/client';
import { InternalToastService } from '@services/internaltoast.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { ToggleSwitchModule } from 'primeng/toggleswitch';

@Component({
  selector: 'app-user-editor',
  imports: [InputTextModule, ButtonModule, ReactiveFormsModule, MessageModule, ToggleSwitchModule],
  templateUrl: './user-editor.component.html',
  styleUrl: './user-editor.component.css'
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

  // imageId?: number;
  // image?: any;
  constructor() {
    this.userForm = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      username: ['', [Validators.required]],
      addressStreet: ['', [Validators.required]],
      addressStreetNumber: ['', [Validators.required]],
      addressFloor: [''],
      addressSide: [''],
      addressPostalCode: ['', [Validators.required]],
      addressCity: ['', [Validators.required]],
      mobile: ['', [Validators.required]],
      consentShowImages: ['', [Validators.required]],
      canBringLaptop: ['', [Validators.required]],
      canBringStationaryPc: ['', [Validators.required]],
      canBringPlaystation: ['', [Validators.required]]
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
        this.f['addressStreet'].setValue(this.userInfo.addressStreet);
        this.f['addressStreetNumber'].setValue(this.userInfo.addressStreetNumber);
        this.f['addressFloor'].setValue(this.userInfo.addressFloor);
        this.f['addressSide'].setValue(this.userInfo.addressSide);
        this.f['addressPostalCode'].setValue(this.userInfo.addressPostalCode);
        this.f['addressCity'].setValue(this.userInfo.addressCity);
        this.f['mobile'].setValue(this.userInfo.mobile);
        this.f['consentShowImages'].setValue(this.userInfo.consentShowImages);
        this.f['canBringLaptop'].setValue(this.userInfo.canBringLaptop);
        this.f['canBringStationaryPc'].setValue(this.userInfo.canBringStationaryPc);
        this.f['canBringPlaystation'].setValue(this.userInfo.canBringPlaystation);
      });
  }
  isInvalid(controlName: string) {
    const control = this.userForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }

  onSubmit() {
    this.formSubmitted = true;
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
