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
import { ConSiteService } from '../../../../services/con-site/con-site.service';
import { Add_Construction_Site_Data } from '../../../../shared/interfaces/API_Input_Models/Construction_Site_Models/Add_Construction_Site_Data';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { EditDialogComponent } from '../../../partials/edit-dialog/edit-dialog.component';

@Component({
  selector: 'app-con-site-add',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule, MatIconModule],
  templateUrl: './con-site-add.component.html',
  styleUrl: './con-site-add.component.css'
})
export class ConSiteAddComponent implements OnInit {


  public add_ConSite_Form!:FormGroup;
  public submitted:boolean = false;
  public emptyError: boolean = false;

  public pl_language:boolean;

  public isLoadedCitiesList:boolean = false;
  public isLoadedAddedConSite:boolean = true;

  @ViewChild('inputCity') inputCity!: ElementRef;

  @ViewChild('inputConSiteName') inputName!: ElementRef;
  @ViewChild('inputAddress') inputAddress!: ElementRef;
  @ViewChild('inputPostalCode') inputPostalCode!: ElementRef;
  @ViewChild('inputComment') inputComment!: ElementRef;

  public cities_list: Array<Output_Cities_List_Model> = new Array<Output_Cities_List_Model>;
  public cities_list_filtered!: Array<Output_Cities_List_Model>;

  constructor(
    private fb: FormBuilder,
    private con_site_service:ConSiteService,
    private router:Router,
    private translate:TranslateService,
    private dialog:MatDialog
  ){
    this.pl_language = translate.currentLang == "pl";


    this.add_ConSite_Form = this.fb.group({
      inputConSiteName: ['', Validators.required],
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
    this.con_site_service.get_cities_list().subscribe({

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

    this.add_ConSite_Form.valueChanges.subscribe(() => {
      this.checkFormValidity();
    });
    this.getAllCities();


  }

  checkFormValidity(): void {
    this.emptyError = this.add_ConSite_Form.get("inputCity")?.value !== null && Object.keys(this.add_ConSite_Form.controls).every(controlName => {
      return !this.add_ConSite_Form.get(controlName)?.hasError('required') || this.add_ConSite_Form.get(controlName)?.value !== '';
    });
  }

  add_con_site(){

    this.submitted = true;
    this.checkFormValidity();


    if (this.add_ConSite_Form.invalid) return;

    this.isLoadedAddedConSite = false;

    const fc = this.add_ConSite_Form.controls;

    const ConSiteName = fc.inputConSiteName.value;
    const Address = fc.inputAddress.value;
    const City = fc.inputCity.value;
    const PostalCode = fc.inputPostalCode.value;
    const Comment = fc.inputComment.value;

    const add_con_site_input:Add_Construction_Site_Data = {
      construction_site_name:ConSiteName,
      address:Address,
      cities_list_id_fk:Number(this.cities_list.find(function(type){
        return type.city == City;
      })!.id),
      postal_code:PostalCode,

      comment:Comment
    }

    this.con_site_service.add_con_site(add_con_site_input).subscribe({

      error: () => {

        this.isLoadedAddedConSite = true;

      },
      complete: () => {

        this.isLoadedAddedConSite = true;

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

        this.con_site_service.add_city({city: result.value}).subscribe(
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