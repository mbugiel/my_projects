import {Component, ElementRef, Inject, OnInit, ViewChild} from '@angular/core';
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
import { ClientService } from '../../../services/client/client.service';
import { Output_Authorized_Worker_Model } from '../../../shared/interfaces/API_Output_Models/Authorized_Worker_Models/Output_Authorized_Worker_Model';
import { Router } from '@angular/router';

interface dialogData{
  auth_worker_id:number;
}


@Component({
  selector: 'app-edit-authorized-worker-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, TranslateModule, MatCheckboxModule, MatInputModule, FormsModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, CommonModule],
  templateUrl: './edit-authorized-worker-dialog.component.html',
  styleUrl: './edit-authorized-worker-dialog.component.css'
})
export class EditAuthorizedWorkerDialogComponent implements OnInit {


  public input_field!:FormGroup;
  public submitted:boolean = false;
  public editing_auth_worker!:Output_Authorized_Worker_Model;

  @ViewChild('surname') surname!: ElementRef;
  @ViewChild('name') name!: ElementRef;
  @ViewChild('inputCompanyName') inputCompanyName!: ElementRef;
  @ViewChild('inputNIP') inputNIP!: ElementRef;
  @ViewChild('phone_number') phone_number!: ElementRef;
  @ViewChild('email') email!: ElementRef;
  @ViewChild('contact') contact!: ElementRef;
  @ViewChild('collection') collection!: ElementRef;
  @ViewChild('comment') comment!: ElementRef;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditAuthorizedWorkerDialogComponent>,
    private client_service: ClientService,
    @Inject(MAT_DIALOG_DATA) public data: dialogData,
    private router: Router
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

  ngOnInit(){

    console.log(this.data.auth_worker_id)

    this.client_service.get_auth_worker_by_id({id_to_get:this.data.auth_worker_id}).subscribe({

      next: response => {

        this.editing_auth_worker = response.responseData;

      },
      error: err => {

        this.redirectOnSessionError(err);

      },
      complete: () =>{


        const fc = this.input_field.controls;
        const e_auth_worker = this.editing_auth_worker;

        fc.name.setValue(e_auth_worker.name);
        fc.surname.setValue(e_auth_worker.surname);
        fc.phone_number.setValue(e_auth_worker.phone_number);
        fc.email.setValue(e_auth_worker.email);
        fc.contact.setValue(e_auth_worker.contact);
        fc.collection.setValue(e_auth_worker.collection);
        fc.comment.setValue(e_auth_worker.comment);


      }
      
    })

}

  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

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
