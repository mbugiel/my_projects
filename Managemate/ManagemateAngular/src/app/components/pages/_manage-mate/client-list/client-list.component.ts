import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MenuComponent } from '../../../partials/menu/menu.component';
import { ClientService } from '../../../../services/client/client.service';
import { Output_Client_Model } from '../../../../shared/interfaces/API_Output_Models/Client_Models/Output_Client_Model';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import { MatTable, MatTableDataSource, MatTableModule} from '@angular/material/table';
import {TooltipPosition, MatTooltipModule} from '@angular/material/tooltip';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../../partials/delete-dialog/delete-dialog.component';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [ MatSortModule, MatIconModule, MatInputModule, MatFormFieldModule, MenuComponent, MatTooltipModule, CommonModule, RouterModule, TranslateModule, MatTableModule, MatPaginatorModule],
  templateUrl: './client-list.component.html',
  styleUrl: './client-list.component.css',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class ClientListComponent implements OnInit{

  public client_list:MatTableDataSource<Output_Client_Model>;
  public isLoaded:boolean;
  public clients_count:number = 0;
  public expandedClient!: Output_Client_Model | null;

  // public columnsToDisplay = ['surname', 'name', 'company_name', 'phone_number', /*'comment',*/ 'expand','edit', 'delete'];
  // public columnsToDisplayWithExpand = [...this.columnsToDisplay];

  public columnsToDisplay = ['surname', 'name', 'company_name', 'phone_number', /*'comment',*/];
  public columnsActions = ['edit', 'delete'];
  public columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand', ...this.columnsActions];
  
  @ViewChild('pageNum') pageNum_el!: ElementRef;

  @ViewChild('client_table') myTable!: MatTable<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.client_list.sort = sort;
  }

  @ViewChild('inputSurname') inputSurname!: ElementRef;
  @ViewChild('inputName') inputName!: ElementRef;
  @ViewChild('inputCompanyName') inputCompanyName!: ElementRef;
  @ViewChild('inputPhoneNumber') inputPhoneNumber!: ElementRef;

  constructor(
    private client_service:ClientService,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService
  ){
    this.client_list = new MatTableDataSource<Output_Client_Model>();
    this.isLoaded = false;
  }
  

  ngOnInit(){

    this.client_service.get_client_list().subscribe(
      {
        next: response => {
          // console.log(response)

          this.clients_count = response.responseData.length;

          this.client_list = new MatTableDataSource<Output_Client_Model>(response.responseData);
        },
        error: err => {

          this.isLoaded=true;
          if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){

            this.router.navigateByUrl('/login');

          }

        },
        complete:()=>{
          
          this.isLoaded=true;

          this.client_list.paginator = this.paginator;
          this.client_list.filterPredicate = this.filterOverride();

        }
      }
    );

  }

  applyFilter() {

    const Surname = (this.inputSurname.nativeElement.value === null || this.inputSurname.nativeElement.value === '') ? '' : this.inputSurname.nativeElement.value;
    const Name = (this.inputName.nativeElement.value === null || this.inputName.nativeElement.value === '') ? '' : this.inputName.nativeElement.value;
    const CompanyName = (this.inputCompanyName.nativeElement.value === null || this.inputCompanyName.nativeElement.value === '') ? '' : this.inputCompanyName.nativeElement.value;
    const PhoneNumber = (this.inputPhoneNumber.nativeElement.value === null || this.inputPhoneNumber.nativeElement.value === '') ? '' : this.inputPhoneNumber.nativeElement.value;


    const filterValue = Surname + '$' + Name + '$' + CompanyName + '$' + PhoneNumber;

    this.client_list.filter = filterValue.trim().toLowerCase();

    // const filterValue = (event.target as HTMLInputElement).value;
    // this.client_list.filter = filterValue.trim().toLowerCase();
  }

  filterOverride(){

    return (data: Output_Client_Model, filter: string) => {

      const filtersArray = filter.split('$');

      const Surname = filtersArray[0];
      const Name = filtersArray[1];
      const CompanyName = filtersArray[2];
      const PhoneNumber = filtersArray[3];

      const matchFilters = [];

      const columnSurname = data.surname;
      const columnName = data.name;
      const columnCompanyName = data.company_name;
      const columnPhoneNumber = data.phone_number;

      const filter_0 = columnSurname.toLowerCase().includes(Surname);
      const filter_1 = columnName.toLowerCase().includes(Name);
      const filter_2 = columnCompanyName.toString().toLowerCase().includes(CompanyName);
      const filter_3 = columnPhoneNumber.toString().toLowerCase().includes(PhoneNumber);

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


  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, name_surname:string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.clientTitle'), message: this.translate.instant('deleteDialog.clientMessage'), first_param: name_surname},
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.deleted){


        this.openPasswordDialog('100ms', '100ms', id, name_surname);
        

      }

    });

  }

  openPasswordDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, catalogNumber:string): void {
    const dialogRef = this.dialog.open(PasswordDialogComponent, {
      data: {value: catalogNumber},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.success){

        this.client_service.delete_client({id:parseInt(id)}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () => {

              const client_index = this.client_list.data.findIndex(client => client.id.toString() == id);
        
              if(client_index != -1){
      
                // console.log("znalazło index "+ item_index + " , jest teraz: " + this.item_list.data.length);
                
                this.client_list.data.splice(client_index, 1);
      
                // console.log("ilosc po usunieciu" + " " + this.item_list.data.length + "czy jest id= "+id+"? "+this.item_list.data.findIndex(item => item.id.toString() === id));
                this.client_list.paginator = this.paginator;
                this.myTable.renderRows();
      
      
              }

            }
          }
        );

      }

    });

  }

}