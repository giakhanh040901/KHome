import { Component, ElementRef, EventEmitter, Inject, Injector, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { NgSelectComponent } from '@ng-select/ng-select';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts, ContractTemplateConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { ContractTemplateServiceProxy, ContractTypeServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { AngularFileUploaderComponent, AngularFileUploaderConfig } from 'angular-file-uploader';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-or-update-contract-template',
  templateUrl: './create-or-update-contract-template.component.html',
  styleUrls: ['./create-or-update-contract-template.component.scss']
})
export class CreateOrUpdateContractTemplateComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _datePickerLocale: BsLocaleService,
    private _contractTemplateService: ContractTemplateServiceProxy,
    private _contractTypeService: ContractTypeServiceProxy,
    private _render: Renderer2,
    @Inject(API_BASE_URL) baseUrl?: string,

  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    if(AppConsts.remoteServiceBaseUrlLocal) {
      this.baseUrl = AppConsts.remoteServiceBaseUrlLocal;
    }
  }
  private baseUrl:string;
  urlUpload: string;
  contractTemplate: any = {
    "id": 0,
    "code": null,
    "name": "",
    "contractTypeId": null,
    "contractTempType": null,
    "contractTempUrl": "",
    "contractTempContent": ""
  }

  title: string;
  page = new Page();
  contractTempTypeConst = ContractTemplateConst.contractTempType;
  contractTypes:any = [];
  afuConfig: AngularFileUploaderConfig = {
    uploadAPI:  {
      url: '',
    },
    // id: 888888,
    hideResetBtn: true,
    multiple: true,
    formatsAllowed: ".repx, .pdf",
    maxSize: 2.99,
    fileNameIndex: false,
    replaceTexts: {
      selectFileBtn: 'Chọn file upload',
      resetBtn: 'Reset file',
      uploadBtn: 'Tải lên',
      afterUploadMsg_success: '',
      afterUploadMsg_error: '',
      sizeLimit: 'Kích thước file tối đa'
    }
};

  @Output() onSave = new EventEmitter<any>();

  @ViewChild('fileUpload') private fileUpload:  AngularFileUploaderComponent;
  elementSelect = ['contractTempType','contractTypeId'];

  // Element Input
  @ViewChild('id') id : ElementRef<HTMLElement>;
  @ViewChild('code') code: ElementRef<HTMLElement>;
  @ViewChild('name') name: ElementRef<HTMLElement>;
  @ViewChild('contractTypeId') contractTypeId: NgSelectComponent;
  @ViewChild('contractTempType') contractTempType: NgSelectComponent;
  @ViewChild('contractTempUrl') contractTempUrl: ElementRef<HTMLElement>;
  @ViewChild('contractTempContent') contractTempContent: ElementRef<HTMLElement>;

  ngOnInit(): void {
    this.afuConfig.uploadAPI.url = this.baseUrl + '/api/image/file/upload?folder=contract-template';
    if(this.contractTemplate.id) {
      this.isLoading = true
      this._contractTemplateService.get(this.contractTemplate.id).subscribe((res) => {
        this.contractTemplate = res.data;
        this.isLoading = false;
        console.log({ itemContractTemplate: res.data});
      });
    }

    this._contractTypeService.getAll(this.page).subscribe((resContractType) => {
      this.contractTypes = resContractType.data?.items;

      console.log({ 'resContractType': resContractType });
    });
  }

  disableWarningSelect(nameElement: string) {
    let elSelect = this[nameElement].element.querySelector('div.ng-select-container');
    this._render.setStyle(elSelect, 'border', '1px solid #d9d9d9', );
    // elSelect.ownerDocument.body.style.setProperty('border','1px solid #d9d9d9', '!important');
  }


  triggerFieldError(name: string) {
    if(this.elementSelect.includes(name)) {
      let elSelect = this[name].element.querySelector('div.ng-select-container');
      // this[name].focus();
      this._render.setStyle(elSelect, 'border', '1px solid red');
    } else {
      let el: HTMLElement = this[name].nativeElement;
      el.focus(); // focus
      el.classList.add('is-invalid');
    }
  }

  callTriggerFieldError(response) {
    let keyFirstNameError = this.getKeyFirstNameError(response);
    if(this.contractTemplate[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  fileChange(e) {
    alert(1);
  }

  changeFileUpload() {
    this.fileUpload.hideResetBtn = false;
  }

  responseUpload(response) {
    console.log({ responseUpload: response });
    if(response.body) {
      if(response.body.status === 0) {
        this.fileUpload.uploadMsgText = response.body.message;
        this.fileUpload.uploadMsgClass = "error";
      } else {
        this.fileUpload.progressBarShow = true;
        this.fileUpload.uploadMsgText = "Upload thành công";
        this.contractTemplate.contractTempUrl = response.body.data;
        setTimeout(() => {
          this.fileUpload.progressBarShow = false;
        }, 1000);
      }
    } else {
      this.fileUpload.uploadMsgText = "Upload thất bại!";
      this.fileUpload.uploadMsgClass = "error";
    }
  }

  save(): void {
    this.saving = true;

    const data = { ...this.contractTemplate };
    if (!this.contractTemplate.id) {
      this._contractTemplateService.create(data).subscribe(
          (response) => {
              if(this.handleResponseInterceptor(response, 'Thêm thành công')) {
                this.bsModalRef.hide();
                this.onSave.emit();
              } else {
                this.saving = false;
                this.callTriggerFieldError(response);
              }
          },() => {
              this.saving = false;
          }
      );
    }
    else {
      this._contractTemplateService.update(data, this.contractTemplate.id).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
              this.bsModalRef.hide();
              this.onSave.emit();
            } else {
              this.saving = false;
              this.callTriggerFieldError(response);
            }
          },() => {
              this.saving = false;
          }
      );
    }
  }
}





