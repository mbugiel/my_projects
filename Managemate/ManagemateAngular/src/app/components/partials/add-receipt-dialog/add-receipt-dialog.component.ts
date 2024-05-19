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
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';


interface dialogData{
  in_out:boolean;
}


@Component({
  selector: 'app-add-receipt-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatCheckboxModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './add-receipt-dialog.component.html',
  styleUrl: './add-receipt-dialog.component.css'
})
export class AddReceiptDialogComponent {


  public input_field!:FormGroup;
  public submitted:boolean = false;


  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AddReceiptDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: dialogData
  ) {
    
    this.input_field = this.fb.group({

      element: ['', Validators.required],
      transport: ['', Validators.required],
      reservation: [false]

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

    this.dialogRef.close(
      {
        success: true,
        element: this.input_field.controls.element.value,
        transport: this.input_field.controls.transport.value,
        reservation: this.input_field.controls.reservation.value
      }
    );
  }



}






