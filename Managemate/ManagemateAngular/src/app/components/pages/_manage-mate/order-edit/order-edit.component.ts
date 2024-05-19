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
import { DateHandlerService } from '../../../../services/date-handler/date-handler.service';

@Component({
  selector: 'app-order-edit',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule ],
  templateUrl: './order-edit.component.html',
  styleUrl: './order-edit.component.css'
})
export class OrderEditComponent implements OnInit {

  
  public edit_Order_Form!:FormGroup;
  public submitted:boolean = false;

  public isLoadedClients:boolean = false;
  public isLoadedConSites:boolean = false;

  public isLoadedEditedOrder:boolean = true;
  public isLoadedEditingOrder:boolean = false;

  
  @ViewChild('inputClient') inputClient!: ElementRef;
  @ViewChild('inputConSite') inputConSite!: ElementRef;

  public clients: Array<Output_Client_Model> = new Array<Output_Client_Model>;
  public clients_filtered!: Array<Output_Client_Model>;

  public con_sites: Array<Output_Construction_Site_Model> = new Array<Output_Construction_Site_Model>;
  public con_sites_filtered!: Array<Output_Construction_Site_Model>;

  public editing_order!:Output_Order_Advanced_Model;


  constructor(
    private url:ActivatedRoute,
    private fb: FormBuilder,
    private order_service:OrderService,
    private client_service:ClientService,
    private conSite_service:ConSiteService,
    private router:Router,
    private translate:TranslateService,
    private dateHandler:DateHandlerService
  ){

    this.edit_Order_Form = this.fb.group({
      inputOrderName: ['', Validators.required],
      inputClient: ['', Validators.required],
      inputConSite: ['', Validators.required],
      inputStatus: ['', Validators.required],
      inputCreationDate: ['', Validators.required],
      inputDefaultPaymentMethod: ['', Validators.required],
      inputDefaultPaymentDateOffset: ['', Validators.required],
      inputDefaultDiscount: ['', Validators.required],
      inputComment: ['']
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10 || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  ngOnInit(){

    this.url.params.subscribe(params => {

      let edit_order_input:Get_By_ID_Order_Data = {

        orderId: params['order_id']

      }

      this.order_service.get_order_by_id(edit_order_input).subscribe({

        next: response => {

          this.editing_order = response.responseData;

          this.isLoadedEditingOrder = true;

        },

        error: err => {

          this.isLoadedEditingOrder = true;
  
          this.redirectOnSessionError(err);
  
        },

        complete: () =>{
  
          this.isLoadedEditingOrder = true;

          const fc = this.edit_Order_Form.controls;
          const e_order = this.editing_order;

          fc.inputOrderName.setValue(e_order.order_name);

          if(e_order.client_id_FK.company_name){

            fc.inputClient.setValue(e_order.client_id_FK.name+" - "+e_order.client_id_FK.company_name);

          }else{

            fc.inputClient.setValue(e_order.client_id_FK.name);

          }

          fc.inputConSite.setValue(e_order.construction_site_id_FK.construction_site_name);
          fc.inputStatus.setValue(e_order.status.toString());


          const read_date = new Date(e_order.creation_date).toISOString();
          
          fc.inputCreationDate.setValue(read_date.substring(0, read_date.indexOf('.')));

          fc.inputDefaultPaymentMethod.setValue(e_order.default_payment_method);

          fc.inputDefaultPaymentDateOffset.setValue(e_order.default_payment_date_offset);

          fc.inputDefaultDiscount.setValue(e_order.default_discount * 100);

          fc.inputComment.setValue(e_order.comment);

  
        }
        
      })

    });




    this.client_service.get_client_list().subscribe({

      next: response => {

        this.clients = response.responseData;

        this.isLoadedClients = true;

        // console.log(this.item_types);

      },
      error: err => {

        this.isLoadedClients = true;

        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isLoadedClients = true;

      }

    });





    this.conSite_service.get_con_site_list().subscribe({

      next: response => {

        this.con_sites = response.responseData;

        this.isLoadedConSites = true;

        // console.log(this.trading_types);

      },
      error: err => {

        this.isLoadedConSites = true;

        this.redirectOnSessionError(err);

      },
      complete: () =>{

        this.isLoadedConSites = true;

      }

    });



  }




  edit_order(){

    this.submitted = true;

    if (this.edit_Order_Form.invalid) return;

    this.url.params.subscribe(params => {


      this.isLoadedEditedOrder = false;

      const fc = this.edit_Order_Form.controls;

      const OrderName = fc.inputOrderName.value;
      const Client_fk = fc.inputClient.value.split("-");
      const ConSite_fk = fc.inputConSite.value;
      const Status = fc.inputStatus.value;
      const CreationDate = fc.inputCreationDate.value;
      const PayMethod = fc.inputDefaultPaymentMethod.value;
      const PayOffset = fc.inputDefaultPaymentDateOffset.value;
      const Discount = fc.inputDefaultDiscount.value / 100;
      const Comment = fc.inputComment.value;
  
      const edit_order_input:Edit_Order_Data = {

        id: params['order_id'],
  
        order_name: OrderName,
  
        client_id_FK: Number(this.clients.find(function(client){
          return client.name == Client_fk[0].trim() && client.company_name == Client_fk[1].trim();
        })!.id),
  
        construction_site_id_FK: Number(this.con_sites.find(function(conSite){
          return conSite.construction_site_name == ConSite_fk;
        })!.id),
  
        status: Status,

        creation_date: this.dateHandler.changeToUtc(new Date(CreationDate)),

        default_payment_method: PayMethod,

        default_payment_date_offset: PayOffset,

        default_discount: Discount,
        
        comment:Comment
      }


      this.order_service.edit_order(edit_order_input).subscribe({

        error: () => {

          this.isLoadedEditedOrder = true;

        },
        complete: () => {

          this.isLoadedEditedOrder = true;

        }

      })

    });


  }



  filterClients(): void {
    const filterValue = this.inputClient.nativeElement.value.toLowerCase();
    this.clients_filtered = this.clients.filter(o => o.name.toLowerCase().includes(filterValue) || o.company_name.toLowerCase().includes(filterValue));
  }

  filterConSites(): void {
    const filterValue = this.inputConSite.nativeElement.value.toLowerCase();
    this.con_sites_filtered = this.con_sites.filter(o => o.construction_site_name.toLowerCase().includes(filterValue));
  }


}
