import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ContractTemplateServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { FileServiceProxy } from '@shared/service-proxies/file-service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Observable, forkJoin } from 'rxjs';
import {decode} from 'html-entities'
import { AppConsts } from '@shared/AppConsts';
@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.scss']
})
export class UploadImageComponent implements OnInit {
  uploadedFiles = [];
  isUploading: boolean = false;
  constructor(
    private _contractTemplateService: ContractTemplateServiceProxy,
    private messageService: MessageService,
    public ref: DynamicDialogRef, 
    public config: DynamicDialogConfig
  ) { }

  AppConsts = AppConsts;

  ngOnInit(): void {
    
  }
  onUpload(event) { 
    if(event) {
      this.isUploading = true;
      let uploadFilesProcess  = []
      event.files.forEach(file => {
        uploadFilesProcess.push(this._contractTemplateService.uploadFileGetUrl(file, "media"))
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
    this.ref.close(this.uploadedFiles);
  }
}
