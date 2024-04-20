import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChildFn } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const router: Router = inject(Router);
  const cookie: CookieService = inject(CookieService);
  const specificRoutes: string[] = ['/login','/register'];

  if (
    //jeżeli nie jest zalogowany i nie jest na danej ścieżce
    !cookie.check('session_data') &&
    !specificRoutes.includes(state.url)
  ) {
    //do strony głównej
    return router.navigateByUrl('/');
  } else if (
    //jeżeli jest zalogowany i probuje sie dostac na daną ścieżkę
    specificRoutes.includes(state.url) &&
    cookie.check('session_data')
  ) {
    //do strony głównej
    return router.navigateByUrl('/');
  }
  return true;

};

export const authGuardChild: CanActivateChildFn = (childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  return authGuard(childRoute, state);
};
