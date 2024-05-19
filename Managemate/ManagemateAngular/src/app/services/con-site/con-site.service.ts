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
import { Location } from '@angular/common';
//interfaces
import { Add_Construction_Site_Data } from '../../shared/interfaces/API_Input_Models/Construction_Site_Models/Add_Construction_Site_Data';
import { Edit_Construction_Site_Data } from '../../shared/interfaces/API_Input_Models/Construction_Site_Models/Edit_Construction_Site_Data';
import { Delete_Construction_Site_Data } from '../../shared/interfaces/API_Input_Models/Construction_Site_Models/Delete_Construction_Site_Data';
import { Get_By_ID_Construction_Site_Data } from '../../shared/interfaces/API_Input_Models/Construction_Site_Models/Get_By_ID_Construction_Site_Data';
import { Output_Construction_Site_Model_Response } from '../../shared/interfaces/API_Output_Models/Construction_Site_Models/Output_Construction_Site_Model_Response';
import { Output_Construction_Site_Model_List } from '../../shared/interfaces/API_Output_Models/Construction_Site_Models/Output_Construction_Site_Model_List';
import { Get_All_Construction_Site_Data } from '../../shared/interfaces/API_Input_Models/Construction_Site_Models/Get_All_Construction_Site_Data';
import { Output_Cities_List_Model_List } from '../../shared/interfaces/API_Output_Models/Cities_List_Models/Output_Cities_List_Model_List';
import { Add_Cities_List_Data } from '../../shared/interfaces/API_Input_Models/Cities_List_Models/Add_Cities_List_Data';

@Injectable({
  providedIn: 'root'
})
export class ConSiteService{
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



  add_con_site(input:Add_Construction_Site_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Add_Construction_Site`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addConSite'));
            this.location.back();
            
          }
        })
      );

  }

  edit_con_site(input:Edit_Construction_Site_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Edit_Construction_Site`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editConSite'));
            this.location.back();
            
          }
        })
      );

  }


  delete_con_site(input:Delete_Construction_Site_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Delete_Construction_Site`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.deleteConSite'));
            
          }
        })
      );

  }

  get_con_site_by_id(input:Get_By_ID_Construction_Site_Data) : Observable<Output_Construction_Site_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);



    return this.http
      .post<Output_Construction_Site_Model_Response>(`${this.apiUrl}/Get_Construction_Site_By_ID`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }


  get_con_site_list(): Observable<Output_Construction_Site_Model_List> {
    
    const sessionData = this.encryption.decrypt('session_data');

    const get_con_site_list_input:Get_All_Construction_Site_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_con_site_list_input);



    return this.http
      .post<Output_Construction_Site_Model_List>(`${this.apiUrl}/Get_All_Construction_Site`, jsonData, this.options)
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

    const get_con_site_list_input:Get_All_Construction_Site_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_con_site_list_input);



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