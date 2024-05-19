import {Component, OnInit, Inject, ViewChild} from '@angular/core';
import {
  MatDialog,
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { MatButtonModule} from '@angular/material/button';
import { FormBuilder, FormGroup, FormsModule, Validators, ReactiveFormsModule} from '@angular/forms';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule} from '@angular/material/form-field';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { Output_Item_Model } from '../../../shared/interfaces/API_Output_Models/Item_Models/Output_Item_Model';
import { MatTable, MatTableDataSource, MatTableModule } from '@angular/material/table';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { ItemService } from '../../../services/item/item.service';
import { SelectNumberDialogComponent } from '../select-number-dialog/select-number-dialog.component';
import { Add_Item_On_Receipt_Data } from '../../../shared/interfaces/API_Input_Models/Item_On_Receipt_Models/Add_Item_On_Receipt_Data';
import { Router } from '@angular/router';
import { ItemOnReceipt } from '../../pages/_manage-mate/receipt-edit/receipt-edit.component';
import { Output_Item_On_Receipt_Model } from '../../../shared/interfaces/API_Output_Models/Item_On_Receipt_Models/Output_Item_On_Receipt_Model';
import { ReceiptService } from '../../../services/receipt/receipt.service';
import { Get_By_ID_Receipt_Data } from '../../../shared/interfaces/API_Input_Models/Receipt_Models/Get_By_ID_Receipt_Data';
import { Output_Item_On_Receipt_Return_Available_Model } from '../../../shared/interfaces/API_Output_Models/Item_On_Receipt_Models/Item_On_Receipt_Return_Available_Model';

interface dialogData{
  receipt_id:number;
  order_id:number;
  get_item:boolean;
}

@Component({
  selector: 'app-select-item-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule, MatTableModule, MatIconModule, MatSortModule],
  templateUrl: './select-return-item-dialog.component.html',
  styleUrl: './select-return-item-dialog.component.css'
})
export class SelectReturnItemDialogComponent implements OnInit {

  public submitted:boolean = false;
  @ViewChild('items_available_table') my_table!:MatTable<any>

  public columns_To_Display = ['catalog_number', 'product_name', 'count', 'button'];

  public items_to_select:MatTableDataSource<Output_Item_On_Receipt_Return_Available_Model> = new MatTableDataSource<Output_Item_On_Receipt_Return_Available_Model>();
  
  public items_already_on_receipt!:Array<Output_Item_On_Receipt_Model>;

  public selected_items_on_receipt:Array<ItemOnReceipt> = new Array<ItemOnReceipt>;

  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.items_to_select.sort = sort;
  }

  public is_loaded_items:boolean = false;
  public is_loaded_receipt_items:boolean = false;
  public is_loaded_item_add:boolean = true;

  constructor(
    public dialogRef: MatDialogRef<SelectReturnItemDialogComponent>,
    public item_service:ItemService,
    public receipt_service:ReceiptService,
    public dialog: MatDialog,
    private router:Router,
    private translate:TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: dialogData
  ) { }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }



  ngOnInit(): void {
    
    this.item_service.get_available_items_to_return({order_id: this.data.order_id}).subscribe({

      next: response => {

        this.items_to_select = new MatTableDataSource<Output_Item_On_Receipt_Return_Available_Model>(response.responseData);

        this.is_loaded_items = true;

      },
      error: err => {
      
        this.redirectOnSessionError(err);

        this.is_loaded_items = true;

      }

    });


    this.receipt_service.get_receipt_by_id({receipt_id: this.data.receipt_id}).subscribe({

      next: response => {

        this.items_already_on_receipt = response.responseData.items_on_receipt;

        this.is_loaded_receipt_items = true;
      },

      error: err => {
        this.is_loaded_receipt_items = true;
        this.redirectOnSessionError(err);
      }

    });

  }

  ok(): void {
    this.dialogRef.close(this.selected_items_on_receipt);
  }



  open_quantity_dialog(item_id:number): void {


    const item_to_return = this.items_to_select.data.find(item => item.item_id == item_id);


    let display_annotation = true;

    this.items_already_on_receipt.forEach(item => {

      if(item.item_id == item_id){
        display_annotation = false;
      }

    });


    const dialogRef = this.dialog.open(SelectNumberDialogComponent, {
      data: {
        display_annotation: display_annotation,
        receipt_id: this.data.receipt_id,
        item_id: item_id,
        annotation_value: "",
        max_value: item_to_return!.available_count,
        get_item: this.data.get_item
      },
      width: '300px',
      enterAnimationDuration: '100ms',
      exitAnimationDuration: '100ms',
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.success){


        if(result.item_id_check == item_id && result.quantity > 0 && result.annotation != null){

          const input:Add_Item_On_Receipt_Data = {

            receipt_id_FK: this.data.receipt_id,
            item_id_FK: item_id,
            count: parseFloat(result.quantity),
            annotation: result.annotation

          }

          this.is_loaded_item_add = false;

          this.item_service.add_item_on_receipt(input, this.data.get_item).subscribe(
            {
              next: response => {

                const already_on_receipt = this.selected_items_on_receipt.find(item => item.id == response.responseData.id);


                if(already_on_receipt != undefined){

                  already_on_receipt.count += result.quantity;
                  already_on_receipt.summary_weight += result.quantity * already_on_receipt.weight_kg;

                }else{

                  const added_item_new:ItemOnReceipt = {

                    id: response.responseData.id,
                    product_name: item_to_return!.product_name,
                    catalog_number: item_to_return!.catalog_number,
                    weight_kg: item_to_return!.weight,
                    count: result.quantity,
                    counting_type: item_to_return!.counting_type,
                    comment: result.annotation,
                    summary_weight: result.quantity * item_to_return!.weight
  
                  }
  
                  this.selected_items_on_receipt.push(added_item_new);

                }

                item_to_return!.available_count -= result.quantity;
                
                this.my_table.renderRows();

                this.is_loaded_item_add = true;

                //-----------------------------------------------------------
                // UPDATE ITEM ON RECEIPT LIST 
                //-----------------------------------------------------------

                this.is_loaded_receipt_items = false;

                this.receipt_service.get_receipt_by_id({receipt_id: this.data.receipt_id}).subscribe({

                  next: response => {
            
                    this.items_already_on_receipt = response.responseData.items_on_receipt;
            
                    this.is_loaded_receipt_items = true;

                  },
            
                  error: err => {
                    this.is_loaded_receipt_items = true;
                    this.redirectOnSessionError(err);
                  }
            
                });

              },

              error: err => {
      
                this.is_loaded_item_add = true;
                this.redirectOnSessionError(err);
      
              }
            }
          );

        }

        
      }

    });

  }



}
