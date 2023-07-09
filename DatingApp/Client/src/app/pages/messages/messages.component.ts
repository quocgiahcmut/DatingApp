import { Component, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { Pagination } from 'src/app/models/pagination';
import { MessageService } from 'src/app/services/message.service';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
    messages?: Message[];
    pagination?: Pagination;
    container = 'Unread';
    pageNumber = 1;
    pageSize = 5;

    constructor(private messageService: MessageService) {}

    ngOnInit(): void {
        this.loadMessages();
    }

    loadMessages() {
        this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
            next: (response) => {
                this.messages = response.result;
                this.pagination = response.pagination;
            },
        });
    }

    pageChanged(event: any) {
        if (this.pageNumber !== event.page) {
            this.pageNumber = event.page;
            this.loadMessages();
        }
    }
}
