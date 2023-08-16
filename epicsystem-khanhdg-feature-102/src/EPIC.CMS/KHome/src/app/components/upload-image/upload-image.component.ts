import { HttpClient } from '@angular/common/http';
import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts, ProjectMedia } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { FileServiceProxy } from '@shared/service-proxies/file-service';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Observable, forkJoin } from 'rxjs';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.scss']
})
export class UploadImageComponent extends CrudComponentBase implements OnInit {
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

  ngOnInit(): void {}

  onUpload(event) { 
    if(event) {
      this.isUploading = true;
      let uploadFilesProcess  = []
      event.files.forEach(file => {
        uploadFilesProcess.push(this._distributionService.uploadFileGetUrl(file, "media"))
      });
      forkJoin(uploadFilesProcess).subscribe(results => {
        this.uploadedFiles = results;
        
        this.isUploading = false
        this.messageSuccess("Ảnh đã được tải lên");
        
      }, error => {
        this.isUploading = false
        this.messageError("Có lỗi khi tải ảnh lên, vui lòng thử lại");
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
