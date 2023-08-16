import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { BlockageLiberationConst, TableConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { BlockageLiberationService } from '@shared/services/blockade-liberation.service';
import { ContractBlockageDetailComponent } from './contract-blockage-detail/contract-blockage-detail.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { Subscription } from 'rxjs';
import { DataTableEmit, IAction, IColumn } from '@shared/interface/p-table.model';
import { OrderBlockageFilter } from '@shared/interface/filter.model';

@Component({
  selector: 'app-contract-blockage',
  templateUrl: './contract-blockage.component.html',
  styleUrls: ['./contract-blockage.component.scss']
})
export class ContractBlockageComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private breadcrumbService: BreadcrumbService,
        private __blockageLiberationService: BlockageLiberationService,
        private _tradingProviderSelectedService: TradingProviderSelectedService,
        private dialogService: DialogService,
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
        { label: 'Trang chủ', routerLink: ['/home'] },
        { label: 'Hợp đồng phân phối' },
        { label: 'Phong tỏa, giải tỏa' },
        ]);
    }

    liberationDialog: boolean;
    BlockageLiberationConst = BlockageLiberationConst

    rows: any[] = [];
    columns: IColumn[] = [];
    listAction: IAction[][] = [];
    page = new Page();

    blockageLiberation: any = {
        "id": 0,
        "type": null,
        "blockadeDescription": "",
        "blockadeDate": null,
        "orderId": 0,
        "blockader": null,
        "blockadeTime": null,
        "liberationDescription": null,
        "liberationDate": null,
        "liberator": null,
        "liberationTime": null,
        "status": null,
        "contractCode": null,
        "totalValue": null
    };

    order: any = {};
    isPartner: boolean;

    dataFilter: OrderBlockageFilter = new OrderBlockageFilter();

    dataTableEmit: DataTableEmit = new DataTableEmit();

    tradingProviderSub: Subscription;

    // Menu otions thao tác
    ngOnInit() {
        this.isPartner = this.getIsPartner();
        this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change: number[]=[]) => {
            this.dataFilter.tradingProviderIds = change;
            this.setPage();
        })
       
        this.columns = [
            { field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
            { field: 'contractCodeGen', header: 'Mã hợp đồng', width: 12, isPin: true, isResize: true, isSort: true },
            { field: 'blockadeDate', header: 'Ngày phong tỏa', width: 12, isPin: true, isResize: true, isSort: true, type: TableConst.columnTypes.DATETIME, },
            { field: 'blockader', header: 'Người phong tỏa', width: 12, isSort: true },
            { field: 'blockadeDescription', header: 'Nội dung phong tỏa', width: 15, isResize: true, isSort: true },
            { field: 'liberationDate', header: 'Ngày giải tỏa', width: 12, isResize: true, isSort: true, type: TableConst.columnTypes.DATETIME, },
            { field: 'liberator', header: 'Người giải tỏa', width: 12, isSort: true },
            { field: 'liberationDescription', header: 'Nội dung giải tỏa', width: 15, isResize: true, isSort: true },
            { field: 'status', header: 'Trạng thái', width: 7, isFrozen: true, alignFrozen: 'right', type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, isFrozen: true, alignFrozen: 'right', displaySettingColumn: false, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
        ];
        //
    }

    ngOnDestroy(): void {
        if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
    }
  
    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this.__blockageLiberationService.getAll(this.page, this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                if(this.page.pageSize === this.page.pageSizeAll) {
                    // LOAD MORE DATA
                    if(this.page.pageNumberLoadMore === 1) this.rows = [];
                    this.rows = [...this.rows, ...res?.data?.items];
                } else {
                    this.rows = res?.data?.items;
                }
                if (this.rows?.length) {
                this.genListAction(this.rows);
                this.handleDate(this.rows);
                }
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }

    handleDate(rows) {
        for (let row of rows) {
            row.blockadeDescription = row.blockadeDescription;
            row.liberationDescription = row.liberationDescription;
            row.statusElement = BlockageLiberationConst.getStatusInfo(row.status);
        };
    }

    genListAction(data = []) {
        this.listAction = data.map(blockadeLiberation => {
        const actions = [];

        if (this.isGranted([this.PermissionInvestConst.InvestHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa])) {
            actions.push({
                data: blockadeLiberation,
                label: 'Thông tin chi tiết',
                icon: 'pi pi-info-circle',
                command: ($event) => {
                    this.detail($event.item.data);
                }
            })
        }

        if (!this.isPartner && (blockadeLiberation.status == this.BlockageLiberationConst.PHONG_TOA) && this.isGranted([this.PermissionInvestConst.InvestHopDong_PhongToaGiaiToa_GiaiToaHD])) {
            actions.push({
                data: blockadeLiberation,
                label: 'Giải tỏa HĐ',
                icon: 'pi pi-key',
                command: ($event) => {
                    this.liberation($event.item.data);
                }
            })
        }
        
        return actions;
        });
    }

    detail(order) { 
        this.dialogService.open(ContractBlockageDetailComponent, {
            header: 'Thông tin chi tiết',
            width: '800px',
            data: {
                inputData: order
            },
        });
    }
    

    liberation(contractActive) {
        this.blockageLiberation.orderId = contractActive?.orderId;
        this.blockageLiberation.totalValue = contractActive?.totalValue;
        this.blockageLiberation.liberationDate = new Date();
        this.blockageLiberation.id = contractActive?.id;
        this.liberationDialog = true;
    }

    hideDialog() {
        this.liberationDialog = false;
    }
    
    saveLiberation() {
        this.__blockageLiberationService.liberationContractActive(this.blockageLiberation, this.blockageLiberation.id).subscribe((response) => {
            if (this.handleResponseInterceptor(response, "Phong toả thành công")) {
                this.setPage();
                this.liberationDialog = false;
            }
        });
    }
}
