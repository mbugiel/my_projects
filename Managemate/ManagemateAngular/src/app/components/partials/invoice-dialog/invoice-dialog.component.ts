import { Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule} from '@angular/material/button';
import {
  MatDialogRef,
  MatDialogActions,
  MatDialogClose,
  MatDialogTitle,
  MatDialogContent,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { ItemService } from '../../../services/item/item.service';
import { ItemListComponent } from '../../pages/_manage-mate/item-list/item-list.component';
import {FormBuilder, FormGroup, FormsModule, Validators, ReactiveFormsModule} from '@angular/forms';
import { Router } from '@angular/router';
import { OrderService } from '../../../services/order/order.service';
import { Output_Order_Advanced_Model } from '../../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Advanced_Model';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { DateHandlerService } from '../../../services/date-handler/date-handler.service';

interface dialogData{
  order_id:number;
  month:string;
  year:string;
  sale_lease:string;
}
@Component({
  selector: 'app-invoice-dialog',
  standalone: true,
  imports: [ ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './invoice-dialog.component.html',
  styleUrl: './invoice-dialog.component.css'
})
export class InvoiceDialogComponent implements OnInit{

  public order!:Output_Order_Advanced_Model;

  public invoice_form!:FormGroup;

  public is_loaded_order:boolean = false;

  constructor(
    private order_service:OrderService,
    private router: Router,
    public date_Handler:DateHandlerService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<InvoiceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:dialogData)
  {

    this.invoice_form = this.fb.group({

      inputIssueDate:['', Validators.required],
      inputSaleDate:['', Validators.required],
      inputPaymentDate:['', Validators.required],
      inputDiscount:['', Validators.required],
      inputPaymentMethod:['', Validators.required],
      inputComment:['', Validators.required]

    });

  }

  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }



  ngOnInit(): void {
    
    this.order_service.get_order_by_id({ orderId: this.data.order_id }).subscribe({

      next: response => {

        this.order = response.responseData;

        const fc = this.invoice_form.controls;


        const date = this.date_Handler.changeToUtc(new Date()).toISOString();

        fc.inputIssueDate.setValue(date.substring(0, date.indexOf('.')));

        fc.inputSaleDate.setValue(date.substring(0, date.indexOf('.')));

        let payment_date = new Date();
        payment_date.setDate(payment_date.getDate() + this.order.default_payment_date_offset);
        
        const payment_date_to_form = this.date_Handler.changeToUtc(payment_date).toISOString();
        fc.inputPaymentDate.setValue(payment_date_to_form.substring(0, date.indexOf('.')));

        fc.inputDiscount.setValue(this.order.default_discount * 100);

        fc.inputPaymentMethod.setValue(this.order.default_payment_method);

        fc.inputComment.setValue(this.order.comment);
        

        this.is_loaded_order = true;

      },
      error: err => {

        this.is_loaded_order = true;

        this.redirectOnSessionError(err);

      }

    })
    
  }


  closeDialog(){

    this.dialogRef.close();
  
  }

  IssueInvoice(display_or_download:boolean){

    this.dialogRef.close({
      issue_date: this.date_Handler.changeToUtc(new Date(this.invoice_form.controls.inputIssueDate.value)),
      sale_date: this.date_Handler.changeToUtc(new Date(this.invoice_form.controls.inputSaleDate.value)),
      payment_date: this.date_Handler.changeToUtc(new Date(this.invoice_form.controls.inputPaymentDate.value)),
      payment_method: this.invoice_form.controls.inputPaymentMethod.value,
      discount: Number(this.invoice_form.controls.inputDiscount.value) / 100,
      comment: this.invoice_form.controls.inputComment.value,
      display_or_download: display_or_download
    });

  }

}
