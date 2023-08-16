import { Component, ElementRef, Injector, OnInit, ViewChild } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { CrudComponentBase } from "@shared/crud-component-base";
import { AvatarService } from "@shared/services/avatar.service";
import { ImageCroppedEvent } from "ngx-image-cropper";
import { MessageService } from "primeng/api";
import { DynamicDialogRef } from "primeng/dynamicdialog";

@Component({
	selector: "app-change-avatar",
	templateUrl: "./change-avatar.component.html",
	styleUrls: ["./change-avatar.component.scss"],
})
export class ChangeAvatarComponent extends CrudComponentBase implements OnInit {

	imageInfo = {
		imgChangeEvt: null,
		cropImgPreview: null,
		avatarUrl: '',
		dataCrop: null
	}


	constructor(
		injector: Injector,
		messageService: MessageService,
		private sanitizer: DomSanitizer,
		private _avatarService: AvatarService,
		private ref: DynamicDialogRef,
	) {
		super(injector, messageService);
	}

	@ViewChild('fileInput', { static: true }) fileInput: ElementRef<HTMLInputElement>;

	ngOnInit(): void {} 

	ngAfterViewInit() {
		this.fileInput.nativeElement.click();
	  }

	onFileChange(event: any): void {
        this.imageInfo.imgChangeEvt = event;
    }
    cropImg(e: ImageCroppedEvent) {
		this.imageInfo.cropImgPreview = this.sanitizer.bypassSecurityTrustUrl(e.objectUrl);
		this.imageInfo.dataCrop = e.blob;		
    }
	updateAvatar(){
		this._avatarService.updateImage(this.imageInfo.avatarUrl).subscribe( (res) => {
			if (this.handleResponseInterceptor(res, "Cập nhật thành công")) {
				this.isLoading = false;			
				this.ref.close(true);
			}
		}, (err) => {
			console.log("err---", err);
			this.isLoading = false;			
		})
	}
	save(event: any) {
		if (this.imageInfo.dataCrop){
			const file = new File([this.imageInfo.dataCrop], 'cropped_image.png');
			this._avatarService.uploadFileGetUrl(file, 'avatar').subscribe(res => {
				if(res?.data){
					this.imageInfo.avatarUrl = res.data;
					this.updateAvatar();
				}
			})

		}

	}

	close(event: any) {
		this.ref.close();
	}
}
