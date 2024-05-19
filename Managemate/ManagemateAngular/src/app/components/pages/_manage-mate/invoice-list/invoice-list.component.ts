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
import { DeleteDialogComponent } from '../../../partials/delete-dialog/delete-dialog.component';
import { Output_Order_Model } from '../../../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Model';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';
import { OrderService } from '../../../../services/order/order.service';
import { MatSelect, MatSelectModule} from '@angular/material/select';
import { DateHandlerService } from '../../../../services/date-handler/date-handler.service';
import { Get_All_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Get_All_Order_Data';
import { Output_Invoice_Available } from '../../../../shared/interfaces/API_Output_Models/Invoice_Models/Output_Invoice_Available';
import { InvoiceService } from '../../../../services/invoice/invoice.service';
import { Get_Invoice_Available_List_Data } from '../../../../shared/interfaces/API_Input_Models/Invoice_Models/Get_Invoice_Available_List_Data';
import { Output_Order_Advanced_Model } from '../../../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Advanced_Model';
import { InvoiceDialogComponent } from '../../../partials/invoice-dialog/invoice-dialog.component';
import { Issue_Invoice_Data } from '../../../../shared/interfaces/API_Input_Models/Invoice_Models/Issue_Invoice_Data';


@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [MatSelectModule, MatSortModule, MatIconModule, MatInputModule, MatFormFieldModule, MenuComponent, MatTooltipModule, CommonModule, RouterModule, TranslateModule, MatTableModule, MatPaginatorModule],
  templateUrl: './invoice-list.component.html',
  styleUrl: './invoice-list.component.css'
})
export class InvoiceListComponent {

  public invoice_available_list:MatTableDataSource<Output_Invoice_Available>;
  public isLoaded:boolean = false;
  public isLoadedOrder:boolean = false;
  public invoices_count:number = 0;

  public isDownloadedInvoice:boolean = true;

  public order!:Output_Order_Advanced_Model;

  public invoice_type:string = "";

  public pl_language:boolean;


  public columnsToDisplay = ['month', 'year', 'sale_invoice', 'issue'];
  
  @ViewChild('pageNum') pageNum_el!: ElementRef;

  @ViewChild('order_table') myTable!: MatTable<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.invoice_available_list.sort = sort;
  }

  @ViewChild('inputOrderName') inputOrderName!: ElementRef;
  @ViewChild('inputClientName') inputClientName!: ElementRef;
  @ViewChild('inputConstructionSiteName') inputConstructionSiteName!: ElementRef;
  // @ViewChild('inputStatus') inputStatus!: MatSelect;

  constructor(
    private invoice_service:InvoiceService,
    private order_service:OrderService,
    private router:Router,
    private url:ActivatedRoute,
    public dialog: MatDialog,
    private translate:TranslateService,
    public dateHandler:DateHandlerService
  ){
    this.invoice_available_list = new MatTableDataSource<Output_Invoice_Available>();
    this.pl_language = translate.currentLang == "pl";
  }



  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }



  ngOnInit(){

    this.url.params.subscribe(params => {

      const input:Get_Invoice_Available_List_Data = {
        order_id: Number(params['order_id'])
      }
  
      this.invoice_service.get_invoice_available_list(input).subscribe(
        {
          next: response => {
  
            this.invoices_count = response.body.responseData.length;
  
            this.invoice_available_list = new MatTableDataSource<Output_Invoice_Available>(response.body.responseData);
  
          },
          error: err => {
  
            this.isLoaded=true;
            
            this.redirectOnSessionError(err);
  
          },
          complete:()=>{
            
            this.isLoaded=true;
  
            this.invoice_available_list.paginator = this.paginator;
            this.invoice_available_list.filterPredicate = this.filterOverride();
  
  
          }
        }
      );


      this.order_service.get_order_by_id({orderId: params['order_id']}).subscribe(
        {
          next: response => {
  
            this.order = response.responseData;

            this.isLoadedOrder = true;
  
          },
          error: err => {
  
            this.isLoadedOrder = true;
            
            this.redirectOnSessionError(err);
  
          }
        }
      );


    });

  }


  change_bool_to_invoice_type(input:boolean){

    if(input){
      return this.translate.instant("invoiceList.invoiceTypeLease");
    }else{
      return this.translate.instant("invoiceList.invoiceTypeSale");
    }

  }



  applyFilter(valueFromSelect:string = this.invoice_type) {

    const invoice_type = (valueFromSelect === null || valueFromSelect === '') ? '' : valueFromSelect;
    this.invoice_type = invoice_type;

    this.invoice_available_list.filter = invoice_type.trim().toLowerCase();

  }

  filterOverride(){

    return (data: Output_Invoice_Available, filter: string) => {

      const columnInvoiceType = data.sale_lease;

      const filter_0 = columnInvoiceType.toString().toLowerCase().includes(filter);

      return filter_0;
    };

  }



  open_invoice_dialog(enterAnimationDuration: string, exitAnimationDuration: string, month:string, year: string, sale_lease:string, sale_lease_boolean:boolean): void {
    const dialogRef = this.dialog.open(InvoiceDialogComponent, {
      data: {month: month, year: year, sale_lease: sale_lease, order_id: this.order.id},
      width: '50%',
      minWidth: '700px',
      disableClose: true,
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.issue_date != undefined && result.sale_date != undefined &&
        result.payment_date != undefined && result.payment_method != undefined &&
        result.discount != undefined && result.comment != undefined &&
        result.display_or_download != undefined
      ){

        this.isDownloadedInvoice = false;

        const input:Issue_Invoice_Data = {
          order_id: this.order.id,
          invoice_type: sale_lease_boolean,
          language_tag: this.translate.currentLang,
          month: Number(month),
          year: Number(year),
          issue_date: result.issue_date,
          sale_date: result.sale_date,
          payment_date: result.payment_date,
          payment_method: result.payment_method,
          discount: result.discount,
          comment: result.comment
        }

        this.invoice_service.issue_invoice(input).subscribe({

          next: response => {

            let blob = new Blob([response.body], {type: 'application/pdf'});

            let url = window.URL.createObjectURL(blob);

            if(result.display_or_download){

              const file_name = response.headers.get('content-disposition').split(';')[1].split('=')[1];

              let a = document.createElement('a');
              a.href = url;
              a.download = file_name;
              a.click();

            }else{

              window.open(url, '_blank');

            }            

            this.isDownloadedInvoice = true;

          },

          error: err => {

            this.isDownloadedInvoice = true;

            this.redirectOnSessionError(err);

          }

        })

      }

    });

  }




}
