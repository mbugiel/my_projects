import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MenuComponent } from '../../../partials/menu/menu.component';
import { ItemService } from '../../../../services/item/item.service';
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
import { Output_Item_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Models/Output_Item_Model';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';

@Component({
  selector: 'app-item-list',
  standalone: true,
  imports: [ MatSortModule, MatIconModule, MatInputModule, MatFormFieldModule, MenuComponent, MatTooltipModule, CommonModule, RouterModule, TranslateModule, MatTableModule, MatPaginatorModule],
  templateUrl: './item-list.component.html',
  styleUrl: './item-list.component.css',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class ItemListComponent implements OnInit{

  public item_list:MatTableDataSource<Output_Item_Model>;
  public isLoaded:boolean;
  public items_count:number = 0;

  public pl_language:boolean;

  public expandedItem!: Output_Item_Model | null;

  public columnsToDisplay = ['catalog_number', 'product_name', 'weight_kg', 'count', 'blocked_count', 'expand', 'edit', 'delete'];
  
  @ViewChild('pageNum') pageNum_el!: ElementRef;

  @ViewChild('item_table') myTable!: MatTable<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.item_list.sort = sort;
  }

  @ViewChild('inputCatalogNumber') inputCatalogNumber!: ElementRef;
  @ViewChild('inputName') inputName!: ElementRef;
  @ViewChild('inputWeight') inputWeight!: ElementRef;
  @ViewChild('inputCount') inputCount!: ElementRef;
  @ViewChild('inputBlockedCount') inputBlockedCount!: ElementRef;

  constructor(
    private item_service:ItemService,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService
  ){
    this.item_list = new MatTableDataSource<Output_Item_Model>();
    this.pl_language = translate.currentLang == "pl";
    this.isLoaded = false;
  }
  

  ngOnInit(){

    this.item_service.get_item_list().subscribe(
      {
        next: response => {
          // console.log(response)

          this.items_count = response.responseData.length;

          this.item_list = new MatTableDataSource<Output_Item_Model>(response.responseData);
        },
        error: err => {

          this.isLoaded=true;
          if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

            this.router.navigateByUrl('/login');

          }

        },
        complete:()=>{
          
          this.isLoaded=true;

          this.item_list.paginator = this.paginator;
          this.item_list.filterPredicate = this.filterOverride();


        }
      }
    );

  }

  applyFilter() {

    const CatalogNumber = (this.inputCatalogNumber.nativeElement.value === null || this.inputCatalogNumber.nativeElement.value === '') ? '' : this.inputCatalogNumber.nativeElement.value;
    const Name = (this.inputName.nativeElement.value === null || this.inputName.nativeElement.value === '') ? '' : this.inputName.nativeElement.value;
    const Weight = (this.inputWeight.nativeElement.value === null || this.inputWeight.nativeElement.value === '') ? '' : this.inputWeight.nativeElement.value;
    const Count = (this.inputCount.nativeElement.value === null || this.inputCount.nativeElement.value === '') ? '' : this.inputCount.nativeElement.value;
    const BlockedCount = (this.inputBlockedCount.nativeElement.value === null || this.inputBlockedCount.nativeElement.value === '') ? '' : this.inputBlockedCount.nativeElement.value;



    const filterValue = CatalogNumber + '$' + Name + '$' + Weight + '$' + Count + '$' + BlockedCount;

    this.item_list.filter = filterValue.trim().toLowerCase();

    // const filterValue = (event.target as HTMLInputElement).value;
    // this.item_list.filter = filterValue.trim().toLowerCase();
  }

  filterOverride(){

    return (data: Output_Item_Model, filter: string) => {

      const filtersArray = filter.split('$');

      const CatalogNumber = filtersArray[0];
      const Name = filtersArray[1];
      const Weight = filtersArray[2];
      const Count = filtersArray[3];
      const BlockedCount = filtersArray[4];

      const matchFilters = [];

      const columnCatalogNumber = data.catalog_number;
      const columnName = data.product_name;
      const columnWeight = data.weight_kg;
      const columnCount = data.count;
      const columnBlockedCount = data.blocked_count;

      const filter_0 = columnCatalogNumber.toLowerCase().includes(CatalogNumber);
      const filter_1 = columnName.toLowerCase().includes(Name);
      const filter_2 = columnWeight.toString().toLowerCase().includes(Weight);
      const filter_3 = columnCount.toString().toLowerCase().includes(Count);
      const filter_4 = columnBlockedCount.toString().toLowerCase().includes(BlockedCount);

      // push boolean values into array
      matchFilters.push(filter_0);
      matchFilters.push(filter_1);
      matchFilters.push(filter_2);
      matchFilters.push(filter_3);
      matchFilters.push(filter_4);

      // return true if all values in array is true
      // else return false
      return matchFilters.every(Boolean);
    };

  }



  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, catalogNumber:string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.itemTitle') , message: this.translate.instant('deleteDialog.itemMessage') , first_param: catalogNumber},
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.deleted){


        this.openPasswordDialog('100ms', '100ms', id, catalogNumber);


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


        this.item_service.delete_item({id:parseInt(id)}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () => {

              const item_index = this.item_list.data.findIndex(item => item.id.toString() == id);
        
              if(item_index != -1){
      
                // console.log("znalazło index "+ item_index + " , jest teraz: " + this.item_list.data.length);
                
                this.item_list.data.splice(item_index, 1);
      
                // console.log("ilosc po usunieciu" + " " + this.item_list.data.length + "czy jest id= "+id+"? "+this.item_list.data.findIndex(item => item.id.toString() === id));
                this.item_list.paginator = this.paginator;
                this.myTable.renderRows();
      
      
              }

            }
          }
        );

        // console.log("kliknales usun id = "+id);


      }

    });

  }

  

}
