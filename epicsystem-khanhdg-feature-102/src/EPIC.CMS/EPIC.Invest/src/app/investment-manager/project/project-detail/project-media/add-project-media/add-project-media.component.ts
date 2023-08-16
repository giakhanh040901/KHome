import { Component, OnInit } from '@angular/core';
import { AppConsts, MediaConst } from '@shared/AppConsts';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { UploadImageComponent } from 'src/app/components/upload-image/upload-image.component';
import {decode} from 'html-entities'
import { MediaService } from '@shared/services/media.service';
@Component({
  selector: 'app-add-project-media',
  templateUrl: './add-project-media.component.html',
  styleUrls: ['./add-project-media.component.scss']
})
export class AddProjectMediaComponent implements OnInit {
  
    baseUrl: string;
    title: string;
    inputData: any;
    postForm: FormGroup;
    types = []
    statuses = []
    imageFiles = []
    mainImg: any = "";
    caretPos: number = 0;
    htmlMarkdownOptions: any = [
        {
            value: 'MARKDOWN',
            name: 'MARKDOWN'
        },
        {
            value: 'HTML',
            name: 'HTML'
        }
    ]
    positions = [];

    constructor( 
        private fb: FormBuilder,
        private broadcastService: MediaService,
        protected messageService: MessageService,
        public dialogService: DialogService,
        public ref: DynamicDialogRef, 
        public config: DynamicDialogConfig,
    ) {  
    }
    projectId: any;
    
    formatVideo:boolean;
    formatImage:boolean;

    newsMedia: any = {}
    
    ngOnInit() {
        this.projectId = this.config.data.projectId
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl; 

        if (this.config.data.inputData) {
            this.inputData = this.config.data.inputData;
            this.mainImg = this.inputData.mainImg; 
            this.postForm = this.fb.group({
                title: [this.inputData.title, Validators.required],
                content: [decode(this.inputData.content), [Validators.required]],
                type: "invest_product",
                displayText: [this.inputData.displayText, []],
                productId: [this.projectId.toString(), []],
                isFeatured: [this.inputData.isFeatured, []],
                sort: [this.inputData.sort, []],
                contentType: [this.inputData.contentType, []],
            });
            this.positions = []
        } else {
            this.postForm = this.fb.group({
                title: ['', Validators.required],
                content: ['', [Validators.required]],
                type: ['invest_product', []], 
                displayText: ['', []], 
                productId: [this.projectId.toString(), []],
                isFeatured: [false, []], 
                status: ['DRAFT', []], 
                contentType: ['MARKDOWN', []], 
                sort: [1, []], 
            });
        }
      
        this.detectVideo();
        for (let key in MediaConst.mediaStatus) {
            this.statuses.push({ key: key, value: MediaConst.mediaStatus[key] });
        }
    }

    get postFormControl() {
        return this.postForm.controls;
    }

    header() {
        return this.title
    }

    selectImg() {
        const ref = this.dialogService.open(UploadImageComponent, {
            header: 'Tải hình ảnh',
            width: '500px',
            data: {
                inputData: []
            },
        });
        //
        ref.onClose.subscribe(images => { 
            if(images && images.length > 0) {
                this.mainImg = images[0].data;
                this.detectVideo();
            }
        });
    }
    

    onSubmit() {
        if (this.postForm.valid) {
            if (this.inputData) {
                this.postForm.value.mainImg = this.mainImg;
                this.postForm.value.id = this.inputData.id;
                this.broadcastService.saveMedia(this.postForm.value).subscribe((result) => {
                    this.ref.close(result);
                }, () => {
                    this.messageService.add({
                        severity: 'error',
                        summary: "Lỗi khi cập nhật hình ảnh",
                        detail: "Vui lòng thử lại",
                        life: 3000,
                    })
                })
            } else { 
                this.postForm.value.mainImg = this.mainImg;
                this.broadcastService.createMedia(this.postForm.value).subscribe((result) => {
                    this.ref.close(result);
                }, () => {
                    this.messageService.add({
                        severity: 'error',
                        summary: "",
                        detail: "Vui lòng thử lại",
                        life: 3000,
                    })
                })
            }
        }
    }

    close() { 
        this.ref.close();
    }

    insertImage() {
        const ref = this.dialogService.open(UploadImageComponent, {
            header: 'Chèn hình ảnh',
            width: '600px',
            data: {
                inputData: [],
                showOrder: false
            },
        });
        ref.onClose.subscribe(images => {
            let imagesUrl = "";
            images.forEach(image => {
                imagesUrl +=  `![](${this.baseUrl}/${image.data}) \n`;
            })
            
            let oldContentValue = this.postForm.value.content;
            let a = oldContentValue.slice(0, this.caretPos) + imagesUrl + oldContentValue.slice(this.caretPos); 
            this.postForm.controls['content'].setValue(a);
        })
    }

    getCaretPos(oField) {
        if (oField.selectionStart || oField.selectionStart == '0') {
            this.caretPos = oField.selectionStart;
        }
    }

    detectVideo() {
        const images = ["jpg", "gif", "png"]
        let videos = ["mp4", "3gp", "ogg", "mkv"]
        for (var i = 0; i < videos.length; i++) {
            let position = this.mainImg.search(videos[i])
            if (this.mainImg.search(videos[i]) > -1) {
                this.formatVideo = true;
                this.formatImage = false;
                break;
            }
            
            if(this.mainImg.search(images[i]) > -1){
                this.formatImage = true;
                this.formatVideo = false;
                break;
            }
        }
    }
}
