import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { ContractFormConst, PolicyDetailTemplateConst, PolicyTempConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { ContractFormService } from '@shared/services/contract-form.service';
import { Page } from '@shared/model/page';
import { ConfigContractCodeDetail, IContractCodeStructure } from '@shared/interfaces/contractCodeStructure.interface';

@Component({
  selector: 'app-create-contract-code',
  templateUrl: './create-contract-code.component.html',
  styleUrls: ['./create-contract-code.component.scss'],
})
export class CreateContractCodeComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _contractFormService: ContractFormService,
    private dialogService: DialogService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    public confirmationService: ConfirmationService,
  ) {
    super(injector, messageService);
  }

  index = 0;
  YesNoConst = YesNoConst;
  ContractFormConst = ContractFormConst;
  contractForm: IContractCodeStructure = {};
  policyTempId: number;
  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;
  isContractTemp: boolean;
  page = new Page();
  offset = 0;

  isCollapse = false;
  configContractCodeDetails: ConfigContractCodeDetail[] = [];

  ngOnInit(): void {
    
    if( this.configDialog?.data?.contractCodeStructure) {
      this.contractForm = this.configDialog?.data?.contractCodeStructure;
      this.configContractCodeDetails = this.contractForm?.configContractCodeDetails
    }
    this.genContractCodeStructure();
  }

  genContractCodeStructure() {
    this.contractForm.contractCodeStructure = '';
    this.configContractCodeDetails.forEach(element => {
        this.contractForm.contractCodeStructure += '<'+ (element?.value ? element.value : element?.key) +'>';
    });
  }

  addValue() {
    this.index = this.index + 1;
    this.configContractCodeDetails.push({sortOrder: this.index, id: 0});
  }

  removeElement(index) {
    this.confirmationService.confirm({
      message: 'Xóa giá trị này?',
      acceptLabel: 'Đồng ý',
      rejectLabel: 'Hủy',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.configContractCodeDetails.splice(index, 1);
        this.genContractCodeStructure();
      }
    });
  }

  save() {
    let index = 1;
    this.configContractCodeDetails = this.configContractCodeDetails.map(configContractCodeDetail => {
      configContractCodeDetail.sortOrder = index++;
      return configContractCodeDetail;
    });

    this.contractForm.configContractCodeDetails = this.configContractCodeDetails;
    this.submitted = true;

    if (this.contractForm.id) {
      let body = {...this.contractForm};
      
      this._contractFormService.update(body).subscribe((response) => {
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công!')) {
            this.ref.close(true);
            this.submitted = false;
          } else {
            this.submitted = false;
          }
        }, (err) => {
          console.log('err----', err);
          this.submitted = false;
        }
      );
    } else {
      this._contractFormService.create(this.contractForm).subscribe((response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công!')) {
            this.ref.close(true);
            this.submitted = false;
          } else {
            this.submitted = false;
          }
        }, (err) => {
          console.log('err----', err);
          this.submitted = false;
        }
      );
    }
    //
  }

  close() {
    this.ref.close();
  }
}


