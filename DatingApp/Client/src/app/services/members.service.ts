import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { of, map } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class MembersService {
    baseUrl = environment.apiUrl;
    members: Member[] = [];

    constructor(private http: HttpClient) {}

    public getMembers() {
        if (this.members.length > 0) return of(this.members);
        return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
            map((members) => {
                this.members = members;
                return members;
            })
        );
    }

    public getMember(username: string) {
        const member = this.members.find((m) => m.userName === username);
        if (member) return of(member);
        return this.http.get<Member>(this.baseUrl + 'users/' + username);
    }

    public updateMember(member: Member) {
        return this.http.put(this.baseUrl + 'users', member).pipe(
            map(() => {
                const index = this.members.indexOf(member);
                this.members[index] = { ...this.members[index], ...member };
            })
        );
    }

    public setMainPhoto(photoId: number) {
        return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
    }

    public deletePhoto(photoId: number) {
        return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
    }
}
