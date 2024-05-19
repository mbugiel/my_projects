import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MenuComponent } from '../../../partials/menu/menu.component';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
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
import { animate, state, style, transition, trigger} from '@angular/animations';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';
import { OrderService } from '../../../../services/order/order.service';
import { MatSelect, MatSelectModule} from '@angular/material/select';
import { DateHandlerService } from '../../../../services/date-handler/date-handler.service';
import { Get_All_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Get_All_Order_Data';


@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [ MatSelectModule, MatSortModule, MatIconModule, MatInputModule, MatFormFieldModule, MenuComponent, MatTooltipModule, CommonModule, RouterModule, TranslateModule, MatTableModule, MatPaginatorModule],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class OrderListComponent implements OnInit{


  public order_list:MatTableDataSource<Output_Order_Model>;
  public isLoaded:boolean;
  public orders_count:number = 0;

  public order_status:string = "";

  public pl_language:boolean;


  public columnsToDisplay = ['order_name', 'client_name', 'construction_site_name', 'creation_date', 'status', 'edit', 'delete'];
  
  @ViewChild('pageNum') pageNum_el!: ElementRef;

  @ViewChild('order_table') myTable!: MatTable<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.order_list.sort = sort;
  }

  @ViewChild('inputOrderName') inputOrderName!: ElementRef;
  @ViewChild('inputClientName') inputClientName!: ElementRef;
  @ViewChild('inputConstructionSiteName') inputConstructionSiteName!: ElementRef;
  // @ViewChild('inputStatus') inputStatus!: MatSelect;

  constructor(
    private order_service:OrderService,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService,
    public dateHandler:DateHandlerService
  ){
    this.order_list = new MatTableDataSource<Output_Order_Model>();
    this.pl_language = translate.currentLang == "pl";
    this.isLoaded = false;
  }



  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }



  ngOnInit(){

    const input:Get_All_Order_Data = {
      all_or_active_only: false
    }

    this.order_service.get_orders(input).subscribe(
      {
        next: response => {
          
          console.log(response)

          this.orders_count = response.responseData.length;

          this.order_list = new MatTableDataSource<Output_Order_Model>(response.responseData);

        },
        error: err => {

          this.isLoaded=true;
          
          this.redirectOnSessionError(err);

        },
        complete:()=>{
          
          this.isLoaded=true;

          this.order_list.paginator = this.paginator;
          this.order_list.filterPredicate = this.filterOverride();


        }
      }
    );

  }



  applyFilter(valueFromSelect:string = this.order_status) {

    const OrderName = (this.inputOrderName.nativeElement.value === null || this.inputOrderName.nativeElement.value === '') ? '' : this.inputOrderName.nativeElement.value;
    const ClientName = (this.inputClientName.nativeElement.value === null || this.inputClientName.nativeElement.value === '') ? '' : this.inputClientName.nativeElement.value;
    const ConstructionSiteName = (this.inputConstructionSiteName.nativeElement.value === null || this.inputConstructionSiteName.nativeElement.value === '') ? '' : this.inputConstructionSiteName.nativeElement.value;
    const Status = (valueFromSelect === null || valueFromSelect === '') ? '' : valueFromSelect;
    this.order_status = Status;


    const filterValue = OrderName + '$' + ClientName + '$' + ConstructionSiteName + '$' + Status;

    this.order_list.filter = filterValue.trim().toLowerCase();

  }

  filterOverride(){

    return (data: Output_Order_Model, filter: string) => {

      const filtersArray = filter.split('$');

      const OrderName = filtersArray[0];
      const ClientName = filtersArray[1];
      const ConSiteName = filtersArray[2];
      const Status = filtersArray[3];

      const matchFilters = [];

      const columnOrderName = data.order_name;
      const columnClientName = data.client_name;
      const columnConSiteName = data.construction_site_name;
      const columnStatus = data.status;

      const filter_0 = columnOrderName.toLowerCase().includes(OrderName);
      const filter_1 = columnClientName.toLowerCase().includes(ClientName);
      const filter_2 = columnConSiteName.toString().toLowerCase().includes(ConSiteName);
      const filter_3 = columnStatus.toString().toLowerCase().includes(Status);

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



  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, order_name:string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.orderTitle') , message: this.translate.instant('deleteDialog.orderMessage') , first_param: order_name},
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.deleted){


        this.openPasswordDialog('100ms', '100ms', id, order_name);


      }

    });

  }




  openPasswordDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, order_name:string): void {
    const dialogRef = this.dialog.open(PasswordDialogComponent, {
      data: {value: order_name},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.success){


        this.order_service.delete_order({id:parseInt(id)}).subscribe(
          {
            error: err => {
    
              this.redirectOnSessionError(err);
    
            },
            complete: () => {

              const item_index = this.order_list.data.findIndex(item => item.id.toString() == id);
        
              if(item_index != -1){
      
                
                this.order_list.data.splice(item_index, 1);

                this.orders_count--;
      
                this.order_list.paginator = this.paginator;
                this.myTable.renderRows();
      
      
              }

            }
          }
        );
        


      }

    });

  }




}
