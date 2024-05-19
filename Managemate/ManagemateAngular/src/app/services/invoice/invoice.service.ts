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
import { DateHandlerService } from '../date-handler/date-handler.service';
import { Output_Invoice_Available_Response } from '../../shared/interfaces/API_Output_Models/Invoice_Models/Output_Invoice_Available_Response';
import { Get_Invoice_Available_List_Data } from '../../shared/interfaces/API_Input_Models/Invoice_Models/Get_Invoice_Available_List_Data';
import { Issue_Invoice_Data } from '../../shared/interfaces/API_Input_Models/Invoice_Models/Issue_Invoice_Data';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  private apiUrl = environment.apiUrl;

  private options = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
    observe: 'response' as 'body',
    
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


  //----------------------- GET LIST OF AVAILABLE INVOICES ---------------------


  get_invoice_available_list(input:Get_Invoice_Available_List_Data):Observable<any>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post<any>(`${this.apiUrl}/Get_Invoice_Available_List`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }


  issue_invoice(input: Issue_Invoice_Data): Observable<any> {
    const sessionData = this.encryption.decrypt('session_data');
    input.session = sessionData;
    const jsonData = JSON.stringify(input);
  
    const new_options: {
      headers?: HttpHeaders,
      observe?: any,
      responseType?: any,
      withCredentials?: boolean
    } = { 
      headers: this.options.headers,
      observe: 'response',
      responseType: 'arraybuffer',
      withCredentials: this.options.withCredentials
    };
  
    return this.http
      .post(`${this.apiUrl}/Invoice_Issuer`, jsonData, new_options)
      .pipe(
        tap({
          error: err => {
            this.displayError(err);
          }
        })
      );
  }

}
