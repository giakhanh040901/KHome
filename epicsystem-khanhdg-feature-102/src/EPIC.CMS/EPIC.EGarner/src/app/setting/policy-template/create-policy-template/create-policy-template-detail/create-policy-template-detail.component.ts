import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { PolicyTempConst, PolicyDetailTemplateConst, MessageErrorConst } from '@shared/AppConsts';
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
  PolicyTempConst = PolicyTempConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;
  MessageErrorConst= MessageErrorConst;
  row: any;
  col: any;

  policyDetail: any = {
    'policyTempId': 0,
    'fakeId': 0,
    'sortOrder': null, //Số thứ tự
    'shortName': null, //Tên viết tắt
    'name': null,  //Tên kỳ hạn (Tên sản phẩm)
    'periodQuantity': null,   // Số kỳ đáo hạn
    'periodType': null,   // Đơn vị (D:ngày, M: tháng, Y: năm)
    'profit': null,   // Lợi nhuận
    'interestDays': null,  // Số ngày
    'interestPeriodQuantity': null,
    'tradingProviderId' : 0,
    'isShowApp' : null,
    'status' : null,
    // "interestType": 0,
    "interestPeriodType": null,
    // "repeatFixedDate": 0,

  }

  policyTemplateType: any;
  policyTempId: number;

  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;

  cols: any[];
  statuses: any[];

  listRepeatFixedDate = [];

  ngOnInit(): void {


    console.log('this.configDialog?.data: ', this.configDialog?.data);
    

    this.keyToast = 'policyTempDetail';

    this.listRepeatFixedDate = [...PolicyTempConst.getListRepeatFixedDate()];
    //
    this.policyDetail.sortOrder = this.configDialog?.data?.sortOrder;
    this.policyTempId = this.configDialog?.data?.policyTempId;
    // if(this.configDialog?.data?.policyTemplateType == PolicyTempConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY) {
      this.policyTemplateType = this.configDialog?.data?.policyTemplateType;
    // }

    if (this.policyTemplateType == PolicyTempConst.GARNERTYPE_KC_KY_HAN && this.configDialog?.data?.interestType) {
      this.policyDetail.interestType = this.configDialog?.data?.interestType;
    }

    if (this.policyTemplateType == PolicyTempConst.GARNERTYPE_KC_KY_HAN && this.configDialog?.data?.repeatFixedDate) {
      this.policyDetail.repeatFixedDate = this.configDialog?.data?.repeatFixedDate;
    }

    if(this.configDialog?.data?.policyDetail) {
      this.policyDetail = {...this.configDialog.data.policyDetail};
    }
  }

  close() {
    this.ref.close();
  }

  changeInterestType(interestType?: number) {
    // KIỂU TRẢ ĐỊNH KỲ CẦN NHẬP SỐ KỲ VÀ ĐƠN VỊ
    interestType = interestType || this.policyDetail.interestType;
    //
    if(interestType != PolicyTempConst.INTEREST_TYPE_DINH_KY) {
      this.policyDetail.interestPeriodQuantity = null;
      this.policyDetail.interestPeriodType = null;
    }
    //
    if(interestType != PolicyTempConst.INTEREST_TYPE_NGAY_CO_DINH) {
      this.policyDetail.repeatFixedDate = null;
    } 
    else if(interestType == PolicyTempConst.INTEREST_TYPE_NGAY_CO_DINH) {
      this.policyDetail.repeatFixedDate = PolicyTempConst.REPEAT_FIXED_DATE_DEFAULT;
    }
  }


  save() {
    console.log("this.policyTempId save",this?.policyTempId);
    if(this.validForm()) {
      if(this?.policyTempId) {
        this.onSave(); // Gọi Api lưu
      } else {
        this.saveTemporary(); // Chưa gọi Api chỉ lưu dữ liệu trên frontend
      }
    } else {
			this.messageError(MessageErrorConst.message.Validate);
		}
  }

  // Lưu vào db call Api khi chỉnh sửa Chính sách
  onSave() {
    this.submitted = true;
    this.policyDetail.policyTempId = this.policyTempId;
    
    if (this.policyDetail.fakeId != 0) {
      this._policyTemplateService.updatePolicyDetail(this.policyDetail).subscribe(
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
            this.ref.close(this.policyDetail);
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
    this.messageSuccess('Thêm thành công');
    if(!this.policyDetail?.fakeId) this.policyDetail.fakeId = new Date().getTime();
    this.ref.close(this.policyDetail);
  }

  validForm(): boolean {
    const validRequired = this.policyDetail?.shortName
                          && this.policyDetail?.name
                          && this.policyDetail?.sortOrder
                          && this.policyDetail?.profit
                          && this.policyDetail?.periodQuantity
                          && this.policyDetail?.periodType;
    //                       && this.policyDetail?.periodType
    //                       && ((this.policyDetail?.interesortOrderype == PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIODIC 
    //                       && (this.policyDetail?.interestPeriodQuantity || this.policyDetail?.fixedPaymentDate) 
                          
    //                       || this.policyDetail?.interesortOrderype == PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END);
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }
}
