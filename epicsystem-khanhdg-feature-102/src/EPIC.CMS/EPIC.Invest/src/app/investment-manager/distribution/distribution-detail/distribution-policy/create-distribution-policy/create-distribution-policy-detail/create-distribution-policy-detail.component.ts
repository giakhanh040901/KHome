import { DistributionService } from '@shared/services/distribution.service';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { PolicyTemplateConst, PolicyDetailTemplateConst } from '@shared/AppConsts';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { OJBECT_DISTRIBUTION_CONST } from '@shared/base-object';
import { ActivatedRoute } from '@angular/router';

const { BASE } = OJBECT_DISTRIBUTION_CONST;

@Component({
  selector: 'app-create-distribution-policy-detail',
  templateUrl: './create-distribution-policy-detail.component.html',
  styleUrls: ['./create-distribution-policy-detail.component.scss']
})
export class CreateDistributionPolicyDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef,
    private _distributionService: DistributionService,
    public configDialog: DynamicDialogConfig,
    private routeActive: ActivatedRoute,
    ) {
    super(injector, messageService);
    this.distributionId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  distributionId: number;
  policyId: number;

  policyTemp: any[] = [];
  policyDetailTemps: any[] = [];

  fieldErrors: any = {};

  // Data Init
  PolicyTemplateConst = PolicyTemplateConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;

  policyDetail: any = {...BASE.POLICY_DETAIL}
  policyTemplateType: any;

  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;

  ngOnInit(): void {
    // GET DATA CHINH SACH MAU
    this.isLoadingPage = true;
    this._distributionService.getAllPolicyTempNoPermission().subscribe((response) => {
      if (this.handleResponseInterceptor(response, '')) {
        this.policyTemp = response?.data?.items;
        
        console.log("this.policyTemp",this.policyTemp);

        if(this.policyTemplateType != null) {
          this.policyTemp = this.policyTemp.filter(item => item.type == 4);
        } else {
          this.policyTemp = this.policyTemp.filter(item => item.type != 4);
        }

      }
      this.isLoadingPage = false;
    }, (err) => {
      console.log('err----', err);
      this.isLoadingPage = false;
    });
    // SET DATA ĐẨY SANG TỪ DYNAMIC DIALOG
    
    this.policyId = this.configDialog?.data?.policyId;
    if(this.configDialog?.data?.policyTemplateType == PolicyTemplateConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY) {
      this.policyTemplateType = this.configDialog?.data?.policyTemplateType;
    }
    this.distributionId = this.configDialog?.data?.distributionId;
    if(this.configDialog?.data?.policyDetail) {
      this.policyDetail = this.configDialog.data.policyDetail;
    }
    console.log('policyDetail', this.policyDetail);
  }

  // CHỌN CHÍNH SÁCH MẪU
  changePolicyTemp(policyTemp) {
    console.log('policyTemp', policyTemp);
    this.policyDetailTemps = policyTemp?.policyDetailTemps ?? [];
    if(this.policyDetailTemps?.length) {
      this.setDataPolicyDetail(this.policyDetailTemps[0]);
    } 
  }

  // SET DATA KỲ HẠN VỚI DỮ LIỆU CỦA KỲ HẠN MẪU 
  setDataPolicyDetail(policyDetailTemp) {
    this.policyDetail = {...policyDetailTemp};
    delete this.policyDetail.id;
  }

  // CHỌN KỲ HẠN MẪU
  changePolicyDetailTemp(policyDetailTemp) {
    this.setDataPolicyDetail(policyDetailTemp);
  }

  // CHANGE KIEU KY HAN
  changeInterestType(value) {
    if (value == this.PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END) {
      this.policyDetail.interestPeriodType = null;
      this.policyDetail.interestPeriodQuantity = null;
    } else {
      this.policyDetail.interestPeriodType = this.PolicyDetailTemplateConst.INTEREST_PERIOD_TYPE_MONTH;
    }
  }

  save() {
    this.policyDetail.distributionId = this.distributionId;
    // this.submitted = true;
    if (this.policyDetail?.id) {
      this._distributionService.updatePolicyDetail(this.policyDetail).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
            this.ref.close({data: this.policyDetail, accept: true});
          } 
            this.submitted = false;
        }, (err) => {
          console.log('errUpdate---', err);
          this.submitted = false;
        }
      );
    } else {
      this.policyDetail.distributionId = this.distributionId;
      this.policyDetail.policyId = this.policyId;
      if (this.policyTemplateType != null) {
        this.policyDetail.interestPeriodQuantity = null;
      }
      this._distributionService.addPolicyDetail(this.policyDetail).subscribe((response) => {
          if (this.handleResponseInterceptor(response, 'Thêm kỳ hạn thành công')) {
            this.submitted = false;
            this.ref.close({data: this.policyDetail, accept: true});
          } 
        }, (err) => {
          console.log('errCreate---', err, this.policyDetail);
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
                          && this.policyDetail?.stt
                          && this.policyDetail?.profit
                          && this.policyDetail?.periodQuantity
                          && this.policyDetail?.periodType
                          && ((this.policyDetail?.interestType == PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIODIC 
                            && (this.policyDetail?.interestPeriodQuantity || this.policyDetail?.fixedPaymentDate) 
                            && this.policyDetail?.interestPeriodType) 
                            || this.policyDetail?.interestType == PolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END);
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }
}
