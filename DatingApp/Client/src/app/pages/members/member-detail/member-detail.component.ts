import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
    selector: 'app-member-detail',
    templateUrl: './member-detail.component.html',
    styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
    member: Member | undefined;
    galleryOptions: NgxGalleryOptions[] = [];
    galleryImages: NgxGalleryImage[] = [];

    constructor(private memberService: MembersService, private route: ActivatedRoute) {}

    ngOnInit(): void {
        this.loadMember();

        this.galleryOptions = [
            {
                width: '500px',
                height: '500px',
                imagePercent: 100,
                thumbnailsColumns: 4,
                imageAnimation: NgxGalleryAnimation.Slide,
                preview: false,
            },
        ];
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

    public loadMember() {
        const username = this.route.snapshot.paramMap.get('username');
        if (!username) return;

        this.memberService.getMember(username).subscribe({
            next: (member) => {
                this.member = member;
                this.galleryImages = this.getImages();
            },
        });
    }
}
