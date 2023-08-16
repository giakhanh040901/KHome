import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { PolicyTempConst, PolicyDetailTemplateConst } from '@shared/AppConsts';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { DistributionService } from '@shared/services/distribution.service';

@Component({
  selector: 'app-create-policy-contract-template',
  templateUrl: './create-policy-contract-template.component.html',
  styleUrls: ['./create-policy-contract-template.component.scss'],
})
export class CreatePolicyContractTemplateComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef,
    private _policyTemplateService: PolicyTemplateService,
    public configDialog: DynamicDialogConfig,
    private _distributionService: DistributionService,
    ) {
    super(injector, messageService);
  }

  fieldErrors: any = {};
  
  // Data Init
  productBondPrimary: any = [];
  PolicyTempConst = PolicyTempConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;

  row: any;
  col: any;

  contractTemplateTemps: any = {
    'policyTempId': 0,
    'fakeId': 0,


  }
  policyTemplateType: any;
  policyTempId: number;

  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;
  cols: any[];
  statuses: any[];

  ngOnInit(): void {

    this.keyToast = 'contractPolicyTemp';

    this.policyTempId = this.configDialog?.data?.policyTempId;
    if(this.configDialog?.data?.policyTemplateType == PolicyTempConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY) {
      this.policyTemplateType = this.configDialog?.data?.policyTemplateType;
    }
    if(this.configDialog?.data?.policyDetail) {
      this.contractTemplateTemps = {...this.configDialog.data.policyDetail};
    }
    console.log("this.policyDetail oninit",this.policyTempId);
    console.log("this.policyDetail oninit2",this.contractTemplateTemps);
    console.log("this.policyDetail oninit3",this.policyTemplateType);
  }

  myUploader(event) {
		if (event?.files[0]) {
			this._distributionService.uploadFileGetUrl(event?.files[0], "policy-contract-template").subscribe((response) => {
          if(this.handleResponseInterceptor(response, "Upload file thành công")) {
            this.contractTemplateTemps.contractTempUrl = response.data;
          }
				},(err) => {
					this.messageError("Có sự cố khi upload!");
				}
			);
		}
	}

  close() {
    this.ref.close();
  }

  save() {
    if(this?.policyTempId) {
      this.onSave(); // Gọi Api lưu
    } else {
      this.saveTemporary(); // Chưa gọi Api chỉ lưu dữ liệu trên frontend
    }
  }

  // Lưu vào db call Api khi chỉnh sửa Chính sách
  onSave() {
    this.submitted = true;
    this.contractTemplateTemps.policyTempId = this.policyTempId;
    
    if (this.contractTemplateTemps.fakeId != 0) {
      this._policyTemplateService.updatePolicyContractTemp(this.contractTemplateTemps).subscribe((response) => {        
          if (this.handleResponseInterceptor(response, 'Cập nhật kỳ hạn thành công')) {
            this.ref.close(this.contractTemplateTemps);
          } 
          this.submitted = false;
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    } else {
      this._policyTemplateService.createPolicyContractTemp(this.contractTemplateTemps).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.ref.close(this.contractTemplateTemps);
          } 
          this.submitted = false;
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    }
  }

  // Lưu trên frontend khi thêm mới Chính sách
  saveTemporary() {
    if(!this.contractTemplateTemps?.fakeId)this.contractTemplateTemps.fakeId = new Date().getTime();
    this.ref.close(this.contractTemplateTemps);
  }

  validForm(): boolean {
    const validRequired = 
                         this.contractTemplateTemps?.name;
                          
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }
}
