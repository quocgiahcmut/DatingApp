import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { of, map } from 'rxjs';
import { PaginatedResult } from '../models/pagination';
import { UserParams } from '../models/userParams';

@Injectable({
    providedIn: 'root',
})
export class MembersService {
    baseUrl = environment.apiUrl;
    members: Member[] = [];

    constructor(private http: HttpClient) {}

    public getMembers(userParams: UserParams) {
        let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

        params = params.append('minAge', userParams.minAge);
        params = params.append('maxAge', userParams.maxAge);
        params = params.append('gender', userParams.gender);

        return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params);
    }

    private getPaginatedResult<T>(url: string, params: HttpParams) {
        const pagiantedResult: PaginatedResult<T> = new PaginatedResult<T>();
        return this.http.get<T>(url, { observe: 'response', params }).pipe(
            map((response) => {
                if (response.body) {
                    pagiantedResult.result = response.body;
                }
                const pagination = response.headers.get('Pagination');
                if (pagination) {
                    pagiantedResult.pagination = JSON.parse(pagination);
                }
                return pagiantedResult;
            })
        );
    }

    private getPaginationHeaders(pageNumber: number, pageSize: number) {
        let params = new HttpParams();

        params = params.append('pageNumber', pageNumber);
        params = params.append('pageSize', pageSize);

        return params;
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
