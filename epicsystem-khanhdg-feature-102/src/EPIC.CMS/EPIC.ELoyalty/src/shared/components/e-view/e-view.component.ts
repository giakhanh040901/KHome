import { Component, OnInit, ViewChild } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ContentTypeEView } from '@shared/consts/base.const';
import { HelpersService } from '@shared/services/helpers.service';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Image } from 'primeng/image';

@Component({
	selector: 'e-view',
	templateUrl: './e-view.component.html',
	styleUrls: ['./e-view.component.scss'],
})
export class EViewComponent implements OnInit {
	constructor(
		public ref: DynamicDialogRef, 
        public config: DynamicDialogConfig,
        public _helperService: HelpersService,
	) {}

	ContentTypeEView = ContentTypeEView;
    dialogData: {
        content: any;
        type: ContentTypeEView;
    }

	ngOnInit(): void {
		this.dialogData = this.config.data;
        this.showImage();
	}
	
    onClose() {
        this.ref.close();
    }

    genHtml = () => {
        return this._helperService.getContentHtml(this.dialogData.content);
    }

    blobImage = () => {
        this.showImage();
        return this._helperService.getBlobUrlImage(this.dialogData.content)        
    }

	src: SafeUrl;
    @ViewChild('pImage') pImage: Image;
	showImage() {
        if(this.dialogData.type === ContentTypeEView.IMAGE) {
            setTimeout(() => {
                this.src = this._helperService.getBlobUrlImage(this.dialogData.content);
                this.pImage.onImageClick();
            }, 0);
        }
	}
}
