import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs';
import { PaginatedResult } from '../models/pagination';

export function getPaginatedResult<T>(url: string, params: HttpParams, http: HttpClient) {
    const pagiantedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
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

export function getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    return params;
}
