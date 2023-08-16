import { Component, Injector, OnInit } from "@angular/core";
import { AppConsts, ProjectMedia } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { DistributionService } from "@shared/services/distribution.service";
import { MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";

@Component({
	selector: "app-upload-media",
	templateUrl: "./upload-media.component.html",
	styleUrls: ["./upload-media.component.scss"],
})
export class UploadMediaComponent extends CrudComponentBase implements OnInit {
	uploadedFiles = [];
	isUploading: boolean = false;
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _distributionService: DistributionService,
		public ref: DynamicDialogRef, 
		public config: DynamicDialogConfig
	) {
		super(injector, messageService);
	}
	
	AppConsts = AppConsts;
	ProjectMedia = ProjectMedia;
	media: any;
	mediaItem: any;
	urlPath: string;
	isCreateMediaGroup: boolean;
	isMultiple: boolean;
	mediaType: string;
	groupTitle: string;

	listImage = [];	
	listImageDelete = [];
	
	ngOnInit(): void {
		this.media = this.config?.data?.media;
		this.mediaItem = this.config?.data?.mediaItem;
		this.urlPath = this.mediaItem?.urlPath;
		this.isCreateMediaGroup = this.config?.data?.isCreateMediaGroup;
		this.isMultiple = this.config?.data?.isMultiple;
		this.mediaType = this.config?.data?.mediaType;
		
	}

	onUpload(event) { 
		if(event) {
			if (this.listImageDelete.length > 0) {
				this.listImage = event.files.filter((item) => this.listImageDelete.indexOf(item.name) === -1);
			} else {
				this.listImage = event.files
			}
			this.isUploading = true;
			let uploadFilesProcess  = []
			this.listImage.forEach(file => {
				uploadFilesProcess.push(this._distributionService.uploadFileGetUrl(file, "media"))
			});
			event = null;
			forkJoin(uploadFilesProcess).subscribe(results => {
				this.uploadedFiles = results;
				
				this.isUploading = false;
				this.messageSuccess("Ảnh đã được tải lên");
				
			}, error => {
				this.isUploading = false;
				this.messageError("Có lỗi khi tải ảnh lên, vui lòng thử lại");
			})
		} 
	} 
	  
	removeFile(fileToDelete, index) {
		this.listImageDelete.push(this.listImage[index].name);
		this.uploadedFiles = this.uploadedFiles.filter(fileItem => {
			return fileItem.data !== fileToDelete.data
		})
	}
	   
	hideDialog() {
		this.ref.close();
	}

	isPdfFile(file) {
        var parts = file.split('.');
        var typeFile = parts[parts.length - 1];
        switch (typeFile.toLowerCase()) {
            case 'pdf':
                return true;
        }
        return false;
    }
	
	save() {
		if (this.uploadedFiles.length > 0){
			if(this.urlPath){
				if(this.uploadedFiles[0]){
					this.uploadedFiles[0].urlPath = this.urlPath;
				}
				else {
					this.ref.close({urlPath: this.urlPath});
				}
			}
			if (this.isCreateMediaGroup) {
				if (this.groupTitle){
					let body = {
					groupTitle: this.groupTitle,
					uploadedFiles: this.uploadedFiles
					}
					this.ref.close(body);
				} else {
					this.messageError("Vui lòng nhập tên nhóm hình ảnh!");
				}
			} else {
				this.ref.close(this.uploadedFiles);
			}
		} else {
			if(this.urlPath){
				this.ref.close({urlPath: this.urlPath});
			}
			else {
				this.messageError("Vui lòng thêm hình ảnh!");
			}
		}
	}
}
	