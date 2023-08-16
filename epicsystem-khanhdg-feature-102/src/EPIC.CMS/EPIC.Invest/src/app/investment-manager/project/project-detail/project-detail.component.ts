import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts, AtributionConfirmConst, FormNotificationConst, ProjectConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { OwnerServiceProxy } from '@shared/services/owner-service';
import { ProjectServiceProxy } from '@shared/services/project-manager-service';
import { ConfirmationService } from 'primeng/api';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { TabView } from 'primeng/tabview';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { FormApproveComponent } from 'src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component';
import { FormCancelComponent } from 'src/app/form-general/form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormRequestComponent } from 'src/app/form-general/form-request-approve-cancel/form-request/form-request.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { FilterTradingProviderComponent } from '../filter-trading-provider/filter-trading-provider.component';
import { ContractTemplateService } from '@shared/services/contract-template.service';
import { GeneralContractorService } from '@shared/services/general-contractor.service';
import { forkJoin } from 'rxjs';
import { IColumn } from '@shared/interface/p-table.model';
@Component({
	selector: 'app-project-detail',
	templateUrl: './project-detail.component.html',
	styleUrls: ['./project-detail.component.scss']
})
export class ProjectDetailComponent extends CrudComponentBase {


	constructor(
		injector: Injector,
		messageService: MessageService,
		private _projectService: ProjectServiceProxy,
		private routeActive: ActivatedRoute,
		private confirmationService: ConfirmationService,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
		private ownerService: OwnerServiceProxy,
		private generateContractorService: GeneralContractorService,
		private _contractTemplateService: ContractTemplateService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Sản phẩm đầu tư", routerLink: ['/invest-manager/project'] },
			{ label: "Chi tiết sản phẩm đầu tư" },
		]);
		this.projectId = + this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}

	AppConsts = AppConsts;
	ProjectConst = ProjectConst;

	YesOrNo = YesNoConst.list;

	ProjectTypes = ProjectConst.projectTypes;
	startDate: any;

	isEdit = false;
	fieldErrors = {};
	fieldDates = ['startDate', 'endDate'];
	projectId: number;
	project: any = {}
	endDate: any;
	labelButtonEdit = "Chỉnh sửa";
	projectDetail: any = {
		"projectTypes": [
		],
	};
	tradingProviders: any = [];
	tradingProvider = {
		id: 0,
		fakeId: 0,
		tradingProviderId: 0,
		totalInvestmentSub: 0,
	};

	listTradingProvider: any = [];
	deleteItemDialog: boolean = false;
	checked: boolean = true;
	rows: any[] = [];
	actions: any[] = [];  // list button actions
	actionsDisplay: any[] = [];
	//
	owner: any = {};
	generateContractor: any = {};
	businessCustomer: any = {};
	// tradingProviders: any = [];
	// tradingProvider = {};
	tradingProviderId = null;
	issuerDetail: any = {};
	imageDefault = AppConsts.imageDefault;

	hasTotalInvestmentSub: boolean = false;

	projectUpdate = {
		"id": 0,
		"ownerId": null,
		"generalContractorId": null,
		"invCode": null,
		"invName": null,
		"content": null,
		"startDate": null,
		"endDate": null,
		"image": null,
		"isPaymentGuarantee": null,
		"area": null,
		"longitude": null,
		"latitude": null,
		"locationDescription": null,
		"totalInvestment": null,
		"totalInvestmentDisplay": null,
		"projectType": null,
		"projectProgress": null,
		"guaranteeOrganization": null,
		"hasTotalInvestmentSub": null,
		"projectTypes": null,
		"projectTradingProvider": null,
	};

	tabViewActive = {
		'thongTinChung': true,
		'hinhAnhDauTu': false,
		'hoSoPhapLy': false,
		'tinTucSanPham': false,
		'share': false
	};
	
	@Input() contentHeights: number[] = [];
	@ViewChild(TabView) tabView: TabView;

	columnTradingProviders: IColumn[] = [];

	ngOnInit() {
		this.columnTradingProviders = [
			{field: 'tradingProviderName', header: 'Địa lý sơ cấp', width: 25, isResize: true },
			{field: 'aliasName', header: 'Tên alias', width: 20, },
			{field: 'totalInvestmentSub', header: 'Hạn mức', width: 15, type: TableConst.columnTypes.CURRENCY },
			{field: 'editTradingProvider', header: 'Cập nhật', width: 7, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-pencil', classButton: 'p-button-rounded p-button-text', class: 'justify-content-center' },
			{field: 'deleteTradingProvider', header: 'Xóa', width: 5, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-trash', classButton: 'p-button-rounded p-button-text', class: 'justify-content-center' },
		];
		//
		this.getDetail();
	}

	setDataTradingProviders(rows) {
		this.listTradingProvider = rows.map(row => {
			row.tradingProviderName = row.tradingProvider.businessCustomer.name ;
			row.aliasName = row?.tradingProvider?.aliasName;
			row.editTradingProvider = (row) => this.editTradingProvider(row);
			row.deleteTradingProvider = (row) => this.deleteTradingProvider(row);
			return row;
		})
	}

	editTradingProvider(data){
		const ref = this.dialogService.open(
			FilterTradingProviderComponent,
			{
				header: 'Sửa đại lý sơ cấp',
				width: '500px',
				data: {
					inputData: data,
					hasTotalInvestmentSub: this.hasTotalInvestmentSub,
				}, 
			}
		);
		ref.onClose.subscribe((tradingProvider) => {
			
		});
		
	}

	deleteTradingProvider(item) {
		const ref = this.dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					data: {
						title : "Bạn có chắc chắn xoá đại lý sơ cấp này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this.removeElement(this.listTradingProvider, item);
                this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!' });
            }
		});
	}

	showTrading() {
		const ref = this.dialogService.open(FilterTradingProviderComponent,
			{
				header: 'Thêm đại lý sơ cấp',
				width: '500px',
				data: {
					hasTotalInvestmentSub: this.hasTotalInvestmentSub,						
				}, 
			});

			ref.onClose.subscribe((tradingProvider) => {
				let id = this.tradingProvider?.fakeId || this.tradingProvider?.tradingProviderId;
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
						});
					} 
				}
			});
	}

	removeElement(array, elem) {
        var index = array.indexOf(elem);
        if (index > -1) {
            array.splice(index, 1);
        }
    }

	openThongTinChung: boolean = true;
	changeTabview(e) {
		let tabHeader = this.tabView.tabs[e.index].header;
		this.openThongTinChung = (tabHeader == 'thongTinChung');
		if(!this.tabViewActive[tabHeader]) {
			this.tabViewActive[tabHeader] = true;
		}
	}

	// Update ảnh đại diện
	myUploader(event) {
		if (event?.files[0]) {
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "project").subscribe((response) => {
				if (this.handleResponseInterceptor(response, "")) {
					this.projectDetail.image = response?.data;
				}
			}, (err) => {
				console.log('err-----', err);
				this.messageError("Có sự cố khi upload!");
			});
		}
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
						this.getDetail();
					}
				});
			}
		});
	}

	approveSharing(investor) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				data: {
					id: investor.id
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				const body1 = {
					approveNote: dataCallBack?.data?.approveNote,
					id: investor.id,
					cancelNote: dataCallBack?.data?.approveNote,
				}
				if (dataCallBack?.checkApprove) {	
					this._projectService.approve(body1).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
							this.getDetail();
						}
					});
				} else {
					this._projectService.cancel(body1).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
							this.getDetail();
						}
					});
				} 
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
						console.log();

						this.getDetail();
					}
				});
			}
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
				this._projectService['cancel'](dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
						this.getDetail();
					}
				});
			}
		});
	}

	// Api Epic kiểm tra
	check(project) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt sản phẩm đầu tư này không?',
			...AtributionConfirmConst,
			accept: () => {
				this._projectService.check({ id: project?.id }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.getDetail();
					}
				});
			},
			reject: () => {
			},
		});
	}

	closeOrOpen(project) {
		let message = project.status === ProjectConst.HOAT_DONG ? 'Bạn có muốn đóng sản phẩm?' : 'Bạn có muốn mở sản phẩm?';
		this.confirmationService.confirm({
			message: message,
			...AtributionConfirmConst,
			accept: () => {
				this._projectService.closeOrOpen(project?.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Mở thành công")) {
						this.getDetail();
					}
				});
			},
		});
	}

	changeEdit() {
		this.projectDetail.projectTradingProvider = this.listTradingProvider;
		if (this.isEdit) {
			this.projectDetail.hasTotalInvestmentSub = this.hasTotalInvestmentSub ? YesNoConst.YES : YesNoConst.NO;
			let body = this.formatCalendar(this.fieldDates, 
				{...this.filterField(this.projectDetail, this.projectUpdate)}
			);
			this._projectService.update(body).subscribe((response) => {
				if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
					this.setStatusEdit();
					this.getDetail(false);
				}
			});
		} else {
			this.setStatusEdit();
		}
	}

	getDetail(isLoading = true) {
		this.isLoading = isLoading;
		forkJoin([this._projectService.get(this.projectId), this._projectService.getProjectTradingProvider(this.projectId)]).subscribe(([resproject, resTradingProvider]) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(resproject, '')) {
				this.projectDetail = {
					...resproject?.data, startDate: new Date(resproject?.data?.startDate),
					endDate: new Date(resproject?.data?.endDate)
				}
				this.hasTotalInvestmentSub = this.projectDetail.hasTotalInvestmentSub == YesNoConst.YES;
				this.tradingProviderId = this.projectDetail?.tradingProviderId;
				this.initOwner(this.projectDetail?.ownerId);
				this.initGeneralContractor(this.projectDetail?.generalContractorId);
			}
			//
			if (this.handleResponseInterceptor(resTradingProvider, '')) {
				if(resTradingProvider?.data && resTradingProvider?.data?.length) {
					this.setDataTradingProviders(resTradingProvider?.data);
				}
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		})
	}
	
	delete() {
		this.deleteItemDialog = true;
	}
	
	setStatusEdit() {
		this.isEdit = !this.isEdit;
		this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
		this.editorConfig.editable = this.isEdit;
	}
	
	initOwner(ownerId) {
		this.ownerService.getOwner(ownerId).subscribe((res) => {
			this.owner = res?.data;
		}, (err) => {
			console.log('Error-------', err);
		});
	}

	initGeneralContractor(generalContractorId) {
		this.generateContractorService.getContractor(generalContractorId).subscribe((res) => {
			this.generateContractor = res?.data
		}, (err) => {
			console.log('Error-------', err);
		});
	}
}
