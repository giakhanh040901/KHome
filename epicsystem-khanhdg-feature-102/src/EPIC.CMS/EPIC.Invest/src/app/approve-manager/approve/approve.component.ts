import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { IssuerConst, DistributionContractConst, ApproveConst, TableConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ProjectServiceProxy } from "@shared/services/project-manager-service";
import { ApproveService } from "@shared/services/approve.service";
import { DistributionService } from "@shared/services/distribution.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { FormApproveComponent } from "src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DataTableEmit, IAction, IColumn } from "@shared/interface/p-table.model";
import { ApproveFilter } from "@shared/interface/filter.model";

@Component({
	selector: "app-approve",
	templateUrl: "./approve.component.html",
	styleUrls: ["./approve.component.scss"],
	providers: [DialogService, ConfirmationService, MessageService],
})
export class ApproveComponent extends CrudComponentBase implements OnDestroy {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private router: Router,
		private _projectService: ProjectServiceProxy,
		private routeActive: ActivatedRoute,
		private approveService: ApproveService,
		private breadcrumbService: BreadcrumbService,
		private _distributionService: DistributionService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Quản lý duyệt", routerLink: ["/approve-manager/approve/1"] },
		]);
	}

	ApproveConst = ApproveConst;
	IssuerConst = IssuerConst;
	DistributionContractConst = DistributionContractConst;

	approve: any = {};
	routeSubcribe: any = null;
	//
	tradingProviders: any[] = [];
	page = new Page();
	rows: any[] = [];
	columns: IColumn[] = [];
	listAction: IAction[][] = [];

	dataTableEmit: DataTableEmit = new DataTableEmit();

	dataFilter: ApproveFilter = new ApproveFilter();

	ngOnInit(): void {
		this.isLoading = true;
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params?.dataType) {
				this.dataFilter.dataType = +params.dataType;
				this.setPage();
			}
		});

		this.columns = [
			{ field: 'action', header: 'Action', width: 5, type: TableConst.columnTypes.STATUS, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT,},
			{ field: 'investmentProduct', header: 'Sản phẩm đầu tư', width: 20, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT },
			{ field: 'summary', header: 'Nội dung duyệt', width: 25, isResize: true },
			{ field: 'requestDate', header: 'Ngày yêu cầu', width: 12, type: TableConst.columnTypes.DATETIME },
			{ field: 'confirmDate', header: 'Ngày duyệt/ hủy', width: 12, type: TableConst.columnTypes.DATETIME },
			{ field: 'requestNote', header: 'Ghi chú trình duyệt', width: 18 },
			{ field: 'approveNote', header: 'Ghi chú duyệt', width: 18 },
			{ field: 'cancelNote', header: 'Ghi chú hủy duyệt', width: 18 },
			{ field: 'isCheck', header: 'EPIC duyệt', width: 10, type: TableConst.columnTypes.CHECKBOX_SHOW},
			{ field: 'status', header: 'Trạng thái', width: 8, type: TableConst.columnTypes.STATUS, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
		];
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this.approveService.getAll(this.page, this.dataFilter).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
				this.page.totalItems = res.data.totalItems;
				this.rows = (res.data?.items || []).map(item => {
					return {
						...item,
						confirmDate: item.approveDate || item.cancelDate,
					}
				});
				if (this.rows?.length) {
					this.setData(this.rows);
					this.genListAction(this.rows);
				}
			}
		},
			(err) => {
				this.isLoading = false;
				console.log('Error-------', err);
			}
		);
	}

	setData(rows) {
		for (let row of rows) {
			row.actionElement = ApproveConst.getActionTypeName(row?.actionType),
			row.statusElement = ApproveConst.getStatusInfo(row.status);
		};
	}

	getAllTradingProvider() {
		this.approveService.getAllTradingProvider().subscribe( (res) => {
			if (this.handleResponseInterceptor(res, '')) {
			  this.isLoadingPage = false;
			  this.tradingProviders = res?.data?.items; 
			}
		})
	}

	genListAction(data = []) {
		this.listAction = data.map((item) => {
			const actions = [
				{
					data: item,
					label: "Thông tin chi tiết",
					icon: "pi pi-info-circle",
					command: ($event) => {
						this.detail($event.item.data);
					},
				}
			];
			
			if ((item?.status == ApproveConst.STATUS_REQUEST) && 
				((this.isGranted([this.PermissionInvestConst.InvestPDSPDT_PheDuyetOrHuy]) && this.dataFilter.dataType === ApproveConst.PROJECT) 
				|| (this.isGranted([this.PermissionInvestConst.InvestPDPPDT_PheDuyetOrHuy]) && this.dataFilter.dataType === ApproveConst.DISTRIBUTION))
			) {
				actions.push({
					data: item,
					label: "Phê duyệt",
					icon: "pi pi-check",
					command: ($event) => {
						this.approveSharing($event.item.data);
					},
				});
			}
			return actions;
		});
	}

	approveSharing(approve) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				data: {
					id: approve.referId
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				const body = {
					approveNote: dataCallBack?.data?.approveNote,
					id: approve.referId,
					cancelNote: dataCallBack?.data?.approveNote,
				}

				let isApprove = dataCallBack?.checkApprove === true;
				let typeApprove = isApprove ? 'approve' : 'cancel';
				if (approve.dataType === ApproveConst.PROJECT) {
					this._projectService[typeApprove](body).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Thao tác thành công")) {
							this.setPage();
						}
					});
				} else if (approve.dataType === ApproveConst.DISTRIBUTION) {
					this._distributionService[typeApprove](body).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Thao tác thành công")) {
							this.setPage();
						}
					});
				} 
			}
		});
	}

	
	onSort(event) {
		this.sortData = event;
		this.setPage();
	}

	ngOnDestroy(): void {
		this.routeSubcribe.unsubscribe();
	}

	detail(approve) {
		let url_: string;
		if(this.dataFilter.dataType === ApproveConst.PROJECT) url_="/invest-manager/project/detail/";
		if(this.dataFilter.dataType === ApproveConst.DISTRIBUTION) url_="/invest-manager/distribution/detail/";
		if(url_) this.router.navigate([url_ + this.cryptEncode(approve.referId)]);
	}

}
