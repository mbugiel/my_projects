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
import { MatSelectModule} from '@angular/material/select';
import { Output_Item_Counting_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Counting_Type_Models/Output_Item_Counting_Type_Model';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { Add_Item_Data } from '../../../../shared/interfaces/API_Input_Models/Item_Models/Add_Item_Data';

@Component({
  selector: 'app-item-add',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule],
  templateUrl: './item-add.component.html',
  styleUrl: './item-add.component.css'
})
export class ItemAddComponent implements OnInit {

  public add_Item_Form!:FormGroup;
  public submitted:boolean = false;

  public pl_language:boolean;

  public isLoadedItemTypes:boolean = false;
  public isLoadedTradingTypes:boolean = false;
  public isLoadedCountingTypes:boolean = false;
  public isLoadedAddedItem:boolean = true;

  @ViewChild('inputItemType') inputItemType!: ElementRef;
  @ViewChild('inputTradingType') inputTradingType!: ElementRef;
  @ViewChild('inputCountingType') inputCountingType!: ElementRef;

  public item_types: Array<Output_Item_Type_Model> = new Array<Output_Item_Type_Model>;
  public item_types_filtered!: Array<Output_Item_Type_Model>;

  public trading_types: Array<Output_Item_Trading_Type_Model> = new Array<Output_Item_Trading_Type_Model>;
  public trading_types_filtered!: Array<Output_Item_Trading_Type_Model>;

  public counting_types: Array<Output_Item_Counting_Type_Model> = new Array<Output_Item_Counting_Type_Model>;
  public counting_types_filtered!: Array<Output_Item_Counting_Type_Model>;


  constructor(
    private fb: FormBuilder,
    private item_manager:ItemService,
    private router:Router,
    private translate:TranslateService
  ){
    this.pl_language = translate.currentLang == "pl";


    this.add_Item_Form = this.fb.group({
      inputCatalogNumber: ['', Validators.required],
      inputProductName: ['', Validators.required],
      inputItemType: ['', Validators.required],
      inputWeightKg: ['', Validators.required],
      inputCount: ['', Validators.required],
      inputBlockedCount: ['', Validators.required],
      inputPrice: ['', Validators.required],
      inputTaxPct: ['23', Validators.required],
      inputTradingType: ['', Validators.required],
      inputCountingType: ['', Validators.required],
      inputComment: [''],
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  ngOnInit(){


    this.item_manager.get_item_types().subscribe({

      next: response => {

        this.item_types = response.responseData;

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





    this.item_manager.get_item_trading_types().subscribe({

      next: response => {

        this.trading_types = response.responseData;

        const service_type = this.trading_types.findIndex(type => type.id == 3);

        if(service_type != -1){
          this.trading_types.splice(service_type, 1);
        }

        this.isLoadedTradingTypes = true;

      },
      error: err => {

        this.isLoadedTradingTypes = true;

        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isLoadedTradingTypes = true;

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




  add_item(){

    this.submitted = true;
    if (this.add_Item_Form.invalid) return;

    this.isLoadedAddedItem = false;

    const fc = this.add_Item_Form.controls;

    const CatalogNumber = fc.inputCatalogNumber.value;
    const ProductName = fc.inputProductName.value;
    const ItemType = fc.inputItemType.value;
    const WeightKg = fc.inputWeightKg.value;
    const Count = fc.inputCount.value;
    const BlockedCount = fc.inputBlockedCount.value;
    const Price = fc.inputPrice.value;
    const TaxPct = fc.inputTaxPct.value;
    const TradingType = fc.inputTradingType.value;
    const CountingType = fc.inputCountingType.value;
    const Comment = fc.inputComment.value;

    const add_item_input:Add_Item_Data = {
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
      item_trading_type_id_FK:TradingType,
      item_counting_type_id_FK:Number(this.counting_types.find(function(type){
        return type.counting_type == CountingType;
      })!.id),
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





  filterItemTypes(): void {
    const filterValue = this.inputItemType.nativeElement.value.toLowerCase();
    this.item_types_filtered = this.item_types.filter(o => o.item_type.toLowerCase().includes(filterValue));
  }

  filterTradingTypes(): void {
    
    const filterValue = this.inputTradingType.nativeElement.value.toLowerCase();

    if(this.pl_language){

      this.trading_types_filtered = this.trading_types.filter(o => o.trading_type_pl.toLowerCase().includes(filterValue));

    }else{

      this.trading_types_filtered = this.trading_types.filter(o => o.trading_type_en.toLowerCase().includes(filterValue));

    }

    
  }

  filterCountingTypes(): void {
    const filterValue = this.inputCountingType.nativeElement.value.toLowerCase();
    this.counting_types_filtered = this.counting_types.filter(o => o.counting_type.toLowerCase().includes(filterValue));
  }

}
