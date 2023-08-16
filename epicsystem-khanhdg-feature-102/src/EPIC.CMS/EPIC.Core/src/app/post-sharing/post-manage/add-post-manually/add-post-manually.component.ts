import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppConsts, TypeFormatDateConst } from '@shared/AppConsts';
import { NotificationService } from '@shared/service-proxies/notification-service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { UploadImageComponent } from 'src/app/components/upload-image/upload-image.component';
import { decode } from 'html-entities';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {ConfirmationService} from 'primeng/api';
import { CrudComponentBase } from '@shared/crud-component-base';
import { FacebookServiceProxy } from '@shared/service-proxies/facebook-service';
@Component({
  selector: 'app-add-post-manually',
  templateUrl: './add-post-manually.component.html',
  styleUrls: ['./add-post-manually.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService],
})
export class AddPostManuallyComponent extends CrudComponentBase implements OnInit {
  
  selectedTopic: any;
  topicTest: any;
  title: string;
  inputData: any;
  actions = []
  statuses = []
  topicList = []
  mainImg: any;
  baseUrl: string;
  caretPos: number = 0;

  posts: any = {};



  constructor( 
    private fb: FormBuilder,
    private facebookService: FacebookServiceProxy,
    public dialogService: DialogService,
    public ref: DynamicDialogRef, 
    public config: DynamicDialogConfig,
    injector: Injector,
    messageService: MessageService,

   ) {  
      super(injector, messageService);
    } 
   ngOnInit() { 
    
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl; 
    if (this.config.data.inputData) {    
      this.posts = {
        ...this.config.data.inputData,
        created_time: this.config.data.inputData.created_time ? new Date(this.config.data.inputData.created_time) : null,
        updated_time: this.config.data.inputData.updated_time ? new Date(this.config.data.inputData.updated_time) : null,
      };
    } else {
      this.posts.created_time = new Date();
    }
   }

   selectImg(type) {
    const ref = this.dialogService.open(UploadImageComponent, {
      data: {
        inputData: []
      },
      header: 'Tải hình ảnh',
      width: '500px',
      footer: ""
    });
    ref.onClose.subscribe(images => { 
      console.log("data",images);
      if(images && images.length > 0) { 
        if(type == 'FULL_PICTURE') {
          this.posts.full_picture = images[0].data; 
        } else if(type == 'PAGE_IMAGE') {
          this.posts.pageImage = images[0].data; 
        } 
      } 
      console.log("this.posts",this.posts);
      
    });
  }

  removeFile(file) {
    this.posts.full_picture = null;
  }
  onSubmit() {
  
    let body = {
      ...this.posts,
      created_time: this.formatDate(this.posts.created_time,TypeFormatDateConst.YMDTHmsZ),
      updated_time: this.formatDate(this.posts.updated_time,TypeFormatDateConst.YMDTHmsZ)
    }
    console.log("this.posts",this.posts);
    if(this.posts.id) {
      this.facebookService.updateFacebookPost(body).subscribe(res => {
        if(res?.id) {
          this.ref.close(true);
        }
        
      })
    } else {
      this.facebookService.addFacebookPost(body).subscribe(res => {
        if(res?.id) {
          this.ref.close(true);
        }
        
      })
    }
  }

   close() { 
    this.ref.close();
  }
}

