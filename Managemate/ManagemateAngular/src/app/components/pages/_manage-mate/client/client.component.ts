import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { Get_By_ID_Client_Data } from '../../../../shared/interfaces/API_Input_Models/Client_Models/Get_By_ID_Client_Data';
import { ClientService } from '../../../../services/client/client.service';
import { Output_Client_Model } from '../../../../shared/interfaces/API_Output_Models/Client_Models/Output_Client_Model';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import { MatTable, MatTableDataSource, MatTableModule} from '@angular/material/table';
import {TooltipPosition, MatTooltipModule} from '@angular/material/tooltip';
import {MatSort, Sort, MatSortModule} from '@angular/material/sort';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { Output_Authorized_Worker_Model } from '../../../../shared/interfaces/API_Output_Models/Authorized_Worker_Models/Output_Authorized_Worker_Model';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';
import { DeleteDialogComponent } from '../../../partials/delete-dialog/delete-dialog.component';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { AddAuthorizedWorkerDialogComponent } from '../../../partials/add-authorized-worker-dialog/add-authorized-worker-dialog.component';
import { Get_All_Authorized_Worker_Data } from '../../../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Get_All_Authorized_Worker_Data';
import { Add_Authorized_Worker_Data } from '../../../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Add_Authorized_Worker_Data';
import { EditAuthorizedWorkerDialogComponent } from '../../../partials/edit-authorized-worker-dialog/edit-authorized-worker-dialog.component';
import { Edit_Authorized_Worker_Data } from '../../../../shared/interfaces/API_Input_Models/Authorized_Worker_Models/Edit_Authorized_Worker_Data';

@Component({
  selector: 'app-client',
  standalone: true,
  imports: [MatSortModule, MatIconModule, MatInputModule, MatFormFieldModule, MatTooltipModule, CommonModule, RouterModule, TranslateModule, MatTableModule, MatPaginatorModule],
  templateUrl: './client.component.html',
  styleUrl: './client.component.css',
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class ClientComponent implements OnInit{

  public is_Loaded_Client:boolean = false;
  public isLoaded:boolean;

  public selected_client!: Output_Client_Model;

  private client_id!: number;

  public auth_workers_count:number = 0;
  public expandedAuthWorker!: Output_Authorized_Worker_Model | null;
  public auth_worker_list:MatTableDataSource<Output_Authorized_Worker_Model>;

  public columnsToDisplay = ['surname', 'name', 'contact', 'collection'];
  public columnsActions = ['edit', 'delete'];
  public columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand', ...this.columnsActions];

  @ViewChild('pageNum') pageNum_el!: ElementRef;

  @ViewChild('auth_worker_table') myTable!: MatTable<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  @ViewChild(MatSort) set matSort(sort: MatSort) {
    this.auth_worker_list.sort = sort;
  }

  constructor(
    private url:ActivatedRoute,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService,
    private client_service:ClientService
  ){
    this.auth_worker_list = new MatTableDataSource<Output_Authorized_Worker_Model>();
    this.isLoaded = false;
   }



  
  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  ngOnInit(): void {

    this.url.params.subscribe(params => {

      this.client_id = params['client_id'];

      let edit_client_input:Get_By_ID_Client_Data = {

        id_to_get: this.client_id

      }

      this.client_service.get_client_by_id(edit_client_input).subscribe({

        next: response => {

          this.selected_client = response.responseData;

          this.is_Loaded_Client = true;

        },

        error: err => {

          this.is_Loaded_Client = true;
  
          this.redirectOnSessionError(err);
  
        }
        
      })

    });

    this.get_all_auth_workers();

  }

  get_all_auth_workers(){

    let get_auth_worker_input:Get_All_Authorized_Worker_Data = {

      client_id: this.client_id

    }

    this.client_service.get_auth_worker_list(get_auth_worker_input).subscribe(
      {
        next: response => {
          // console.log(response)

          this.auth_workers_count = response.responseData.length;

          this.auth_worker_list = new MatTableDataSource<Output_Authorized_Worker_Model>(response.responseData);
        },
        error: err => {

          this.isLoaded=true;
          if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10){

            this.router.navigateByUrl('/login');

          }

        },
        complete:()=>{
          
          this.isLoaded=true;

          this.auth_worker_list.paginator = this.paginator;
          // this.auth_worker_list.filterPredicate = this.filterOverride();

        }
      }
    );
  }
  
  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, name_surname:string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.authWorkerTitle'), message: this.translate.instant('deleteDialog.authWorkerMessage'), first_param: name_surname},
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

        this.client_service.delete_auth_worker({id:parseInt(id)}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () => {

              const auth_worker_index = this.auth_worker_list.data.findIndex(auth_worker => auth_worker.id.toString() == id);
        
              if(auth_worker_index != -1){
      
                // console.log("znalazło index "+ item_index + " , jest teraz: " + this.item_list.data.length);
                
                this.auth_worker_list.data.splice(auth_worker_index, 1);
      
                // console.log("ilosc po usunieciu" + " " + this.item_list.data.length + "czy jest id= "+id+"? "+this.item_list.data.findIndex(item => item.id.toString() === id));
                this.auth_worker_list.paginator = this.paginator;
                this.auth_workers_count--;
                this.myTable.renderRows();
      
      
              }

            }
          }
        );

      }

    });

  }

  openAddDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    const dialogRef = this.dialog.open(AddAuthorizedWorkerDialogComponent, {
      data: null,
      width: '600px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {

      if(result.success){

        let add_auth_worker:Add_Authorized_Worker_Data = {

          client_id_FK: this.client_id,
          name: result.name,
          surname: result.surname,
          phone_number: result.phone_number,
          email: result.email,
          contact: result.contact,
          collection: result.collection,
          comment: result.comment
  
        }

        this.client_service.add_auth_worker(add_auth_worker).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () =>{

              this.auth_worker_list.paginator = this.paginator;
              this.auth_workers_count++;
              this.get_all_auth_workers();

            }
          }
        );


      }


    });
  }

  openEditDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:number): void {
    const dialogRef = this.dialog.open(EditAuthorizedWorkerDialogComponent, {
      data: {auth_worker_id:id},
      width: '600px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {

      if(result.success){

        let edit_auth_worker:Edit_Authorized_Worker_Data = {

          id:id,
          client_id_FK: this.client_id,
          name: result.name,
          surname: result.surname,
          phone_number: result.phone_number,
          email: result.email,
          contact: result.contact,
          collection: result.collection,
          comment: result.comment
  
        }

        this.client_service.edit_auth_worker(edit_auth_worker).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () =>{

              this.auth_worker_list.paginator = this.paginator;
              this.get_all_auth_workers();

            }
          }
        );


      }


    });
  }




}