import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { AccountService } from '~/services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard {
  constructor(private accountService: AccountService, private toastr: ToastrService) { }
  
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (!user) return false
        if (user.roles.includes('Admin') || user.roles.includes('Moderator')) {
          return true
        } else {
          this.toastr.error('You cannot enter this area')
          return false
        }
      })
    )
  }
}
