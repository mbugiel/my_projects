import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { EncryptionService } from '../../../services/encryption/encryption.service';
import { ErrorHandlerService } from '../../../services/error-handler/error-handler.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  isSubmitted = false;
  public isLoaded:boolean = true;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private cookie: CookieService,
    private encryption: EncryptionService,
    private errorHandler: ErrorHandlerService,
    private toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  get fc() {
    return this.loginForm.controls;
  }

  onLogin() {
    this.isSubmitted = true;
    if (this.loginForm.invalid) return;

    this.isLoaded = false; //loader - visible

    const username = this.fc.username.value;
    const password = this.fc.password.value;

    this.auth
      .hasTwoStepLogin({
        username: username,
        password: password,
      })
      .subscribe({
        next: (response) => {
          if (response.responseData === '#') {
            this.auth.login({
              username: username,
              password: password,
              emailToken: '',
            }).subscribe({
              next: () => {

                this.isLoaded = true;// loader - hide

                this.router.navigateByUrl('/manage-mate/home');

              },
              error: err => {

                this.isLoaded = true;// loader - hide

                if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){
    
                  this.router.navigateByUrl('/login');
    
                }

              }
            });
          } else {
            const expiration = new Date();
            expiration.setMinutes(expiration.getMinutes() + 4);

            this.cookie.set(
              'login_data',
              this.encryption.encrypt({
                username: this.fc.username.value,
                password: this.fc.password.value,
                emailToken: '',
                email: response.responseData
              }),
              { expires: expiration, path: '/' }
            );

            this.auth.sendEmail({
              username: username,
              email: response.responseData,
              password: password,
              twoStepLogin: true,
              template: 1,
            });

            this.isLoaded = true;// loader - hide

            this.router.navigateByUrl('/two-step-login');
          }
        },
        error: (err: any) => {

          this.isLoaded = true;// loader - hide

          const errorCode = err.error.code;
          const errorMessage = this.errorHandler.getErrorMessage(errorCode);
          this.toastrService.error(errorMessage, 'Login Failed');
        },
      });
  }
}
