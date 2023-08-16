import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { PolicyTemplateConst, SearchConst, PolicyDetailTemplateConst, FormNotificationConst, TableConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { CreatePolicyTemplateComponent } from './create-policy-template/create-policy-template.component';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { Page } from '@shared/model/page';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { PolicyTemplateFilter } from '@shared/interface/filter.model';

@Component({
    selector: 'app-policy-template',
    templateUrl: './policy-template.component.html',
    styleUrls: ['./policy-template.component.scss'],
    providers: [DialogService, ConfirmationService, MessageService]
})
export class PolicyTemplateComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _policyTemplateService: PolicyTemplateService,
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

    rows: any[] = [];

    PolicyTemplateConst = PolicyTemplateConst;
    PolicyDetailTemplateConst = PolicyDetailTemplateConst;

    listAction: any[] = [];
    columns: IColumn[] = [];
    page = new Page();

    dataTableEmit: DataTableEmit = new DataTableEmit();

    dataFilter: PolicyTemplateFilter = new PolicyTemplateFilter();
    
    ngOnInit(): void {
        // Xử lý ẩn hiện cột trong bảng
        this.columns = [
			{ field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true},
            { field: 'code', header: 'Mã chính sách', width: 20, isPin: true, isResize: true, isSort: true },
            { field: 'name', header: 'Tên chính sách', width: 20, isPin: true, isResize: true, isSort: true},
            { field: 'type', header: 'Loại chính sách', width: 11, isSort: true, type: TableConst.columnTypes.CONVERT_DISPLAY },
            { field: 'incomeTax', header: 'Thuế LN ', width: 8, type: TableConst.columnTypes.CURRENCY, unit: '%', isSort: true },
            { field: 'transferTax', header: 'Thuế CN ', width: 8, type: TableConst.columnTypes.CURRENCY, unit: '%', isSort: true },
            { field: 'classify', header: 'Phân loại', width: 8, type: TableConst.columnTypes.CONVERT_DISPLAY },
			{ field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
        ];
        //
        this.setPage();
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this._policyTemplateService.getAll(this.page, this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res?.data?.items;
                //
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                    this.showData(this.rows);
                }
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }

    showData(rows) {
        for (let row of rows) {
            row.typeDisplay = this.PolicyTemplateConst.getNameType(row.type),
            row.classifyDisplay = this.PolicyTemplateConst.getNameClassify(row.classify),
            row.statusElement = PolicyTemplateConst.getStatusInfo(row?.status);
        }
        console.log('row', rows);
    }

    genListAction(data = []) {
        this.listAction = data.map((policy, index) => {
            const actions = [];
            if (this.isGranted([this.PermissionInvestConst.InvestCSM_KyHan_ThemMoi])) {
                actions.push({
                    data: policy,
                    label: 'Thêm kỳ hạn',
                    icon: 'pi pi-plus',
                    command: ($event) => {
                        this.edit($event.item.data, true);
                    }
                })
            }

            if (this.isGranted([this.PermissionInvestConst.InvestCSM_CapNhat])) {
                actions.push({
                    data: policy,
                    index: index,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                })
            }

            if (this.isGranted([this.PermissionInvestConst.InvestCSM_Xoa])) {
                actions.push({
                    data: policy,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                })
            }

            if (policy.status == this.PolicyTemplateConst.KICH_HOAT && this.isGranted([this.PermissionInvestConst.InvestCSM_KichHoatOrHuy])) {
                actions.push({
                    data: policy,
                    label: 'Huỷ kích hoạt',
                    icon: 'pi pi-times',
                    command: ($event) => {
                        this.changeStatusPolicy($event.item.data);
                    }
                });
            }

            if (policy.status == this.PolicyTemplateConst.KHOA && this.isGranted([this.PermissionInvestConst.InvestCSM_KichHoatOrHuy])) {
                actions.push({
                    data: policy,
                    label: 'Kích hoạt',
                    icon: 'pi pi-check',
                    command: ($event) => {
                        this.changeStatusPolicy($event.item.data);
                    }
                });
            }
            return actions;
        });
    }

    onSort(event) {
		this.dataFilter.sortFields = event;
		this.setPage();
	}


    create() {
        const ref = this.dialogService.open(
            CreatePolicyTemplateComponent,
            {
                header: 'Thêm chính sách',
                width: '1000px',
            }
        );
        //
        ref.onClose.subscribe((response) => {
            if (response?.accept) {
                this.messageSuccess('Thêm thành công');
                this.setPage();
            }
        });
    }

    edit(policy, isCreateDetail?: boolean) {
        const ref = this.dialogService.open(
            CreatePolicyTemplateComponent,
            {
                header: 'Sửa chính sách',
                width: '1000px',
                data: {
                    policyTempId: policy.id,
                    isCreateDetail: isCreateDetail,
                }
            }
        );
        //
        ref.onClose.subscribe((response) => {
            if (response?.accept) {
                this.messageSuccess('Cập nhật thành công');
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
                    title: "Bạn có chắc chắn muốn xóa chính sách này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._policyTemplateService.delete(policy.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa chính sách thành công")) {
                        this.setPage(this.page);
                    }
                }, (err) => {
                    console.log('err____', err);
                    this.messageError(`Không xóa được chính sách ${policy.name}`,'');
                });
            } 
        });
    }


    changeStatusPolicy(item) {
        this._policyTemplateService.changeStatusPolicy(item.id).subscribe(
            (response) => {
                let message = item.status == 'A' ? "Hủy kích hoạt thành công" : "Kích hoạt thành công";
                if (this.handleResponseInterceptor(response, message)) {
                    this.setPage(this.page);
                }
            }, () => {
                this.messageError(`Không thay đổi được trạng thái của chính sách ${item.name}`);
            }
        );
    }

    
}
