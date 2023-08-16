import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { AtributionConfirmConst, MessageErrorConst, PolicyDetailTemplateConst, PolicyTemplateConst, TableConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { CreatePolicyTemplateDetailComponent } from './create-policy-template-detail/create-policy-template-detail.component';
import { Page } from '@shared/model/page';
import { IColumn } from '@shared/interface/p-table.model';

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

    fieldErrors: any = {};
    isCreateDetail: boolean;
    rows: any[] = [];
    // Data Init

    PolicyTemplateConst = PolicyTemplateConst;
    PolicyDetailTemplateConst = PolicyDetailTemplateConst;

    row: any;
    col: any;

    policyTemplate: any = {
        'code': null,   // Mã chính sách
        'name': null,   // Tên chính sách
        'minMoney': null,  // Số tiền đầu tư tối thiểu
        'type': null,   // Kiểu chính sách
        'minInvestDay': 0, // Số ngày đầu tư tối thiểu
        'incomeTax': 0,  // Thuế lợi nhuận
        'classify': null, // Kiểu chính sách
        'calculateType': null,  // Cách tính lợi tức
        'policyDisplayOrder': null, // Thứ tự hiển thị
        'minTakeContract': 0, // Gửi yêu cầu nhận hợp đồng từ (VND) 
        'description': null, // Mô tả
        'minWithdraw': null, // Số tiền rút vốn tối thiểu 
        'maxWithDraw': null, // Số tiền rút vốn tối đa
        'exitFee': 0,  // Phí rút tiền (%) 
        'exitFeeType': null, // Kiểu của phí rút tiền
        'isTransfer': null, // Chuyển đổi tài sản
        'transferTax': 0, // Phí chuyển đổi tài sản (%)
        'remindRenewals': null, // Nhắc tái tục trước (ngày)
        'expirationRenewals': null, // Gửi yêu cầu tái tục trước (ngày)
        'renewalsType': null, // Loại hợp đồng tái tục
        'policyDetailTemp': [],  // Kỳ hạn
        'profitRateDefault': 0, // Phần trăm cố định,
        'calculateWithdrawType': null, // Cách tính lợi tức rút    
        'maxMoney': null, // Số tiền đầu tư tối đa
    }

    policyTempId: number;

    blockText: RegExp = /[0-9,.]/;
    submitted: boolean;
    //
    cols: any[];

    classifies: any[] = [];

    listActionPolicyDetail: any[] = [];

    page = new Page();

    isCollapse = false;

    columnPolicyDetails: IColumn[] = [];

    ngOnInit(): void {
        // Đổ dữ liệu Phân loại theo Kiểu chính sách
        if (PolicyTemplateConst.types[0].code == PolicyTemplateConst.TYPE_CO_DINH) {
            this.classifies = PolicyTemplateConst.classifyFix;
        } else {
            this.classifies = PolicyTemplateConst.classifyNoFix;
        }
        //
        this.policyTempId = this.configDialog?.data?.policyTempId;
        this.isCreateDetail = this.configDialog?.data?.isCreateDetail;
        if (this.policyTempId) {
            this.getDetail(this.policyTempId);
        }
        //
        this.columnPolicyDetails = [
            { field: 'stt', header: 'STT', width: 5 },
            { field: 'periodQuantity', header: 'Kỳ đầu tư', width: 8, type: TableConst.columnTypes.CONVERT_DISPLAY },
            { field: 'profit', header: 'Lợi tức', width: 8, type: TableConst.columnTypes.CURRENCY, unit: '%' },
            { field: 'interestDays', header: 'Số ngày', width: 8, type: TableConst.columnTypes.CURRENCY },
            { field: '', header: '', width: 0, isResize: true },
            { field: '', header: 'Thao tác', width: 8, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-center' },
        ]
    }

    collapse() {
        this.isCollapse = !this.isCollapse;
    }
    
    getDetail(policyTempId) {
        if (policyTempId != null) {
            this._policyTemplateService.get(policyTempId).subscribe((res) => {  
                if (this.handleResponseInterceptor(res, '')) {
                    this.policyTemplate = res?.data || [];
                    this.genlistActionPolicyDetail(this.policyTemplate?.policyDetailTemp);
                    this.setData(this.policyTemplate);
                    if (this.isCreateDetail) {
                        this.isCollapse = true;
                        this.isCreateDetail = false;
                        this.createPolicyDetail();
                    }
                    //
                    this.changePolicyType();
                }
                this.submitted = false;
            });
        } 
    }

    setData(policyTemplate) {
        this.policyTemplate.policyDetailTemp = policyTemplate.policyDetailTemp.map(policyDetail => {
            policyDetail.periodQuantityDisplay = policyDetail.periodQuantity + ' ' + PolicyDetailTemplateConst.getNameInterestPeriodType(policyDetail.periodType);
            return policyDetail;
        })
    }

    genlistActionPolicyDetail(data = []) {
        this.listActionPolicyDetail = data.map((policyDetail, index) => {
            const actions = [];
            if (this.isGranted([this.PermissionInvestConst.InvestCSM_KyHan_CapNhat])) {
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

            if (this.isGranted([this.PermissionInvestConst.InvestCSM_KyHan_Xoa])) {
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

            if (policyDetail.status == this.PolicyDetailTemplateConst.KICH_HOAT && this.isGranted([this.PermissionInvestConst.InvestCSM_KyHan_KichHoatOrHuy])) {
                actions.push({
                    data: policyDetail,
                    label: 'Huỷ kích hoạt',
                    icon: 'pi pi-times',
                    command: ($event) => {
                        this.changeStatusPolicyDetail($event.item.data);
                    }
                });
            }

            if (policyDetail.status == this.PolicyDetailTemplateConst.KHOA && this.isGranted([this.PermissionInvestConst.InvestCSM_KyHan_KichHoatOrHuy])) {
                actions.push({
                    data: policyDetail,
                    label: 'Kích hoạt',
                    icon: 'pi pi-check',
                    command: ($event) => {
                        this.changeStatusPolicyDetail($event.item.data);
                    }
                });
            }
            return actions;
        });
    }

    changeCalculateWithdrawType(){
        if(this.policyTemplate.calculateWithdrawType == PolicyTemplateConst.KY_HAN_THAP_HON_GAN_NHAT){
          this.policyTemplate.profitRateDefault = 0;
        }
    }


    createPolicyDetail() {
        const ref = this.dialogService.open(
        CreatePolicyTemplateDetailComponent,
        {
            header: 'Thêm kỳ hạn',
            width: '700px',
            baseZIndex: 10000,
            data: {
                policyTempId: this.policyTemplate.id,
                policyTemplateType: this.policyTemplate.type,
            }
        });

        ref.onClose.subscribe((policyDetail) => {
            if (policyDetail?.bondPolicyDetailTempId || policyDetail?.fakeId) {
                // Thêm mới kỳ hạn khi Chính sách chưa được tạo
                if (!policyDetail?.bondPolicyDetailTempId) {
                    this.policyTemplate.policyDetailTemp.push(policyDetail);
                    this.setData(this.policyTemplate);
                    this.genlistActionPolicyDetail(this.policyTemplate.policyDetailTemp);
                if (policyDetail.policyTempId != null) {
                    this.getDetail(this.policyTemplate.id);
                }
                } else {

                }
            }
            if (policyDetail?.policyTempId != null) {
                this.getDetail(this.policyTemplate.id);
            }
        });
    }

    editPolicyDetail(policyDetail) {
        const ref = this.dialogService.open(
        CreatePolicyTemplateDetailComponent,
        {
            header: 'Sửa kỳ hạn',
            width: '700px',
            baseZIndex: 10000,
            data: {
                policyDetail: policyDetail,
                policyTempId: this.policyTemplate.id,
                policyTemplateType: this.policyTemplate.type,
            },
        });
        //
        ref.onClose.subscribe((policyDetail) => {
            if (policyDetail?.fakeId) {
                const index = this.policyTemplate.policyDetailTemp.findIndex(item => item.fakeId == policyDetail.fakeId);
                if (index !== -1) this.policyTemplate.policyDetailTemp[index] = policyDetail;
            } else {
                // Gọi lại Api
            }
        });
    }

    changePolicyType() {
        if (this.policyTemplate.type == PolicyTemplateConst.TYPE_CO_DINH) {
            this.classifies = PolicyTemplateConst.classifyFix;
            this.policyTemplate.minInvestDay = 0; // nếu loại chính sách là cố định thì không có ngày đầu tư tối thiểu
        } else 
        if (this.policyTemplate.type == PolicyTemplateConst.TYPE_CHI_TRA_CO_DINH_THEO_NGAY) {
            this.classifies = PolicyTemplateConst.classify;
        } 
        else {
            this.classifies = PolicyTemplateConst.classifyNoFix;
        }
    }

    deletePolicyDetail(policyDetail, index) {
        this.confirmationService.confirm({
            message: `Bạn có chắc chắn muốn xóa kỳ hạn ${policyDetail.name}`,
            ...AtributionConfirmConst,
            accept: () => {
                if (policyDetail.id) {
                    this._policyTemplateService.deletePolicyDetail(policyDetail.id).subscribe((response) => {
                        if (this.handleResponseInterceptor(response, '')) {
                            this.messageSuccess("Xóa thành công");
                            if (policyDetail.policyTempId != null) {
                                this.getDetail(this.policyTemplate.id);
                            }
                        }
                    }, () => {
                        this.messageError(`Không xóa được chính sách ${this.policyTemplate.name}`);
                    });
                } else {
                    this.policyTemplate.policyDetailTemp.splice(index, 1);
                    this.messageSuccess("Xóa thành công");
                }
                this.genlistActionPolicyDetail(this.policyTemplate.policyDetailTemp);
            }
        });
    }

    save() {
        if (this.validForm()){
            this.submitted = true;
            if (this.policyTemplate.id) {
                this._policyTemplateService.update(this.policyTemplate, this.policyTemplate.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response)) {
                        this.ref.close({ data: response, accept: true });
                    }
                    this.submitted = false;
                }, (err) => {
                    console.log('err----', err);
                    this.submitted = false;
                });
            } else {
                this._policyTemplateService.create(this.policyTemplate).subscribe((response) => {
                    if (this.handleResponseInterceptor(response)) {
                        this.ref.close({ data: response, accept: true });
                    } 
                    this.submitted = false;
                }, (err) => {
                    console.log('err----', err);
                    this.submitted = false;
                });
            }
        } else {
            this.messageError(MessageErrorConst.message.Validate);
        }
    }

    close() {
        this.ref.close();
    }

    changeTrafer(value){
        if (value == PolicyTemplateConst.NO){
            this.policyTemplate.transferTax = 0;
        }
    }

    changeStatusPolicyDetail(item) {
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
            this.messageError(`Không thay đổi được trạng thái của kỳ hạn ${item.name}`);
        });
    }

    validForm(): boolean {
        const validRequired = this.policyTemplate?.code
                            && this.policyTemplate?.name
                            && this.policyTemplate?.minMoney
                            && this.policyTemplate?.type
                            && this.policyTemplate?.incomeTax != null
                            && this.policyTemplate?.classify
                            && this.policyTemplate?.calculateType
                            && this.policyTemplate?.minTakeContract
                            && this.policyTemplate?.minWithdraw
                            && this.policyTemplate?.maxWithDraw != null
                            && this.policyTemplate?.exitFee != null
                            && this.policyTemplate?.exitFeeType
                            && this.policyTemplate?.isTransfer != null
                            && this.policyTemplate?.remindRenewals
                            && this.policyTemplate?.expirationRenewals
                            && this.policyTemplate?.calculateWithdrawType
                            && this.policyTemplate?.profitRateDefault != null    
                            && (this.policyTemplate?.minInvestDay || this.policyTemplate?.type == this.PolicyTemplateConst.TYPE_CO_DINH)
                            && (this.policyTemplate?.profitRateDefault || this.policyTemplate.calculateWithdrawType == PolicyTemplateConst.KY_HAN_THAP_HON_GAN_NHAT);
        return !!validRequired;
    }

    formatCurrency(value) {
        return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
    }

    public get tootipLabelExpirationRenewals() {
        return 'Trước thời điểm hợp đồng đầu tư đáo hạn số ngày trên, thì khách hàng có thể yêu cầu tái tục hợp đồng trên App.';
    }
}
