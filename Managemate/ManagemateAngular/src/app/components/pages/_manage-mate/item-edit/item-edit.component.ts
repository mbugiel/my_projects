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
  selector: 'app-item-edit',
  standalone: true,
  imports: [ ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule],
  templateUrl: './item-edit.component.html',
  styleUrl: './item-edit.component.css'
})
export class ItemEditComponent implements OnInit {



  public edit_Item_Form!:FormGroup;
  public submitted:boolean = false;

  public pl_language:boolean;

  public isLoadedItemTypes:boolean = false;
  public isLoadedCountingTypes:boolean = false;
  public isLoadedEditedItem:boolean = true;
  public isLoadedEditingItem:boolean = false;

  @ViewChild('inputItemType') inputItemType!: ElementRef;
  @ViewChild('inputTradingType') inputTradingType!: ElementRef;
  @ViewChild('inputCountingType') inputCountingType!: ElementRef;

  @ViewChild('inputCatalogNumber') inputCatalogNumber!: ElementRef;
  @ViewChild('inputProductName') inputProductName!: ElementRef;
  @ViewChild('inputWeightKg') inputWeightKg!: ElementRef;
  @ViewChild('inputCount') inputCount!: ElementRef;
  @ViewChild('inputBlockedCount') inputBlockedCount!: ElementRef;
  @ViewChild('inputPrice') inputPrice!: ElementRef;
  @ViewChild('inputTaxPct') inputTaxPct!: ElementRef;
  @ViewChild('inputComment') inputComment!: ElementRef;

  public item_types: Array<Output_Item_Type_Model> = new Array<Output_Item_Type_Model>;
  public item_types_filtered!: Array<Output_Item_Type_Model>;

  public counting_types: Array<Output_Item_Counting_Type_Model> = new Array<Output_Item_Counting_Type_Model>;
  public counting_types_filtered!: Array<Output_Item_Counting_Type_Model>;

  public editing_item!:Output_Item_Advanced_Model;


  constructor(
    private url:ActivatedRoute,
    private fb: FormBuilder,
    private item_manager:ItemService,
    private router:Router,
    private translate:TranslateService
  ){
    this.pl_language = translate.currentLang == "pl";


    this.edit_Item_Form = this.fb.group({
      inputCatalogNumber: ['', Validators.required],
      inputProductName: ['', Validators.required],
      inputItemType: ['', Validators.required],
      inputWeightKg: ['', Validators.required],
      inputCount: ['', Validators.required],
      inputBlockedCount: ['', Validators.required],
      inputPrice: ['', Validators.required],
      inputTaxPct: ['23', Validators.required],
      inputCountingType: ['', Validators.required],
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

        id_to_get: params['item_id']

      }

      this.item_manager.get_item_by_id(edit_item_input).subscribe({

        next: response => {

          //console.log(response.responseData+"\n");
          //console.log(this.editing_item+"\n");


          this.editing_item = response.responseData;

          //console.log(this.editing_item+"\n");

          this.isLoadedEditingItem = true;

        },
        error: err => {

          this.isLoadedEditingItem = true;
  
          this.redirectOnSessionError(err);
  
        },
        complete: () =>{
  
          this.isLoadedEditingItem = true;

          const fc = this.edit_Item_Form.controls;
          const e_item = this.editing_item;

          fc.inputCatalogNumber.setValue(e_item.catalog_number);
          fc.inputProductName.setValue(e_item.product_name);
          fc.inputItemType.setValue(e_item.item_type);
          fc.inputWeightKg.setValue(e_item.weight_kg);
          fc.inputCount.setValue(e_item.count);
          fc.inputBlockedCount.setValue(e_item.blocked_count);
          fc.inputPrice.setValue(e_item.price);
          fc.inputTaxPct.setValue(e_item.tax_pct);
          fc.inputCountingType.setValue(e_item.item_counting_type_id_FK.counting_type);
          fc.inputComment.setValue(e_item.comment);

  
        }
        
      })

    });




    this.item_manager.get_item_types().subscribe({

      next: response => {

        this.item_types = response.responseData;

        this.isLoadedItemTypes = true;

        // console.log(this.item_types);

      },
      error: err => {

        this.isLoadedItemTypes = true;

        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isLoadedItemTypes = true;

      }

    });





    this.item_manager.get_item_counting_types().subscribe({

      next: response => {

        this.counting_types = response.responseData;

        this.isLoadedCountingTypes = true;

        // console.log(this.counting_types);

      },
      error: err => {

        this.isLoadedCountingTypes = true;

        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isLoadedCountingTypes = true;

      }

    });






  }




  edit_item(){

    this.submitted = true;
    if (this.edit_Item_Form.invalid) return;

    this.url.params.subscribe(params => {


      this.isLoadedEditedItem = false;

      const fc = this.edit_Item_Form.controls;

      const CatalogNumber = fc.inputCatalogNumber.value;
      const ProductName = fc.inputProductName.value;
      const ItemType = fc.inputItemType.value;
      const WeightKg = fc.inputWeightKg.value;
      const Count = fc.inputCount.value;
      const BlockedCount = fc.inputBlockedCount.value;
      const Price = fc.inputPrice.value;
      const TaxPct = fc.inputTaxPct.value;
      const CountingType = fc.inputCountingType.value;
      const Comment = fc.inputComment.value;

      const edit_item_input:Edit_Item_Data = {

        id: params['item_id'],
        catalog_number:CatalogNumber,
        product_name:ProductName,

        item_type_id_FK: Number(this.item_types.find(function(item){
            return item.item_type == ItemType;
          })!.id),

        weight_kg:WeightKg,
        count:Count,
        blocked_count:BlockedCount,
        price:Price,
        tax_pct:TaxPct,

        item_counting_type_id_FK:Number(this.counting_types.find(function(type){
          return type.counting_type == CountingType;
        })!.id),

        comment:Comment

      }

      this.item_manager.edit_item(edit_item_input).subscribe({

        error: () => {

          this.isLoadedEditedItem = true;

        },
        complete: () => {

          this.isLoadedEditedItem = true;

        }

      })

    });


  }





  filterItemTypes(): void {
    const filterValue = this.inputItemType.nativeElement.value.toLowerCase();
    this.item_types_filtered = this.item_types.filter(o => o.item_type.toLowerCase().includes(filterValue));
  }

  filterCountingTypes(): void {
    const filterValue = this.inputCountingType.nativeElement.value.toLowerCase();
    this.counting_types_filtered = this.counting_types.filter(o => o.counting_type.toLowerCase().includes(filterValue));
  }



}
