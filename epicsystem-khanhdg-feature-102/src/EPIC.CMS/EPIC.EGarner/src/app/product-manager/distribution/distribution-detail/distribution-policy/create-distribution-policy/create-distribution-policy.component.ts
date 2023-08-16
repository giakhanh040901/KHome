import { Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ActiveDeactiveConst, AppConsts, MessageErrorConst, PolicyDetailTemplateConst, PolicyTempConst, YesNoConst, } from "@shared/AppConsts";
import { OJBECT_DISTRIBUTION_CONST } from "@shared/base-object";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { DistributionService } from "@shared/services/distribution.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { CreateDistributionPolicyContractComponent } from "./create-distribution-policy-contract/create-distribution-policy-contract.component";
import { CreateDistributionPolicyDetailComponent } from "./create-distribution-policy-detail/create-distribution-policy-detail.component";

@Component({
  selector: "app-create-distribution-policy",
  templateUrl: "./create-distribution-policy.component.html",
  styleUrls: ["./create-distribution-policy.component.scss"],
  providers: [DialogService, ConfirmationService, MessageService]
})
export class CreateDistributionPolicyComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private routeActive: ActivatedRoute,
    public configDialog: DynamicDialogConfig,
    private _distributionService: DistributionService,
    public confirmationService: ConfirmationService,
    public ref: DynamicDialogRef,

  ) {
    super(injector, messageService);
  }

  distributionId: number;

  fieldErrors: any = {};
  isCreateDetail: boolean;
  isCreateContractTemp: boolean;
  rows: any[] = [];

  ActiveDeactiveConst = ActiveDeactiveConst;
  YesNoConst = YesNoConst;
  PolicyTempConst = PolicyTempConst;
  PolicyDetailTemplateConst = PolicyDetailTemplateConst;
  MessageErrorConst = MessageErrorConst;
  row: any;
  col: any;

  policy: any = {
    'distributionId': null,
    'code': null,   // Mã chính sách
    'name': null,   // Tên chính sách
    'incomeTax': null,  // Thuế lợi nhuận
    'minMoney': null,  // Số tiền tích lũy tối thổi
    'maxMoney': null,  // Số tiền tích lũy tối thổi
    'investorType': null,   // Loại nhà đầu tư
    'minInvestDay': null, // Số ngày tích lũy tối thiểu
    'classify': null, // phân loại
    'calculateType': null,  
    'garnerType': null,
    'interestType': null,
    'interestPeriodType': null,
    'repeatFixedDate': null,
    'minWithdraw': null,    
    'maxWithdraw': null,
    'withdrawFee': null,    
    'orderOfWithdrawal': null,
    'isTransferAssets': null,
    'transferAssetsFee': null, // Thuế CN
    'withdrawFeeType': null,
    'startDate': null,
    'endDate': null,
    'sortOrder': null,
    'description': null,
   
    //'type': null,   // Kiểu chính sách
    'transferTax': null, // Thuế chuyển nhượng %
    // 'isTransfer': null, 
    'policyDetails': [],  // Kỳ hạn
    // 'exitFee': null,   
    // 'exitFeeType': null, 
    'contractTemplates' : [],
   };

  policyId: number;
  listRepeatFixedDate: any[] = [];

  blockText: RegExp = /[0-9,.]/;
  submitted: boolean;
  //
  cols: any[];

  policyTemp: any[] = [];

  listActionPolicyDetail: any[] = [];
  listActionPolicyContractTemp: any[] = [];
  //
  page = new Page();
  offset = 0;
  fieldDates = ["startDate", "endDate"];

  isCollapse: boolean = false;

  ngOnInit(): void {
    this.keyToast = 'policy';
    //
    this.listRepeatFixedDate = [...PolicyTempConst.getListRepeatFixedDate()];
    //
    this.distributionId = this.configDialog?.data?.distributionId;
    this.policyId = this.configDialog?.data?.policyId;
    this.isCreateDetail = this.configDialog?.data?.isCreateDetail;
    this.isCreateContractTemp = this.configDialog?.data?.isCreateContractTemp;
    //
    if (!this.policyId) {
      this.isLoadingPage = true;
      this._distributionService.getAllPolicyTempNoPermission().subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, "")) {
            
            this.policyTemp = response?.data;
            console.log("policyTemp", this.policyTemp);
          }
          this.isLoadingPage = false;
        },
        (err) => {
          console.log("err----", err);
          this.isLoadingPage = false;
        }
      );
    }

    //
    if (this.policyId) {
      this.getDetail(this.policyId);
    }
  }

  collapse() {
    this.isCollapse = !this.isCollapse;
  }

  getDetail(policyId) {
    this.configDialog.header = "Cập nhật chính sách";
    this.isLoadingPage = true;
    this._distributionService.getPolicyById(policyId).subscribe((res) => {
      this.isLoadingPage = false;
        if (this.handleResponseInterceptor(res, "")) {
          if(res?.data) {
            this.policy = res?.data;
            //
            this.formatCalendarDisplay(this.fieldDates, this.policy);
            //
            this.genlistActionPolicyDetail(this.policy?.policyDetails);
            // this.genlistActionPolicyContractTemp(this.policy?.contractTemplates);
          }
          //
          if (this.isCreateDetail) {
            this.isCreateDetail = false;
            this.isCollapse = true;
          }
          //
          if(this.isCreateContractTemp) {
            this.isCreateContractTemp = false;
            this.isCollapse = true;
            this.activeIndex = PolicyTempConst.CONTRACT_TAB;
          }
        }
      },(err) => {
        console.log("err---", err);
      }
    );
  }

  getAllPolicyDetail(policyId) {
    this._distributionService.getAllPolicyDetail(policyId).subscribe(
      (res) => {
        if (this.handleResponseInterceptor(res, "")) {
          this.policy.policyDetails = res?.data;
          this.genlistActionPolicyDetail(this.policy.policyDetails);
        }
      },
      (err) => {
        console.log("err----", err);
      }
    );
  }

  genlistActionPolicyDetail(data = []) {
    this.listActionPolicyDetail = data.map((policyDetail, index) => {
      const actions = [];

      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_KyHan_CapNhat])) {
        actions.push({
          data: policyDetail,
          label: "Sửa",
          icon: "pi pi-pencil",
          command: ($event) => {
            console.log("$event.item.data", $event.item.data);
            this.editPolicyDetail($event.item.data);
          },
        });
      }
      //
      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_KyHan_KichHoatOrHuy])) {
				actions.push({
					data: policyDetail,
					label: policyDetail.status == ActiveDeactiveConst.ACTIVE ? 'Khóa' : 'Kích hoạt',
					icon: policyDetail.status == ActiveDeactiveConst.ACTIVE ? 'pi pi-lock' : 'pi pi-lock-open',
					command: ($event) => {
						this.changeStatusPolicyDetail($event.item.data);
					}
				});
			}

      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_KyHan_Xoa])) {
        actions.push({
          data: policyDetail,
          index: index,
          label: "Xoá kì hạn",
          icon: "pi pi-trash",
          command: ($event) => {
            this.deletePolicyDetail($event.item.data, $event.item.index);
          },
        });
      }

      return actions;
    });
    console.log("listActions ki han", this.listActionPolicyDetail);
  }

  // THÊM MỚI KỲ HẠN
  createPolicyDetail() {
    const ref = this.dialogService.open(
      CreateDistributionPolicyDetailComponent,
      {
        header: "Thêm kỳ hạn",
        width: "800px",
        contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
        baseZIndex: 10000,
        data: {
          distributionId: this.distributionId,
          policyId: this.policy.id,
          policyType: this.policy.garnerType,
        },
      }
    );
    ref.onClose.subscribe((statusCreate) => {      
      if(statusCreate) {
        this.messageSuccess('Thêm mới thành công');
        this.getDetail(this.policy.id);
      }
    });
  }

  // CẬP NHẬT KỲ HẠN
  editPolicyDetail(policyDetail) {
    const ref = this.dialogService.open(
      CreateDistributionPolicyDetailComponent,
      {
        header: "Cập nhật kỳ hạn",
        width: "800px",
        contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
        baseZIndex: 10000,
        data: {
          distributionId: this.distributionId,
          policyDetail: { ...policyDetail },
          policyId: this.policy.id,
          policyType: this.policy.garnerType,
        },
      }
    );
    //
    ref.onClose.subscribe((statusUpdate) => {
      if(statusUpdate) {
        this.messageSuccess('Cập nhật thành công');
        this.getDetail(this.policy.id);
      }
    });
  }

  changeInterestType(interestType?: number) {
    // KIỂU TRẢ ĐỊNH KỲ CẦN NHẬP SỐ KỲ VÀ ĐƠN VỊ
    interestType = interestType || this.policy.interestType;
    if(interestType != PolicyTempConst.INTEREST_TYPE_DINH_KY) {
      this.policy.interestPeriodQuantity = null;
      this.policy.interestPeriodType = null;
    }
    //
    if(interestType != PolicyTempConst.INTEREST_TYPE_NGAY_CO_DINH) {
      this.policy.repeatFixedDate = null;
    } 
    else if(interestType == PolicyTempConst.INTEREST_TYPE_NGAY_CO_DINH) {
      this.policy.repeatFixedDate = PolicyTempConst.REPEAT_FIXED_DATE_DEFAULT;
    }
  }

  // CHON CHINH SACH MAU
  changePolicyTemp(policyTempId) {
    const policyTemp = this.policyTemp.find((item) => item.id == policyTempId);
    if (policyTemp) {
      this.policy = {
        ...policyTemp,
        policyTempId: policyTemp.id,
      };
      delete this.policy.id;
      console.log("policyTemp", this.policy);
    }
  }

  // XOA KỲ HAN
  deletePolicyDetail(policyDetail, index) {
    this.confirmationService.confirm({
      message: `Bạn có chắc chắn muốn xóa kỳ hạn "${policyDetail?.name}"`,
      header: "Xóa kỳ hạn",
      icon: "pi pi-question-circle",
      acceptLabel: "Đồng ý",
      rejectLabel: "Hủy bỏ",
      accept: () => {
        this._distributionService.deletePolicyDetail(policyDetail.id).subscribe((res) => {
          if (this.handleResponseInterceptor(res, "Xóa thành công!")) {
            this.getDetail(policyDetail?.policyId);
          }
        },(err) => {
            console.log("err----", err);
          }
        );
      },
    });
  }

  //
  save() {
    if(this.validForm()) { 

      if (!this.policy.distributionId) this.policy.distributionId = this.distributionId;
      let body = this.formatCalendar(this.fieldDates, { ...this.policy });
      // XÓA DỮ LIỆU THỪA
          delete body.policyDetails;
          delete body.contractTemplates;
          delete body.distribution;
      //
      this.submitted = true;
      if (this.policy.id) {
        this._distributionService.updatePolicy(body).subscribe((response) => {
            if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
              this.ref.close(true);
            }
            this.submitted = false;
            //
          },(err) => {
            console.log("err---", err);
            this.submitted = false;
          }
        );
      } else {
        this._distributionService.addPolicy(body).subscribe((response) => {
            if (this.handleResponseInterceptor(response, "Thêm thành công")) {
              this.getDetail(response?.data?.id);
            }
            this.submitted = false;
          }, (err) => {
            console.log("err----", err);
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

  changeStatusPolicyDetail(policyDetail) {
    this._distributionService.changeStatusPolicyDetail(policyDetail.id).subscribe((response) => {
          let message = "Kích hoạt thành công";
          if (policyDetail.status == "A") message = "Hủy kích hoạt thành công";
          if (this.handleResponseInterceptor(response, message)) {
            this.getAllPolicyDetail(policyDetail.policyId);
          }
        },
        (err) => {
          console.log("err----", err);
          this.messageError(AppConsts.messageError);
        }
      );
  }

  // BAT TAT IS SHOW APP
  toggleIsShowAppPolicyDetail(policyDetail) {
    this._distributionService
      .toggleIsShowAppPolicyDetail(policyDetail.id)
      .subscribe(
        (response) => {
          if (
            this.handleResponseInterceptor(
              response,
              "Cập nhật show app thành công"
            )
          ) {
            this.getAllPolicyDetail(policyDetail.policyId);
            this.submitted = false;
          }
        },
        (err) => {
          console.log("err---", err);
        }
      );
  }

  validForm(): boolean {
    const validRequired = this.policy?.code
                      && this.policy?.name
                      && this.policy?.withdrawFeeType != null
                      // && this.policy?.type
                      // && this.policy?.transferTax != null
                      && this.policy?.incomeTax != null
                      // && this.policy?.isTransfer
                      // && this.policy?.classify
                      && this.policy?.minMoney
                      && this.policy?.minWithdraw
                      // && this.policy?.calculateType
                      // && this.policy?.exitFee
                      // && this.policy?.exitFeeType;
    return validRequired;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

  changeGarnerType() {
    // this.policy.
  }

  formatCurrency(value) {
    return value.toLocaleString("de-DE", {
      style: "currency",
      currency: "USD",
    });
  }
}
