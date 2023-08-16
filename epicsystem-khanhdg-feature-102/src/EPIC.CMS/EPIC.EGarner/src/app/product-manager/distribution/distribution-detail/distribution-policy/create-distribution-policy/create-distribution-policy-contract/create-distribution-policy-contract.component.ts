import { Component, Injector, OnInit } from '@angular/core';
import { PolicyDetailTemplateConst, PolicyTempConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-create-distribution-policy-contract',
  templateUrl: './create-distribution-policy-contract.component.html',
  styleUrls: ['./create-distribution-policy-contract.component.scss']
})
export class CreateDistributionPolicyContractComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef,
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
  
    contractTemplate: any = {
      'distributionId': 0,
      'policyId': 0,
      'fakeId': 0,
    }

    policyType: any;
    policyId: number;
    distributionId: number; 
    blockText: RegExp = /[0-9,.]/;
    submitted: boolean;
    cols: any[];
    statuses: any[];


  ngOnInit(): void {
    this.keyToast = 'policyContract';

    console.log("Data dialog", this.configDialog?.data);
    
    this.policyId = this.configDialog?.data?.policyId;
    this.distributionId = this.configDialog?.data?.distributionId;
    this.contractTemplate.distributionId = this.distributionId;
    console.log('this.contractTemplate.distributionId ', this.contractTemplate.distributionId);
    
    if(this.configDialog?.data?.policyType == PolicyTempConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY) {
      this.policyType = this.configDialog?.data?.policyType;
    }
    if(this.configDialog?.data?.contractTemplate) {
      this.contractTemplate = this.configDialog.data.contractTemplate;
    }
  }

  myUploader(event) {
		if (event?.files[0]) {
			this._distributionService.uploadFileGetUrl(event?.files[0], "distribution/policy-contract").subscribe((response) => {
        if(this.handleResponseInterceptor(response, "Tải file thành công!")) {
          this.contractTemplate.contractTempUrl = response.data;
        }
			},(err) => {
          console.log('err__', err);
					this.messageError("Có sự cố khi upload!");
				}
			);
		}
	}

  close() {
    this.ref.close();
  }

  // Lưu vào db call Api khi chỉnh sửa Chính sách
  save() {
    this.submitted = true;
    this.contractTemplate.policyId = this.policyId;
    //    
    if (this.contractTemplate?.id) {
      this._distributionService.updatePolicyContractTemp(this.contractTemplate).subscribe((response) => {        
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
            this.ref.close(true);
          } 
          this.submitted = false;
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    } else {
      this._distributionService.createPolicyContractTemp(this.contractTemplate).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.submitted = false;
            this.ref.close(true);
          } 
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    }
  }

  validForm(): boolean {
    const validRequired = this.contractTemplate?.name;
                          
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

}
