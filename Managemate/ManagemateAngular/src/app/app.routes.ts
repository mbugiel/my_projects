//import { NgModule } from '@angular/core';
import { /*RouterModule,*/ Routes } from '@angular/router';
import { LoginComponent } from './components/pages/login/login.component';
import { MainComponent } from './components/pages/main/main.component';
import { HomeComponent } from './components/pages/_manage-mate/home/home.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ContactComponent } from './components/pages/contact/contact.component';
import { AboutComponent } from './components/pages/about/about.component';
import { RegisterComponent } from './components/pages/register/register.component';
import { VerifyEmailComponent } from './components/pages/verify-email/verify-email.component';
import { authGuard, authGuardChild } from './services/guard/auth.guard';
import { TwoStepLoginComponent } from './components/pages/two-step-login/two-step-login.component';
import { ItemListComponent } from './components/pages/_manage-mate/item-list/item-list.component';
import { ItemAddComponent } from './components/pages/_manage-mate/item-add/item-add.component';
import { OrderListComponent } from './components/pages/_manage-mate/order-list/order-list.component';
import { OrderAddComponent } from './components/pages/_manage-mate/order-add/order-add.component';
import { ClientListComponent } from './components/pages/_manage-mate/client-list/client-list.component';
import { ClientAddComponent } from './components/pages/_manage-mate/client-add/client-add.component';
import { ConSiteListComponent } from './components/pages/_manage-mate/con-site-list/con-site-list.component';
import { ConSiteAddComponent } from './components/pages/_manage-mate/con-site-add/con-site-add.component';
import { InvoiceListComponent } from './components/pages/_manage-mate/invoice-list/invoice-list.component';
import { InvoiceAddAllComponent } from './components/pages/_manage-mate/invoice-add-all/invoice-add-all.component';
import { InvoiceAddComponent } from './components/pages/_manage-mate/invoice-add/invoice-add.component';
import { ItemEditComponent } from './components/pages/_manage-mate/item-edit/item-edit.component';
import { ItemTypeComponent } from './components/pages/_manage-mate/item-type/item-type.component';
import { ItemCountingTypeComponent } from './components/pages/_manage-mate/item-counting-type/item-counting-type.component';
import { ClientEditComponent } from './components/pages/_manage-mate/client-edit/client-edit.component';
import { OrderEditComponent } from './components/pages/_manage-mate/order-edit/order-edit.component';
import { OrderComponent } from './components/pages/_manage-mate/order/order.component';
import { ReceiptListInOutComponent } from './components/pages/_manage-mate/receipt-list-in-out/receipt-list-in-out.component';
import { ConSiteEditComponent } from './components/pages/_manage-mate/con-site-edit/con-site-edit.component';
import { ReceiptComponent } from './components/pages/_manage-mate/receipt/receipt.component';
import { ReceiptEditComponent } from './components/pages/_manage-mate/receipt-edit/receipt-edit.component';
import { AccountComponent } from './components/pages/_manage-mate/account/account.component';
import { ClientComponent } from './components/pages/_manage-mate/client/client.component';
import { ServiceListComponent } from './components/pages/_manage-mate/service-list/service-list.component';
import { ServiceAddComponent } from './components/pages/_manage-mate/service-add/service-add.component';
import { ServiceEditComponent } from './components/pages/_manage-mate/service-edit/service-edit.component';

export const routes: Routes = [
    {
        path: '', component: MainComponent,
        data: {
          showMainHeader: true,
          showHeader: false,
        },
      },
      {
        path: 'login', component: LoginComponent,
        canActivate: [authGuard],
        data: {
          showMainHeader: true,
          showHeader: false,
        },
      },
      {
        path: 'register', component: RegisterComponent,
        canActivate: [authGuard],
        data: {
          showMainHeader: true,
          showHeader: false,
        }
      },
      {
        path: 'verify-email', component: VerifyEmailComponent,
        data: {
          showMainHeader: true,
          showHeader: false
        }
      },
      {
        path: 'two-step-login', component: TwoStepLoginComponent,
        data: {
          showMainHeader: true,
          showHeader: false
        }
      },
      {
        path: 'about', component: AboutComponent,
        data: {
          showMainHeader: true,
          showHeader: false,
        },
      },
      {
        path: 'contact', component: ContactComponent,
        data: {
          showMainHeader: true,
          showHeader: false,
        },
      },
      {
        path: 'manage-mate',
        canActivateChild: [authGuardChild],
        children: [
          {
            path: '',
            redirectTo: 'home',
            pathMatch: 'full'
          },
          {
            path: 'home', component: HomeComponent,
          },
          {
            path: 'account', component: AccountComponent,
          },
          {
            path: 'item-list', component: ItemListComponent,
          },
          {
            path: 'item-add', component: ItemAddComponent,
          },
          {
            path: 'item-edit/:item_id', component: ItemEditComponent,
          },
          {
            path: 'item-type', component: ItemTypeComponent,
          },
          {
            path: 'item-counting', component: ItemCountingTypeComponent,
          },
          {
            path: 'order-list', component: OrderListComponent,
          },
          {
            path: 'order-add', component: OrderAddComponent,
          },
          {
            path: 'order-edit/:order_id', component: OrderEditComponent,
          },
          {
            path: 'order/:order_id', component: OrderComponent,
          },
          {
            path: 'receipt/:receipt_id', component: ReceiptComponent,
          },
          {
            path: 'receipt-edit/:receipt_id', component: ReceiptEditComponent,
          },
          {
            path: 'receipt-list-in-out/:order_id/:in_out', component: ReceiptListInOutComponent,
          },
          {
            path: 'client-list', component: ClientListComponent,
          },
          {
            path: 'client-add', component: ClientAddComponent,
          },
          {
            path: 'client-edit/:client_id', component: ClientEditComponent,
          },
          {
            path: 'client/:client_id', component: ClientComponent,
          },
          {
            path: 'con-site-list', component: ConSiteListComponent,
          },
          {
            path: 'con-site-add', component: ConSiteAddComponent,
          },
          {
            path: 'service-list', component: ServiceListComponent,
          },
          {
            path: 'service-add', component: ServiceAddComponent,
          },
          {
            path: 'service-edit/:service_id', component: ServiceEditComponent,
          },
          {
            path: 'con-site-edit/:con-site_id', component: ConSiteEditComponent,
          },
          {
            path: 'invoice-list/:order_id', component: InvoiceListComponent,
          },
          {
            path: 'invoice-add', component: InvoiceAddComponent,
          },
          {
            path: 'invoice-add-all', component: InvoiceAddAllComponent,
          },
        ]
      },
    
      //musi być na końcu
      { path: '**', component: NotFoundComponent },
];
