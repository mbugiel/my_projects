import {Component, Inject} from '@angular/core';
import {
  MatDialog,
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {FormBuilder, FormGroup, FormsModule, Validators, ReactiveFormsModule} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';


interface dialogData{
  display_annotation:boolean;
  item_id:number;
  get_item:boolean;
  annotation_value:string;
  max_value:number;
}


@Component({
  selector: 'app-select-number-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './select-number-dialog.component.html',
  styleUrl: './select-number-dialog.component.css'
})
export class SelectNumberDialogComponent {

  public input_field!:FormGroup;
  public submitted:boolean = false;


  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<SelectNumberDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: dialogData
  ) {

    if(data.get_item){

      this.input_field = this.fb.group({

        field: [0, [Validators.required, Validators.min(Number.MIN_VALUE), Validators.max(this.data.max_value)]],
        annotation: [this.data.annotation_value]
  
      });

    }else{

      this.input_field = this.fb.group({

        field: [0, [Validators.required, Validators.min(Number.MIN_VALUE), Validators.max(Number.MAX_VALUE)]],
        annotation: [this.data.annotation_value]
  
      });

    }

  }

  Cancel(): void {
    this.dialogRef.close({success: false});
  }

  Confirm(): void {

    if(this.input_field.invalid){
      this.submitted = true;
      return;
    }

    this.dialogRef.close({
      success: true,
      item_id_check: this.data.item_id,
      quantity: this.input_field.controls.field.value,
      annotation: this.input_field.controls.annotation.value
    });
  }

}
