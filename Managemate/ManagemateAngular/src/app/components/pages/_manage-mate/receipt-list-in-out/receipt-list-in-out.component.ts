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
import { animate, state, style, transition, trigger} from '@angular/animations';
import { MatSelectModule} from '@angular/material/select';
import { Output_Receipt_Model } from '../../../../shared/interfaces/API_Output_Models/Receipt_Models/Output_Receipt_Model';
import { ReceiptService } from '../../../../services/receipt/receipt.service';
import { Get_All_Receipt_Data } from '../../../../shared/interfaces/API_Input_Models/Receipt_Models/Get_All_Receipt_Data';
import { OrderService } from '../../../../services/order/order.service';
import { Get_By_ID_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Get_By_ID_Order_Data';
import { Add_Receipt_Data } from '../../../../shared/interfaces/API_Input_Models/Receipt_Models/Add_Receipt_Data';
import { AddReceiptDialogComponent } from '../../../partials/add-receipt-dialog/add-receipt-dialog.component';

@Component({
  selector: 'app-receipt-list-in-out',
  standalone: true,
  imports: [MatSelectModule, MatSortModule, MatIconModule, MatInputModule, MatFormFieldModule, MenuComponent, MatTooltipModule, CommonModule, RouterModule, TranslateModule, MatTableModule, MatPaginatorModule],
  templateUrl: './receipt-list-in-out.component.html',
  styleUrl: './receipt-list-in-out.component.css',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class ReceiptListInOutComponent implements OnInit{

  public receipt_list:MatTableDataSource<Output_Receipt_Model>;
  public expandedReceipt!:Output_Receipt_Model | null;

  public is_Loaded_Receipts:boolean = false;
  public is_Loaded_Order_Name:boolean = false;
  public is_Loaded_Add_Receipt:boolean = true;
  
  public receipts_count:number = 0;

  public order_id!:string;
  public in_out!:string;
  
  public order_name:string = '';

  public columnsToDisplay = ['element', 'transport', 'summary_weight', 'date', 'expand'];
  
  @ViewChild('pageNum') pageNum_el!: ElementRef;

  @ViewChild('order_table') myTable!: MatTable<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.receipt_list.sort = sort;
  }

  @ViewChild('inputElement') inputElement!: ElementRef;
  @ViewChild('inputTransport') inputTransport!: ElementRef;
  @ViewChild('inputSummaryWeight') inputSummaryWeight!: ElementRef;
  @ViewChild('inputComment') inputComment!: ElementRef;

  constructor(
    private url:ActivatedRoute,
    private receipt_service:ReceiptService,
    private order_service:OrderService,
    private router:Router,
    public dialog: MatDialog
  ){
    this.receipt_list = new MatTableDataSource<Output_Receipt_Model>();
  }



  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }




  ngOnInit(){

    this.url.params.subscribe(params => {

      this.in_out = params['in_out'];
      this.order_id = params['order_id'];

      const input:Get_All_Receipt_Data = {
        order_id: params['order_id'],
        in_out: params['in_out'] == '0' ? false : true
      }
  
      this.receipt_service.get_receipt_list(input).subscribe(
        {
          next: response => {
  
            this.receipts_count = response.responseData.length;
  
            this.receipt_list = new MatTableDataSource<Output_Receipt_Model>(response.responseData);
  
          },
          error: err => {
  
            this.is_Loaded_Receipts=true;
            console.log(err)
            this.redirectOnSessionError(err);
  
          },
          complete:()=>{
            
            this.is_Loaded_Receipts=true;
  
            this.receipt_list.paginator = this.paginator;
            this.receipt_list.filterPredicate = this.filterOverride();
  
  
          }
        }
      );

      const order_input:Get_By_ID_Order_Data = {

        orderId: params['order_id']

      }

      this.order_service.get_order_by_id(order_input).subscribe(
        {
          next: response => {

            this.order_name = response.responseData.order_name;

            this.is_Loaded_Order_Name = true;

          },
          error: err => {

            this.is_Loaded_Order_Name = true;

            this.redirectOnSessionError(err);

          }
        }
      )

    });

  }


  formatDate(input:Date): string {

    const date = new Date(input);
    const dsep = "-";
    const tsep = ":";

    let date_array:string[] = [
      date.getUTCDate().toString(),
      (date.getUTCMonth()+1).toString(),
      date.getUTCFullYear().toString(),
      date.getUTCHours().toString(),
      date.getUTCMinutes().toString()
    ];

    for(let i = 0; i < 5; i++){

      if(date_array[i].length < 2) date_array[i] = "0"+date_array[i];

    }    

    return date_array[0] + dsep + date_array[1] + dsep + date_array[2] + ", " + date_array[3] + tsep + date_array[4];

  }



  applyFilter() {

    const Element = (this.inputElement.nativeElement.value === null || this.inputElement.nativeElement.value === '') ? '' : this.inputElement.nativeElement.value;
    const Transport = (this.inputTransport.nativeElement.value === null || this.inputTransport.nativeElement.value === '') ? '' : this.inputTransport.nativeElement.value;
    const SummaryWeight = (this.inputSummaryWeight.nativeElement.value === null || this.inputSummaryWeight.nativeElement.value === '') ? '' : this.inputSummaryWeight.nativeElement.value;
    const Comment = (this.inputComment.nativeElement.value === null || this.inputComment.nativeElement.value === '') ? '' : this.inputComment.nativeElement.value;

    const filterValue = Element + '$' + Transport + '$' + SummaryWeight + '$' + Comment;

    this.receipt_list.filter = filterValue.trim().toLowerCase();

  }

  filterOverride(){

    return (data: Output_Receipt_Model, filter: string) => {

      const filtersArray = filter.split('$');

      const Element = filtersArray[0];
      const Transport = filtersArray[1];
      const SummaryWeight = filtersArray[2];
      const Comment = filtersArray[3];

      const matchFilters = [];

      const columnElement = data.element;
      const columnTransport = data.transport;
      const columnSummaryWeight = data.summary_weight;
      const columnComment = data.comment;

      const filter_0 = columnElement.toLowerCase().includes(Element);
      const filter_1 = columnTransport.toLowerCase().includes(Transport);
      const filter_2 = columnSummaryWeight.toString().toLowerCase().includes(SummaryWeight);
      const filter_3 = columnComment.toLowerCase().includes(Comment);

      // push boolean values into array
      matchFilters.push(filter_0);
      matchFilters.push(filter_1);
      matchFilters.push(filter_2);
      matchFilters.push(filter_3);

      // return true if all values in array is true
      // else return false
      return matchFilters.every(Boolean);
    };

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
          order_id_FK: parseInt(this.order_id),
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

