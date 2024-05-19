import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { EncryptionService } from '../../../services/encryption/encryption.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {RouterModule} from '@angular/router';
import { Subscription, timer } from 'rxjs';


@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  standalone: true,
  imports:[ReactiveFormsModule, CommonModule, RouterModule],
  styleUrl: './verify-email.component.css'
})
export class VerifyEmailComponent implements OnInit {
  verificationForm!: FormGroup;
  email!: string;
  registrationData!: any;
  isResendDisabled = false;
  countdown = 30; //sekundy
  cooldownSubscription!: Subscription;
  public isLoaded:boolean = true;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private cookie: CookieService,
    private encryption: EncryptionService
  ) { 
    if(!this.cookie.check('registration_data')){
      this.router.navigateByUrl('/register');
    }
  }

  ngOnInit(): void {
    this.verificationForm = this.fb.group({
      verificationCode: ['', [Validators.required, Validators.minLength(6)]]
    })
    this.registrationData = this.encryption.decrypt('registration_data');
    this.email = this.registrationData.email;

    this.isResendDisabled = true;
    this.startCoooldown();
  }

  checkEmail() {
    if (this.verificationForm.invalid) return;

    const verificationCode = this.verificationForm.controls.verificationCode.value.toUpperCase();
    this.registrationData.emailToken = verificationCode;

    this.isLoaded = false;

    this.auth.register(this.registrationData).subscribe(() => {

      this.isLoaded = true;
      this.router.navigateByUrl('/manage-mate/home');

    })

    this.isLoaded = true;

  }

  resendEmail(){
    if (this.isResendDisabled) return;

    this.isResendDisabled = true;

    this.startCoooldown();

    this.auth.sendEmail({
      username: this.registrationData.username,
      email: this.registrationData.email,
      password: this.registrationData.password,
      twoStepLogin: this.registrationData.twoStepLogin,
      template: 0,
    });
  }

  startCoooldown(){

    this.cooldownSubscription = timer(1000,1000).subscribe(() => {
      if (this.countdown > 0) {
        this.countdown--;
      } else {
        this.isResendDisabled = false;
        this.countdown = 30;
        this.cooldownSubscription?.unsubscribe();
      }
    });

  }
}
