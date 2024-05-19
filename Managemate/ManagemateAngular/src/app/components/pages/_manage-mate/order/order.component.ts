import { Component, OnInit, ElementRef } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Output_Order_Advanced_Model } from '../../../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Advanced_Model';
import { Get_By_ID_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Get_By_ID_Order_Data';
import { OrderService } from '../../../../services/order/order.service';
import { MatIconModule } from '@angular/material/icon';
import { DateHandlerService } from '../../../../services/date-handler/date-handler.service';
import { ReceiptService } from '../../../../services/receipt/receipt.service';
import { Add_Receipt_Data } from '../../../../shared/interfaces/API_Input_Models/Receipt_Models/Add_Receipt_Data';
import { AddReceiptDialogComponent } from '../../../partials/add-receipt-dialog/add-receipt-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [ RouterModule, CommonModule, TranslateModule, MatIconModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent implements OnInit{

  public is_Loaded_Order:boolean = false;

  public is_Loaded_Add_Receipt:boolean = true;

  public selected_order!: Output_Order_Advanced_Model;

  constructor(
    private url:ActivatedRoute,
    private order_service:OrderService,
    private receipt_service:ReceiptService,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService,
    public date_Handler:DateHandlerService
  ){ }



  
  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  ngOnInit(): void {

    this.url.params.subscribe(params => {

      let edit_order_input:Get_By_ID_Order_Data = {

        orderId: params['order_id']

      }

      this.order_service.get_order_by_id(edit_order_input).subscribe({

        next: response => {

          this.selected_order = response.responseData;

          this.is_Loaded_Order = true;

        },

        error: err => {

          this.is_Loaded_Order = true;
  
          this.redirectOnSessionError(err);
  
        }
        
      })

    });


  }



  add_receipt(param_in_out:boolean){

    const dialogRef = this.dialog.open(AddReceiptDialogComponent, {
      data: { in_out: param_in_out },
      width: '400px',
      enterAnimationDuration: '100ms',
      exitAnimationDuration: '100ms',
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.success){


        this.is_Loaded_Add_Receipt = false;

        const input :Add_Receipt_Data = {
          in_out: param_in_out,
          order_id_FK: this.selected_order.id,
          element: result.element,
          transport: result.transport,
          reservation: result.reservation
        }
      
        this.receipt_service.add_receipt(input).subscribe({
        
          next: response => {
          
            this.router.navigateByUrl('/manage-mate/receipt-edit/'+response.responseData.receipt_id);
          
          },
          error: err => {
          
            this.is_Loaded_Add_Receipt = true;
          
            this.redirectOnSessionError(err);
          
          },
          complete: () => {
          
            this.is_Loaded_Add_Receipt = true;

          }
        
        });


      }

    });


  }



}
