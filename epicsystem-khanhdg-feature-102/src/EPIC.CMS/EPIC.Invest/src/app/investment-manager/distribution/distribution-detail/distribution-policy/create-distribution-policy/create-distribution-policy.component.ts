import { Component, Injector} from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { AppConsts, AtributionConfirmConst, PolicyDetailTemplateConst, PolicyTemplateConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { CreateDistributionPolicyDetailComponent } from './create-distribution-policy-detail/create-distribution-policy-detail.component';
import { OJBECT_DISTRIBUTION_CONST } from '@shared/base-object';
import { ActivatedRoute } from '@angular/router';
import { DistributionService } from '@shared/services/distribution.service';
import { Page } from '@shared/model/page';
import { IColumn } from '@shared/interface/p-table.model';

const { BASE } = OJBECT_DISTRIBUTION_CONST;

@Component({
    selector: 'app-create-distribution-policy',
    templateUrl: './create-distribution-policy.component.html',
    styleUrls: ['./create-distribution-policy.component.scss'],
})

export class CreateDistributionPolicyComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _distributionService: DistributionService,
        private dialogService: DialogService,
        private routeActive: ActivatedRoute,
        public configDialog: DynamicDialogConfig,
        public ref: DynamicDialogRef,
        public confirmationService: ConfirmationService,
    ) {
        super(injector, messageService);
        this.distributionId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
    }

    distributionId: number;
    defaultDate: Date;
    fieldErrors: any = {};
    isCreateDetail: boolean;
    rows: any[] = [];
    // Data Init

    PolicyTemplateConst = PolicyTemplateConst;
    PolicyDetailTemplateConst = PolicyDetailTemplateConst;

    policy: any = { ...BASE.POLICY }

    policyId: number;

    blockText: RegExp = /[0-9,.]/;
    submitted: boolean;
    //
    cols: any[];

    policyTemps: any[] = [];

    classifies: any[] = [];
    test : any;

    listActionPolicyDetail: any[] = [];

    page = new Page();
    offset = 0;
    fieldDates = ['startDate', 'endDate'];
    isCollapse: boolean = false;

    columnPolicyDetails: IColumn[] = [];
    
    ngOnInit(): void {
        // Đổ dữ liệu Phân loại theo Kiểu chính sách
        this.columnPolicyDetails = [
            { field: 'stt', header: 'STT', width: 5, class: 'justify-content-center text-center' },
            { field: 'name', header: 'Tên kỳ hạn', width: 9 },
            { field: 'periodQuantity', header: 'Kỳ đầu tư', width: 9, type: TableConst.columnTypes.CONVERT_DISPLAY },
            { field: 'profit', header: 'Lợi tức', width: 6, type: TableConst.columnTypes.CURRENCY },
            { field: 'interestDays', header: 'Số ngày', width: 8, type: TableConst.columnTypes.CURRENCY },
            { field: 'isShowApp', header: 'Show App', width: 8, type: TableConst.columnTypes.CHECKBOX_SHOW },
            { field: 'status', header: 'Trạng thái', width: 8, type: TableConst.columnTypes.STATUS },
            { field: '', header: '', width: 0, isResize: true, displaySettingColumn: false },
            { field: '', header: '', width: 5, type: TableConst.columnTypes.ACTION_DROPDOWN, displaySettingColumn: false },
        ];

        this.policyId = this.configDialog?.data?.policyId;
        this.isCreateDetail = this.configDialog?.data?.isCreateDetail;
        //
        if (!this.policyId) {
            this.isLoadingPage = true;
            this._distributionService.getAllPolicyTempNoPermission().subscribe((response) => {
                if (this.handleResponseInterceptor(response) && response?.data?.items?.length) {
                    this.policyTemps = response.data.items.map(policyTemp => {
                        policyTemp.labelName = policyTemp.code + ' - ' + policyTemp.name;
                        return policyTemp 
                    });
                }
                this.isLoadingPage = false;
            }, (err) => {
                console.log('err----', err);
                this.isLoadingPage = false;
            });
        }

        if (PolicyTemplateConst.types[0].code == PolicyTemplateConst.TYPE_CO_DINH) {
            this.classifies = PolicyTemplateConst.classifyFix;
        } else {
            this.classifies = PolicyTemplateConst.classifyNoFix;
        }
        //
        if (this.policyId) {
            this.getDetail(this.policyId);
        } else {
            this.defaultDate = new Date();
            this.defaultDate.setHours(0);
            this.defaultDate.setMinutes(0);
            this.defaultDate.setSeconds(0);
        }

    }

    setData(policyDetails) {
        this.policy.policyDetail = policyDetails.map(policyDetail => {
            policyDetail.periodQuantityDisplay = policyDetail.periodQuantity + ' ' + this.PolicyDetailTemplateConst.getNameInterestPeriodType(policyDetail.periodType);
            policyDetail.statusElement = PolicyTemplateConst.getStatusInfo(policyDetail.status);
            return policyDetail;
        })
    }

    collapse() {
        this.isCollapse = !this.isCollapse;
    }

    getDetail(policyId) {
        this.isLoadingPage = true;
        this._distributionService.getPolicyById(policyId).subscribe((res) => {
            if (this.handleResponseInterceptor(res, '') && res?.data) {
                this.policy = res.data;
                // if(this.policy.startDate) this.policy.startDate = new Date(this.policy.startDate);
                // if(this.policy.endDate) this.policy.endDate = new Date(this.policy.endDate);
                //
                this.setData(this.policy.policyDetail);
                this.formatCalendarDisplay(this.fieldDates, this.policy);
                this.genlistActionPolicyDetail(this.policy?.policyDetail);
                if (this.isCreateDetail) {
                    this.isCreateDetail = false;
                    this.isCollapse = true;
                    this.createPolicyDetail();
                }
                //
                this.changePolicyType();
            }
            setTimeout(() => {
                this.isLoadingPage = false;
            }, 150);
        }, (err) => {
            console.log('err---', err);
        });
    }

    getAllPolicyDetail(policyId) {
        this._distributionService.getAllPolicyDetail(policyId).subscribe((res) => {
            if (this.handleResponseInterceptor(res, '')) {
                this.policy.policyDetail = res?.data;
                this.genlistActionPolicyDetail(this.policy.policyDetail);
                this.setData(this.policy.policyDetail);
            }
        }, (err) => {
            console.log('err----', err);
        });
    }

    genlistActionPolicyDetail(data = []) {
        this.listActionPolicyDetail = data.map((policyDetail) => {
            const actions = [];
            if (this.isGranted([this.PermissionInvestConst.InvestPPDT_KyHan_CapNhat])) {
                actions.push({
                    data: policyDetail,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        console.log("$event.item.data", $event.item.data);
                        this.editPolicyDetail($event.item.data);
                    }
                });
            }

            if (this.isGranted([this.PermissionInvestConst.InvestPPDT_KyHan_Xoa])) {
                actions.push({
                    data: policyDetail,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.deletePolicyDetail($event.item.data);
                    }
                });
            }

            if (policyDetail.status == this.PolicyDetailTemplateConst.KICH_HOAT && this.isGranted([this.PermissionInvestConst.InvestPPDT_KyHan_KichHoatOrHuy])) {
                actions.push({
                    data: policyDetail,
                    label: 'Huỷ kích hoạt',
                    icon: 'pi pi-times',
                    command: ($event) => {
                        this.changeStatusPolicyDetail($event.item.data);
                    }
                });
            }

            if (policyDetail.status == this.PolicyDetailTemplateConst.KHOA && this.isGranted([this.PermissionInvestConst.InvestPPDT_KyHan_KichHoatOrHuy])) {
                actions.push({
                    data: policyDetail,
                    label: 'Kích hoạt',
                    icon: 'pi pi-check',
                    command: ($event) => {
                        this.changeStatusPolicyDetail($event.item.data);
                    }
                });
            }

            if (this.isGranted([this.PermissionInvestConst.InvestPPDT_KyHan_BatTatShowApp])) {
                actions.push({
                    data: policyDetail,
                    label: policyDetail.isShowApp == YesNoConst.NO ? 'Bật show app' : 'Tắt show app',
                    icon: policyDetail.isShowApp == YesNoConst.NO ? 'pi pi-eye' : 'pi pi-eye-slash',
                    command: ($event) => {
                        this.toggleIsShowAppPolicyDetail($event.item.data);
                    }
                });
            }

            return actions;
        });
        console.log('listActions', this.listActionPolicyDetail);
    }

    // THÊM MỚI KỲ HẠN
    createPolicyDetail() {
        const ref = this.dialogService.open(
            CreateDistributionPolicyDetailComponent,
            {
                header: 'Thêm kỳ hạn',
                width: '600px',
                data: {
                    policyId: this.policy.id,
                    policyTemplateType: this.policy.type,
                }
            }
        );
        //
        ref.onClose.subscribe((response) => {
            // Thêm mới kỳ hạn khi Chính sách chưa được tạo
            if (response?.accept) {
                this.getAllPolicyDetail(this.policy.id);
            }
        });
    }

    // CẬP NHẬT KỲ HẠN
    editPolicyDetail(policyDetail) {
        const ref = this.dialogService.open(
            CreateDistributionPolicyDetailComponent,
            {
                header: 'Cập nhật kỳ hạn',
                width: '650px',
                data: {
                    policyDetail: { ...policyDetail },
                    policyId: this.policy.id,
                },
            }
        );
        //
        ref.onClose.subscribe((response) => {
            if (response?.accept) {
                this.getAllPolicyDetail(this.policy.id);
            }
        });
    }

    // TRA DU LIEU PHAN LOAI THEO KIEU CHINH SACH
    changePolicyType() {
        if (this.policy.type == PolicyTemplateConst.TYPE_CO_DINH) {
            this.classifies = PolicyTemplateConst.classifyFix;
        } else if (this.policy.type == PolicyTemplateConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY) {
            this.classifies = PolicyTemplateConst.classify;
        }
        else {
            this.classifies = PolicyTemplateConst.classifyNoFix;
        }
    }

    changeCalculateWithdrawType(){
        if(this.policy.calculateWithdrawType == PolicyTemplateConst.KY_HAN_THAP_HON_GAN_NHAT){
          this.policy.profitRateDefault = 0;
        }
    }

    // CHON CHINH SACH MAU
    changePolicyTemp(policyTempId) {
        const policyTemp = this.policyTemps.find(item => item.id == policyTempId);
        if (policyTemp) {
            this.policy = {
                ...policyTemp,
                policyTempId: policyTemp.id,
            };
            this.changePolicyType();
            delete this.policy.id;
        }
    }

    // XOA KỲ HAN
    deletePolicyDetail(policyDetail) {
        this.confirmationService.confirm({
            message: `Bạn có chắc chắn muốn xóa kỳ hạn "${policyDetail?.name}"`,
            ...AtributionConfirmConst,
            accept: () => {
                if (policyDetail.id) {
                    this._distributionService.deletePolicyDetail(policyDetail.id).subscribe((res) => {
                        if (this.handleResponseInterceptor(res, 'Xóa thành công!')) {
                            this.getAllPolicyDetail(policyDetail?.policyId);
                        }
                    }, (err) => {
                        console.log('err----', err);
                        this.messageService.add({ severity: 'error', detail: AppConsts.messageError, life: 2000 });
                    });
                }
            }
        });
    }

    // 
    save() {
        // KIỂM TRA LẠI
        if (!this.policy.distributionId) this.policy.distributionId = this.distributionId;
        let body = this.formatCalendar(this.fieldDates, {...this.policy});
        
        this.submitted = true;
        this.isLoading = true;
        if (this.policy.id) {
            this._distributionService.updatePolicy(body, body.id).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {   
                    this.ref.close({ data: {}, accept: true });
                }
                this.submitted = false;
                this.isLoading = false;
            }, (err) => {
                console.log('err---', err);
                this.submitted = false;
                this.isLoading = false;
            });
        } else {
            this.submitted = true;
            this.isLoading = true;
            this._distributionService.addPolicy(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
                    this.submitted = false;
                    this.isLoading = false;
                    this.configDialog.header = "Cập nhật chính sách";
                    this.policyId = response.id; 
                    if (response?.data?.policyDetail) {
                        this.setData(response?.data?.policyDetail);
                        this.genlistActionPolicyDetail(response?.data?.policyDetail)
                    }
                    //
                    this.isLoadingPage = true;
                    setTimeout(() => {
                        this.policy = response?.data ?? [];
                        this.formatCalendarDisplay(this.fieldDates, this.policy);
                        this.isLoadingPage = false;
                    }, 500);
                }
            }, (err) => {
                console.log('err----', err);
                this.submitted = false;
                this.isLoading = false;
            });
        }
    }

    close() {
        this.ref.close();
    }

    changeStatusPolicyDetail(policyDetail) {
        this._distributionService.changeStatusPolicyDetail(policyDetail.id).subscribe((response) => {
            let message = "Kích hoạt thành công";
            if (policyDetail.status == 'A') message = "Hủy kích hoạt thành công";
            if (this.handleResponseInterceptor(response, message)) {
                this.getAllPolicyDetail(policyDetail.policyId);
            }
        }, (err) => {
            console.log('err----', err);
            this.messageService.add({ severity: 'error', detail: AppConsts.messageError, life: 3000 });
        }
        );
    }

    // BAT TAT IS SHOW APP
    toggleIsShowAppPolicyDetail(policyDetail) {
        this._distributionService.toggleIsShowAppPolicyDetail(policyDetail.id).subscribe(
            (response) => {
                if (this.handleResponseInterceptor(response, "Cập nhật show app thành công")) {
                    this.getAllPolicyDetail(policyDetail.policyId);
                    this.submitted = false;
                }
            }, (err) => {
                console.log('err---', err);
            }
        );
    }

    validForm(): boolean {
        const validRequired = this.policy?.code
                            && this.policy?.name
                            && this.policy?.minMoney
                            && this.policy?.type
                            && this.policy?.incomeTax != null
                            && this.policy?.classify
                            && this.policy?.calculateType
                            && this.policy?.minTakeContract
                            && this.policy?.minWithDraw
                            && this.policy?.maxWithDraw
                            && this.policy?.exitFee
                            && this.policy?.exitFeeType
                            && this.policy?.isTransfer
                            && this.policy?.transferTax != null
                            && this.policy?.remindRenewals
                            && this.policy?.expirationRenewals;
        return validRequired;
    }

    resetValid(field) {
        this.fieldErrors[field] = false;
    }

    formatCurrency(value) {
        return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
    }

}
