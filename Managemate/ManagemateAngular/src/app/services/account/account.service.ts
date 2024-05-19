import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '../error-handler/error-handler.service';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service';
import { TranslateService } from '@ngx-translate/core';
import { EncryptionService } from '../encryption/encryption.service';
import { Add_Company_Data } from '../../shared/interfaces/API_Input_Models/Company_Models/Add_Company_Data';
import { Output_Cities_List_Model_List } from '../../shared/interfaces/API_Output_Models/Cities_List_Models/Output_Cities_List_Model_List';
import { Add_Cities_List_Data } from '../../shared/interfaces/API_Input_Models/Cities_List_Models/Add_Cities_List_Data';
import { Output_Company_Model_Response } from '../../shared/interfaces/API_Output_Models/Company_Models/Output_Company_Model_Response';
import { Get_All_Company_Data } from '../../shared/interfaces/API_Input_Models/Company_Models/Get_All_Company_Data';
import { Edit_Company_Data } from '../../shared/interfaces/API_Input_Models/Company_Models/Edit_Company_Data';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private apiUrl = environment.apiUrl;

  private options = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
    
  };

  constructor(
    private http: HttpClient,
    private toastrService: ToastrService,
    private errorHandler: ErrorHandlerService,
    private encryption: EncryptionService,
    private router: Router,
    private cookie: CookieService,
    private translate: TranslateService) { }

  displayError(err:any){

    const errorCode = err.error.code;
    const errorMessage = this.errorHandler.getErrorMessage(errorCode);
    this.toastrService.error(errorMessage, this.translate.instant('errorMessages.error'));

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      if(this.cookie.check('session_data')) this.cookie.delete('session_data');

    }

  }

  displaySuccess(message:string){
    this.toastrService.success(message, this.translate.instant('successMessages.success'));
  }

  add_account_info(input:Add_Company_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Add_Company`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addCompany'));
            
          }
        })
      );

  }

  edit_account_info(input:Edit_Company_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Edit_Company`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editCompany'));
            
          }
        })
      );

  }

  get_company_data() : Observable<Output_Company_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    const get_company_data_input:Get_All_Company_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_company_data_input);



    return this.http
      .post<Output_Company_Model_Response>(`${this.apiUrl}/Get_Company`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            if(parseInt(err.error.code) !== 19){
              this.displayError(err);
            }

          }
        })
      );

  }

  get_cities_list():Observable<Output_Cities_List_Model_List>{
   
    const sessionData = this.encryption.decrypt('session_data');

    const get_company_data_input:Get_All_Company_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_company_data_input);



    return this.http
      .post<Output_Cities_List_Model_List>(`${this.apiUrl}/Get_All_Cities_List`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );
    
  }

  add_city(input:Add_Cities_List_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Add_Cities_List`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addCity'));
            
          }
        })
      );

  }
}
