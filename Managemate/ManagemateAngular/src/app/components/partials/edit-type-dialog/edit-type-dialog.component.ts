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
  title:string;
  label:string;
  label_2:string;
  value:string;
  value_2:number;
}

@Component({
  selector: 'app-edit-dialog',
  standalone: true,
  imports: [ ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './edit-type-dialog.component.html',
  styleUrl: './edit-type-dialog.component.css'
})
export class EditTypeDialogComponent {

  public input_field!:FormGroup;
  public submitted:boolean = false;


  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: dialogData,
  ) {
    
    this.input_field = this.fb.group({

      field: [data.value, Validators.required],
      field_2: [data.value_2, Validators.required]

    });

  }

  Cancel(): void {
    this.dialogRef.close({success: false});
  }

  Confirm(): void {

    if(this.input_field.invalid){
      this.submitted = true;
      return;
    }

    this.dialogRef.close({success: true, value: this.input_field.controls.field.value, value_2:this.input_field.controls.field_2.value});
  }

}
