import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MemberEditComponent } from '~/pages/members/member-edit/member-edit.component'
import { ConfirmService } from '~/services/confirm.service';

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesGuard {

  constructor(private confirmService: ConfirmService) { }

  canDeactivate(component: MemberEditComponent): Observable<boolean> {
    if (component.editForm?.dirty) {
      return this.confirmService.comfirm()
    }

    return of(true);
  }
}
