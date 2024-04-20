import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { UserLogin } from '../../shared/interfaces/API_Other_Models/User_Models/UserLogin';
import { UserRegister } from '../../shared/interfaces/API_Other_Models/User_Models/UserRegister';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '../error-handler/error-handler.service';
import { Router } from '@angular/router';
import { SendEmail } from '../../shared/interfaces/API_Other_Models/Email_Models/SendEmail';
import { environment } from '../../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service';
import { UserExist } from '../../shared/interfaces/API_Other_Models/User_Models/UserExist';
import { TranslateService } from '@ngx-translate/core';
import { EncryptionService } from '../encryption/encryption.service';
import { Session_Data_Response } from '../../shared/interfaces/API_Other_Models/Session_Models/Session_Data_Response';
import { TwoStepLogin } from '../../shared/interfaces/API_Other_Models/TwoStepLogin_Models/TwoStepLogin';
import { TwoStepLogin_Data_Response } from '../../shared/interfaces/API_Other_Models/TwoStepLogin_Models/TwoStepLogin_Data_Response';
import { ValidatePassword } from '../../shared/interfaces/API_Input_Models/User_Models/ValidatePassword';


@Injectable({
  providedIn: 'root',
})
export class AuthService implements OnInit {
  private apiUrl = environment.apiUrl;
  

  private options = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
    
  };

  private registrationData!: UserRegister;

  constructor(
    private http: HttpClient,
    private toastrService: ToastrService,
    private errorHandler: ErrorHandlerService,
    private encryption: EncryptionService,
    private router: Router,
    private cookie: CookieService,
    private translate: TranslateService
  ) {  }

  ngOnInit(): void {}






  displayError(err:any){

    const errorCode = err.error.code;
    const errorMessage = this.errorHandler.getErrorMessage(errorCode);
    this.toastrService.error(errorMessage, this.translate.instant('errorMessages.error'));

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      if(this.cookie.check('login_data')) this.cookie.delete('login_data', '/');
      if(this.cookie.check('session_data')) this.cookie.delete('session_data');

    }

  }


  displaySuccess(message:string){
    this.toastrService.success(message, this.translate.instant('successMessages.success'));
  }










  login(userLogin: UserLogin): Observable<Session_Data_Response> {
    const jsonData = JSON.stringify(userLogin);

    return this.http
      .post<Session_Data_Response>(`${this.apiUrl}/Login`, jsonData, this.options)
      .pipe(
        tap({
          next: (response) => {

            const expiration = new Date();
            expiration.setHours(expiration.getHours()+168);   

            this.cookie.set(
              'session_data',
              this.encryption.encrypt({
                token: response.responseData.token,
                userid: response.responseData.userId,
              }),
              { expires: expiration, path: '/' }
            );

            if(this.cookie.check('login_data')) this.cookie.delete('login_data', '/');

            this.displaySuccess(this.translate.instant('successMessages.welcome')+userLogin.username+"!");

          },
          error: err => {

            this.displayError(err);

          }
        })
      )
  }


  logout() {

    const session_Data = this.encryption.decrypt('session_data');

    const jsonData = JSON.stringify(session_Data);


    this.http
      .post(`${this.apiUrl}/Logout`, jsonData, this.options)
      .subscribe({
        next: () => {

          this.cookie.deleteAll('/');

          this.displaySuccess(this.translate.instant('successMessages.logout'));

          this.router.navigateByUrl('/');
          
        },
        error: err => {

          this.displayError(err);

        }
      });

      // this.router.navigateByUrl('/');
  }

  isLoggedIn(): boolean {

    if (this.cookie.check('session_data')) {
      return true;
    } else {
      return false;
    }

  }



  userExist(userExist: UserExist): Observable<any> {

    const jsonData = JSON.stringify(userExist);

    return this.http
      .post(`${this.apiUrl}/UserExist`, jsonData, this.options)
      .pipe(
        tap({
          error: (errorResponse) => {
            
            this.displayError(errorResponse);

          },
        })
      );
  }



  register(userRegister: UserRegister): Observable<Session_Data_Response> {
    const jsonData = JSON.stringify(userRegister);

    return this.http
      .post<Session_Data_Response>(`${this.apiUrl}/AddUser`, jsonData, this.options)
      .pipe(
        tap({
          next: (response) => {

            const expiration = new Date();
            expiration.setHours(expiration.getHours()+168);   

            this.cookie.set(
              'session_data',
              this.encryption.encrypt({
                token: response.responseData.token,
                userid: response.responseData.userId,
              }),
              { expires: expiration, path: '/' }
            );

            this.cookie.delete('registration_data', '/');

            this.displaySuccess(this.translate.instant('successMessages.welcome')+userRegister.username+"!");

          },
          error: (errorResponse) => {

            this.displayError(errorResponse);

          },
        })
      );
  }



  setRegistrationData(registrationData: UserRegister) {
    this.registrationData = registrationData;
  }



  getRegistrationData() {
    return this.registrationData;
  }



  sendEmail(sendEmail: SendEmail) {

    const jsonData = JSON.stringify(sendEmail);

    return this.http
      .post(`${this.apiUrl}/SendEmail`, jsonData, this.options)
      .subscribe(() => {
        // console.log('The email has been sent!', data);

        this.toastrService.show(
          this.translate.instant('successMessages.emailInfo').replace('{0}', sendEmail.email),
          this.translate.instant('successMessages.emailSent')
        );
      });

  }



  hasTwoStepLogin(twoStepLogin: TwoStepLogin){
    const jsonData = JSON.stringify(twoStepLogin);
    return this.http.post<TwoStepLogin_Data_Response>(`${this.apiUrl}/HasTwoStepLogin`, jsonData, this.options);
  }


/* ---------------------- Roboty Drogowe ----------------------------------- */

  validatePassword(input: ValidatePassword) : Observable<any>{

    const sessionData = this.encryption.decrypt('session_data');

    input.sessionData = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/ValidatePassword`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }



}
