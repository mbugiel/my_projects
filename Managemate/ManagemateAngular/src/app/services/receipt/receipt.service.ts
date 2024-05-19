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
import { Get_All_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Get_All_Order_Data';
import { Output_Order_Model_Response } from '../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Model_Response';
import { Add_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Add_Order_Data';
import { Edit_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Edit_Order_Data';
import { Delete_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Delete_Order_Data';
import { Get_By_ID_Order_Data } from '../../shared/interfaces/API_Input_Models/Order_Models/Get_By_ID_Order_Data';
import { Output_Order_Advanced_Model_Response } from '../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Advanced_Model_Response';
import { Get_All_Receipt_Data } from '../../shared/interfaces/API_Input_Models/Receipt_Models/Get_All_Receipt_Data';
import { Output_Receipt_Model_Response } from '../../shared/interfaces/API_Output_Models/Receipt_Models/Output_Receipt_Model_Response';
import { Add_Receipt_Data } from '../../shared/interfaces/API_Input_Models/Receipt_Models/Add_Receipt_Data';
import { Output_Receipt_Model_Id_Response } from '../../shared/interfaces/API_Output_Models/Receipt_Models/Output_Receipt_Model_Id_Response';
import { DateHandlerService } from '../date-handler/date-handler.service';
import { Get_By_ID_Receipt_Data } from '../../shared/interfaces/API_Input_Models/Receipt_Models/Get_By_ID_Receipt_Data';
import { Output_Receipt_Advanced_Model_Response } from '../../shared/interfaces/API_Output_Models/Receipt_Models/Output_Receipt_Advanced_Model_Response';
import { Edit_Receipt_Data } from '../../shared/interfaces/API_Input_Models/Receipt_Models/Edit_Receipt_Data';

@Injectable({
  providedIn: 'root'
})
export class ReceiptService {

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
    private error_Handler: ErrorHandlerService,
    private encryption: EncryptionService,
    public date_Handler:DateHandlerService,
    private router: Router,
    private cookie: CookieService,
    private translate: TranslateService
  ) { }




  displayError(err:any){

    const errorCode = err.error.code;
    const errorMessage = this.error_Handler.getErrorMessage(errorCode);
    this.toastrService.error(errorMessage, this.translate.instant('errorMessages.error'));

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      if(this.cookie.check('session_data')) this.cookie.delete('session_data', '/');

    }

  }

  displaySuccess(message:string){
    this.toastrService.success(message, this.translate.instant('successMessages.success'));
  }


  add_receipt(input:Add_Receipt_Data) :Observable<Output_Receipt_Model_Id_Response>{

    const sessionData = this.encryption.decrypt('session_data');
    const current_date = this.date_Handler.changeToUtc(new Date());

    input.session = sessionData;
    input.creation_date = current_date;

    const jsonData = JSON.stringify(input);


    return this.http
      .post<Output_Receipt_Model_Id_Response>(`${this.apiUrl}/Add_Receipt`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }




  edit_receipt(input:Edit_Receipt_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/Edit_Receipt`, jsonData, this.options)
      .pipe(
        tap({
          next: () => {
            this.displaySuccess(this.translate.instant('successMessages.addEditReceipt'))
          },
          error: err => {

            this.displayError(err);

          }
        })
      );

  }




  get_receipt_list(input:Get_All_Receipt_Data): Observable<Output_Receipt_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post<Output_Receipt_Model_Response>(`${this.apiUrl}/Get_In_Out_Receipt`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }


  get_receipt_by_id(input:Get_By_ID_Receipt_Data): Observable<Output_Receipt_Advanced_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post<Output_Receipt_Advanced_Model_Response>(`${this.apiUrl}/Get_Receipt_By_ID`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }


}
