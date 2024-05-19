import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { AuthService } from './services/auth/auth.service';
import { RouterOutlet } from '@angular/router';
import { MainHeaderComponent } from './components/partials/main-header/main-header.component';
import { MenuComponent } from './components/partials/menu/menu.component';
import { HeaderTabsComponent } from './components/partials/header-tabs/header-tabs.component';
import { CommonModule, DOCUMENT  } from '@angular/common';


@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [RouterOutlet, MainHeaderComponent, MenuComponent, HeaderTabsComponent, CommonModule]
})
export class AppComponent implements OnInit {
  isLoaded: boolean = false;
  showMenu: boolean = false;
  showMainHeader: boolean = false;
  showHeader: boolean = true;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private translate: TranslateService,
    private auth: AuthService,
    @Inject(DOCUMENT) private document: Document,
  ) {

    const browserLang = this.translate.getBrowserLang();

    if(browserLang) browserLang.match(/en|pl/) ? this.translate.use(browserLang) : this.translate.use('pl');

   }

  checkDataRecursive(route: ActivatedRoute) {
    const result = { showMenu: false, showMainHeader: false, showHeader: true };

    route.children.forEach(child => {
      const data = child.snapshot.data;
      result.showMenu = result.showMenu || data?.['showMenu'] || this.checkDataRecursive(child).showMenu;
      result.showMainHeader = result.showMainHeader || data?.['showMainHeader'] || this.checkDataRecursive(child).showMainHeader;
      result.showHeader = result.showHeader && (data?.['showHeader'] !== undefined ? data['showHeader'] : true) && this.checkDataRecursive(child).showHeader;
    });

    return result;
  }

  ngOnInit(): void {

    if(this.document.readyState === 'complete'){

      this.isLoaded = true;

    }else{
      
      this.document.addEventListener('DOMContentLoaded', () => {
        this.isLoaded = true;
      });
      
    }

    

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {

      const flags = this.checkDataRecursive(this.activatedRoute);
      this.showMenu = flags.showMenu;
      this.showMainHeader = flags.showMainHeader;
      this.showHeader = flags.showHeader;

    });

    // this.router.events.subscribe((event) => {
    //   const flags = this.checkDataRecursive(this.activatedRoute);
    //   if (event instanceof NavigationStart) {
    //   } else if (
    //     event instanceof NavigationEnd ||
    //     event instanceof NavigationCancel ||
    //     event instanceof NavigationError
    //   ) {

    //     this.showHeader = flags.showHeader;
    //     this.showMenu = flags.showMenu;
    //     this.showMainHeader = flags.showMainHeader;
    //     // setTimeout(() => {
    //       // this.isLoaded = true;
    //     // }, 100);

    //   }
    // });
  
  }


}
