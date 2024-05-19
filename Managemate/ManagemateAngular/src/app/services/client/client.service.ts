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
//interfaces
import { Output_Client_Model_List } from '../../shared/interfaces/API_Output_Models/Client_Models/Output_Client_Model_List';
import { Get_All_Client_Data } from '../../shared/interfaces/API_Input_Models/Client_Models/Get_All_Client_Data';
import { Add_Client_Data } from '../../shared/interfaces/API_Input_Models/Client_Models/Add_Client_Data';
import { Delete_Client_Data } from '../../shared/interfaces/API_Input_Models/Client_Models/Delete_Client_Data';
import { Output_Cities_List_Model_List } from '../../shared/interfaces/API_Output_Models/Cities_List_Models/Output_Cities_List_Model_List';
import { Edit_Client_Data } from '../../shared/interfaces/API_Input_Models/Client_Models/Edit_Client_Data';
import { Get_By_ID_Client_Data } from '../../shared/interfaces/API_Input_Models/Client_Models/Get_By_ID_Client_Data';
import { Output_Client_Model_Response } from '../../shared/interfaces/API_Output_Models/Client_Models/Output_Client_Model_Response';
import { Add_Cities_List_Data } from '../../shared/interfaces/API_Input_Models/Cities_List_Models/Add_Cities_List_Data';
import { Output_Authorized_Worker_Model_List } from '../../shared/interfaces/API_Output_Models/Authorized_Worker_Models/Output_Authorized_Worker_Model_List';
import { Get_All_Authorized_Worker_Data } from '../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Get_All_Authorized_Worker_Data';
import { Get_By_ID_Authorized_Worker_Data } from '../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Get_By_ID_Authorized_Worker_Data';
import { Output_Authorized_Worker_Model_Response } from '../../shared/interfaces/API_Output_Models/Authorized_Worker_Models/Output_Authorized_Worker_Model_Response';
import { Delete_Authorized_Worker_Data } from '../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Delete_Authorized_Worker_Data';
import { Add_Authorized_Worker_Data } from '../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Add_Authorized_Worker_Data';
import { Edit_Authorized_Worker_Data } from '../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Edit_Authorized_Worker_Data';
import { Location } from '@angular/common';


@Injectable({
  providedIn: 'root'
})
export class ClientService {
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
    private translate: TranslateService,
    private location: Location
  ) { }

  
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



  add_client(input:Add_Client_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Add_Client`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addClient'));
            this.location.back();
            
          }
        })
      );

  }

  edit_client(input:Edit_Client_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Edit_Client`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editClient'));
            this.location.back();
            
          }
        })
      );

  }


  delete_client(input:Delete_Client_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Delete_Client`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.deleteClient'));
            
          }
        })
      );

  }

  get_client_by_id(input:Get_By_ID_Client_Data) : Observable<Output_Client_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);



    return this.http
      .post<Output_Client_Model_Response>(`${this.apiUrl}/Get_Client_by_ID`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }


  get_client_list(): Observable<Output_Client_Model_List> {
    
    const sessionData = this.encryption.decrypt('session_data');

    const get_client_list_input:Get_All_Client_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_client_list_input);



    return this.http
      .post<Output_Client_Model_List>(`${this.apiUrl}/Get_All_Client`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );
  }

  get_auth_worker_list(input: Get_All_Authorized_Worker_Data): Observable<Output_Authorized_Worker_Model_List> {
    
    const sessionData = this.encryption.decrypt('session_data');

    const get_auth_worker_list_input:Get_All_Authorized_Worker_Data = {session:sessionData, client_id:input.client_id};

    const jsonData = JSON.stringify(get_auth_worker_list_input);



    return this.http
      .post<Output_Authorized_Worker_Model_List>(`${this.apiUrl}/Get_All_Authorized_Worker`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );
  }

  get_auth_worker_by_id(input:Get_By_ID_Authorized_Worker_Data) : Observable<Output_Authorized_Worker_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);

    return this.http
      .post<Output_Authorized_Worker_Model_Response>(`${this.apiUrl}/Get_Authorized_Worker_By_ID`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }

  add_auth_worker(input:Add_Authorized_Worker_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Add_Authorized_Worker`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addAuthWorker'));
            
          }
        })
      );

  }

  edit_auth_worker(input:Edit_Authorized_Worker_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Edit_Authorized_Worker`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editAuthWorker'));
            
          }
        })
      );

  }

  delete_auth_worker(input:Delete_Authorized_Worker_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Delete_Authorized_Worker`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.deleteAuthWorker'));
            
          }
        })
      );

  }

  get_cities_list():Observable<Output_Cities_List_Model_List>{
   
    const sessionData = this.encryption.decrypt('session_data');

    const get_client_list_input:Get_All_Client_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_client_list_input);



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