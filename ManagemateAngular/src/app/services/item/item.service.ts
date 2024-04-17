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
import { Output_Item_Model_Response } from '../../shared/interfaces/API_Output_Models/Item_Models/Output_Item_Model_Response';
import { Get_All_Item_Data } from '../../shared/interfaces/API_Input_Models/Item_Models/Get_All_Item_Data';
import { Output_Item_Type_Model_List } from '../../shared/interfaces/API_Output_Models/Item_Type_Models/Output_Item_Type_Model_List';
import { Output_Item_Trading_Type_Model_List } from '../../shared/interfaces/API_Output_Models/Item_Trading_Type_Models/Output_Item_Trading_Type_Model_List';
import { Output_Item_Counting_Type_Model_List } from '../../shared/interfaces/API_Output_Models/Item_Counting_Type_Models/Output_Item_Counting_Type_Model_List';
import { Add_Item_Data } from '../../shared/interfaces/API_Input_Models/Item_Models/Add_Item_Data';
import { Delete_Item_Data } from '../../shared/interfaces/API_Input_Models/Item_Models/Delete_Item_Data';
import { Get_By_ID_Item_Data } from '../../shared/interfaces/API_Input_Models/Item_Models/Get_By_ID_Item_Data';
import { Output_Item_Advanced_Model_Response } from '../../shared/interfaces/API_Output_Models/Item_Models/Output_Item_Advanced_Model_Response';
import { Edit_Item_Data } from '../../shared/interfaces/API_Input_Models/Item_Models/Edit_Item_Data';
import { Delete_Item_Type_Data } from '../../shared/interfaces/API_Input_Models/Item_Type_Models/Delete_Item_Type_Data';
import { Add_Item_Type_Data } from '../../shared/interfaces/API_Input_Models/Item_Type_Models/Add_Item_Type_Data';
import { Edit_Item_Type_Data } from '../../shared/interfaces/API_Input_Models/Item_Type_Models/Edit_Item_Type_Data';
import { Add_Item_Counting_Type_Data } from '../../shared/interfaces/API_Input_Models/Item_Counting_Type_Models/Add_Item_Counting_Type_Data';
import { Edit_Item_Counting_Type_Data } from '../../shared/interfaces/API_Input_Models/Item_Counting_Type_Models/Edit_Item_Counting_Type_Data';
import { Delete_Item_Counting_Type_Data } from '../../shared/interfaces/API_Input_Models/Item_Counting_Type_Models/Delete_Item_Counting_Type_Data';


@Injectable({
  providedIn: 'root'
})
export class ItemService {
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

      if(this.cookie.check('session_data')) this.cookie.delete('session_data', '/');

    }

  }

  displaySuccess(message:string){
    this.toastrService.success(message, this.translate.instant('successMessages.success'));
  }

// --------------- ITEM --------------------------------------------------

  add_item(input:Add_Item_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/AddItem`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addItem'));
            
          }
        })
      );

  }

  edit_item(input:Edit_Item_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/EditItem`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editItem'));
            
          }
        })
      );

  }

  delete_item(input:Delete_Item_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/DeleteItem`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.deleteItem'));
            
          }
        })
      );

  }

  get_item_by_id(input:Get_By_ID_Item_Data) : Observable<Output_Item_Advanced_Model_Response>{

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);



    return this.http
      .post<Output_Item_Advanced_Model_Response>(`${this.apiUrl}/Get_Item_by_ID`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );

  }

  get_item_list(): Observable<Output_Item_Model_Response> {
    
    const sessionData = this.encryption.decrypt('session_data');

    const get_item_list_input:Get_All_Item_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_item_list_input);



    return this.http
      .post<Output_Item_Model_Response>(`${this.apiUrl}/Get_All_Items`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );
  }



// ------------- ITEM TYPE ----------------------------------------------------------

  add_item_type(input:Add_Item_Type_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/AddItemType`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addItemType'));
            
          }
        })
      );

  }

  edit_item_type(input:Edit_Item_Type_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/EditItemType`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editItemType'));
            
          }
        })
      );

  }

  delete_item_type(input:Delete_Item_Type_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/DeleteItemType`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.deleteItemType'));
            
          }
        })
      );

  }

  get_item_types():Observable<Output_Item_Type_Model_List>{
   
    const sessionData = this.encryption.decrypt('session_data');

    const get_item_list_input:Get_All_Item_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_item_list_input);



    return this.http
      .post<Output_Item_Type_Model_List>(`${this.apiUrl}/GetItemTypes`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );
    
  }



// ------------- ITEM TRADING TYPES ------------------------------------------------------

  get_item_trading_types():Observable<Output_Item_Trading_Type_Model_List>{
   
    const sessionData = this.encryption.decrypt('session_data');

    const get_item_list_input:Get_All_Item_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_item_list_input);



    return this.http
      .post<Output_Item_Trading_Type_Model_List>(`${this.apiUrl}/GetItemTradingTypes`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );
    
  }



// --------------- ITEM COUNTING TYPES -----------------------------------------------------

  add_item_counting_type(input:Add_Item_Counting_Type_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/AddItemCountingType`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.addItemCountingType'));
            
          }
        })
      );

  }

  edit_item_counting_type(input:Edit_Item_Counting_Type_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/EditItemCountingType`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.editItemCountingType'));
            
          }
        })
      );

  }

  delete_item_counting_type(input:Delete_Item_Counting_Type_Data){

    const sessionData = this.encryption.decrypt('session_data');

    input.session = sessionData;

    const jsonData = JSON.stringify(input);


    return this.http
      .post(`${this.apiUrl}/DeleteItemCountingType`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          },
          complete: () => {

            this.displaySuccess(this.translate.instant('successMessages.deleteItemCountingType'));
            
          }
        })
      );

  }

  get_item_counting_types():Observable<Output_Item_Counting_Type_Model_List>{
   
    const sessionData = this.encryption.decrypt('session_data');

    const get_item_list_input:Get_All_Item_Data = {session:sessionData};

    const jsonData = JSON.stringify(get_item_list_input);



    return this.http
      .post<Output_Item_Counting_Type_Model_List>(`${this.apiUrl}/GetItemCountingTypes`, jsonData, this.options)
      .pipe(
        tap({
          error: err => {

            this.displayError(err);

          }
        })
      );
    
  }




}









  // get_item_list(pageId:number, pageSize: number): Observable<Item_List_Response> {
    
  //   const sessionData = this.encryption.decrypt('session_data');

  //   //console.log(sessionData);

  //   const get_item_list_input:Get_Item_List = {session:sessionData, page_ID:pageId, page_Size:pageSize};

  //   const jsonData = JSON.stringify(get_item_list_input);



  //   return this.http
  //     .post<Item_List_Response>(`${this.apiUrl}/Get_Item_Page`, jsonData, this.options)
  //     .pipe(
  //       tap({
  //         error: err => {

  //           const errorCode = err.error.code;
  //           const errorMessage = this.errorHandler.getErrorMessage(errorCode);
  //           this.toastrService.error(errorMessage, 'Login Failed');

  //           if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){

  //             if(this.cookie.check('session_data')) this.cookie.delete('session_data');

  //             this.router.navigateByUrl('/login');

  //           }

  //         }
  //       })
  //     );
  // }