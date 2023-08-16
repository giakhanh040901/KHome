import { Component, Inject, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { FormNotificationConst, ContractFormConst, SampleContractConst, ActiveDeactiveConst, TableConst, IssuerConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { SampleContractService } from '@shared/services/sample-contract.service';
import { AppUtilsService } from '@shared/services/utils.service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { CreateSampleContractComponent } from './create-sample-contract/create-sample-contract.component';
import { Page } from '@shared/model/page';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { SampleContractFilter } from '@shared/interface/filter.model';

@Component({
  selector: 'app-sample-contract',
  templateUrl: './sample-contract.component.html',
  styleUrls: ['./sample-contract.component.scss'],
    providers: [DialogService, ConfirmationService]
})
export class SampleContractComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private dialogService: DialogService,
        private breadcrumbService: BreadcrumbService,
        public confirmationService: ConfirmationService,
        private _dialogService: DialogService,
		private _sampleContractService: SampleContractService,
        private _utilsService: AppUtilsService,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Mẫu hợp đồng' },
        ]);
    }

    private baseUrl: string;
    SampleContractConst = SampleContractConst;
    ContractFormConst = ContractFormConst;

    listAction: any[] = [];
    page = new Page();

    dataFiler: SampleContractFilter = new SampleContractFilter();

    rows: any[] = [];
	columns: IColumn[] = [];
    dataTableEmit: DataTableEmit = new DataTableEmit();

    ngOnInit(): void {
        this.setPage();
        // Xử lý ẩn hiện cột trong bảng
        this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
            { field: 'name', header: 'Tên hợp đồng', width: 16, isPin: true, isSort: true, isResize: true },
            { field: 'description', header: 'Mô tả', width: 15, isSort: true, isResize: true },
            { field: 'contractType', header: 'Loại hợp đồng ', width: 12, type: TableConst.columnTypes.CONVERT_DISPLAY },
            { field: 'contractSource', header: 'Kiểu hợp đồng', width: 10, type: TableConst.columnTypes.CONVERT_DISPLAY },
            { field: 'createdBy', header: 'Người tạo', width: 12 },
            { field: 'createdDate', header: 'Ngày tạo', width: 11, isSort: true, type: TableConst.columnTypes.DATETIME },
			{ field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
        ];
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this._sampleContractService.getAll(this.page, this.dataFiler).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res?.data?.items;
                //
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                    this.setData(this.rows);
                }
            }
        }, (err) => {
            this.isLoading = false;
        });
    }

    setData(rows) {
        for (let row of rows) {
			row.contractTypeDisplay = SampleContractConst.getContractType(row.contractType, 'name');
            row.contractSourceDisplay = SampleContractConst.getContractSource(row.contractSource, 'name');
			row.statusElement = ContractFormConst.getStatusInfo(row?.status);
        }
    }

    genListAction(data = []) {
        this.listAction = data.map((sampleContract, index) => {
            const actions = [];
            if (this.isGranted([this.PermissionInvestConst.InvestMauHD_TaiFileCaNhan])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Tải file cá nhân',
                    icon: 'pi pi-download',
                    command: ($event) => {
                        this.downloadFileInvestor($event.item.data);
                    }
                });

                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Test Fill HĐ cá nhân',
                    icon: 'pi pi-download',
                    command: ($event) => {
                        this.testFillData($event.item.data, IssuerConst.INVESTOR);
                    }
                })
            }

            if (this.isGranted([this.PermissionInvestConst.InvestMauHD_TaiFileDoanhNghiep])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Tải file d/nghiệp',
                    icon: 'pi pi-download',
                    command: ($event) => {
                        this.downloadFileBusiCustomer($event.item.data);
                    }
                });

                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Test Fill HĐ doanh nghiệp',
                    icon: 'pi pi-download',
                    command: ($event) => {
                        this.testFillData($event.item.data, IssuerConst.BUSINESSCUSTOMER);
                    }
                })
            }
           
            if (this.isGranted([this.PermissionInvestConst.InvestMauHD_CapNhat])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                })
            }

            if (sampleContract.status == ActiveDeactiveConst.ACTIVE && this.isGranted([this.PermissionInvestConst.InvestMauHD_CapNhat])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Đóng',
                    icon: 'pi pi-lock',
                    command: ($event) => {
                        this.changeActionStatus($event.item.data);
                    }
                })
            }

            if (sampleContract.status == ActiveDeactiveConst.DEACTIVE && this.isGranted([this.PermissionInvestConst.InvestMauHD_CapNhat])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Kích hoạt',
                    icon: 'pi pi-lock-open',
                    command: ($event) => {
                        this.changeActionStatus($event.item.data);
                    }
                })
            }

            if (this.isGranted([this.PermissionInvestConst.InvestMauHD_Xoa])) {
                actions.push({
                    data: sampleContract,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                })
            }
            return actions;
        });
    }

    changeActionStatus(sampleContract) {
        const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {	
                    title: sampleContract.status == ActiveDeactiveConst.ACTIVE ? 'Đóng hợp đồng' : 'Kích hoạt hợp đồng',
                    icon: sampleContract.status == ActiveDeactiveConst.ACTIVE ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this._sampleContractService.updateStatus(sampleContract).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Cập nhật thành công")) {
                        this.setPage();
                    }
                }, (err) => {
                    console.log('err____', err);
                });
            } else {
            }
        });
    }

	onSort(event) {
		this.dataFiler.sortFields = event;
		this.setPage();
	}

    downloadFileInvestor(file) {
        let fileUrl = file.fileInvestor;
        const url = this.baseUrl + "/" + fileUrl;
        this._utilsService.makeDownload("", url);
    }

    IssuerConst = IssuerConst;
    isDownload:boolean = false;

    testFillData(contractTemplate, type) {
        const params = {
            type: type,
            contractTemplateTempId: contractTemplate.id,
        }
        //
        this.isDownload = true;
        this._sampleContractService.testFillContract(params).subscribe((response) => {
            this.isDownload = false;
            if(response?.status === 0) this.handleResponseInterceptor(response);
        }, (err) => {
            this.isDownload = false;
        });
    }

    downloadFileBusiCustomer(file) {
        let fileUrl = file.fileBusinessCustomer;
        const url = this.baseUrl + "/" + fileUrl;
        this._utilsService.makeDownload("", url);
    }

    create() {
		this.dialogService.open(CreateSampleContractComponent, {
            header: "Thêm mới hợp đồng mẫu",
            width: '800px',
        }).onClose.subscribe((res) => {
            if(res) {
                this.setPage();
            }
        });
	}

    edit(sampleContract) {
        this.dialogService.open(CreateSampleContractComponent, {
            header: "Sửa hợp đồng mẫu",
            width: '800px',
            data: {
                sampleContract : sampleContract
            },
        }).onClose.subscribe((res) => {
            if(res) {
                this.setPage();
            }
        });
    }

    delete(sampleContract) {
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
                this._sampleContractService.delete(sampleContract.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa mẫu hợp đồng thành công")) {
                        this.setPage();
                    }
                }, (err) => {
                    console.log('err____', err);
                    this.messageError(`Không xóa được mẫu hợp đồng ${sampleContract.name}`);
                });
            } 
        });
    }
}





