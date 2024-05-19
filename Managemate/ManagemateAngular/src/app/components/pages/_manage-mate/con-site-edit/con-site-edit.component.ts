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
import { ConSiteService } from '../../../../services/con-site/con-site.service';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { EditDialogComponent } from '../../../partials/edit-dialog/edit-dialog.component';
import { Get_By_ID_Construction_Site_Data } from '../../../../shared/interfaces/API_Input_Models/Construction_Site_Models/Get_By_ID_Construction_Site_Data';
import { Output_Construction_Site_Model } from '../../../../shared/interfaces/API_Output_Models/Construction_Site_Models/Output_Construction_Site_Model';
import { Edit_Construction_Site_Data } from '../../../../shared/interfaces/API_Input_Models/Construction_Site_Models/Edit_Construction_Site_Data';

@Component({
  selector: 'app-con-site-edit',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule, MatIconModule],
  templateUrl: './con-site-edit.component.html',
  styleUrl: './con-site-edit.component.css'
})
export class ConSiteEditComponent implements OnInit {


  public edit_ConSite_Form!:FormGroup;
  public submitted:boolean = false;
  public emptyError: boolean = false;

  public pl_language:boolean;

  public isLoadedCitiesList:boolean = false;
  public isLoadedEditingConSite:boolean = true;

  @ViewChild('inputCity') inputCity!: ElementRef;

  @ViewChild('inputConSiteName') inputName!: ElementRef;
  @ViewChild('inputAddress') inputAddress!: ElementRef;
  @ViewChild('inputPostalCode') inputPostalCode!: ElementRef;
  @ViewChild('inputComment') inputComment!: ElementRef;

  public cities_list: Array<Output_Cities_List_Model> = new Array<Output_Cities_List_Model>;
  public cities_list_filtered!: Array<Output_Cities_List_Model>;

  public editing_con_site!:Output_Construction_Site_Model;

  constructor(
    private url:ActivatedRoute,
    private fb: FormBuilder,
    private con_site_service:ConSiteService,
    private router:Router,
    private translate:TranslateService,
    private dialog:MatDialog
  ){
    this.pl_language = translate.currentLang == "pl";


    this.edit_ConSite_Form = this.fb.group({
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

    this.url.params.subscribe(params => {

      let edit_con_site_input:Get_By_ID_Construction_Site_Data = {

        id_to_get: params['con-site_id']

      }

      this.con_site_service.get_con_site_by_id(edit_con_site_input).subscribe({

        next: response => {

          //console.log(response.responseData+"\n");
          //console.log(this.editing_client+"\n");


          this.editing_con_site = response.responseData;

          //console.log(this.editing_client+"\n");

          this.isLoadedEditingConSite = true;

        },
        error: err => {

          this.isLoadedEditingConSite = true;
  
          this.redirectOnSessionError(err);
  
        },
        complete: () =>{
  
          this.isLoadedEditingConSite = true;

          const fc = this.edit_ConSite_Form.controls;
          const e_client = this.editing_con_site;
      
          fc.inputConSiteName.setValue(e_client.construction_site_name);
          fc.inputAddress.setValue(e_client.address);
          fc.inputCity.setValue(e_client.cities_list_id_FK);
          fc.inputPostalCode.setValue(e_client.postal_code);
          fc.inputComment.setValue(e_client.comment);

  
        }
        
      })

    });

    this.edit_ConSite_Form.valueChanges.subscribe(() => {
      this.checkFormValidity();
    });
    this.getAllCities();


  }

  checkFormValidity(): void {
    this.emptyError = this.edit_ConSite_Form.get("inputCity")?.value !== null && Object.keys(this.edit_ConSite_Form.controls).every(controlName => {
      return !this.edit_ConSite_Form.get(controlName)?.hasError('required') || this.edit_ConSite_Form.get(controlName)?.value !== '';
    });
  }

  edit_con_site(){

    this.submitted = true;
    this.checkFormValidity();


    if (this.edit_ConSite_Form.invalid) return;

    this.url.params.subscribe(params => {

    this.isLoadedEditingConSite = false;

    const fc = this.edit_ConSite_Form.controls;

    const ConSiteName = fc.inputConSiteName.value;
    const Address = fc.inputAddress.value;
    const City = fc.inputCity.value;
    const PostalCode = fc.inputPostalCode.value;
    const Comment = fc.inputComment.value;

    const edit_con_site_input:Edit_Construction_Site_Data = {
      id: params['con-site_id'],
      construction_site_name:ConSiteName,
      address:Address,
      cities_list_id_fk:Number(this.cities_list.find(function(type){
        return type.city == City;
      })!.id),
      postal_code:PostalCode,

      comment:Comment
    }

    this.con_site_service.edit_con_site(edit_con_site_input).subscribe({

      error: () => {

        this.isLoadedEditingConSite = true;

      },
      complete: () => {

        this.isLoadedEditingConSite = true;

      }

    })

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