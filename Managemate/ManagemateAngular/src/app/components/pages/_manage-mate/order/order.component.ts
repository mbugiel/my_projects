import { Component, OnInit,  ElementRef, ViewChild } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatSelectModule} from '@angular/material/select';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Output_Client_Model } from '../../../../shared/interfaces/API_Output_Models/Client_Models/Output_Client_Model';
import { Output_Construction_Site_Model } from '../../../../shared/interfaces/API_Output_Models/Construction_Site_Models/Output_Construction_Site_Model';
import { Output_Order_Advanced_Model } from '../../../../shared/interfaces/API_Output_Models/Order_Models/Output_Order_Advanced_Model';
import { Get_By_ID_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Get_By_ID_Order_Data';
import { OrderService } from '../../../../services/order/order.service';
import { ConSiteService } from '../../../../services/con-site/con-site.service';
import { ClientService } from '../../../../services/client/client.service';
import { Edit_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Edit_Order_Data';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [ RouterModule, CommonModule, TranslateModule, MatIconModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent implements OnInit{

  public isLoadedOrder:boolean = false;

  public selected_order!: Output_Order_Advanced_Model;

  constructor(
    private url:ActivatedRoute,
    private order_service:OrderService,
    private client_service:ClientService,
    private con_site_Service:ConSiteService,
    private router:Router,
    private translate:TranslateService
  ){ }



  
  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  formatDate(input:Date): string {

    const date = new Date(input);
    const dsep = "-";
    const tsep = ":";

    console.log(date.getUTCMonth())

    let date_array:string[] = [
      date.getUTCDate().toString(),
      (date.getUTCMonth()+1).toString(),
      date.getUTCFullYear().toString(),
      date.getUTCHours().toString(),
      date.getUTCMinutes().toString()
    ];

    for(let i = 0; i < 5; i++){

      if(date_array[i].length < 2) date_array[i] = "0"+date_array[i];

    }    

    return date_array[0] + dsep + date_array[1] + dsep + date_array[2] + ", " + date_array[3] + tsep + date_array[4];

  }


  ngOnInit(): void {

    this.url.params.subscribe(params => {

      let edit_order_input:Get_By_ID_Order_Data = {

        orderId: params['order_id']

      }

      this.order_service.get_order_by_id(edit_order_input).subscribe({

        next: response => {

          this.selected_order = response.responseData;

          this.isLoadedOrder = true;

        },

        error: err => {

          this.isLoadedOrder = true;
  
          this.redirectOnSessionError(err);
  
        }
        
      })

    });


  }

}
