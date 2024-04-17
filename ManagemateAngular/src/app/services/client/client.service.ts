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
    private translate: TranslateService
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
      .post(`${this.apiUrl}/AddClient`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addClient'));
            
          }
        })
      );

  }

  edit_client(input:Edit_Client_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/EditClient`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editClient'));
            
          }
        })
      );

  }


  delete_client(input:Delete_Client_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/DeleteClient`, jsonData, this.options)
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
      .post<Output_Client_Model_List>(`${this.apiUrl}/Get_All_Clients`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

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