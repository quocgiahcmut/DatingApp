import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './components/nav/nav.component';
import { HomeComponent } from './pages/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { MemberListComponent } from './pages/members/member-list/member-list.component';
import { MemberDetailComponent } from './pages/members/member-detail/member-detail.component';
import { MessagesComponent } from './pages/messages/messages.component';
import { ListsComponent } from './pages/lists/lists.component';
import { SharedModule } from './modules/shared.module';

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
    ],
    imports: [
        BrowserAnimationsModule,
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        BrowserAnimationsModule,
        FormsModule,
        SharedModule,
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
