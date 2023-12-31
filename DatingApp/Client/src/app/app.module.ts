import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from '~/app-routing.module';
import { AppComponent } from '~/app.component';
import { NavComponent } from '~/components/nav/nav.component';
import { HomeComponent } from '~/pages/home/home.component';
import { RegisterComponent } from '~/components/register/register.component';
import { MemberListComponent } from '~/pages/members/member-list/member-list.component';
import { MemberDetailComponent } from '~/pages/members/member-detail/member-detail.component';
import { MessagesComponent } from '~/pages/messages/messages.component';
import { ListsComponent } from '~/pages/lists/lists.component';
import { SharedModule } from '~/modules/shared.module';
import { TestErrorComponent } from '~/errors/test-error/test-error.component';
import { ErrorInterceptor } from '~/interceptor/error.interceptor';
import { NotFoundComponent } from '~/errors/not-found/not-found.component';
import { ServerErrorComponent } from '~/errors/server-error/server-error.component';
import { MemberCardComponent } from '~/components/members/member-card/member-card.component';
import { JwtInterceptor } from '~/interceptor/jwt.interceptor';
import { MemberEditComponent } from '~/pages/members/member-edit/member-edit.component';
import { LoadingInterceptor } from '~/interceptor/loading.interceptor';
import { PhotoEditorComponent } from '~/components/members/photo-editor/photo-editor.component';
import { TextInputComponent } from '~/components/forms/text-input/text-input.component';
import { DatePickerComponent } from '~/components/forms/date-picker/date-picker.component';
import { MemberMessageComponent } from '~/pages/members/member-message/member-message.component';
import { AdminPanelComponent } from '~/pages/admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from '~/directives/has-role.directive';
import { UserManagementComponent } from '~/pages/admin/user-management/user-management.component';
import { PhotoManagementComponent } from '~/pages/admin/photo-management/photo-management.component';
import { RolesModalComponent } from '~/components/modals/roles-modal/roles-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    MessagesComponent,
    ListsComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DatePickerComponent,
    MemberMessageComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    SharedModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
