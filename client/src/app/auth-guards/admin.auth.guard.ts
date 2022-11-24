import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { MsalService } from '@azure/msal-angular';

@Injectable()
export class AdminAuthGuard implements CanActivate {
  constructor(private msalService: MsalService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    return this.isAdmin();
  }

  isAdmin() {
    let account = this.msalService.instance.getAllAccounts()[0];
    return account.idTokenClaims?.roles
      ? account.idTokenClaims.roles.includes('admin')
      : false;
  }
}
