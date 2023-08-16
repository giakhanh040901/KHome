import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { forkJoin } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { PolicyTempConst, SearchConst, PolicyDetailTemplateConst, FormNotificationConst, ActiveDeactiveConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { CreatePolicyTemplateComponent } from './create-policy-template/create-policy-template.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { Page } from '@shared/model/page';
import { IBaseListAction, IColumn } from '@shared/interfaces/base.interface';
import { Policy } from '@shared/model/policy-template.model';

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

    rows: Policy[] = [];
    ActiveDeactiveConst = ActiveDeactiveConst;

    PolicyTempConst = PolicyTempConst;
    PolicyDetailTemplateConst = PolicyDetailTemplateConst;
    _selectedColumns: IColumn[];

    cols: IColumn[];

    listAction: IBaseListAction[] = [];
    offset = 0;


    ngOnInit(): void {
        this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPage({ page: this.offset });
            } else {
                this.setPage();
            }
        });
        // Xử lý ẩn hiện cột trong bảng
        this.cols = [
            // { field: 'id', header: '#ID', width: '5rem', position: 1, class:"b-border-frozen-left", isPin:true },
            { field: 'code', header: 'Mã chính sách', width: '12rem', isPin: true },
            { field: 'name', header: 'Tên chính sách', width: '16rem', isPin: true },
            { field: 'nameType', header: 'Loại chính sách', width: '10rem' },
            { field: 'investorTypeName', header: 'Loại nhà đầu tư', width: '10rem' },
            { field: 'incomeTaxDisplay', header: 'Thuế LN ', width: '8rem', pTooltip: 'Thuế lợi nhuận', tooltipPosition: 'top', class: 'justify-content-end' },
            { field: 'transferTaxDisplay', header: 'Thuế CN ', width: '8rem', pTooltip: 'Thuế chuyển nhượng', tooltipPosition: 'top', class: 'justify-content-end' },
            { field: 'nameClassify', header: 'Phân loại', width: '8rem' },
            { field: 'columnResize', header: '', type:'hidden' },
        ];

        this.cols = this.cols.map((item, index) => {
            item.position = index + 1;
            return item;
          });
        // this._selectedColumns = this.cols;
        this._selectedColumns = this.getLocalStorage('policyTemGan') ?? this.cols;
    }

    changeStatus() {
        this.setPage(this.getPageCurrentInfo());

    }

    setColumn(col, _selectedColumns) {
        console.log({ 'cols:': col, '_selectedColumns': _selectedColumns });

        const ref = this.dialogService.open(
            FormSetDisplayColumnComponent,
            this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
        );
        //
        ref.onClose.subscribe((dataCallBack) => {
            console.log('dataCallBack', dataCallBack);
            if (dataCallBack?.accept) {
                this._selectedColumns = dataCallBack.data.sort(function (a, b) {
                    return a.position - b.position;
                });
                this.setLocalStorage(this._selectedColumns, 'policyTemGan')
            }
        });
    }

    getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}


    showData(rows) {
        for (let row of rows) {
            // row.id = row.id,
            row.code = row.code,
            row.name = row.name,
            row.nameType = this.PolicyTempConst.getInterestType(row.interestType),
            row.investorTypeName = this.PolicyTempConst.getInvestorType(row.investorType),
            row.incomeTaxDisplay = row.incomeTax ? (this.utils.transformPercent(row.incomeTax) + '%') : null;
            row.transferTaxDisplay = row.transferTax ? (this.utils.transformPercent(row.transferTax) + '%') : null;
            row.nameClassify = this.PolicyTempConst.getNameClassify(row.classify)
        }
        console.log('row', rows);
    }

    genListAction(data = []) {
        this.listAction = data.map((policy, index) => {

            const actions = [];

            if (this.isGranted([this.PermissionGarnerConst.GarnerCSM_KyHan_ThemMoi])) {
                actions.push({
                    data: policy,
                    label: 'Thêm kỳ hạn',
                    icon: 'pi pi-plus',
                    command: ($event) => {
                        this.edit($event.item.data, true);
                    }
                })
            }

            if (this.isGranted([this.PermissionGarnerConst.GarnerCSM_CapNhat])) {
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

            
            if (this.isGranted([this.PermissionGarnerConst.GarnerCSM_KichHoatOrHuy])) {
                actions.push({
                    data: policy,
                    label: policy.status == ActiveDeactiveConst.ACTIVE ? 'Khóa' : 'Kích hoạt',
                    icon: policy.status == ActiveDeactiveConst.ACTIVE ? 'pi pi-lock' : 'pi pi-lock-open',
                    command: ($event) => {
                        this.changeStatusPolicy($event.item.data);
                    }
                });
            }

            if (this.isGranted([this.PermissionGarnerConst.GarnerCSM_Xoa])) {
                actions.push({
                    data: policy,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                })
            }

            return actions;
        });
        console.log('listActions', this.listAction);

    }

    create() {
        const ref = this.dialogService.open(
            CreatePolicyTemplateComponent,
            {
                header: 'Thêm chính sách',
                width: '1000px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
            }
        );
        //
        ref.onClose.subscribe((response) => {
            console.log('dataCallBack', response);
            if (response?.accept) {
                this.messageSuccess('Thêm thành công');
                this.setPage();
            }
        });
    }

    edit(policy, isCreateDetail?: boolean) {
        console.log("policy",policy);
        
        const ref = this.dialogService.open(
            CreatePolicyTemplateComponent,
            {
                header: 'Sửa chính sách',
                width: '1000px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
                data: {
                    policyTempId: policy.id,
                    isCreateDetail: isCreateDetail,
                    isContractTemp: true,
                }
            }
        );
        //
        ref.onClose.subscribe((response) => {
            console.log('policyUpdate', response);
            if (response?.accept) {
                this.messageSuccess('Cập nhật thành công');
                this.setPage(this.getPageCurrentInfo());
            }
        });
    }

    delete(policy) {
        const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {
                    title: "Bạn có chắc chắn muốn xóa chính sách này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this._policyTemplateService.delete(policy.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa chính sách thành công")) {
                        this.setPage(this.getPageCurrentInfo());
                    }
                }, (err) => {
                    console.log('err____', err);
                    this.messageError(`Không xóa được chính sách ${policy.name}`);
                });
            } else {
            }
        });
    }


    changeStatusPolicy(item) {
        console.log("item.bondPolicyTempId", item);

        this._policyTemplateService.changeStatusPolicy(item.id).subscribe(
            (response) => {
                var message = "";
                if (item.status == 'A') {
                    message = "Hủy kích hoạt thành công";
                } else {
                    message = "Kích hoạt thành công";
                }
                if (this.handleResponseInterceptor(response, message)) {
                    this.setPage(this.getPageCurrentInfo());
                }
            }, () => {
                this.messageService.add({
                    severity: 'error',
                    summary: '',
                    detail: `Không thay đổi được trạng thái của chính sách ${item.name}`,
                    life: 3000,
                });
            }
        );
    }

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

        this.page.keyword = this.keyword;
        this.isLoading = true;
        this._policyTemplateService.getAll(this.page, this.status).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                console.log('data', res?.data?.items);
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

    formatCurrency(value) {
        return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
    }

}
