import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { AtributionConfirmConst, DeliveryContractConst, OrderConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DeliveryContractService } from '@shared/services/delivery-contract.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { OrderService } from '@shared/services/order.service';
import { DataTableEmit, IAction, IColumn } from '@shared/interface/p-table.model';
import { Observable, Subscription } from 'rxjs';
import { OrderDeliveryFilter } from '@shared/interface/filter.model';

@Component({
  selector: 'app-delivery-contract',
  templateUrl: './delivery-contract.component.html',
  styleUrls: ['./delivery-contract.component.scss']
})
export class DeliveryContractComponent extends CrudComponentBase implements OnInit {

    constructor(
        injector: Injector,
        messageservice: MessageService,
        private _orderService: OrderService,
        private _deliveryContractService: DeliveryContractService,
        private breadcrumbService: BreadcrumbService,
        private confirmationService: ConfirmationService,
        private _tradingProviderSelectedService: TradingProviderSelectedService,
    ) {
        super(injector, messageservice);
        this.breadcrumbService.setItems([
        { label: 'Trang chủ', routerLink: ['/home'] },
        { label: 'Hợp đồng phân phối' },
        { label: 'Giao nhận hợp đồng' },
        ]);
    }

    liberationDialog: boolean;

    rows: any[] = [];
    columns: IColumn[] = [];

    listAction: IAction[][] = [];

    dataFilter: OrderDeliveryFilter = new OrderDeliveryFilter();

    page = new Page();

    order: any = {};
    // Menu otions thao tác
    DeliveryContractConst = DeliveryContractConst;
    OrderConst = OrderConst;
    
    isPartner: boolean;
    tradingProviderSub: Subscription;

    dataTableEmit: DataTableEmit = new DataTableEmit();

