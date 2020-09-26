import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private _router: Router,
        private _authenticationService: AuthenticationService
    ) { }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this._authenticationService.currentUserValue;
        if (currentUser) {
            return true;
        }

        this._router.navigate(['/unauthorized'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}