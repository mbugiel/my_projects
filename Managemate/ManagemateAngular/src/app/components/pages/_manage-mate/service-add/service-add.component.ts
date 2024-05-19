import { Component,  ElementRef, ViewChild } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Output_Item_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Type_Models/Output_Item_Type_Model';
import { ItemService } from '../../../../services/item/item.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Output_Item_Trading_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Trading_Type_Models/Output_Item_Trading_Type_Model';
import { MatSelectModule} from '@angular/material/select';
import { Output_Item_Counting_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Counting_Type_Models/Output_Item_Counting_Type_Model';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { Add_Item_Data } from '../../../../shared/interfaces/API_Input_Models/Item_Models/Add_Item_Data';

@Component({
  selector: 'app-service-add',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule],
  templateUrl: './service-add.component.html',
  styleUrl: './service-add.component.css'
})
export class ServiceAddComponent {



  public add_service_form!:FormGroup;
  public submitted:boolean = false;

  public pl_language:boolean;

  public isLoadedAddedItem:boolean = true;

  @ViewChild('inputItemType') inputItemType!: ElementRef;
  @ViewChild('inputTradingType') inputTradingType!: ElementRef;
  @ViewChild('inputCountingType') inputCountingType!: ElementRef;


  constructor(
    private fb: FormBuilder,
    private item_manager:ItemService,
    private router:Router,
    private translate:TranslateService
  ){
    this.pl_language = translate.currentLang == "pl";


    this.add_service_form = this.fb.group({
      inputServiceName: ['', Validators.required],
      inputPrice: ['', Validators.required],
      inputTaxPct: ['23', Validators.required],
      inputComment: [''],
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }





  add_service(){

    this.submitted = true;
    if (this.add_service_form.invalid) return;

    this.isLoadedAddedItem = false;

    const fc = this.add_service_form.controls;

    const ServiceName = fc.inputServiceName.value;
    const Price = fc.inputPrice.value;
    const TaxPct = fc.inputTaxPct.value;
    const Comment = fc.inputComment.value;

    const add_item_input:Add_Item_Data = {
      catalog_number:"-",
      product_name: ServiceName,
      item_type_id_FK: -1,
      weight_kg: 0,
      count: 0,
      blocked_count: 0,
      price: Price,
      tax_pct: TaxPct,
      item_trading_type_id_FK: 3,
      item_counting_type_id_FK: -1,
      comment:Comment
    }

    this.item_manager.add_item(add_item_input).subscribe({

      error: () => {

        this.isLoadedAddedItem = true;

      },
      complete: () => {

        this.isLoadedAddedItem = true;

      }

    })

  }


}
