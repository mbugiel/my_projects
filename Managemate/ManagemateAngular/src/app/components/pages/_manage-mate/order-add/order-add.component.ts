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
import { OrderService } from '../../../../services/order/order.service';
import { ClientService } from '../../../../services/client/client.service';
import { Output_Client_Model } from '../../../../shared/interfaces/API_Output_Models/Client_Models/Output_Client_Model';
import { Output_Construction_Site_Model } from '../../../../shared/interfaces/API_Output_Models/Construction_Site_Models/Output_Construction_Site_Model';
import { ConSiteService } from '../../../../services/con-site/con-site.service';
import { Add_Order_Data } from '../../../../shared/interfaces/API_Input_Models/Order_Models/Add_Order_Data';

@Component({
  selector: 'app-order-add',
  standalone: true,
  imports: [ReactiveFormsModule, MatAutocompleteModule, MatSelectModule, RouterModule, CommonModule, TranslateModule, MatInputModule, MatFormFieldModule],
  templateUrl: './order-add.component.html',
  styleUrl: './order-add.component.css'
})
export class OrderAddComponent implements OnInit {


  public add_Order_Form!:FormGroup;
  public submitted:boolean = false;

  public pl_language:boolean;

  public isLoadedClients:boolean = false;
  public isLoadedConSites:boolean = false;

  public isLoadedAddedOrder:boolean = true;

  @ViewChild('inputClient') inputClient!: ElementRef;
  @ViewChild('inputConSite') inputConSite!: ElementRef;

  public clients: Array<Output_Client_Model> = new Array<Output_Client_Model>;
  public clients_filtered!: Array<Output_Client_Model>;

  public con_sites: Array<Output_Construction_Site_Model> = new Array<Output_Construction_Site_Model>;
  public con_sites_filtered!: Array<Output_Construction_Site_Model>;


  constructor(
    private fb: FormBuilder,
    private order_service:OrderService,
    private client_service:ClientService,
    private con_site_Service:ConSiteService,
    private router:Router,
    private translate:TranslateService
  ){
    this.pl_language = translate.currentLang == "pl";


    this.add_Order_Form = this.fb.group({
      inputOrderName: ['', Validators.required],
      inputClient: ['', Validators.required],
      inputConSite: ['', Validators.required],
      inputStatus: ['', Validators.required],
      inputComment: ['']
    });

  }


  redirectOnSessionError(err:any){

    if(parseInt(err.error.code) == 9 || parseInt(err.error.code) == 10  || parseInt(err.error.code) == 1){

      this.router.navigateByUrl('/login');

    }

  }


  ngOnInit(){


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





    this.con_site_Service.get_con_site_list().subscribe({

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




  add_order(){

    this.submitted = true;
    if (this.add_Order_Form.invalid) return;

    this.isLoadedAddedOrder = false;

    const fc = this.add_Order_Form.controls;

    const OrderName = fc.inputOrderName.value;
    const Client_fk = fc.inputClient.value.split("-");
    const ConSite_fk = fc.inputConSite.value;
    const Status = fc.inputStatus.value;
    const Comment = fc.inputComment.value;

    const add_order_input:Add_Order_Data = {

      order_name: OrderName,

      client_id_FK: Number(this.clients.find(function(client){
        return client.name == Client_fk[0].trim() && client.company_name == Client_fk[1].trim();
      })!.id),

      construction_site_id_FK: Number(this.con_sites.find(function(conSite){
        return conSite.construction_site_name == ConSite_fk;
      })!.id),

      status: Status,
      
      comment:Comment
    }

    this.order_service.add_order(add_order_input).subscribe({

      error: () => {

        this.isLoadedAddedOrder = true;

      },
      complete: () => {

        this.isLoadedAddedOrder = true;

      }

    })

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
