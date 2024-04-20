import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AuthService } from '../../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { RouterModule} from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';


@Component({
  selector: 'app-header',
  templateUrl: './header-tabs.component.html',
  standalone: true,
  imports:[CommonModule, TranslateModule, RouterModule],
  styleUrl: './header-tabs.component.css'
})
export class HeaderTabsComponent implements OnInit{

  tabs: string[] = ['item', 'order', 'client', 'con-site','invoice'];
  tabTranslations: any = {
    'item': 'headerTabs.storage.selfName',
    'order': 'headerTabs.orders.selfName',
    'client': 'headerTabs.clients.selfName',
    'con-site': 'headerTabs.constructions.selfName',
    'invoice': 'headerTabs.invoices.selfName'
  };
  tabPrefix: string = '';
  tabChecks: boolean[] = [true, false, false, false, false];

  constructor(private location: Location, private auth: AuthService){}

  ngOnInit(): void {

    const url = this.location.path();

    const splited_url = url.split('/');

    const regex = /^[0-9]+$/;

    if(regex.test(splited_url[splited_url.length - 1])){
      this.tabPrefix = splited_url[splited_url.length - 2];
    }else{
      this.tabPrefix = splited_url[splited_url.length - 1];
    }

    

    this.tabs.forEach((tab,index:number) => {
      if(this.tabPrefix === 'home') return; //default magazyn
      else{
      if (this.tabPrefix.includes(tab)) this.tabChecks[index] = true;
      else this.tabChecks[index] = false;
      }
    });
        
  }

  ActivateTabs(tab:string){

    this.tabs.forEach((tabs,index:number) => {
      if (tabs === tab) this.tabChecks[index] = true;
      else this.tabChecks[index] = false;
    });

  }

  logout(){
    return this.auth.logout();
  }
}
