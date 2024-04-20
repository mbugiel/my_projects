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
  selector: 'app-two-step-login',
  standalone: true,
  imports:[ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './two-step-login.component.html',
  styleUrl: './two-step-login.component.css'
})
export class TwoStepLoginComponent implements OnInit {
  verificationForm!: FormGroup;
  email!: string;
  loginData!: any;
  isResendDisabled = false;
  countdown = 30; //sekundy
  cooldownSubscription!: Subscription;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private cookie: CookieService,
    private encryption: EncryptionService
  ) { 
    if(!this.cookie.check('login_data')){
      this.router.navigateByUrl('/login');
    }
  }

  ngOnInit(): void {
    this.verificationForm = this.fb.group({
      verificationCode: ['', [Validators.required, Validators.minLength(6)]]
    })
    this.loginData = this.encryption.decrypt('login_data');
    this.email = this.loginData.email.toLowerCase();

    this.isResendDisabled = true;
    this.startCoooldown();
  }

  checkCode() {

    if (this.verificationForm.invalid) return;

    const verificationCode = this.verificationForm.controls.verificationCode.value.toUpperCase();
    this.loginData.emailToken = verificationCode;

    this.auth.login(this.loginData).subscribe({

      next: () => {

        this.router.navigateByUrl('/manage-mate/home');

      },
      error: err => {

        if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){

          this.router.navigateByUrl('/login');

        }

      }

    });

  }

  resendEmail(){
    if (this.isResendDisabled) return;

    this.isResendDisabled = true;

    this.startCoooldown();

    this.auth.sendEmail({
      username: this.loginData.username,
      email: this.loginData.email,
      password: this.loginData.password,
      twoStepLogin: true,
      template: 1,
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

  ngOnDestroy() {
    this.cooldownSubscription?.unsubscribe();
  }
}
