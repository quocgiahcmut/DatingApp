import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';
import { MemberListComponent } from './pages/members/member-list/member-list.component';
import { MemberDetailComponent } from './pages/members/member-detail/member-detail.component';
import { ListsComponent } from './pages/lists/lists.component';
import { MessagesComponent } from './pages/messages/messages.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent },
            { path: 'members/:id', component: MemberDetailComponent },
            { path: 'lists', component: ListsComponent },
            { path: 'messages', component: MessagesComponent },
        ],
    },
    { path: '**', component: HomeComponent, pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
