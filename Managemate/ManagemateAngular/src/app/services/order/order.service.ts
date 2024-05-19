import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Location } from '@angular/common';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '../error-handler/error-handler.service';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service';
import { TranslateService } from '@ngx-translate/core';
import { EncryptionService } from '../encryption/encryption.service';
import { Get_All_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Get_All_Order_Data';
import { Output_Order_Model_Response } from '../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Model_Response';
import { Add_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Add_Order_Data';
import { Edit_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Edit_Order_Data';
import { Delete_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Delete_Order_Data';
import { Get_By_ID_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Get_By_ID_Order_Data';
import { Output_Order_Advanced_Model_Response } from '../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Advanced_Model_Response';
import { DateHandlerService } from '../date-handler/date-handler.service';


@Injectable({
  providedIn: 'root'
})
export class OrderService {


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
    private location: Location,
    private cookie: CookieService,
    private translate: TranslateService,
    private dateHandler: DateHandlerService
  ) { }




  displayError(err:any){

    const errorCode = err.error.code;
    const errorMessage = this.errorHandler.getErrorMessage(errorCode);
    this.toastrService.error(errorMessage, this.translate.instant('errorMessages.error'));

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      if(this.cookie.check('session_data')) this.cookie.delete('session_data', '/');

    }

  }

  displaySuccess(message:string){
    this.toastrService.success(message, this.translate.instant('successMessages.success'));
  }




  // ------------------------ ORDERS -------------------------------------------------- //



  add_order(input:Add_Order_Data){

    const sessionData = this.encryption.decrypt('session_data');
    const current_date = this.dateHandler.changeToUtc(new Date());

    input.session = sessionData;
    input.creation_date = current_date;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Add_Order`, jsonData, this.options)
      .pipe(
        tap({
          next: resp => {
            console.log(resp)
          },
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addOrder'));
            this.location.back();
            
          }
        })
      );

  }

  edit_order(input:Edit_Order_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Edit_Order`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editOrder'));
            this.location.back();
            
          }
        })
      );

  }

  delete_order(input:Delete_Order_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Delete_Order`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.deleteOrder'));
            
          }
        })
      );

  }

  get_order_by_id(input:Get_By_ID_Order_Data) : Observable<Output_Order_Advanced_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);



    return this.http
      .post<Output_Order_Advanced_Model_Response>(`${this.apiUrl}/Get_Order_By_ID`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }



  get_orders(input:Get_All_Order_Data): Observable<Output_Order_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post<Output_Order_Model_Response>(`${this.apiUrl}/Get_All_Order`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }

  

}
