import { Component, OnInit,  ElementRef, ViewChild } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {MatSelectModule} from '@angular/material/select';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { Output_Cities_List_Model } from '../../../../shared/interfaces/API_Output_Models/Cities_List_Models/Output_Cities_List_Model';
import { ClientService } from '../../../../services/client/client.service';
import { Edit_Client_Data } from '../../../../shared/interfaces/API_Input_Models/Client_Models/Edit_Client_Data';
import { Output_Client_Model } from '../../../../shared/interfaces/API_Output_Models/Client_Models/Output_Client_Model';
import { Get_By_ID_Client_Data } from '../../../../shared/interfaces/API_Input_Models/Client_Models/Get_By_ID_Client_Data';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { EditDialogComponent } from '../../../partials/edit-dialog/edit-dialog.component';

@Component({
  selector: 'app-client-edit',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule, MatIconModule],
  templateUrl: './client-edit.component.html',
  styleUrl: './client-edit.component.css'
})
export class ClientEditComponent implements OnInit {


  public edit_Client_Form!:FormGroup;
  public submitted:boolean = false;
  public emptyError: boolean = false;

  public pl_language:boolean;

  public isLoadedCitiesList:boolean = false;
  public isLoadedEditingClient:boolean = true;

  @ViewChild('inputCity') inputCity!: ElementRef;

  @ViewChild('inputSurname') inputSurname!: ElementRef;
  @ViewChild('inputName') inputName!: ElementRef;
  @ViewChild('inputCompanyName') inputCompanyName!: ElementRef;
  @ViewChild('inputNIP') inputNIP!: ElementRef;
  @ViewChild('inputPhoneNumber') inputPhoneNumber!: ElementRef;
  @ViewChild('inputEmail') inputEmail!: ElementRef;
  @ViewChild('inputAddress') inputAddress!: ElementRef;
  @ViewChild('inputPostalCode') inputPostalCode!: ElementRef;
  @ViewChild('inputComment') inputComment!: ElementRef;
  public cities_list: Array<Output_Cities_List_Model> = new Array<Output_Cities_List_Model>;
  public cities_list_filtered!: Array<Output_Cities_List_Model>;

  public editing_client!:Output_Client_Model;

  constructor(
    private url:ActivatedRoute,
    private fb: FormBuilder,
    private client_service:ClientService,
    private router:Router,
    private translate:TranslateService,
    private dialog:MatDialog
  ){
    this.pl_language = translate.currentLang == "pl";


    this.edit_Client_Form = this.fb.group({
      inputSurname: ['', Validators.required],
      inputName: ['', Validators.required],
      inputCompanyName: ['', Validators.required],
      inputNIP: ['', Validators.required],
      inputPhoneNumber: ['', [Validators.required, Validators.pattern("\\+?[0-9]+(\\s[0-9]+)*")]],
      inputEmail: ['', Validators.required],
      inputAddress: ['', Validators.required],
      inputCity: ['', Validators.required],
      inputPostalCode: ['', Validators.required],
      inputComment: [''],
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }

  getAllCities(){
    this.client_service.get_cities_list().subscribe({

      next: response => {

        this.cities_list = response.responseData;

        this.isLoadedCitiesList = true;

        // console.log(this.item_types);

      },
      error: err => {

        this.isLoadedCitiesList = true;

        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isLoadedCitiesList = true;

      }

    });
  }


  ngOnInit(){


    this.url.params.subscribe(params => {

      let edit_client_input:Get_By_ID_Client_Data = {

        id_to_get: params['client_id']

      }

      this.client_service.get_client_by_id(edit_client_input).subscribe({

        next: response => {

          //console.log(response.responseData+"\n");
          //console.log(this.editing_client+"\n");


          this.editing_client = response.responseData;

          //console.log(this.editing_client+"\n");

          this.isLoadedEditingClient = true;

        },
        error: err => {

          this.isLoadedEditingClient = true;
  
          this.redirectOnSessionError(err);
  
        },
        complete: () =>{
  
          this.isLoadedEditingClient = true;

          const fc = this.edit_Client_Form.controls;
          const e_client = this.editing_client;

          fc.inputSurname.setValue(e_client.surname);
          fc.inputName.setValue(e_client.name);
          fc.inputCompanyName.setValue(e_client.company_name);
          fc.inputNIP.setValue(e_client.nip);
          fc.inputPhoneNumber.setValue(e_client.phone_number);
          fc.inputEmail.setValue(e_client.email);
          fc.inputAddress.setValue(e_client.address);
          fc.inputCity.setValue(e_client.city_id_FK);
          fc.inputPostalCode.setValue(e_client.postal_code);
          fc.inputComment.setValue(e_client.comment);

  
        }
        
      })

    });

    this.edit_Client_Form.valueChanges.subscribe(() => {
      this.checkFormValidity();
    });
    this.getAllCities();


  }

  checkFormValidity(): void {
    this.emptyError = Object.keys(this.edit_Client_Form.controls).every(controlName => {
      return !this.edit_Client_Form.get(controlName)?.hasError('required') || this.edit_Client_Form.get(controlName)?.value !== '';
    });
  }

  edit_client(){

    this.submitted = true;
    this.checkFormValidity();

    if (this.edit_Client_Form.invalid) return;

    this.url.params.subscribe(params => {

      this.isLoadedEditingClient = false;

      const fc = this.edit_Client_Form.controls;

      const Surname = fc.inputSurname.value;
      const Name = fc.inputName.value;
      const CompanyName = fc.inputCompanyName.value;
      const NIP = fc.inputNIP.value;
      const PhoneNumber = fc.inputPhoneNumber.value;
      const Email = fc.inputEmail.value;
      const Address = fc.inputAddress.value;
      const City = fc.inputCity.value;
      const PostalCode = fc.inputPostalCode.value;
      const Comment = fc.inputComment.value;

      const edit_client_input:Edit_Client_Data = {
        id: params['client_id'],
        surname:Surname,
        name:Name,
        company_name:CompanyName,
        nip:NIP,
        phone_number:PhoneNumber,
        email:Email,
        address:Address,
        city_id_fk:Number(this.cities_list.find(function(type){
          return type.city == City;
        })!.id),
        postal_code:PostalCode,

        comment:Comment
      }

      this.client_service.edit_client(edit_client_input).subscribe({

        error: () => {

          this.isLoadedEditingClient = true;

        },
        complete: () => {

          this.isLoadedEditingClient = true;

        }

      })
    });

  }


  openAddDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      data: {title: this.translate.instant('addDialog.cityTitle'), label:  this.translate.instant('addDialog.cityLabel')},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.isLoadedCitiesList = true;

      if(result.success){

        this.client_service.add_city({city: result.value}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () =>{


              this.getAllCities();

            }
          }
        );


      }


    });
  }


  filterCities(): void {
    const filterValue = this.inputCity.nativeElement.value.toLowerCase();
    this.cities_list_filtered = this.cities_list.filter(o => o.city.toLowerCase().includes(filterValue));
  }


}