import { Component, OnInit, ViewChild } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ContentTypeEView } from '@shared/consts/base.const';
import { HelpersService } from '@shared/services/helper.service';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Image } from 'primeng/image';

@Component({
  selector: 'app-e-view',
  templateUrl: './e-view.component.html',
  styleUrls: ['./e-view.component.scss'],
})
export class EViewComponent implements OnInit {

    constructor(
        public ref: DynamicDialogRef, 
        public config: DynamicDialogConfig,
        public _helperService: HelpersService,
    ) { }

    ContentTypeEView = ContentTypeEView;
    dialogData: {
        content: any;
        type: ContentTypeEView;
    }

    isLoading: boolean = false;
    contentHeight: string = '';

    ngOnInit(): void {
        this.dialogData = this.config.data;
        this.removeImageMaskOther();
        this.showImage();
        // XỬ LÝ LOAD FILE (KHÔNG ỔN ĐỊNH CẦN CHECK LẠI- CHƯA DÙNG ĐC)
        if([ContentTypeEView.FILE].includes(this.dialogData.type)) {
            this.isLoading = true;
            setTimeout(() => {
                if(this.isLoading) {
                    this._helperService.messageError("Không thể tải được nội dung!");
                    this.isLoading = false;
                    this.hideDialog();
                }
            }, 8000);
        }
    }

    hideDialog() {
        this.ref.close();
    }

    ngAfterViewInit() {
        const contentHeight = document.querySelectorAll(".p-dialog-content")[0].clientHeight;
        this.contentHeight = contentHeight ? contentHeight+'px' : '75vh';
        this.removeImageMaskOther();
    }

    // Xóa các thẻ ImageMask khác 
    removeImageMaskOther() {
        const elementImageMask: any = document.querySelectorAll(".p-image-mask");
        for(let i=0; i < elementImageMask.length; i++) {
            if(elementImageMask[i]) elementImageMask[i].remove();
        }
    }
   
    genHtml = () => {
        return this._helperService.getContentHtml(this.dialogData.content);
    }

    src: SafeUrl | string;
    @ViewChild('pImage') pImage: Image;
	showImage() {
        if(this.dialogData.type === ContentTypeEView.IMAGE) {
            setTimeout(() => {
                if(this.dialogData.content instanceof File) {
                    // content dạng file xử lý hiển thị local khi chưa upload sever
                    this.src = this._helperService.getBlobUrlImage(this.dialogData.content);
                } else {
                    // Đường dẫn ảnh online
                    this.src = this.dialogData.content;
                }
                this.pImage.onImageClick();
            }, 0);
        }
	}

}
