import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AuthService } from '../../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule} from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';


@Component({
  selector: 'app-header',
  templateUrl: './header-tabs.component.html',
  standalone: true,
  imports:[CommonModule, TranslateModule, RouterModule],
  styleUrl: './header-tabs.component.css'
})
export class HeaderTabsComponent implements OnInit{

  public LoggedOut:boolean = true;

  tabs: string[] = ['item', 'order', 'client', 'con-site', 'service','invoice'];
  tabTranslations: any = {
    'item': 'headerTabs.storage.selfName',
    'order': 'headerTabs.orders.selfName',
    'client': 'headerTabs.clients.selfName',
    'con-site': 'headerTabs.constructions.selfName',
    'service': 'headerTabs.services.selfName',
    'invoice': 'headerTabs.invoices.selfName'
  };
  
  tabChecks: boolean[] = [true, false, false, false, false];

  constructor(
    private location: Location,
    private auth: AuthService
  ){}

  ngOnInit(): void {

    const url = this.location.path();


    this.tabs.forEach((tab,index:number) => {

      if(url.includes('home')){

        return; //default magazyn

      }
      else{

        if (url.includes(tab)) this.tabChecks[index] = true;
        else this.tabChecks[index] = false;

      }

    });

    if(this.tabChecks.every(value => value === false)){
      
      if(url.includes('receipt')) this.tabChecks[1] = true;

    }
        
  }

  ActivateTabs(tab:string){

    this.tabs.forEach((tabs,index:number) => {
      if (tabs === tab) this.tabChecks[index] = true;
      else this.tabChecks[index] = false;
    });

  }

  logout(){

    this.LoggedOut = false;
    
    this.auth.logout().subscribe(() => this.LoggedOut = true);

  }


}
