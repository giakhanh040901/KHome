import { Component, Injector, OnInit } from '@angular/core';
import { AtributionConfirmConst, FormNotificationConst, ProjectConst, SearchConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { OwnerServiceProxy } from '@shared/services/owner-service';
import { ProjectServiceProxy } from '@shared/services/project-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Router } from '@angular/router';
import { FilterOwnerComponent } from './filter-owner/filter-owner.component';
import { FilterGeneralContractorComponent } from './filter-general-contractor/filter-general-contractor.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { OBJECT_INVESTOR_EKYC } from '@shared/base-object';
import { FilterTradingProviderComponent } from './filter-trading-provider/filter-trading-provider.component';
import { GeneralContractorService } from '@shared/services/general-contractor.service';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { FormRequestComponent } from 'src/app/form-general/form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from 'src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component';
import { FormCancelComponent } from 'src/app/form-general/form-request-approve-cancel/form-cancel/form-cancel.component';
import { BasicFilter } from '@shared/interface/filter.model';

const { DEFAULT_IMAGE } = OBJECT_INVESTOR_EKYC;
@Component({
	selector: 'app-project',
	templateUrl: './project.component.html',
	styleUrls: ['./project.component.scss']
})
export class ProjectComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private _projectService: ProjectServiceProxy,
		private _ownerService: OwnerServiceProxy,
		private _generalContractorService: GeneralContractorService,
		private confirmationService: ConfirmationService,
		private breadcrumbService: BreadcrumbService,
		private router: Router,
		private _dialogService: DialogService,
	) {
		super(injector, messageService)
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: "Sản phẩm đầu tư", },
		]);
	}

	ref: DynamicDialogRef;

	isLoading: boolean;
	
	page: Page = new Page();
	
	modalDialog: boolean;
	listAction: any[] = []
	owner: any = {}
	ownerId: number;
	projectId:any;
	tradingProviders: any = [];

	tradingProvider = {
		id: 0,
		fakeId: 0,
		tradingProviderId: 0,
		totalInvestmentSub: 0,
	};

	listTradingProvider: any = [];
	generalContractor: any = {};
	generalContractorId: number;
	hasTotalInvestmentSub: boolean;

	project: any = {
		"partnerId": 0,
		"ownerId": 0,
		"generalContractorId": 0,
		"invCode": null,
		"invName": null,
		"content": null,
		"startDate": null,
		"endDate": null,
		"image": null,
		"isPaymentGuarantee": null,
		"area": 0,
		"longitude": 0,
		"latitude": 0,
		"locationDescription": null,
		"totalInvestment": 0,
		"totalInvestmentDisplay": 0,
		"hasTotalInvestmentSub": YesNoConst.NO,
		"projectTypes": [],
		"projectProgress": null,
		"guaranteeOrganization": null,
		"id": 0,
	};

	DEFAULT_IMAGE = DEFAULT_IMAGE;
	projectImage = DEFAULT_IMAGE.IMAGE_PROJECT;

	fieldDates = ['startDate','endDate'];
	
	rows: any[] = [];
	columnProjects: IColumn[] = [];
	columnTradingProviders: IColumn[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();

	ProjectConst = ProjectConst;
	YesNoConst = YesNoConst;
	ProjectTypes = ProjectConst.projectTypes;

	dataFilter: BasicFilter = new BasicFilter();

	ngOnInit(): void {
		this.projectImage = DEFAULT_IMAGE.IMAGE_PROJECT;
		this.columnProjects = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
			{ field: 'invCode', header: 'Mã dự án', width: 15, isPin: true, isSort: true },
			{ field: 'invName', header: 'Tên dự án', width: 30, isPin: true, isResize:true, isSort: true },
			{ field: 'startDate', header: 'Ngày bắt đầu', width: 11,type: TableConst.columnTypes.DATE, isSort: true },
			{ field: 'endDate', header: 'Ngày kết thúc', width: 11, type: TableConst.columnTypes.DATE, isSort: true },
			{ field: 'isCheck', header: 'Kiểm tra', width: 8, type: TableConst.columnTypes.CHECKBOX_SHOW, isSort: true },
			{ field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end b-table-actions' },
		];
		//
		this.columnTradingProviders = [
			{ field: 'id', header: '#ID', width: 3, isPin: true },
			{ field: 'tradingProviderName', header: 'Tên đại lý sơ cấp', width: 25, isPin: true },
			{ field: 'totalInvestmentSub', header: 'Hạn mức', width: 14, type: TableConst.columnTypes.CURRENCY },
			// { field: 'editTradingProvider', header: 'Cập nhật', width: 7, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-pencil' },
			{ field: 'deleteTradingProvider', header: 'Xóa', width: 8.5, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-trash' },
		];
		//
		this.setPage();
		this.isLoading = true;
		this._projectService.getAllTradingProvider().subscribe((resTradingProvider) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(resTradingProvider, '')) {
				
				if (resTradingProvider?.data && resTradingProvider?.data?.length){
					this.tradingProviders = resTradingProvider?.data.map(trading => {
						trading.labelName = trading.aliasName ?? trading.name;

						return trading;
					});
				}
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this._projectService.getAll(this.page, this.dataFilter).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data?.items || [];
				if (this.rows?.length) {
					this.setData(this.rows);
					this.genListAction(this.rows);
				}
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

	setData(rows) {
		for (let row of rows) {
			row.statusElement = ProjectConst.getStatusInfo(row?.status);
		}
	}

	genListAction(data = []) {
		this.listAction = data.map(projectItem => {
			const actions = [];

			if (this.isGranted([this.PermissionInvestConst.InvestSPDT_ThongTinSPDT])) {
				actions.push({
					data: projectItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}

			if (projectItem.status == this.ProjectConst.KHOI_TAO && this.isGranted([this.PermissionInvestConst.InvestSPDT_Xoa])) {
				actions.push({
					data: projectItem,
					label: 'Xoá',
					icon: 'pi pi-trash',
					command: ($event) => {
						this.delete($event.item.data);
					}
				});
			}


			if (projectItem.status == this.ProjectConst.KHOI_TAO && this.isGranted([this.PermissionInvestConst.InvestSPDT_TrinhDuyet])) {
				actions.push({
					data: projectItem,
					label: 'Trình duyệt',
					icon: 'pi pi-arrow-up',
					command: ($event) => {
						this.request($event.item.data);
					}
				});
			}

			// if (projectItem.status == this.ProjectConst.CHO_DUYET && this.isGranted([])) {
			// 	actions.push({
			// 		data: projectItem,
			// 		label: 'Phê duyệt',
			// 		icon: 'pi pi-check',
			// 		command: ($event) => {
			// 			this.approve($event.item.data);
			// 		}
			// 	});
			// }

			if (projectItem.status == this.ProjectConst.HOAT_DONG && projectItem.isCheck === YesNoConst.NO && this.isGranted([this.PermissionInvestConst.InvestSPDT_EpicXacMinh])) {
				actions.push({
					data: projectItem,
					label: 'Phê duyệt (Epic)',
					icon: 'pi pi-check',
					command: ($event) => {
						this.check($event.item.data);
					}
				});
			}

			// if (projectItem.status == this.ProjectConst.CHO_DUYET && this.isGranted([])) {
			// 	actions.push({
			// 		data: projectItem,
			// 		label: 'Hủy duyệt',
			// 		icon: 'pi pi-times',
			// 		command: ($event) => {
			// 			this.cancel($event.item.data);
			// 		}
			// 	});
			// }

			if (this.isGranted([this.PermissionInvestConst.InvestSPDT_Dong])) {
				actions.push({
					data: projectItem,
					label: 'Đóng sản phẩm',
					icon: 'pi pi-lock',
					command: ($event) => {
						this.closeOrOpen($event.item.data);
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

	delete(project) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					data: {
						title : "Bạn có chắc chắn xoá sản phẩm đầu tư này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._projectService.delete(project.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Xóa sản phẩm đầu tư thành công")) {
						this.setPage(this.page);
						this.project = {};
					}
				});
			}
		});
	  }

	create() {
		this.project = {};
		this.submitted = false;
		this.modalDialog = true;
		this.listTradingProvider = [];
	}

	hideDialog() {
		this.projectImage = DEFAULT_IMAGE.IMAGE_PROJECT;
		this.modalDialog = false;
		this.submitted = false;
		this.isLoading = false;
	}

	detail(project) {
		this.router.navigate(['invest-manager/project/detail/' + this.cryptEncode(project?.id)]);
	}

	request(project) {
		const params = {
			id: project.id,
			summary: 'Sản phẩm đầu tư: ' + project?.invCode + ' - ' + project?.invName + ' ( ID: ' + project.id + ' )',
		}
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", params)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._projectService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.setPage();
					}
				});
			}
		});
	}

	approve(project) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", { id: project?.id })
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._projectService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	// Api Epic kiểm tra
	check(project) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt sản phẩm này không?',
			...AtributionConfirmConst,
			accept: () => {
				this._projectService.check({ id: project?.id }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			},
			reject: () => {
			},
		});
	}

	cancel(project) {
		const ref = this.dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC("Hủy phê duyệt", { id: project?.id })
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._projectService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	closeOrOpen(project) {
		let message = project.status == this.ProjectConst.DONG ? 'Bạn có muốn mở sản phẩm?' : 'Bạn có muốn đóng sản phẩm?';
		this.confirmationService.confirm({
			message: message,
			...AtributionConfirmConst,
			accept: () => {
				this._projectService.closeOrOpen(project?.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Mở thành công")) {
						this.setPage();
					}
				});
			},
			reject: () => {
			}
		});
	}

	// changeTradingProvider(tradingProviderId) {
	// 	this.tradingProvider = this.tradingProviders.find(item => item.tradingProviderId == tradingProviderId);
	// 	console.log({ tradingProvider: this.tradingProvider, value: tradingProviderId });
	// }

	showTrading() {
		const ref = this.dialogService.open(FilterTradingProviderComponent,
			{
				header: 'Thêm đại lý sơ cấp',
				width: '500px',
				height: '250px',
				data: {
					hasTotalInvestmentSub: this.hasTotalInvestmentSub,
				}, 
			});

			ref.onClose.subscribe((tradingProvider) => {
				let id = this.tradingProvider?.fakeId || this.tradingProvider?.id;
				if (tradingProvider != null) {
					if(this.listTradingProvider.length > 0) {
						let test = this.listTradingProvider.findIndex(element => element.tradingProviderId === tradingProvider.tradingProviderId);
						this.listTradingProvider.splice(test,test+1);
					}
					if (!id) {
						this.listTradingProvider.push({
							fakeId: new Date().getTime(),
							tradingProviderId: tradingProvider?.tradingProviderId,
							tradingProviderName: tradingProvider?.tradingProviderName,
							totalInvestmentSub: tradingProvider?.totalInvestmentSub,
							// editTradingProvider: (trading) => this.editTradingProvider(trading),
							deleteTradingProvider: (trading) => this.deleteTradingProvider(trading),
						})
					} 
						
				}
			});
	}

	editTradingProvider(data){
		const ref = this.dialogService.open(FilterTradingProviderComponent,
			{
				header: 'Sửa đại lý sơ cấp',
				width: '500px',
				data: {
					inputData: data,
					hasTotalInvestmentSub: this.hasTotalInvestmentSub,
				}, 
				height: '400px',
			});

			ref.onClose.subscribe((tradingProvider) => {
			});
		
	}

	deleteTradingProvider(trading) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					data: {
						title : "Bạn có chắc chắn xoá đại lý này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this.removeElement(this.listTradingProvider, trading);
                this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!' });
            }
		});
	}

	showOwner() {
		const ref = this.dialogService.open(FilterOwnerComponent,
			{
				header: 'Tìm kiếm chủ đầu tư',
				width: '60%',
				contentStyle: {'padding-bottom': '10px'}
			});

		ref.onClose.subscribe((ownerId) => {
			if(ownerId) {
				this.changeOwner(ownerId);
				this.project.ownerId = ownerId;
			} 
		});
	}

	changeOwner(ownerId) {
		this.isLoading = true;
		if (ownerId != null) {
			this._ownerService.getOwner(ownerId).subscribe(
				(item) => {
					this.isLoading = false
					if (this.handleResponseInterceptor(item, '')) {
						this.owner = item?.data;
						this.ownerId = item?.data.ownerId;
					}
				}, (err) => {
					this.isLoading = false;
					console.log('Error-------', err);
				}
			);
		}
	}

	showGeneralContractor() {
		this.dialogService.open(
			FilterGeneralContractorComponent,
			{
				header: 'Tìm kiếm đơn vị tổng thầu',
				width: '60%',
				contentStyle: {'padding-bottom': '10px'}
			}).onClose.subscribe((generalContractorId) => {
				if(generalContractorId){
					this.changeGeneralContractor(generalContractorId);
					this.project.generalContractorId = generalContractorId;
				}
			}
		);
	}

	changeGeneralContractor(generalContractorId) {
		this.isLoading = true;
		if (generalContractorId != null) {
			this._generalContractorService.getContractor(generalContractorId).subscribe(
				(item) => {
					this.isLoading = false
					if (this.handleResponseInterceptor(item, '')) {
						this.generalContractor = item.data;
						this.generalContractorId = item.data.generalContractorId;
					}
				}, (err) => {
					this.isLoading = false;
					console.log('Error-------', err);
				}
			);
		}
	}

	removeElement(array, elem) {
        var index = array.indexOf(elem);
        if (index > -1) {
            array.splice(index, 1);
        }
    }

	save() {
		this.submitted = true;
		this.project.hasTotalInvestmentSub = this.hasTotalInvestmentSub ? YesNoConst.YES : YesNoConst.NO;
		this.project.projectTradingProvider = this.listTradingProvider;
		let body = this.formatCalendar(this.fieldDates, {...this.project});
		this.isLoadingPage = true;
		this._projectService.create(body).subscribe((response) => {
				if (this.handleResponseInterceptor(response, 'Thêm thành công') && response) {
					this.projectId = response?.data.id;
					setTimeout(() => {
						this.router.navigate(['/invest-manager/project/detail/', this.cryptEncode(this.projectId)]);
					}, 1000);
				} 
				this.submitted = false;
				this.isLoadingPage = false;
			}, () => {
				this.submitted = false;
			}
		);
	}

	validForm(): boolean {
        const validRequired = this.project?.invName?.trim() && this.project?.invCode.trim()
            && this.project?.guaranteeOrganization?.trim() && this.project.startDate < this.project.endDate;
        return validRequired;
    }
}
