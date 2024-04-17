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
import { EditDialogComponent } from '../../../partials/edit-dialog/edit-dialog.component';
import { Output_Item_Counting_Type_Model } from '../../../../shared/interfaces/API_Output_Models/Item_Counting_Type_Models/Output_Item_Counting_Type_Model';
import { Add_Item_Counting_Type_Data } from '../../../../shared/interfaces/API_Input_Models/Item_Counting_Type_Models/Add_Item_Counting_Type_Data';
import { PasswordDialogComponent } from '../../../partials/password-dialog/password-dialog.component';

@Component({
  selector: 'app-item-counting-type',
  standalone: true,
  imports: [MatIconModule, MatPaginatorModule, ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule, MatTableModule],
  templateUrl: './item-counting-type.component.html',
  styleUrl: './item-counting-type.component.css'
})
export class ItemCountingTypeComponent {



  public item_counting_type_Form!:FormGroup;
  public submitted:boolean = false;
  public isLoaded:boolean = true;
  public item_counting_types_count:number = 0;
  public item_counting_types:MatTableDataSource<Output_Item_Counting_Type_Model>;
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('item_table') myTable!: MatTable<any>;
  public columnsToDisplay = ['counting_type_name', 'edit', 'delete'];

  constructor(
    private fb: FormBuilder,
    private item_service:ItemService,
    private router:Router,
    public dialog: MatDialog,
    private translate:TranslateService
  ){

    this.item_counting_type_Form = this.fb.group({
      inputItemType: ['', Validators.required],
    });

    this.item_counting_types = new MatTableDataSource<Output_Item_Counting_Type_Model>();

  }


  add_item_counting_type(){

    this.isLoaded = false;

    this.submitted = true;
    if (this.item_counting_type_Form.invalid){
      this.isLoaded = true;
      return;
    }

    const fc = this.item_counting_type_Form.controls;

    const counting_type_value = fc.inputItemType.value;

    const add_counting_type:Add_Item_Counting_Type_Data = {
      counting_type: counting_type_value
    }

    this.item_service.add_item_counting_type(add_counting_type).subscribe({

      error: () => {

        this.isLoaded = true;

      },
      complete: () => {

        this.isLoaded = true;

        this.get_item_counting_types();
      }

    })

  }


  get_item_counting_types(){

    this.isLoaded = false;

    this.item_service.get_item_counting_types().subscribe({

      next: response => {

        this.item_counting_types = new MatTableDataSource<Output_Item_Counting_Type_Model>(response.responseData);
        this.item_counting_types_count = this.item_counting_types.data.length;

      },
      error: err => {

        this.isLoaded = true;

        if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){

          this.router.navigateByUrl('/login');
    
        }

      },
      complete: () => {

        this.item_counting_types.paginator = this.paginator;
        this.isLoaded = true;


      }

    })

  }


  ngOnInit(): void {

    this.get_item_counting_types();


  }



  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, UnitName:string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {title: this.translate.instant('deleteDialog.itemCountingTypeTitle') , message: this.translate.instant('deleteDialog.itemCountingTypeMessage') , first_param: UnitName},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.deleted){

        this.openPasswordDialog(enterAnimationDuration, exitAnimationDuration, id, UnitName);

      }

    });

  }


  openEditDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, UnitName:string): void {
    const dialogRef = this.dialog.open(EditDialogComponent, {
      data: {title: this.translate.instant('editDialog.itemCountingTypeTitle'), label:  this.translate.instant('itemCountingType.selfName'), value: UnitName},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {

      if(result.success){


        this.item_service.edit_item_counting_type({item_counting_type_id:parseInt(id), counting_type: result.value}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () =>{


              const ict = this.item_counting_types.data.find(function(item_counting_type){
                return item_counting_type.id.toString() == id;
              });

              if(ict){

                ict.counting_type = result.value;

              }

            }
          }
        );


      }


    });
  }




  openPasswordDialog(enterAnimationDuration: string, exitAnimationDuration: string, id:string, UnitName:string): void {
    const dialogRef = this.dialog.open(PasswordDialogComponent, {
      data: {value: UnitName},
      width: '300px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    dialogRef.afterClosed().subscribe(result => {
      
      if(result.success){


        this.item_service.delete_item_counting_type({item_counting_type_id:parseInt(id)}).subscribe(
          {
            error: err => {
    
              if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){
    
                this.router.navigateByUrl('/login');
    
              }
    
            },
            complete: () =>{

              const counting_type_index = this.item_counting_types.data.findIndex(item_type => item_type.id.toString() == id);
        
              if(counting_type_index != -1){

                
                this.item_counting_types.data.splice(counting_type_index, 1);

                this.item_counting_types.paginator = this.paginator;
                this.item_counting_types_count--;

      
      
              }

            }
          }

        );


      }

    });

  }



}
