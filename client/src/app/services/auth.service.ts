import { Inject, Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { AdminAuthGuard } from '../auth-guards/admin.auth.guard';
import { AccountInfo } from '@azure/msal-common';

@Injectable()
export class AuthService {
  constructor(
    private adminAuth: AdminAuthGuard,
    @Inject('BASE_URL') private baseUrl: string,
    private msalService: MsalService
  ) {}

  public get user(): AccountInfo {
    return this.msalService.instance.getAllAccounts()[0];
  }

  isAdmin(): boolean {
    return this.adminAuth.isAdmin();
  }

  logout() {
    this.msalService.logoutRedirect();
  }
}
