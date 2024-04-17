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
  value:string | null;
}

@Component({
  selector: 'app-edit-dialog',
  standalone: true,
  imports: [ ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './edit-dialog.component.html',
  styleUrl: './edit-dialog.component.css'
})
export class EditDialogComponent {

  public input_field!:FormGroup;
  public submitted:boolean = false;


  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: dialogData,
  ) {
    
    console.log(data.value)

    data.value = data.value ? data.value : "";
    
    this.input_field = this.fb.group({

      field: [data.value, Validators.required]

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

    this.dialogRef.close({success: true, value: this.input_field.controls.field.value});
  }

}
