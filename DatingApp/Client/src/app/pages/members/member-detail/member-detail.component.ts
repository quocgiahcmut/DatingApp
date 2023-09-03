import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';

import { Member } from 'src/app/models/member';
import { Message } from 'src/app/models/message';
import { MembersService } from 'src/app/services/members.service';
import { MessageService } from 'src/app/services/message.service';
import { User } from '~/models/user';
import { AccountService } from '~/services/account.service';
import { PresenceService } from '~/services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent
  member: Member = {} as Member;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  activeTab?: TabDirective
  messages: Message[] = []
  user?: User

  constructor(private accountService: AccountService, 
    private messageService: MessageService, 
    private route: ActivatedRoute,
    public presenceService: PresenceService,
    private router: Router
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })

    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    //this.loadMember();
    this.route.data.subscribe({
      next: data => this.member = data['member']
    })

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })


    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ]

    this.galleryImages = this.getImages()
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection()
  }

  public getImages(): NgxGalleryImage[] {
    if (!this.member) return [];

    const imagesUrl = [];
    for (const photo of this.member.photos) {
      imagesUrl.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
      });
    }

    return imagesUrl;
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find(t => t.heading === heading)!.active = true
    }
  }

  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.user) {
      //this.loadMessages()
      this.messageService.createHubConnection(this.user, this.member.userName)
    } else {
      this.messageService.stopHubConnection()
    }
  }
}
