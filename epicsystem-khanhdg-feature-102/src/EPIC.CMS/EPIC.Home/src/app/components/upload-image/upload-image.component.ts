import { Component, Injector, OnInit } from "@angular/core";
import { AppConsts } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { AvatarService } from "@shared/services/avatar.service";
import { MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";

@Component({
	selector: "app-upload-image",
	templateUrl: "./upload-image.component.html",
	styleUrls: ["./upload-image.component.scss"],
})
export class UploadImageComponent extends CrudComponentBase implements OnInit {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _avatarService: AvatarService,
		public ref: DynamicDialogRef, 
		public config: DynamicDialogConfig
	) {
		super(injector, messageService);
	}

	AppConsts = AppConsts;

	uploadedFiles = [];
	isUploading: boolean = false;

	ngOnInit(): void {}

	onUpload(event) { 
		if(event) {
		  this.isUploading = true;
		  let uploadFilesProcess  = []
		  event.files.forEach(file => {
			uploadFilesProcess.push(this._avatarService.uploadFileGetUrl(file, "home"))
		  });
		  forkJoin(uploadFilesProcess).subscribe(results => {
			this.uploadedFiles = results;
			
			this.isUploading = false
			
			this.messageService.add({severity: 'info', summary: 'Ảnh đã được tải lên', detail: ''});
			
		  }, error => {
			this.isUploading = false
			this.messageService.add({severity: 'error', summary: 'Có lỗi khi tải ảnh lên, vui lòng thử lại', detail: ''});
	
		  })
		   
		} 
	}
	
	removeFile(fileToDelete) {
		this.uploadedFiles = this.uploadedFiles.filter(fileItem => {
		  return fileItem.data !== fileToDelete.data
		})
	}
	   
	hideDialog() {
		this.ref.close();
	}

	save() {
		if (this.uploadedFiles.length > 0){
			this.ref.close(this.uploadedFiles);
		}
	}
}
