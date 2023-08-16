import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { ActiveDeactiveConst, MessageErrorConst, PolicyDetailTemplateConst, PolicyTempConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { CreatePolicyTemplateDetailComponent } from './create-policy-template-detail/create-policy-template-detail.component';
import { CreatePolicyContractTemplateComponent } from './create-policy-contract-template/create-policy-contract-template.component';
import { Page } from '@shared/model/page';
import { FieldErrors } from '@shared/model/field-errors.model';
import { Policy, RepeatFixedDate } from '@shared/model/policy-template.model';
import { IBaseListAction } from '@shared/interfaces/base.interface';

@Component({
  selector: 'app-create-policy-template',
  templateUrl: './create-policy-template.component.html',
  styleUrls: ['./create-policy-template.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService]
})
export class CreatePolicyTemplateComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _policyTemplateService: PolicyTemplateService,
    private dialogService: DialogService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    public confirmationService: ConfirmationService,
  ) {
    super(injector, messageService);
  }

  fieldErrors: FieldErrors = {};
  isCreateDetail: boolean;
  isCreateContractTemp: boolean;
  YesNoConst = YesNoConst;
  PolicyTempConst = PolicyTempConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;
  ActiveDeactiveConst = ActiveDeactiveConst;
  MessageErrorConst = MessageErrorConst;
  policyTemplate: Policy  = new Policy();
  policyTempId: number;
  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;
  isContractTemp: boolean;
  listActionPolicyDetail: IBaseListAction[] = [];
  page = new Page();
  offset = 0;
  listRepeatFixedDate: RepeatFixedDate[]= [];
  isCollapse = false;

  ngOnInit(): void {
    this.keyToast = 'policyTemp';
    this.listRepeatFixedDate = [...PolicyTempConst.getListRepeatFixedDate()];
    //
    this.policyTempId = this.configDialog?.data?.policyTempId;
    this.isContractTemp = this.configDialog?.data?.isContractTemp;
    console.log("this.policyTempId ", this.policyTempId);

    this.isCreateDetail = this.configDialog?.data?.isCreateDetail;
    this.isCreateContractTemp = this.configDialog?.data?.isCreateContractTemp;

    if (this.policyTempId) {
      this.getDetail(this.policyTempId);
    }
  }

  collapse() {
    this.isCollapse = !this.isCollapse;
  }

  getDetail(policyTempId) {
    if (policyTempId != null) {
      this._policyTemplateService.get(policyTempId).subscribe((res) => {
          if (this.handleResponseInterceptor(res, '')) {
            this.policyTemplate = res?.data;
            //
            this.genlistActionPolicyDetail(this.policyTemplate?.policyDetails);
            
            // THÊM KỲ HẠN KHI GỌI TỪ DANH SÁCH POLICY 
            if (this.isCreateDetail) {
              this.isCollapse = true;
              this.isCreateDetail = false;
            }
            // THÊM HĐ KHI GỌI TỪ DANH SÁCH POLICY
            if(this.isCreateContractTemp) {
              this.isCollapse = true;
              this.isCreateContractTemp = false;
              this.activeIndex = PolicyTempConst.CONTRACT_TAB;
            }
          }
          this.submitted = false;
        });
    } 

  }

  genlistActionPolicyDetail(data = []) {
    this.listActionPolicyDetail = data.map((policyDetail, index) => {
      const actions = [];

      if (this.isGranted([this.PermissionGarnerConst.GarnerCSM_KyHan_CapNhat])) {
        actions.push({
          data: policyDetail,
          label: 'Sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            console.log("$event.item.data", $event.item.data);
            this.editPolicyDetail($event.item.data);
          }
        })
      }

      if (this.isGranted([this.PermissionGarnerConst.GarnerCSM_KyHan_Xoa])) {
        actions.push({
          data: policyDetail,
          index: index,
          label: 'Xoá',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.deletePolicyDetail($event.item.data, $event.item.index);
          }
        })
      }
      return actions;
    });
    console.log('listActions', this.listActionPolicyDetail);
  }

  createPolicyDetail() {
    const ref = this.dialogService.open(
      CreatePolicyTemplateDetailComponent,
      {
        header: 'Thêm kỳ hạn',
        width: '700px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
        baseZIndex: 10000,
        data: {
          policyTempId: this.policyTemplate.id,
          policyTemplateType: this.policyTemplate.garnerType,
          interestType: this.policyTemplate.interestType,
          repeatFixedDate: this.policyTemplate.repeatFixedDate
        }
      }
    );
    //
    ref.onClose.subscribe((policyDetail) => {
      if(policyDetail) {
        if (!this.policyTemplate?.id) {
          // THÊM MỚI LƯU Ở FRONTEND
            this.policyTemplate.policyDetails.push(policyDetail);
            this.genlistActionPolicyDetail(this.policyTemplate.policyDetails);
        }
        // THÊM MỚI LƯU BACKEND
        else {
          this._policyTemplateService.getPolicyDetails(this.policyTemplate.id).subscribe((res) => {
            this.policyTemplate.policyDetails = [...res?.data];
            this.genlistActionPolicyDetail(this.policyTemplate.policyDetails);
          });
        }
        this.messageSuccess('Thêm thành công');
      }
    });
  }

  editPolicyDetail(policyDetail) {
    const ref = this.dialogService.open(
      CreatePolicyTemplateDetailComponent,
      {
        header: 'Sửa kỳ hạn',
        width: '700px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
        baseZIndex: 10000,
        data: {
          policyDetail: policyDetail,
          policyTempId: this.policyTemplate.id,
          policyTemplateType: this.policyTemplate.garnerType,
        },
      }
    );
    //
    ref.onClose.subscribe((policyDetail) => {
      if(policyDetail) {
        if (!this.policyTemplate?.id) {
          const index = this.policyTemplate.policyDetails.findIndex(item => item.fakeId == policyDetail.fakeId);
          if (index !== -1) this.policyTemplate.policyDetails[index] = policyDetail;
        } else {
          this._policyTemplateService.getPolicyDetails(this.policyTemplate.id).subscribe((res) => {
            this.policyTemplate.policyDetails = [...res?.data];
            this.genlistActionPolicyDetail(this.policyTemplate.policyDetails);
          });
        }
        this.messageSuccess('Cập nhật thành công');
      }
    });
  }

  deletePolicyDetail(policyDetail, index) {
    console.log("policyDetail delete", policyDetail);

    this.confirmationService.confirm({
      message: `Bạn có chắc chắn muốn xóa kỳ hạn ${policyDetail.name}`,
      header: 'Xóa kỳ hạn',
      icon: 'pi pi-info-circle',
      acceptLabel: 'Đồng ý',
      rejectLabel: 'Hủy bỏ',
      accept: () => {
        if (policyDetail.id) {
          // Call Api
          this._policyTemplateService.deletePolicyDetail(policyDetail.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                // this.getDetail(policyDetail.id);
                // this.policyTemplate = {};
                if (policyDetail.policyTempId != null) {
                  this.getDetail(this.policyTemplate.id);
                }
              }
            }, () => {
              this.messageService.add({
                severity: 'error',
                summary: '',
                detail: `Không xóa được chính sách ${this.policyTemplate.name}`,
                life: 3000,
              });
            }
          );
        } else {
          this.policyTemplate.policyDetails.splice(index, 1);
          this.genlistActionPolicyDetail(this.policyTemplate.policyDetails);
          // this.genListActionPolicyContractTemp(this.policyTemplate.contractTemplateTemps);
        }
        this.genlistActionPolicyDetail(this.policyTemplate.policyDetails);
        // this.genListActionPolicyContractTemp(this.policyTemplate?.contractTemplateTemps);
        this.messageService.add({ severity: 'success', detail: 'Xóa thành công !' });
        console.log({ policyDetails: this.policyTemplate.policyDetails });

      }
    });
  }

  changeInterestType(interestType?: number) {
    // KIỂU TRẢ ĐỊNH KỲ CẦN NHẬP SỐ KỲ VÀ ĐƠN VỊ
    interestType = interestType || this.policyTemplate.interestType;
    //
    if(interestType != PolicyTempConst.INTEREST_TYPE_DINH_KY) {
      this.policyTemplate.interestPeriodQuantity = null;
      this.policyTemplate.interestPeriodType = null;
    }
    //
    if(interestType != PolicyTempConst.INTEREST_TYPE_NGAY_CO_DINH) {
      this.policyTemplate.repeatFixedDate = null;
    } 
    else if(interestType == PolicyTempConst.INTEREST_TYPE_NGAY_CO_DINH) {
      this.policyTemplate.repeatFixedDate = PolicyTempConst.REPEAT_FIXED_DATE_DEFAULT;
    }
  }

  save() {
    if(this.validForm()) {

      this.submitted = true;
      if (this.policyTemplate.id) {
        let body = {...this.policyTemplate};
        delete  body.contractTemplateTemps;
        delete  body.policyDetails;
        //
        this._policyTemplateService.update(body, this.policyTemplate.id).subscribe((response) => {
            if (this.handleResponseInterceptor(response)) {
              this.ref.close({ data: response, accept: true });
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
        this._policyTemplateService.create(this.policyTemplate).subscribe((response) => {
            if (this.handleResponseInterceptor(response)) {
              this.ref.close({ data: response, accept: true });
              this.submitted = false;
            } else {
              this.submitted = false;
            }
            // this.getDetail(this.policyTemplate.id);
          }, (err) => {
            console.log('err----', err);
            this.submitted = false;
          }
        );
      }
    } else {
			this.messageError(MessageErrorConst.message.Validate);
		}
  }

  close() {
    this.ref.close();
  }

  changeStatusPolicyDetail(item) {
    console.log("item ....", item);

    this._policyTemplateService.changeStatusPolicyDetail(item.id).subscribe(
      (response) => {
        var message = "";
        if (item.status == 'A') {
          message = "Hủy kích hoạt thành công";
          this.getDetail(this.policyTemplate.id);
        } else {
          message = "Kích hoạt thành công";
          this.getDetail(this.policyTemplate.id);
        }
        if (this.handleResponseInterceptor(response, message)) {
          this.getDetail(this.policyTemplate.id);
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không thay đổi được trạng thái của kỳ hạn ${item.name}`,
          life: 3000,
        });
      }
    );
  }

  validForm(): boolean {
    const validRequired =
      !!this.policyTemplate?.code &&
      !!this.policyTemplate?.name &&
      this.policyTemplate?.withdrawFeeType !== null &&
      this.policyTemplate?.incomeTax !== null &&
      !!this.policyTemplate?.minMoney &&
      !!this.policyTemplate?.minWithdraw;
  
    return validRequired;
  }

  changeTabview(indexTab) {
    console.log({ indexTab: indexTab });
  }

}
