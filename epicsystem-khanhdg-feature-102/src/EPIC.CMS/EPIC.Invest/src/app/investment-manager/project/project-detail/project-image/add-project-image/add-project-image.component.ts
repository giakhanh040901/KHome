import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { ProjectServiceProxy } from '@shared/services/project-manager-service';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';


@Component({
  selector: 'app-add-project-image',
  templateUrl: './add-project-image.component.html',
  styleUrls: ['./add-project-image.component.scss']
})
export class AddProjectImageComponent implements OnInit {
 
    constructor(
        private _distributionService: DistributionService,
        private messageService: MessageService,
        public ref: DynamicDialogRef, 
        public config: DynamicDialogConfig,
        private _guaranteeAssetService: ProjectServiceProxy,
    ) { 
    }

    baseUrl: string;
    uploadedFiles = [];
    isUploading: boolean = false;
    AppConsts = AppConsts;
    projectId: any;
    collateral = {
        'projectId': 0,
        'projectImages': []
    }

    ngOnInit(): void {
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
        this.collateral.projectId  = this.config.data.projectId;
    }
    onUpload(event) { 
        if(event) {
            this.isUploading = true;
            let uploadFilesProcess  = [];
            event.files.forEach(file => {
                uploadFilesProcess.push(this._distributionService.uploadFileGetUrl(file, "project"))
            });
            //
            forkJoin(uploadFilesProcess).subscribe(results => {
                this.uploadedFiles = results;
                results.forEach((element1:any) => {
                    if(element1) {
                        this.collateral.projectImages.push(element1.data);
                    } 
                })
                this.isUploading = false
                this.messageService.add({severity: 'info', summary: 'Ảnh đã được tải lên', detail: ''});
            }, (error) => {
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
        this._guaranteeAssetService.createProjectImage(this.collateral).subscribe((response) => {
            this.messageService.add({severity: 'info', summary: 'Thêm thành công', detail: ''});
            this.ref.close(true);
        });
    }
}

