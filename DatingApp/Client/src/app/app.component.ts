import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
    title = 'This is DATING APP';
    users: any;

    constructor(private http: HttpClient) {}

    ngOnInit(): void {
        this.http.get('https://localhost:7000/api/users').subscribe({
            next: (response) => (this.users = response),
            error: (err) => console.log(err),
            complete: () => console.log('Request has completed'),
        });
    }
}
