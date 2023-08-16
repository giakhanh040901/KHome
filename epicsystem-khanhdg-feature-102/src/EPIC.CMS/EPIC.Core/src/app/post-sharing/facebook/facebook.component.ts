import { Component, OnInit } from '@angular/core';
import { FacebookServiceProxy } from '@shared/service-proxies/facebook-service';
import {  FacebookService, InitParams, LoginOptions, LoginResponse } from 'ngx-facebook';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';

interface FacebookPage {
  id: string,
  name: string,
  picture: any,
}

@Component({
  selector: 'app-facebook',
  templateUrl: './facebook.component.html',
  styleUrls: ['./facebook.component.scss']
})
export class FacebookComponent implements OnInit {

  pages: FacebookPage[];  
  posts: any[]; 
  listPostIds: string[]

  selectedPage: FacebookPage;

  constructor(
    private fb: FacebookService, 
    private facebookService: FacebookServiceProxy,
    protected messageService: MessageService,
    public ref: DynamicDialogRef,
    ) {
    const initParams: InitParams = {
      appId: '288859401588883',
      xfbml: true,
      version: 'v2.8',
    };
 
    fb.init(initParams);
   }

  ngOnInit(): void {
    this.pages = [ ];
    this.posts = [];
    this.selectedPage = { id: "", name: "", picture:""}
    this.facebookService.getFacebookPostIds().subscribe(res => {
      this.listPostIds = res;
    })
    let accessToken = localStorage.getItem('fb_access_token')
    if(accessToken) {
      this.facebookService.getFacebookPages(accessToken).subscribe(res => {
        this.pages = res.data
      })
    }
    
  }

  loginWithFacebook(): void {
    const loginOptions: LoginOptions = {
      enable_profile_selector: true,
      return_scopes: true,
      scope: 'email,pages_messaging,ads_management,leads_retrieval,pages_show_list,business_management,pages_manage_metadata,pages_read_engagement,publish_video,pages_manage_posts,pages_read_user_content,pages_manage_engagement,business_management' 
    }
    this.fb.login(loginOptions)
      .then((response: LoginResponse) => { 
        this.facebookService.exchangeToken(response.authResponse.accessToken).subscribe(res => {
          localStorage.setItem('fb_access_token', res.access_token)
          this.facebookService.getFacebookPages(res.access_token).subscribe(res => {
            this.pages = res.data
          })
        })
      }) 
      .catch((error: any) => console.error(error));
  }
  selectPage(page): void {
    this.selectedPage = page;
    this.facebookService.getFacebookPost(page.access_token, page.id).subscribe(res => {
      this.posts = res.data;
    })
  }

  addPost(post): void {
    post.pageName = this.selectedPage.name
    post.pageImage = this.selectedPage.picture.data.url
    post.pageId = this.selectedPage.id
    this.facebookService.addFacebookPost(post).subscribe(res => {
      this.messageService.add({
        severity: 'success',
        summary: "Nhập bài đăng thành công",
        detail: "Bài đăng facebook đã được nhập vào hệ thống",
        life: 3000,
      })
      this.listPostIds.push(post.id)
    })
  }

  hideDialog(): void {

  }

  save(): void {
    this.ref.close()
  }

}
