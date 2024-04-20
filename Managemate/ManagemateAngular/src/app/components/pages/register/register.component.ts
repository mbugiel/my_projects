import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { PasswordsMatch } from '../../../shared/validators/PasswordsMatch';
import { CookieService } from 'ngx-cookie-service';
import { EncryptionService } from '../../../services/encryption/encryption.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports:[ReactiveFormsModule, CommonModule, RouterModule],
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  isSubmitted = false;
  regDataCookie!: any;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private cookie: CookieService,
    private encryption: EncryptionService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group(
      {
        username: ['', [Validators.required, Validators.minLength(3)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required],
        twoStepLogin: false,
      },
      {
        validators: PasswordsMatch('password', 'confirmPassword'),
      }
    );

    if (this.cookie.check('registration_data')) {
      this.regDataCookie = this.encryption.decrypt('registration_data');

      //console.log(this.regDataCookie);

      this.registerForm = this.fb.group({
        username: [this.regDataCookie.username],
        email: [this.regDataCookie.email],
        password: '',
        confirmPassword: '',
        twoStepLogin: [this.regDataCookie.twoStepLogin],
      });
    }
  }

  get fc() {
    return this.registerForm.controls;
  }

  onRegister() {
    this.isSubmitted = true;
    if (this.registerForm.invalid) return;

    this.auth
      .userExist({
        username: this.fc.username.value,
        email: this.fc.email.value,
      })
      .subscribe({
        next: () => {
          const expiration = new Date();
          expiration.setMinutes(expiration.getMinutes() + 4);
  
          this.cookie.set(
            'registration_data',
            this.encryption.encrypt({
              username: this.fc.username.value,
              email: this.fc.email.value,
              password: this.fc.password.value,
              twoStepLogin: this.fc.twoStepLogin.value,
              emailToken: '',
            }),
            { expires: expiration, path: '/' }
          );
  
          this.auth.sendEmail({
            username: this.fc.username.value,
            email: this.fc.email.value,
            password: this.fc.password.value,
            twoStepLogin: this.fc.twoStepLogin.value,
            template: 0,
          });
  
          this.router.navigateByUrl('/verify-email');
        },
        error: () => {}
      });
  }
}
