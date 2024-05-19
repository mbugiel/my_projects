import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MenuComponent } from '../../../partials/menu/menu.component';
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
import { Output_Construction_Site_Model } from '../../../../shared/interfaces/API_Output_Models/Construction_Site_Models/Output_Construction_Site_Model';
import { ConSiteService } from '../../../../services/con-site/con-site.service';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';

@Component({
  selector: 'app-con-site-list',
  standalone: true,
  imports: [MatSortModule, MatIconModule, MatInputModule, MatFormFieldModule, MenuComponent, MatTooltipModule, CommonModule, RouterModule, TranslateModule, MatTableModule, MatPaginatorModule],
  templateUrl: './con-site-list.component.html',
  styleUrl: './con-site-list.component.css',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class ConSiteListComponent implements OnInit{

  public con_site_list:MatTableDataSource<Output_Construction_Site_Model>;
  public isLoaded:boolean;
  public con_sites_count:number = 0;
  public expandedConSite!: Output_Construction_Site_Model | null;

  public columnsToDisplay = ['construction_site_name', 'address', 'cities_list_id_FK', 'postal_code', /*'comment',*/];
  public columnsActions = ['edit', 'delete'];
  public columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand', ...this.columnsActions];
  
  @ViewChild('pageNum') pageNum_el!: ElementRef;

  @ViewChild('con_site_table') myTable!: MatTable<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.con_site_list.sort = sort;
  }

  @ViewChild('inputConstructionSiteName') inputConstructionSiteName!: ElementRef;
  @ViewChild('inputCity') inputCity!: ElementRef;

  constructor(
    private con_site_service:ConSiteService,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService
  ){
    this.con_site_list = new MatTableDataSource<Output_Construction_Site_Model>();
    this.isLoaded = false;
  }
  

  ngOnInit(){

    this.con_site_service.get_con_site_list().subscribe(
      {
        next: response => {
          // console.log(response)

          this.con_sites_count = response.responseData.length;

          this.con_site_list = new MatTableDataSource<Output_Construction_Site_Model>(response.responseData);
          //console.log(response.responseData)
        },
        error: err => {

          this.isLoaded=true;
          if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

            this.router.navigateByUrl('/login');

          }

        },
        complete:()=>{
          
          this.isLoaded=true;

          this.con_site_list.paginator = this.paginator;
          this.con_site_list.filterPredicate = this.filterOverride();

        }
      }
    );

  }

applyFilter() {

    const ConstructionSiteName = (this.inputConstructionSiteName.nativeElement.value === null || this.inputConstructionSiteName.nativeElement.value === '') ? '' : this.inputConstructionSiteName.nativeElement.value;
    const City = (this.inputCity.nativeElement.value === null || this.inputCity.nativeElement.value === '') ? '' : this.inputCity.nativeElement.value;


    const filterValue = ConstructionSiteName + '$' + City;

    this.con_site_list.filter = filterValue.trim().toLowerCase();

    // const filterValue = (event.target as HTMLInputElement).value;
    // this.con_site_list.filter = filterValue.trim().toLowerCase();
  }

  filterOverride(){

    return (data: Output_Construction_Site_Model, filter: string) => {

      const filtersArray = filter.split('$');

      const ConstructionSiteName = filtersArray[0];
      const City = filtersArray[1];

      const matchFilters = [];

      const columnConstructionSiteName = data.construction_site_name;
      const columnCity = data.cities_list_id_FK;

      const filter_0 = columnConstructionSiteName.toLowerCase().includes(ConstructionSiteName);
      const filter_1 = columnCity.toLowerCase().includes(City);


      // push boolean values into array
      matchFilters.push(filter_0);
      matchFilters.push(filter_1);

      // return true if all values in array is true
      // else return false
      return matchFilters.every(Boolean);
    };

  }


  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, construction_site_name:string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.con_siteTitle'), message: this.translate.instant('deleteDialog.con_siteMessage'), first_param: construction_site_name},
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.deleted){

        this.openPasswordDialog('100ms', '100ms', id, construction_site_name);

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

        this.con_site_service.delete_con_site({id:parseInt(id)}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () => {

              const con_site_index = this.con_site_list.data.findIndex(con_site => con_site.id.toString() == id);
        
              if(con_site_index != -1){
      
                // console.log("znalazło index "+ item_index + " , jest teraz: " + this.item_list.data.length);
                
                this.con_site_list.data.splice(con_site_index, 1);
      
                // console.log("ilosc po usunieciu" + " " + this.item_list.data.length + "czy jest id= "+id+"? "+this.item_list.data.findIndex(item => item.id.toString() === id));
                this.con_site_list.paginator = this.paginator;
                this.con_sites_count--;
                this.myTable.renderRows();
      
      
              }

            }
          }
        );
      }

    });

  }

}