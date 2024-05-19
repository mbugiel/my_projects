import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MenuComponent } from '../../../partials/menu/menu.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import { MatTable, MatTableDataSource, MatTableModule} from '@angular/material/table';
import { TooltipPosition, MatTooltipModule} from '@angular/material/tooltip';
import { MatSort, Sort, MatSortModule} from '@angular/material/sort';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule} from '@angular/material/form-field';
import { MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { ReceiptService } from '../../../../services/receipt/receipt.service';
import { OrderService } from '../../../../services/order/order.service';
import { Get_By_ID_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Get_By_ID_Order_Data';
import { ItemService } from '../../../../services/item/item.service';
import { Output_Item_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Models/Output_Item_Model';
import { Output_Order_Advanced_Model } from '../../../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Advanced_Model';
import { DateHandlerService } from '../../../../services/date-handler/date-handler.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule  } from '@angular/forms';
import { DeleteDialogComponent } from '../../../partials/delete-dialog/delete-dialog.component';
import { SelectItemDialogComponent } from '../../../partials/select-item-dialog/select-item-dialog.component';
import { Get_By_ID_Receipt_Data } from '../../../../shared/interfaces/API_Input_Models/Receipt_Models/Get_By_ID_Receipt_Data';
import { Output_Receipt_Advanced_Model } from '../../../../shared/interfaces/API_Output_Models/Receipt_Models/Output_Receipt_Advanced_Model';
import { Edit_Receipt_Data } from '../../../../shared/interfaces/API_Input_Models/Receipt_Models/Edit_Receipt_Data';
import { BlobOptions } from 'buffer';
import { Delete_Item_On_Receipt_Data } from '../../../../shared/interfaces/API_Input_Models/Item_On_Receipt_Models/Delete_Item_On_Receipt_Data';
import { SelectReturnItemDialogComponent } from '../../../partials/select-return-item-dialog/select-return-item-dialog.component';


export interface ItemOnReceipt{

  id:number;
  catalog_number:string;
  product_name:string;
  counting_type:string;
  count:number;
  weight_kg:number;
  summary_weight:number;
  comment?:string;

}


@Component({
  selector: 'app-receipt-edit',
  standalone: true,
  imports: [TranslateModule, RouterModule, CommonModule, MatFormFieldModule, MatInputModule, MatTableModule, MatIconModule, ReactiveFormsModule],
  templateUrl: './receipt-edit.component.html',
  styleUrl: './receipt-edit.component.css'
})
export class ReceiptEditComponent implements OnInit{

  public editing_receipt!:Output_Receipt_Advanced_Model;

  public order!:Output_Order_Advanced_Model;

  public items_available!:Array<Output_Item_Model>
  public items_count:number = 0;
  public summary_weight:number = 0;


  @ViewChild('items_on_receipt_table') myTable!: MatTable<any>;
  public items_on_receipt:MatTableDataSource<ItemOnReceipt> = new MatTableDataSource<ItemOnReceipt>();
  public items_on_receipt_count:number = 0;
  public columns_To_Display = ['row_count', 'catalog_number', 'product_name', 'counting_type', 'count', 'weight_kg', 'summary_weight', 'comment', 'delete'];

  //isLoaded checkers
  public is_Loaded_Order: boolean = false;
  public is_Loaded_Items: boolean = false;
  public is_Loaded_Receipt: boolean = false;
  public is_Loaded_Edited: boolean = true;


  public receipt_form!:FormGroup;
  public submitted:boolean = false;


  constructor(
    private url:ActivatedRoute,
    private fb: FormBuilder,
    private receipt_service:ReceiptService,
    private order_service:OrderService,
    private item_service:ItemService,
    private router:Router,
    public dialog: MatDialog,
    public date_Handler:DateHandlerService,
    private translate:TranslateService
  ){

    this.receipt_form = this.fb.group({
      inputElement: ['', Validators.required],
      inputTransport: ['', Validators.required],
      inputCreationDate: ['', Validators.required],
      inputComment: ['']
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }



  ngOnInit(){

    this.url.params.subscribe(params => {
  
      this.item_service.get_item_list({get_item:true}).subscribe(
        {
          next: response => {
  
            this.items_count = response.responseData.length;
  
            this.items_available = response.responseData;

            this.is_Loaded_Items = true;

          },
          error: err => {
  
            this.is_Loaded_Items=true;

            this.redirectOnSessionError(err);
  
          }
        }
      );


      const receipt_id = params['receipt_id'];

      const receipt_input:Get_By_ID_Receipt_Data = {
        receipt_id: receipt_id
      }

      this.receipt_service.get_receipt_by_id(receipt_input).subscribe({

        next: response => {


          this.editing_receipt = response.responseData;

          const fc = this.receipt_form.controls;

          fc.inputElement.setValue(this.editing_receipt.element);
          fc.inputTransport.setValue(this.editing_receipt.transport);
          fc.inputComment.setValue(this.editing_receipt.comment);
          
          const date = new Date(this.editing_receipt.date).toISOString();

          fc.inputCreationDate.setValue(date.substring(0, date.indexOf('.')));


          this.editing_receipt.items_on_receipt.forEach(item => {

            this.items_on_receipt.data.push({
              id: item.id,
              catalog_number: item.catalog_number,
              product_name: item.product_name,
              counting_type: item.counting_type,
              weight_kg: item.weight,
              count: item.count,
              summary_weight: item.summary_weight,
              comment: item.annotation
  
            });

            this.summary_weight += item.summary_weight;
  
            this.items_on_receipt_count++;

          });


          this.is_Loaded_Receipt = true;

          
          // ############ get order data #############################

          const order_input:Get_By_ID_Order_Data = {

            orderId: this.editing_receipt.order_id
    
          }


          this.order_service.get_order_by_id(order_input).subscribe(
            {
              next: response => {
    
                this.order = response.responseData;
    
                this.is_Loaded_Order = true;
    
              },
              error: err => {
    
                this.is_Loaded_Order = true;
    
                this.redirectOnSessionError(err);
    
              }
            }
          );

          //##############################################################



        },

        error: err => {
          
          this.is_Loaded_Receipt = true;
          this.is_Loaded_Order = true;

          this.redirectOnSessionError(err);

          if(err.error.code == "19"){
            this.router.navigateByUrl('/manage-mate/order-list');
          }

        }

      });

    });

  }


  open_delete_dialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, item_name: string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.itemOnReceiptTitle') , message: this.translate.instant('deleteDialog.itemOnReceiptMessage') , first_param: item_name},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.deleted){


        const item_index = this.items_on_receipt.data.findIndex(item => item.id.toString() == id);

        const item_summary_weight = this.items_on_receipt.data.find(item => item.id.toString() == id)!.summary_weight;
        
        if(item_index != -1){

          const input:Delete_Item_On_Receipt_Data = {
            item_on_receipt_id: parseInt(id)
          }

          this.is_Loaded_Items = false;

          this.item_service.delete_item_on_receipt(input).subscribe({

            next: () => {

              this.items_on_receipt.data.splice(item_index, 1);

              this.items_on_receipt_count--;

              this.summary_weight -= item_summary_weight;

              this.is_Loaded_Items = true;

            },
            error: err => {

              this.redirectOnSessionError(err);
              this.is_Loaded_Items = true;

            }

          });


        }


      }

    });

  }



  open_select_item_dialog(enterAnimationDuration: string, exitAnimationDuration: string, get_item:boolean){

    // ------------------- RELEASE RECEIPT -------------------------------------------------

    if(this.editing_receipt.in_out){

      const dialogRef = this.dialog.open(SelectItemDialogComponent, {
        data: { receipt_id: this.editing_receipt.id, get_item: get_item},
        width: '70%',
        height: '80%',
        enterAnimationDuration,
        exitAnimationDuration,
        disableClose: true
      });
  
      dialogRef.afterClosed().subscribe((result:Array<Output_Item_Model>) => {
        
  
        if(result.length > 0){
  
  
          result.forEach(item => {
  
            const already_on_receipt = this.items_on_receipt.data.find(receipt_item => receipt_item.id == item.id);
  
  
            if(already_on_receipt != undefined){
  
              already_on_receipt.count += item.count;
              already_on_receipt.summary_weight += item.count * item.weight_kg;
  
            }else{
  
              this.items_on_receipt.data.push({
                id: item.id,
                catalog_number: item.catalog_number,
                product_name: item.product_name,
                counting_type: item.counting_type,
                weight_kg: item.weight_kg,
                count: item.count,
                summary_weight: item.count * item.weight_kg,
                comment: item.comment
    
              });
    
              this.items_on_receipt_count++;
  
            }
            
            
  
            this.summary_weight += item.count * item.weight_kg;
  
          });
  
          this.myTable.renderRows();
  
        }
  
      });

      // ---------------- RETURN RECEIPT -------------------------------------------------

    }else{


      const dialogRef = this.dialog.open(SelectReturnItemDialogComponent, {
        data: { receipt_id: this.editing_receipt.id, order_id: this.editing_receipt.order_id, get_item: get_item},
        width: '70%',
        height: '80%',
        enterAnimationDuration,
        exitAnimationDuration,
        disableClose: true
      });
  
      dialogRef.afterClosed().subscribe((result:Array<Output_Item_Model>) => {
        
  
        if(result.length > 0){
  
  
          result.forEach(item => {
  
            const already_on_receipt = this.items_on_receipt.data.find(receipt_item => receipt_item.id == item.id);
  
  
            if(already_on_receipt != undefined){
  
              already_on_receipt.count += item.count;
              already_on_receipt.summary_weight += item.count * item.weight_kg;
  
            }else{
  
              this.items_on_receipt.data.push({
                id: item.id,
                catalog_number: item.catalog_number,
                product_name: item.product_name,
                counting_type: item.counting_type,
                weight_kg: item.weight_kg,
                count: item.count,
                summary_weight: item.count * item.weight_kg,
                comment: item.comment
    
              });
    
              this.items_on_receipt_count++;
  
            }
            
            
  
            this.summary_weight += item.count * item.weight_kg;
  
          });
  
          this.myTable.renderRows();
  
        }
  
      });


    }

    

  }



  edit_receipt(){

    this.submitted = true;

    if(this.receipt_form.invalid || this.items_on_receipt.data.length == 0) return;

    this.is_Loaded_Edited = false;

    const fc = this.receipt_form.controls;

    const element = fc.inputElement.value;
    const transport = fc.inputTransport.value;
    const comment = fc.inputComment.value;
    const creation_date = fc.inputCreationDate.value;


    const input:Edit_Receipt_Data = {

      receipt_id: this.editing_receipt.id,
      in_out: this.editing_receipt.in_out,
      order_id_FK: this.editing_receipt.order_id,
      element: element,
      transport: transport,
      comment: comment,
      date: this.date_Handler.changeToUtc(new Date(creation_date))

    }


    this.receipt_service.edit_receipt(input).subscribe({

      next: () => {

        this.is_Loaded_Edited = true;

        this.router.navigateByUrl('/manage-mate/receipt-list-in-out/'+this.order.id+'/'+Number(this.editing_receipt.in_out));

      },
      error: err => {

        this.redirectOnSessionError(err);

        this.is_Loaded_Edited = true;

      }

    })


  }



}
