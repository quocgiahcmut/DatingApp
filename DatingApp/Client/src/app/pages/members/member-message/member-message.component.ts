import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css']
})
export class MemberMessageComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm
  @Input() username?: string
  messageContent = ''

  constructor(public messageService: MessageService) {}

  ngOnInit() {
  }

  sendMessage() {
    if (!this.username) return
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm?.reset()
    })
  }
}