    ngOnInit() {
        this.isPartner = this.getIsPartner();
        
        this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change: number[]) => {
            this.dataFilter.tradingProviderIds = change;
            this.setPage();
        });

        this.columns = [
            { field: '', header: '', width: 3, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, type: TableConst.columnTypes.CHECKBOX_ACTION },
            { field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true  },
            { field: 'project.invCode', header: 'Mã dự án', width: 15, isPin: true, isSort: true },
            { field: 'contractCode', header: 'Mã hợp đồng', width: 10.5, isPin: true, isSort: true },
            { field: 'paymentFullDate', header: 'Ngày đầu tư', width: 10, type: TableConst.columnTypes.DATE, isPin: true, isSort: true },
            { field: 'cifCodes.investorId', header: 'Loại KH', width: 8, isSort: true },
            { field: 'nameDisplay', header: 'Khách hàng', width: 18, isSort: true },
            { field: 'policyDetailName', header: 'Kỳ hạn', width: 7, class: 'justify-content-end text-right', isSort: true },
            { field: 'pendingDate', header: 'Ngày gửi y/c', width: 10, isSort: true, type: TableConst.columnTypes.DATE, },
            { field: 'deliveryDate', header: 'Ngày giao', width: 9, isSort: true, type: TableConst.columnTypes.DATE, },
            { field: 'receivedDate', header: 'Ngày nhận', width: 9, isSort: true, type: TableConst.columnTypes.DATE, },
            { field: 'finishedDate', header: 'Ngày hoàn thành', width: 12, isSort: true, type: TableConst.columnTypes.DATE, },
            { field: 'sourceOnline', header: 'Online', width: 5, type: TableConst.columnTypes.CHECKBOX_SHOW },
            { field: 'sourceOffline', header: 'Offline', width: 5, type: TableConst.columnTypes.CHECKBOX_SHOW },
            { 
                field: 'deliveryStatus', header: 'Trạng thái', width: 8, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, 
                type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' 
            },
			{ 
                field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, 
                type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' 
            },
        ];
    }

    ngOnDestroy(): void {
        if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this._orderService.getAllDeliveryContract(this.page, this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                if(this.page.pageSize === this.page.pageSizeAll) {
                    if(this.page.pageNumberLoadMore === 1) this.rows = [];
                    this.rows = [...this.rows, ...res?.data?.items];
                } else {
                    this.rows = res?.data?.items;
                }
                if (this.rows?.length) {
                    this.setData(this.rows);
                    this.genListAction(this.rows);
                }
            }
        });
    }

    setData(rows) {
        for (let row of rows) {
            row.policyDetailName = row.policyDetail?.name;
            row.nameDisplay = (row?.investor ? row.investor?.investorIdentification?.fullname : null) || (row?.businessCustomer ? row?.businessCustomer?.name : null); 
            row.contractCode = row?.genContractCode || row?.contractCode;
            row.sourceOnline = row.source === OrderConst.SOURCE_ONLINE;
            row.sourceOffline = row.source === OrderConst.SOURCE_OFFLINE;
            row.deliveryStatusElement = DeliveryContractConst.getStatus(row.deliveryStatus)
            row['project.invCode'] = row?.project.invCode;
            row['cifCodes.investorId'] = row.customerType;
        };
    }

    genListAction(data = []) {
        this.listAction = data.map(blockadeLiberation => {
        const actions: IAction[] = [];

        if (this.isGranted([this.PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_ThongTinChiTiet])) {
            actions.push({
                data: blockadeLiberation,
                label: 'Thông tin chi tiết',
                icon: 'pi pi-info-circle',
                command: ($event) => {
                    this.detail($event.item.data);
                }
            })
        }

        if (!this.isPartner && this.isGranted([this.PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_XuatHopDong])) {
            actions.push({
                data: blockadeLiberation,
                label: 'Xuất hợp đồng',
                icon: 'pi pi-download',
                command: ($event) => {
                    this.exportContracts([$event.item.data]);
                }
            })
        }

        if (!this.isPartner && blockadeLiberation.deliveryStatus == this.DeliveryContractConst.CHO_XU_LY) {
            actions.push({
                data: blockadeLiberation,
                label: 'Đang giao',
                icon: 'pi pi-send',
                command: ($event) => {
                    this.delivered($event.item.data);
                }
            })
        }
        if (!this.isPartner && blockadeLiberation.deliveryStatus == this.DeliveryContractConst.DANG_GIAO) {
            actions.push({
                data: blockadeLiberation,
                label: 'Đã nhận',
                icon: 'pi pi-send',
                command: ($event) => {
                    this.deliveryStatusReceived($event.item.data);
                }
            })
        }

        if (!this.isPartner && blockadeLiberation.deliveryStatus == this.DeliveryContractConst.DA_NHAN) {
            actions.push({
                data: blockadeLiberation,
                label: 'Hoàn thành',
                icon: 'pi pi-send',
                command: ($event) => {
                    this.deliveryStatusDone($event.item.data);
                }
            })
        }
        return actions;
        });
    }

    exportContracts(fieldExports: any[]) {
        fieldExports.forEach((data, index) => {
            let params = {
                orderId: data.id,
                distributionId: data.distributionId,
                tradingProviderId: data.tradingProviderId,
                source: data.source,
            };
            //
            this.isLoading = true;
            this._deliveryContractService.exportContractReceive(params).subscribe((response) => {
                this.isLoading = false;
                this.handleResponseInterceptor(response);
            }, (err) => {
                this.isLoading = false;
                this.messageError('Có lỗi xảy ra. Vui lòng thử lại sau!', '');
            });
        })
    }

    delivered(order) {
        this.confirmDialog(
            'Xác nhận chuyển trạng thái sang Đang giao?',
            this._deliveryContractService.deliveryStatusDelivered(order.id)
        );
    }
    
    deliveryStatusReceived(order) {
        this.confirmDialog(
            'Xác nhận chuyển trạng thái sang Đã nhận?',
            this._deliveryContractService.deliveryStatusReceived(order.id)
        );
    }

    deliveryStatusDone(order) {
        this.confirmDialog(
            'Xác nhận chuyển trạng thái sang Hoàn thành?',
            this._deliveryContractService.deliveryStatusDone(order.id)
        );
    }

    confirmDialog(message: string, apiCall: Observable<number>) {
        this.confirmationService.confirm({
            message: message,
            ...AtributionConfirmConst,
            accept: () => {
                apiCall.subscribe((response) => {
                if (this.handleResponseInterceptor(response, "Cập nhật thành công!")) {
                    this.setPage();
                }
                });
            },
        });
    }

    detail(order) {
        // this.router.navigate(['/trading-contract/delivery-contract/detail/' + this.cryptEncode(order?.id)]);
        let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.id)); 
            window.open('/trading-contract/order/detail-view/' + (cryptEncodeId), "_blank");
    }

    hideDialog() {
        this.liberationDialog = false;
    }

}
