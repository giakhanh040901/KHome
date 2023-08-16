import { Component, OnInit } from '@angular/core';
import { AppConsts, GeneralDescriptionConst } from '@shared/AppConsts';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-create-product-image',
  templateUrl: './create-product-image.component.html',
  styleUrls: ['./create-product-image.component.scss']
})
export class CreateProductImageComponent implements OnInit {
  uploadedFiles = [];
  isUploading: boolean = false;
  constructor(
    private _distributionService: DistributionService,
    private messageService: MessageService,
    public ref: DynamicDialogRef, 
    public configDialog: DynamicDialogConfig,

  ) { }

  AppConsts = AppConsts;
  GeneralDescriptionConst= GeneralDescriptionConst;
	baseUrl: string;

  path: any;
  image = {
    // id:  0,
    // order: 0,
    position: '',
    path: '',
    status: 'ACTIVE',
  }
  isView: boolean;

  isLoadingPage = false;

  ngOnInit(): void {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    if(this.configDialog?.data?.image) this.image = this.configDialog?.data?.image;
    if(this.configDialog?.data?.isView) this.isView = this.configDialog?.data?.isView;  
    
  }

  onUpload(event) { 
    this.isLoadingPage = true;
    if(event?.files[0]) {
      this._distributionService.uploadFileGetUrl(event?.files[0], "product-image").subscribe(
        (response) => {
          this.image.path = response.data;
          this.isLoadingPage = false;
        this.messageService.add({severity: 'info', summary: 'Ảnh đã được tải lên', detail: ''});
        
      }, error => {
        this.isUploading = false
        this.isLoadingPage = false;
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
    this.ref.close(this.image);
  }
  
  close() {
    this.ref.close();
  }

  changePosition(event){

  }

}
