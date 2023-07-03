import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { of, map } from 'rxjs';
import { PaginatedResult } from '../models/pagination';

@Injectable({
    providedIn: 'root',
})
export class MembersService {
    baseUrl = environment.apiUrl;
    members: Member[] = [];
    pagiantedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

    constructor(private http: HttpClient) {}

    public getMembers(page?: number, itemsPerPage?: number) {
        let params = new HttpParams();

        if (page && itemsPerPage) {
            params = params.append('pageNumber', page);
            params = params.append('pageSize', itemsPerPage);
        }

        return this.http
            .get<Member[]>(this.baseUrl + 'users', { observe: 'response', params })
            .pipe(
                map((response) => {
                    if (response.body) {
                        this.pagiantedResult.result = response.body;
                    }
                    const pagination = response.headers.get('Pagination');
                    if (pagination) {
                        this.pagiantedResult.pagination = JSON.parse(pagination);
                    }
                    return this.pagiantedResult;
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
