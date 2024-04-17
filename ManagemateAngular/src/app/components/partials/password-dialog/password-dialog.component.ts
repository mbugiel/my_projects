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
import { AuthService } from '../../../services/auth/auth.service';


interface dialogData{
  value:string;
}


@Component({
  selector: 'app-password-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './password-dialog.component.html',
  styleUrl: './password-dialog.component.css'
})
export class PasswordDialogComponent {


  public input_field!:FormGroup;
  public submitted:boolean = false;
  public isLoaded:boolean = true;


  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    public dialogRef: MatDialogRef<PasswordDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: dialogData,
  ) {

    
    this.input_field = this.fb.group({

      field: ['', Validators.required]

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

    this.isLoaded = false;

    this.auth.validatePassword({password: this.input_field.controls.field.value}).subscribe({

      error: () => {

        this.isLoaded = true;

      },
      
      complete: () =>{

        this.isLoaded = true;

        this.dialogRef.close({success: true});

      }

    })


  }


}