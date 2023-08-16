import { Component, OnInit } from '@angular/core';
import { AppConsts, GeneralDescriptionConst } from '@shared/AppConsts';
import { DistributionService } from '@shared/services/distribution.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-create-feature',
  templateUrl: './create-feature.component.html',
  styleUrls: ['./create-feature.component.scss'],
  providers: [ConfirmationService],
})
export class CreateFeatureComponent implements OnInit {

  uploadedFiles = [];
  isUploading: boolean = false;
  constructor(
    private _distributionService: DistributionService,
    private messageService: MessageService,
    public ref: DynamicDialogRef, 
    public configDialog: DynamicDialogConfig,
    public confirmationService: ConfirmationService,
  ) { }

  AppConsts = AppConsts;
  GeneralDescriptionConst= GeneralDescriptionConst;

	baseUrl: string;

  feature = {
    fileUrl: '',
    iconUri: '',
    description: '',
    type: null,
    status: 'ACTIVE',
  }
  isView: boolean;
  isLoadingPage = false;
  imageStyle: any = {'background-color': '#D9D9D9', 'margin-right': '0rem'};
  ngOnInit(): void {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    if(this.configDialog?.data?.feature) this.feature = this.configDialog?.data?.feature;
    if(this.configDialog?.data?.isView) this.isView = this.configDialog?.data?.isView;  
  }

  deleteFile(){
    this.feature.fileUrl = '';
  }
  myUploader(event) {
    this.isLoadingPage = true;
    if(event?.files[0]) {
      this._distributionService.uploadFileGetUrl(event?.files[0], "feature").subscribe(
        (response) => {
          this.feature.fileUrl = response.data;
          this.isLoadingPage = false;
        
        this.messageService.add({severity: 'info', summary: 'Tải lên thành công', detail: ''});
        
      }, error => {
        this.isUploading = false;
        this.isLoadingPage = false;
        this.messageService.add({severity: 'error', summary: 'Có lỗi khi tải ảnh lên, vui lòng thử lại', detail: ''});

      })
       
    } 
  }

  onUpload(event) { 
    this.isLoadingPage = true;
    if(event?.files[0]) {
      this._distributionService.uploadFileGetUrl(event?.files[0], "feature").subscribe(
        (response) => {
          this.feature.iconUri = response.data;
          this.isLoadingPage = false;
        
        this.messageService.add({severity: 'info', summary: 'Ảnh đã được tải lên', detail: ''});
        
      }, error => {
        this.isUploading = false;
        this.isLoadingPage = false;
        this.messageService.add({severity: 'error', summary: 'Có lỗi khi tải ảnh lên, vui lòng thử lại', detail: ''});

      })
       
    } 
  } 

  hideDialog() {
    this.ref.close();
  }

  save() {
    this.ref.close(this.feature);
  }
  
  close() {
    this.ref.close();
  }
}
