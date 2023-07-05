import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { of, map, take } from 'rxjs';
import { PaginatedResult } from '../models/pagination';
import { UserParams } from '../models/userParams';
import { AccountService } from './account.service';
import { User } from '../models/user';

@Injectable({
    providedIn: 'root',
})
export class MembersService {
    baseUrl = environment.apiUrl;
    members: Member[] = [];
    memberCache = new Map();
    user: User | undefined;
    userParams: UserParams | undefined;

    constructor(private http: HttpClient, private accountService: AccountService) {
        this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: (user) => {
                if (user) {
                    this.userParams = new UserParams(user);
                    this.user = user;
                }
            },
        });
    }

    public getUserParams() {
        return this.userParams;
    }

    public setUserParams(params: UserParams) {
        this.userParams = params;
    }

    public resetUserParams() {
        if (this.user) {
            this.userParams = new UserParams(this.user);
            return this.userParams;
        }
        return;
    }

    public getMembers(userParams: UserParams) {
        const response = this.memberCache.get(Object.values(userParams).join('-'));

        if (response) return of(response);

        let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

        params = params.append('minAge', userParams.minAge);
        params = params.append('maxAge', userParams.maxAge);
        params = params.append('gender', userParams.gender);
        params = params.append('orderBy', userParams.orderBy);

        return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
            map((res) => {
                this.memberCache.set(Object.values(userParams).join('-'), res);
                return res;
            })
        );
    }

    public getMember(username: string) {
        const member = [...this.memberCache.values()]
            .reduce((arr, elem) => arr.concat(elem.result), [])
            .find((member: Member) => member.userName === username);

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

    public addLike(username: string) {
        return this.http.post(this.baseUrl + 'likes/' + username, {});
    }

    public getLike(predicate: string) {
        return this.http.get(this.baseUrl + 'likes?predicate=' + predicate);
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
}
