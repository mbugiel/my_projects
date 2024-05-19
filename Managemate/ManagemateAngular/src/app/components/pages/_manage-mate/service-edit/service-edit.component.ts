import { Component, OnInit,  ElementRef, ViewChild } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Output_Item_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Type_Models/Output_Item_Type_Model';
import { ItemService } from '../../../../services/item/item.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Output_Item_Trading_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Trading_Type_Models/Output_Item_Trading_Type_Model';
import {MatSelectModule} from '@angular/material/select';
import { Output_Item_Counting_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Counting_Type_Models/Output_Item_Counting_Type_Model';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Output_Item_Advanced_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Models/Output_Item_Advanced_Model';
import { Edit_Item_Data } from '../../../../shared/interfaces/API_Input_Models/Item_Models/Edit_Item_Data';
import { Get_By_ID_Item_Data } from '../../../../shared/interfaces/API_Input_Models/Item_Models/Get_By_ID_Item_Data';

@Component({
  selector: 'app-service-edit',
  standalone: true,
  imports: [ ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule],
  templateUrl: './service-edit.component.html',
  styleUrl: './service-edit.component.css'
})
export class ServiceEditComponent {


  
  public edit_service_form!:FormGroup;
  public submitted:boolean = false;

  public pl_language:boolean;

  public isLoadedEditedItem:boolean = true;
  public isLoadedEditingItem:boolean = false;

  public editing_service!:Output_Item_Advanced_Model;


  constructor(
    private url:ActivatedRoute,
    private fb: FormBuilder,
    private item_manager:ItemService,
    private router:Router,
    private translate:TranslateService
  ){
    this.pl_language = translate.currentLang == "pl";


    this.edit_service_form = this.fb.group({
      inputServiceName: ['', Validators.required],
      inputPrice: ['', Validators.required],
      inputTaxPct: ['23', Validators.required],
      inputComment: [''],
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  ngOnInit(){

    this.url.params.subscribe(params => {

      let edit_item_input:Get_By_ID_Item_Data = {

        id_to_get: params['service_id']

      }

      this.item_manager.get_item_by_id(edit_item_input).subscribe({

        next: response => {

          
          this.editing_service = response.responseData;

          this.isLoadedEditingItem = true;

        },
        error: err => {

          this.isLoadedEditingItem = true;
  
          this.redirectOnSessionError(err);
  
        },
        complete: () =>{
  
          this.isLoadedEditingItem = true;

          const fc = this.edit_service_form.controls;
          const e_service = this.editing_service;

          fc.inputServiceName.setValue(e_service.product_name);
          fc.inputPrice.setValue(e_service.price);
          fc.inputTaxPct.setValue(e_service.tax_pct);
          fc.inputComment.setValue(e_service.comment);

  
        }
        
      })

    });

  }




  edit_item(){

    this.submitted = true;
    if (this.edit_service_form.invalid) return;




    this.isLoadedEditedItem = false;

    const fc = this.edit_service_form.controls;

    const ServiceName = fc.inputServiceName.value;
    const Price = fc.inputPrice.value;
    const TaxPct = fc.inputTaxPct.value;
    const Comment = fc.inputComment.value;

    const edit_item_input:Edit_Item_Data = {

      id: this.editing_service.id,
      catalog_number: "-",
      product_name: ServiceName,

      item_type_id_FK: -1,

      weight_kg: 0,
      count: 0,
      blocked_count: 0,
      price: Price,
      tax_pct: TaxPct,

      item_counting_type_id_FK: -1,

      comment:Comment

    }

    this.item_manager.edit_item(edit_item_input).subscribe({

      error: err => {

        this.isLoadedEditedItem = true;

        this.redirectOnSessionError(err);

      },
      complete: () => {

        this.isLoadedEditedItem = true;

        this.router.navigateByUrl('/manage-mate/service-list');

      }

    })



  }



}
