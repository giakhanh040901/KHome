import { Component, Injector, Input } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { PolicyTemplateConst, PolicyDetailTemplateConst, AppConsts, YesNoConst, FormNotificationConst, ActiveDeactiveConst, TableConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { CreateDistributionPolicyComponent } from './create-distribution-policy/create-distribution-policy.component';
import { DistributionService } from '@shared/services/distribution.service';
import { Page } from '@shared/model/page';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { IAction, IColumn } from '@shared/interface/p-table.model';

@Component({
  selector: 'app-distribution-policy',
  templateUrl: './distribution-policy.component.html',
  styleUrls: ['./distribution-policy.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService]
})
export class DistributionPolicyComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _distributionService: DistributionService,
        private dialogService: DialogService,
        private breadcrumbService: BreadcrumbService,
        public confirmationService: ConfirmationService,
        private _dialogService: DialogService,
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
        { label: 'Trang chủ', routerLink: ['/home'] },
        { label: 'Chính sách mẫu' },
        ]);
    }

    @Input() distributionId: number;

    @Input() contentHeight: number;
    idHeader: string = "distribution-policy";

    PolicyTemplateConst = PolicyTemplateConst;
    PolicyDetailTemplateConst = PolicyDetailTemplateConst;
    YesNoConst = YesNoConst;
    ActiveDeactiveConst = ActiveDeactiveConst;

    status: string;
    
    rows: any[] = [];
    policyTemp: any[] = [];
    columns: IColumn[];
    listAction: IAction[][] = [];
    page = new Page();

    ngOnInit(): void {
        // Xử lý ẩn hiện cột trong bảng
        this.columns = [
            { field: 'id', header: '#ID', width: 5, isPin: true },
            { field: 'code', header: 'Mã chính sách', width: 15, isPin: true, isResize: true },
            { field: 'name', header: 'Tên chính sách', width: 15, isPin: true, isResize: true  },
            { field: 'nameType', header: 'Loại chính sách', width: 10 },
            { field: 'policyDisplayOrderName', header: 'Thứ tự hiển thị', width: 14 },
            { field: 'incomeTax', header: 'Thuế LN ', width: 6, type: TableConst.columnTypes.CURRENCY, unit: '%' },
            { field: 'transfer', header: 'Thuế CN ', width: 6.5, type: TableConst.columnTypes.CURRENCY, unit: '%' },
            { field: 'nameClassify', header: 'Phân loại', width: 7 },
            { field: 'isShowApp', header: 'Show App', width: 8, type: TableConst.columnTypes.CHECKBOX_SHOW },
            { field: 'status', header: 'Trạng thái', displaySettingColumn: false, width: 7, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS , class: 'b-border-frozen-right'},
            { field: '', header: '', displaySettingColumn: false, width: 3, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
        ];
        //
        this.setPage();
    }

    showData(rows) {
        for (let row of rows) {
            row.code = row.code,
            row.name = row.name,
            row.nameType = this.PolicyTemplateConst.getNameType(row?.type),
            row.nameClassify = this.PolicyTemplateConst.getNameClassify(row.classify),
            row.policyDisplayOrderName = this.PolicyTemplateConst.getPolicyDisplayOrderName(row.policyDisplayOrder);
            //
            row.statusElement = PolicyTemplateConst.getStatusInfo(row.status)
        }
    }

    genListAction(data = []) {
        this.listAction = data.map((policy, index) => {
        const actions = [];
        if (this.isGranted([this.PermissionInvestConst.InvestPPDT_ChinhSach_ThemMoi])) {
            actions.push({
                data: policy,
                label: 'Thêm kỳ hạn',
                icon: 'pi pi-plus',
                command: ($event) => {
                    this.edit($event.item.data, true);
                }
            });
        }

        if (this.isGranted([this.PermissionInvestConst.InvestPPDT_ChinhSach_CapNhat])) {
            actions.push({
                data: policy,
                index: index,
                label: 'Sửa',
                icon: 'pi pi-pencil',
                command: ($event) => {
                    this.edit($event.item.data);
                }
            });
        }

        if (this.isGranted([this.PermissionInvestConst.InvestPPDT_ChinhSach_Xoa])) {
            actions.push({
                data: policy,
                label: 'Xoá',
                icon: 'pi pi-trash',
                command: ($event) => {
                    this.delete($event.item.data);
                }
            });
        }

        if (policy.status == this.PolicyTemplateConst.KICH_HOAT && this.isGranted([this.PermissionInvestConst.InvestPPDT_ChinhSach_KichHoatOrHuy])) {
            actions.push({
                data: policy,
                label: 'Huỷ kích hoạt',
                icon: 'pi pi-times',
                command: ($event) => {
                    this.changeStatusPolicy($event.item.data);
                }
            });
        }

        if (policy.status == this.PolicyTemplateConst.KHOA && this.isGranted([this.PermissionInvestConst.InvestPPDT_ChinhSach_KichHoatOrHuy])) {
            actions.push({
                data: policy,
                label: 'Kích hoạt',
                icon: 'pi pi-check',
                command: ($event) => {
                    this.changeStatusPolicy($event.item.data);
                }
            });
        }

        if (this.isGranted([this.PermissionInvestConst.InvestPPDT_ChinhSach_BatTatShowApp]) && !policy.isExpired) {
            actions.push({
                data: policy,
                label: policy.isShowApp ? 'Tắt show app' : 'Bật show app',
                icon: policy.isShowApp ? 'pi pi-eye-slash' : 'pi pi-eye',
                command: ($event) => {
                    this.toggleIsShowAppPolicy($event.item.data?.id);
                }
            });
        }

        return actions;
        });
    }

    setPage(event?: Page, isLoading = true) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = isLoading;
        this._distributionService.getAllPolicyPage(this.page,this.distributionId, this.status).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                let currentDate = new Date();
                this.rows = res?.data?.items.map(row => {
                    row.isExpired = Boolean((currentDate > new Date(row.endDate)) && row.endDate);
                    row.isShowApp = (row.isShowApp == this.YesNoConst.YES) && !row.isExpired;          
                    return row;
                });
                if(this.rows?.length) {
                    this.genListAction(this.rows);
                    this.showData(this.rows)
                }
            }
        });
    }

    
    create() {
        const ref = this.dialogService.open(
            CreateDistributionPolicyComponent,
            {
                header: 'Thêm chính sách',
                width: '1200px',
            });
        ref.onClose.subscribe((res) => {
                this.setPage(null, false);
            }
        );
    }

    edit(policy, isCreateDetail?: boolean) {
        const ref = this.dialogService.open(
            CreateDistributionPolicyComponent,
            {
                header: 'Cập nhật chính sách',
                width: '1200px',
                data: {
                    policyId: policy.id,
                    isCreateDetail: isCreateDetail,
                }
            }
        );
        ref.onClose.subscribe((policyUpdate) => {
            if (policyUpdate) {
                this.setPage();
            }
        });
    }

    delete(policy) {
        const ref = this._dialogService.open(
                FormNotificationComponent,
                {
                    header: "Thông báo",
                    width: '600px',
                    data: {
                        title : "Bạn có chắc chắn muốn xóa chính sách này?",
                        icon: FormNotificationConst.IMAGE_CLOSE,
                    },
                }
            );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._distributionService.deletePolicy(policy.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, "Xóa chính sách thành công")) {
                        this.setPage();
                    }
                });
            } else {
                this.messageService.add({ severity: 'error', detail: AppConsts.messageError ,life: 2000 });
            }
        });
    }

    changeStatusPolicy(policy) {
        this._distributionService.changeStatusPolicy(policy.id).subscribe((response) => {
            let message = "Kích hoạt thành công";
            if (policy.status == 'A') message = "Hủy kích hoạt thành công";
            if (this.handleResponseInterceptor(response, message)) {
            this.setPage(this.page);
            }
        }, (err) => {
            this.messageService.add({ severity: 'error', detail: AppConsts.messageError ,life: 2000 });
        }
        );
    }

    toggleIsShowAppPolicy(policyId) {
        this._distributionService.toggleIsShowAppPolicy(policyId).subscribe(
            (response) => {
                if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                    this.setPage(this.page);
                }
            }, (err) => {
            }
        );
    }

    formatCurrency(value) {
        return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
    }
}
