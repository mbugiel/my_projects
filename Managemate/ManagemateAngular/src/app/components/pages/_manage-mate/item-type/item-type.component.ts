import { Component, OnInit,  ElementRef, ViewChild } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { Output_Item_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Type_Models/Output_Item_Type_Model';
import { ItemService } from '../../../../services/item/item.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import {MatSelectModule} from '@angular/material/select';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatTable, MatTableModule} from '@angular/material/table';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../../partials/delete-dialog/delete-dialog.component';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import { Add_Item_Type_Data } from '../../../../shared/interfaces/API_Input_Models/Item_Type_Models/Add_Item_Type_Data';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';
import { EditTypeDialogComponent } from '../../../partials/edit-type-dialog/edit-type-dialog.component';






@Component({
  selector: 'app-item-type',
  standalone: true,
  imports: [ MatIconModule, MatPaginatorModule, ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule, MatTableModule],
  templateUrl: './item-type.component.html',
  styleUrl: './item-type.component.css'
})
export class ItemTypeComponent implements OnInit {

  public item_type_Form!:FormGroup;
  public submitted:boolean = false;
  public isLoaded:boolean = true;
  public item_types_count:number = 0;
  public item_types:MatTableDataSource<Output_Item_Type_Model>;
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('item_table') myTable!: MatTable<any>;
  public columnsToDisplay = ['group_name', 'group_rate', 'edit', 'delete'];

  constructor(
    private fb: FormBuilder,
    private item_service:ItemService,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService
  ){

    this.item_type_Form = this.fb.group({
      inputItemType: ['', Validators.required],
      inputItemTypeRate: ['', Validators.required],
    });

    this.item_types = new MatTableDataSource<Output_Item_Type_Model>();

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  add_item_type(){

    this.isLoaded = false;

    this.submitted = true;
    if (this.item_type_Form.invalid){
      this.isLoaded = true;
      return;
    }

    const fc = this.item_type_Form.controls;

    const item_type_name = fc.inputItemType.value;
    const item_type_rate = Number(fc.inputItemTypeRate.value) / 100;

    const add_item_type:Add_Item_Type_Data = {
      item_type: item_type_name,
      rate: item_type_rate
    }

    this.item_service.add_item_type(add_item_type).subscribe({

      error: err => {

        this.isLoaded = true;

        this.redirectOnSessionError(err);

      },
      complete: () => {

        this.isLoaded = true;

        this.get_item_types();
      }

    })

  }


  get_item_types(){

    this.isLoaded = false;

    this.item_service.get_item_types().subscribe({

      next: response => {

        this.item_types = new MatTableDataSource<Output_Item_Type_Model>(response.responseData);
        this.item_types_count = this.item_types.data.length;

      },
      error: err => {

        this.isLoaded = true;

        this.redirectOnSessionError(err);

      },
      complete: () => {

        this.item_types.paginator = this.paginator;
        this.isLoaded = true;


      }

    })

  }


  ngOnInit(): void {

    this.get_item_types();


  }



  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, groupName:string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.itemTypeTitle') , message: this.translate.instant('deleteDialog.itemTypeMessage') , first_param: groupName},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.deleted){

        this.openPasswordDialog(enterAnimationDuration, exitAnimationDuration, id, groupName);
        

      }

    });

  }


  openEditDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, group_Name:string, rate_Value:number): void {
    const dialogRef = this.dialog.open(EditTypeDialogComponent, {
      data: {title: this.translate.instant('editDialog.itemTypeTitle'), label:  this.translate.instant('itemType.groupName'), label_2: this.translate.instant('itemType.groupRatePct'), value: group_Name, value_2: rate_Value},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {

      if(result.success){


        this.isLoaded = false;

        this.item_service.edit_item_type({item_type_id:parseInt(id), item_type: result.value, rate: result.value_2 / 100}).subscribe(
          {
            error: err => {

              this.isLoaded = true;

              this.redirectOnSessionError(err);
    
            },
            complete: () =>{


              const it = this.item_types.data.find(function(item_type){
                return item_type.id.toString() == id;
              });


              if(it){

                it.item_type = result.value;
                it.rate = result.value_2 / 100;

              }

              this.isLoaded = true;

            }
          }
        );


      }


    });
  }





  openPasswordDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, groupName:string): void {
    const dialogRef = this.dialog.open(PasswordDialogComponent, {
      data: {value: groupName},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.success){

        this.isLoaded = false;


        this.item_service.delete_item_type({item_type_id:parseInt(id)}).subscribe(
          {
            error: err => {

              this.isLoaded = true;
    
              this.redirectOnSessionError(err);
    
            },
            complete: () =>{

              const item_type_index = this.item_types.data.findIndex(item_type => item_type.id.toString() == id);
        
              if(item_type_index != -1){
      
                // console.log("znalazło index "+ item_index + " , jest teraz: " + this.item_list.data.length);
                
                this.item_types.data.splice(item_type_index, 1);
      
                // console.log("ilosc po usunieciu" + " " + this.item_list.data.length + "czy jest id= "+id+"? "+this.item_list.data.findIndex(item => item.id.toString() === id));
                this.item_types.paginator = this.paginator;
                this.item_types_count--;
                // this.myTable.renderRows();

      
      
              }

              this.isLoaded = true;

            }
          }
        );


      }

    });

  }



}
