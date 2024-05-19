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


@Component({
  selector: 'app-add-authorized-worder-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatCheckboxModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './add-authorized-worker-dialog.component.html',
  styleUrl: './add-authorized-worker-dialog.component.css'
})
export class AddAuthorizedWorkerDialogComponent {


  public input_field!:FormGroup;
  public submitted:boolean = false;


  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AddAuthorizedWorkerDialogComponent>,
  ) {
    
    this.input_field = this.fb.group({

      surname: ['', Validators.required],
      name: ['', Validators.required],
      phone_number: ['', Validators.required /*[Validators.required, Validators.pattern("\\+?[0-9]+(\\s[0-9]+)*")]*/],
      email: ['', Validators.required /*[Validators.email,Validators.required]*/],
      contact: [false],
      collection: [false],
      comment: [''],

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
        name: this.input_field.controls.name.value,
        surname: this.input_field.controls.surname.value,
        phone_number: this.input_field.controls.phone_number.value,
        email: this.input_field.controls.email.value,
        contact: this.input_field.controls.contact.value,
        collection: this.input_field.controls.collection.value,
        comment: this.input_field.controls.comment.value
      }
    );
  }



}
