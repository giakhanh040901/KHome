import { Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { PolicyDetailTemplateConst, PolicyTempConst, } from "@shared/AppConsts";
import { OJBECT_DISTRIBUTION_CONST } from "@shared/base-object";
import { CrudComponentBase } from "@shared/crud-component-base";
import { DistributionService } from "@shared/services/distribution.service";
import { MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";

const { BASE } = OJBECT_DISTRIBUTION_CONST;

@Component({
  selector: "app-create-distribution-policy-detail",
  templateUrl: "./create-distribution-policy-detail.component.html",
  styleUrls: ["./create-distribution-policy-detail.component.scss"],
})
export class CreateDistributionPolicyDetailComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef,
    public configDialog: DynamicDialogConfig,
    private routeActive: ActivatedRoute,
    private _distributionService: DistributionService
  ) {
    super(injector, messageService);
  }

  distributionId: number;
  policyId: number;

  policyTemp: any[] = [];
  policyDetails: any[] = [];

  fieldErrors: any = {};


  // Data Init
  PolicyTempConst = PolicyTempConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;

  policyDetail: any = { 
    'policyId': null,
    'fakeId': null,
    // 'sortOrder': null, //Số thứ tự
    'shortName': null, //Tên viết tắt
    'name': null,  //Tên kỳ hạn (Tên sản phẩm)
    'periodQuantity': null,   // Số kỳ đáo hạn
    'periodType': null,   // Đơn vị (D:ngày, M: tháng, Y: năm)
    'profit': null,   // Lợi nhuận
    'interestDays': null,  // Số ngày
    'interestPeriodQuantity': null,
    'tradingProviderId' : null,
    'isShowApp' : null,
    'status' : null,
    "interestType": null,
    "interestPeriodType": null,
    "repeatFixedDate": null,
  };

  policyTemplateType: any;

  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;

  cols: any[];
  statuses: any[];

  listRepeatFixedDate = [];

  ngOnInit(): void {
    this.keyToast = 'policyDetail';
    //
    this.listRepeatFixedDate = PolicyTempConst.getListRepeatFixedDate();
    // GET DATA CHINH SACH MAU
    this.isLoadingPage = true;

    this._distributionService.getAllPolicyTemp().subscribe((response) => {
        if (this.handleResponseInterceptor(response, "")) {
          this.policyTemp = response?.data?.items;

          console.log("this.policyTemp", this.policyTemp);

          if (this.policyTemplateType != null) {
            this.policyTemp = this.policyTemp.filter((item) => item.type == 4);
          } else {
            this.policyTemp = this.policyTemp.filter((item) => item.type != 4);
          }
        }
        this.isLoadingPage = false;
      },
      (err) => {
        console.log("err----", err);
        this.isLoadingPage = false;
      }
    );
    // SET DATA ĐẨY SANG TỪ DYNAMIC DIALOG
    this.policyId = this.configDialog?.data?.policyId;
    // if ( this.configDialog?.data?.policyTemplateType == PolicyTempConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY ) {
      this.policyTemplateType = this.configDialog?.data?.policyTemplateType;
    // }
    this.distributionId = this.configDialog?.data?.distributionId;
    if (this.configDialog?.data?.policyDetail) {
      this.policyDetail = this.configDialog.data.policyDetail;
    }
    console.log("policyDetail", this.policyDetail);
  }

  changeInteresortOrderype(value) {
    if (value == this.PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END) {
      this.policyDetail.interestPeriodType = null;
      this.policyDetail.interestPeriodQuantity = null;
    } else {
      this.policyDetail.interestPeriodType = this.PolicyDetailTemplateConst.INTEREST_PERIOD_TYPE_MONTH;
    }
  }

  // CHỌN CHÍNH SÁCH MẪU
  changePolicyTemp(policyTemp) {
    console.log("policyTemp", policyTemp);
    this.policyDetails = policyTemp?.policyDetails ?? [];
    if (this.policyDetails.length) {
      this.setDataPolicyDetail(this.policyDetails[0]);
    }
  }
  

  // SET DATA KỲ HẠN VỚI DỮ LIỆU CỦA KỲ HẠN MẪU
  setDataPolicyDetail(policyDetailTemp) {
    this.policyDetail = { ...policyDetailTemp };
    delete this.policyDetail.id;
  }

  // CHỌN KỲ HẠN MẪU
  changePolicyDetailTemp(policyDetailTemp) {
    this.setDataPolicyDetail(policyDetailTemp);
  }

  // CHANGE KIEU KY HAN
  // changeInterestType(value) {
  //   if (value == this.PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END) {
  //     this.policyDetail.interestPeriodType = null;
  //     this.policyDetail.interestPeriodQuantity = null;
  //   } else {
  //     this.policyDetail.interestPeriodType =
  //       this.PolicyDetailTemplateConst.INTEREST_PERIOD_TYPE_MONTH;
  //   }
  // }

  changeInterestType(interestType?: number) {
    // KIỂU TRẢ ĐỊNH KỲ CẦN NHẬP SỐ KỲ VÀ ĐƠN VỊ
    interestType = interestType || this.policyDetail.interestType;
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
    this.policyDetail.distributionId = this.distributionId;
    // this.submitted = true;
    if (this.policyDetail?.id) {
      this._distributionService.updatePolicyDetail(this.policyDetail).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
            this.ref.close(true);
          }
          this.submitted = false;
        },(err) => {
          console.log("errUpdate---", err);
          this.submitted = false;
        }
      );
    } else {
      this.policyDetail.distributionId = this.distributionId;
      this.policyDetail.policyId = this.policyId;
      //
      this._distributionService.addPolicyDetail(this.policyDetail).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Thêm kỳ hạn thành công")) {
            this.ref.close(true);
          }
          this.submitted = false;
        },(err) => {
          console.log("errCreate---", err, this.policyDetail);
          this.submitted = false;
        }
      );
    }
  }

  close() {
    this.ref.close();
  }

  validForm(): boolean {
    const validRequired = this.policyDetail?.shortName 
                          && this.policyDetail?.name 
                          && this.policyDetail?.profit 
                          && this.policyDetail?.periodQuantity 
                          && this.policyDetail?.periodType;
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }
}
