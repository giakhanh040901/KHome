import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { PolicyTemplateConst, PolicyDetailTemplateConst, MessageErrorConst } from '@shared/AppConsts';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';

@Component({
  selector: 'app-create-policy-template-detail',
  templateUrl: './create-policy-template-detail.component.html',
  styleUrls: ['./create-policy-template-detail.component.scss']
})
export class CreatePolicyTemplateDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef,
    private _policyTemplateService: PolicyTemplateService,
    public configDialog: DynamicDialogConfig,
    ) {
    super(injector, messageService);
  }

  fieldErrors: any = {};
  
  // Data Init
  productBondPrimary: any = [];
  PolicyTemplateConst = PolicyTemplateConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;

  row: any;
  col: any;

  policyDetail: any = {
    'policyTempId': null,
    'fakeId': 0,
    'stt': null, //Số thứ tự
    'shortName': null, //Tên viết tắt
    'name': null,  //Tên kỳ hạn (Tên sản phẩm)
    'periodQuantity': null,   // Số kỳ đáo hạn
    'periodType': null,   // Đơn vị (D:ngày, M: tháng, Y: năm)
    'profit': null,   // Lợi nhuận
    'interestDays': null,  // Số ngày
    'interestPeriodQuantity': null,
    'fixedPaymentDate': null
  }
  policyTemplateType: any;
  policyTempId: number;

  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;
  cols: any[];
  listFixedPaymentDate = [];
  statuses: any[];

  ngOnInit(): void {
    this.listFixedPaymentDate = [...PolicyDetailTemplateConst.getListFixedPaymentDate()];
    this.policyTempId = this.configDialog?.data?.policyTempId;
    if(this.configDialog?.data?.policyTemplateType == PolicyTemplateConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY) {
      this.policyTemplateType = this.configDialog?.data?.policyTemplateType;
    }
    if(this.configDialog?.data?.policyDetail) {
      this.policyDetail = this.configDialog.data.policyDetail;
    }
  }

  changeInterestType(interestType) {
    if(interestType != PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIODIC) {
      this.policyDetail.interestPeriodQuantity = null;
      this.policyDetail.interestPeriodType = null;
    }
    //
    if(interestType != PolicyDetailTemplateConst.NGAY_CO_DINH) {
      this.policyDetail.fixedPaymentDate = null;
    } 
    else if(interestType == PolicyDetailTemplateConst.NGAY_CO_DINH) {
      this.policyDetail.fixedPaymentDate = PolicyDetailTemplateConst.FIX_PAYMENT_DATE_DEFAULT;
    }
  }

  close() {
    this.ref.close();
  }

  save() {
    if (this.validForm()){
      if(this?.policyTempId) {
        this.onSave(); // Gọi Api lưu
      } else {
        this.saveTemporary(); // Chưa gọi Api chỉ lưu dữ liệu trên frontend
      }
    } else {
      this.messageError(MessageErrorConst.message.Validate)
    }

  }

  // Lưu vào db call Api khi chỉnh sửa Chính sách
  onSave() {
    this.submitted = true;
    this.policyDetail.policyTempId = this.policyTempId;
    
    if (this.policyDetail.id != null) {
      this._policyTemplateService.updatePolicyDetail(this.policyDetail, this.policyDetail.id).subscribe(
        (response) => {        
          if (this.handleResponseInterceptor(response, 'Cập nhật kỳ hạn thành công')) {
            this.ref.close(this.policyDetail);
          } 
            this.submitted = false;
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    } else {
      this._policyTemplateService.createPolicyDetail(this.policyDetail).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm kỳ hạn thành công')) {
            this.submitted = false;
            this.ref.close(this.policyDetail);
          } 
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    }
  }

  // Lưu trên frontend khi thêm mới Chính sách
  saveTemporary() {
    if(!this.policyDetail?.fakeId)this.policyDetail.fakeId = new Date().getTime();
    this.ref.close(this.policyDetail);
  }

  validForm(): boolean {
    const validRequired = this.policyDetail?.shortName
                          && this.policyDetail?.name
                          && this.policyDetail?.stt
                          && this.policyDetail?.profit
                          && this.policyDetail?.periodQuantity
                          && this.policyDetail?.periodType
                          && ((this.policyDetail?.interestType == PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIODIC && this.policyDetail?.interestPeriodQuantity && this.policyDetail?.interestPeriodType) || this.policyDetail?.interestType != PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIODIC)
                          && ((this.policyDetail?.interestType == PolicyDetailTemplateConst.NGAY_CO_DINH && this.policyDetail?.fixedPaymentDate) || this.policyDetail?.interestType != PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIODIC);
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }
}
