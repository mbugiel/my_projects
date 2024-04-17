import { Component, OnInit,  ElementRef, ViewChild } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import {MatSelectModule} from '@angular/material/select';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { Output_Cities_List_Model } from '../../../../shared/interfaces/API_Output_Models/Cities_List_Models/Output_Cities_List_Model';
import { ClientService } from '../../../../services/client/client.service';
import { Add_Client_Data } from '../../../../shared/interfaces/API_Input_Models/Client_Models/Add_Client_Data';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { EditDialogComponent } from '../../../partials/edit-dialog/edit-dialog.component';

@Component({
  selector: 'app-client-add',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule, MatIconModule],
  templateUrl: './client-add.component.html',
  styleUrl: './client-add.component.css'
})
export class ClientAddComponent implements OnInit {


  public add_Client_Form!:FormGroup;
  public submitted:boolean = false;
  public emptyError: boolean = false;

  public pl_language:boolean;

  public isLoadedCitiesList:boolean = false;
  public isLoadedAddedClient:boolean = true;

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

  constructor(
    private fb: FormBuilder,
    private client_service:ClientService,
    private router:Router,
    private translate:TranslateService,
    private dialog:MatDialog
  ){
    this.pl_language = translate.currentLang == "pl";


    this.add_Client_Form = this.fb.group({
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

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){

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

    this.add_Client_Form.valueChanges.subscribe(() => {
      this.checkFormValidity();
    });
    this.getAllCities();


  }

  checkFormValidity(): void {
    this.emptyError = Object.keys(this.add_Client_Form.controls).every(controlName => {
      return !this.add_Client_Form.get(controlName)?.hasError('required') || this.add_Client_Form.get(controlName)?.value !== '';
    });
  }

  add_client(){

    this.submitted = true;
    this.checkFormValidity();


    if (this.add_Client_Form.invalid) return;

    this.isLoadedAddedClient = false;

    const fc = this.add_Client_Form.controls;

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

    const add_client_input:Add_Client_Data = {
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

    this.client_service.add_client(add_client_input).subscribe({

      error: () => {

        this.isLoadedAddedClient = true;

      },
      complete: () => {

        this.isLoadedAddedClient = true;

      }

    })

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
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){
    
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