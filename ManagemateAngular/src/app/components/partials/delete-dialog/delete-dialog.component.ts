import { Component, Inject } from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {
  MatDialogRef,
  MatDialogActions,
  MatDialogClose,
  MatDialogTitle,
  MatDialogContent,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { ItemService } from '../../../services/item/item.service';
import { ItemListComponent } from '../../pages/_manage-mate/item-list/item-list.component';
import { Router } from '@angular/router';

interface dialogData{
  title:string;
  message:string;
  first_param:string;
}

@Component({
  selector: 'app-delete-dialog',
  standalone: true,
  imports: [ ItemListComponent, TranslateModule, MatButtonModule, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent],
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.css'
})


export class DeleteDialogComponent {

  constructor(
    private item_manager:ItemService,
    private router: Router,
    public dialogRef: MatDialogRef<DeleteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:dialogData)
  {

  }

  closeDialog(){

    this.dialogRef.close({deleted:false});
  
  }

  deleteAndClose(){

    this.dialogRef.close({deleted:true});

  }


}

