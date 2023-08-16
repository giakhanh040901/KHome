import { CompanySharePolicyServiceProxy, CompanyShareSecondaryServiceProxy } from "../../../../shared/service-proxies/company-share-manager-service";
import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import { FormNotificationConst, PermissionCompanyShareConst, CompanyShareInterestConst, CompanyShareSecondaryConst, ProductPolicyConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DialogService } from "primeng/dynamicdialog";
import { DynamicDialogRef } from "primeng/dynamicdialog";
import { ActivatedRoute, Params } from "@angular/router";
import { forkJoin } from "rxjs";
import { OJBECT_SECONDARY_CONST } from "@shared/base-object";
import { Dialog } from "primeng/dialog";
import { FormCancelComponent } from "src/app/form-request-approve-cancel/form-cancel/form-cancel.component";
import { FormNotificationComponent } from "src/app/form-notification/form-notification.component";
import { TabView } from "primeng/tabview";
import { FormRequestComponent } from "src/app/form-request-approve-cancel/form-request/form-request.component";
import { FormCompanyShareInfoApproveComponent } from "src/app/approve-manager/approve/form-company-share-info-approve/form-company-share-info-approve.component";
const { POLICY, POLICY_DETAIL, BASE } = OJBECT_SECONDARY_CONST;

@Component({
	selector: "app-company-share-secondary-update",
	templateUrl: "./company-share-secondary-update.component.html",
	styleUrls: ["./company-share-secondary-update.component.scss"],
	providers: [DialogService],
})
export class CompanyShareSecondaryUpdateComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		private route: ActivatedRoute,
		private routeActive: ActivatedRoute,
		private _secondaryService: CompanyShareSecondaryServiceProxy,
		private _policyService: CompanySharePolicyServiceProxy,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Bán theo kỳ hạn", routerLink: ['/company-share-manager/product-company-share-secondary'] },
			{ label: "Chi tiết bán theo kỳ hạn" },
		]);
		this.secondaryId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));

	}

	ref: DynamicDialogRef;

	secondaryId: number;

	modalDialog: boolean;
	modalDialogPolicy: boolean;
	modalDialogPolicyDetail: boolean;
	policyDetails:any[] = [];
	deleteItemDialog: boolean = false;
	actions: any[] = [];
	actionsDisplay: any[] = [];

	deleteItemsDialog: boolean = false;
	rows: any[] = [];

	// Data Init
	listPrimary: any = [];
	ProductSecondaryConst = CompanyShareSecondaryConst;
	ProductPolicyConst = ProductPolicyConst;
	CompanyShareInterestConst = CompanyShareInterestConst;
	YesNoConst = YesNoConst;

	// Row Expand
	expandedRows = {};
	isExpanded: boolean = false;

	// EDIT
	isEdit: boolean = false;

	// TAB VIEW
	activeIndex = 0;
	// MAIN
	companyShareSecondary: any = { ...BASE.SECODARY };
	policy: any = { ...BASE.POLICY };
	policyDetail: any = { ...BASE.POLICY_DETAIL };
	/**
	 * Dùng cho search policy và detail base
	 *  */
	search = {
		policy: {
			companySharePolicyDetailTemp: [],
		},
		detail: {},
		listPolicy: [],
		listDetails: [],
	};
	policyInit: any = { ...BASE.POLICY };
	submitted: boolean;

	listBanks: any[] = [];

	cols: any[];

	statuses: any[];
	policyId: any;
	
	onPolicyDetailTable:boolean = false; //bien an table của kỳ hạn

	createClick: boolean = true; //biến hiện nut them chinh sach

	updateClick: boolean = false; //bien hien nut luu = cap nhap chinh sach

	listPolicyDetailAction: any[] = [];
	listPolicyAction: any[] = [];
	fieldDates = ["openCellDate", "closeCellDate"];
	fieldDatePolicies = ["startDate", "endDate"];

	page = new Page();
	offset = 0;

	minDate = null; // type new Date()
	maxDate = null;	// type new Date()

	tabViewActive = {
		'thongTinChung': true,
		'tongQuan': false,
		'chinhSach': false,
		'bangGia': false,
		'fileChinhSach': false,
		'mauHopDong': false,
		'mauGiaoNhanHD': false,
	};
	
	@ViewChild(TabView) tabView: TabView;

	
	/* METHODS */

	ngOnInit() {
		this.route.params.subscribe((params: Params) => {
			this.companyShareSecondary.secondaryId = this.cryptDecode(params?.id);
			this.initData(this.cryptDecode(params?.id));
		});
	}

	checkEpic(companyShareSecondary) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô cổ phần này không?',
			header: 'Thông báo',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-check-circle',
			accept: () => {
				this._secondaryService.check({ id: companyShareSecondary?.secondaryId }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.initData(companyShareSecondary.secondaryId);
					}
				});
			},
			reject: () => {
			},
		});
	}

	changeTab(tabIndex) {
		let tabHeader = this.tabView.tabs[tabIndex.index].header;
		this.tabViewActive[tabHeader] = true;
	}

	approveCompanyShare(companyShareData) {
		console.log("companyShareDataaa",companyShareData);
		
		const ref = this.dialogService.open(
			FormCompanyShareInfoApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: companyShareData
			}
		);

		ref.onClose.subscribe((dataCallBack) => {

			if (dataCallBack?.accept) {
				if (dataCallBack.checkApprove == true) {
					this._secondaryService.approve(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
							this.initData(companyShareData.secondaryId);
						}
					});
				}
				else {
					this._secondaryService.cancel(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Huỷ duyệt thành công!")) {
							this.initData(companyShareData.secondaryId);
						}
					});
				}
			}
		});
	}

	genListAction(secondaryItem) {
		this.actions = [];
		// if (secondaryItem.status == this.ProductSecondaryConst.STATUS.TRINH_DUYET && this.isGranted()) {
		// 	this.actions.push({
		// 		data: secondaryItem,
		// 		label: 'Phê duyệt',
		// 		icon: 'pi pi-check',
		// 		command: ($event) => {
		// 			this.approve($event.item.data?.secondaryId);
		// 		}
		// 	});
		// }

		if (secondaryItem.status == this.ProductSecondaryConst.STATUS.NHAP ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Trình duyệt',
				icon: 'pi pi-forward',
				command: ($event) => {
					this.submit($event.item.data?.secondaryId);
				}
			});
		}

		// if (secondaryItem.status == this.ProductSecondaryConst.STATUS.TRINH_DUYET && this.isGranted()) {
		// 	this.actions.push({
		// 		data: secondaryItem,
		// 		label: 'Huỷ duyệt',
		// 		icon: 'pi pi-forward',
		// 		command: ($event) => {
		// 			this.cancel($event.item.data?.secondaryId);
		// 		}
		// 	});
		// }

		if (secondaryItem.status == this.ProductSecondaryConst.STATUS.HOAT_DONG && secondaryItem.isCheck == "N" ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Phê duyệt (Epic)',
				icon: 'pi pi-check',
				command: ($event) => {
					
					this.check($event.item.data);
				}
			});
		}

		if (secondaryItem.isClose == YesNoConst.STATUS_NO ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Đóng tạm',
				icon: 'pi pi-lock',
				command: ($event) => {
					this.toggleClosed($event.item.data?.secondaryId, YesNoConst.STATUS_YES);
				}
			});
		}

		if (secondaryItem.isClose == YesNoConst.STATUS_YES ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Mở',
				icon: 'pi pi-unlock',
				command: ($event) => {
					this.toggleClosed($event.item.data?.secondaryId, YesNoConst.STATUS_NO);
				}
			});
		}

		if (secondaryItem.isShowApp == YesNoConst.STATUS_NO ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Bật show app',
				icon: 'pi pi-eject',
				command: ($event) => {
					this.toggleIsShowApp($event.item.data?.secondaryId, YesNoConst.STATUS_YES);
				}
			});
		}

		if (secondaryItem.isShowApp == YesNoConst.STATUS_YES ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Tắt show app',
				icon: 'pi pi-eye-slash',
				command: ($event) => {
					this.toggleIsShowApp($event.item.data?.secondaryId, YesNoConst.STATUS_NO);
				}
			});
		}
	}

	genListPolicyAction(data = []) {
		if(data) {
			this.listPolicyAction = data.map(policyItem => {
				const actions = [];
				
				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_KyHan_ThemMoi])){
					actions.push({
						data: policyItem,
						label: 'Thêm kỳ hạn',
						icon: 'pi pi-plus-circle',
						command: ($event) => {
							this.createPolicyDetail($event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_ChinhSach_CapNhat])){
					actions.push({
						data: policyItem,
						label: 'Sửa',
						icon: 'pi pi-pencil',
						command: ($event) => {
							this.editPolicy($event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_ChinhSach_Xoa])){
					actions.push({
						data: policyItem,
						label: 'Xoá',
						icon: 'pi pi-trash',
						command: ($event) => {
							this.deletePolicy($event.item.data);
						}
					})
				}
				//
				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_ChinhSach_BatTatShowApp])) {
					const valueIsShowApp = policyItem.isShowApp == YesNoConst.STATUS_YES ? YesNoConst.STATUS_NO : YesNoConst.STATUS_YES;
					actions.push({
						data: policyItem,
						label: policyItem.isShowApp == YesNoConst.STATUS_YES ? 'Tắt show app' : 'Bật show app',
						icon: policyItem.isShowApp == YesNoConst.STATUS_YES ? 'pi pi-eye-slash' : 'pi pi-eye',
						command: ($event) => {
							this.toggleIsShowAppPolicy($event.item.data, valueIsShowApp);
						}
					});
				}
				//
				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_ChinhSach_KichHoatOrHuy])) {
					actions.push({
						data: policyItem,
						label: policyItem.status == this.ProductSecondaryConst.KHOA ? 'Kích hoạt' : 'Hủy kích hoạt',
						icon: policyItem.status == this.ProductSecondaryConst.KHOA ? 'pi pi-check' : 'pi pi-times-circle',
						command: ($event) => {
							this.changeStatusPolicy($event.item.data);
						}
					});
				}
				
				return actions;
			});
		}
	}
	//
	genListPolicyDetailAction(data = []) {		
		if(data) {
			this.listPolicyDetailAction = data.map(policyDetailItem => {
				const actions = [];

				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_KyHan_CapNhat])){
					actions.push({
						data: policyDetailItem,
						label: 'Sửa',
						icon: 'pi pi-pencil',
						command: ($event) => {
							this.editPolicyDetail($event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_KyHan_Xoa])){
					actions.push({
						data: policyDetailItem,
						label: 'Xoá',
						icon: 'pi pi-trash',
						command: ($event) => {
							this.deleteDetail($event.item.data);
							console.log("lốc ra : ", $event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_KyHan_BatTatShowApp])) {
					const valueIsShowApp = policyDetailItem.isShowApp == YesNoConst.STATUS_YES ? YesNoConst.STATUS_NO : YesNoConst.STATUS_YES;
					actions.push({
						data: policyDetailItem,
						label: policyDetailItem.isShowApp == YesNoConst.STATUS_YES ? 'Tắt show app' : 'Bật show app',
						icon: policyDetailItem.isShowApp == YesNoConst.STATUS_YES ? 'pi pi-eye-slash' : 'pi pi-eye',
						command: ($event) => {
							this.toggleIsShowAppPolicyDetail($event.item.data, valueIsShowApp);
						}
					});
				}

				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_KyHan_KichHoatOrHuy])) {
					actions.push({
						data: policyDetailItem,
						label: policyDetailItem.status == this.ProductSecondaryConst.KHOA ? 'Kích hoạt' : 'Hủy kích hoạt',
						icon: policyDetailItem.status == this.ProductSecondaryConst.KHOA ? 'pi pi-check' : 'pi pi-times-circle',
						command: ($event) => {
							this.changeStatusPolicyDetail($event.item.data);
						}
					});
				}
				
				return actions;
			});
		}
	}

	//  ham xoa policyDetail trong table
	deleteDetail(policyDetail){
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn xoá kỳ hạn này không?',
			header: 'Xoá kỳ hạn',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-exclamation-triangle',
			accept: () => {
				console.log("lốc ra policyDetail ", policyDetail);
				
				this._secondaryService.deletePolicyDetail(policyDetail.companySharePolicyDetailId).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Xoá thành công")) {
						this.initPolicyDetail(policyDetail.companySharePolicyId)
					}
				});
			},
			reject: () => {
			},
		});
	}

	approve(secondaryId) {
		this.processChangeStatus(secondaryId, CompanyShareSecondaryConst.STATUS.HOAT_DONG, "Duyệt thành công");
	}

	cancel(companyShareSecondary) {
		const ref = this.dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC("Hủy duyệt", companyShareSecondary?.secondaryId)
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._secondaryService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
						this.initSecondary(companyShareSecondary.secondaryId);
					}
				});
			}
		});
	}

	check(companyShareSecondary) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô cổ phần này không?',
			header: 'Phê duyệt',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-exclamation-triangle',
			accept: () => {
				this._secondaryService.check({ id: companyShareSecondary?.secondaryId }).subscribe((response) => {

					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.initSecondary(companyShareSecondary.secondaryId);
					}
				});
			},
			reject: () => {
			},
		});
	}

	unapprove(secondaryId) {
		this.processChangeStatus(secondaryId, CompanyShareSecondaryConst.STATUS.NHAP, "Đã từ chối duyệt");
	}

	processChangeStatus(secondaryId, status, msg) {
		this._secondaryService.changeStatus(secondaryId, status).subscribe((response) => {
			if (this.handleResponseInterceptor(response, msg)) {
				this.hideDialog();
				this.initSecondary(this.companyShareSecondary.secondaryId);
			}
		});
	}

	request(companyShareSecondary) {
		console.log("companyShareSecondaryyy",companyShareSecondary);
		
		const summary = 'Bán theo kỳ hạn: ' + companyShareSecondary?.companyShareInfo?.companyShareCode + ' - ' + companyShareSecondary?.companyShareInfo?.companyShareName + '( ID: ' + companyShareSecondary.secondaryId + ')';
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", companyShareSecondary.secondaryId, summary)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);

			if (dataCallBack?.accept) {
				this._secondaryService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.initSecondary(companyShareSecondary.secondaryId);
					}
				});
			}
		});
	}
	
	// TRÌNH DUYỆT
	submit(secondaryId) {
		console.log("secondaryId",secondaryId);
		
		this._secondaryService.submit(secondaryId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
				this.hideDialog();
				this.initSecondary(this.companyShareSecondary.secondaryId);
			}
		});
	}
	// KHOA TAM
	toggleClosed(secondaryId, value) {
		this._secondaryService.toggleIsClosed(secondaryId, value).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật trạng thái đóng thành công")) {
				this.initSecondary(secondaryId)
				this.hideDialog();
			}
		});
	}

	// BAT TAT IS SHOW APP
	toggleIsShowApp(secondaryId, value) {
		this._secondaryService.toggleIsShowApp(secondaryId, value).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initSecondary(secondaryId)
				this.hideDialog();
			}
		});
	}

	// BAT TAT IS SHOW APP POLICY
	toggleIsShowAppPolicy(policy, value) {
		this._secondaryService.toggleIsShowAppPolicy(policy.companySharePolicyId, value).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initSecondary(policy.secondaryId)
				this.hideDialog();
			}
		});
	}

	// BAT TAT IS SHOW APP POLICY DETAIL
	toggleIsShowAppPolicyDetail(policyDetail, value) {
		this._secondaryService.toggleIsShowAppPolicyDetail(policyDetail.companySharePolicyDetailId, value).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initPolicyDetail(policyDetail.companySharePolicyId);
				this.hideDialog();
			}
		});
	}
	// BAT TAT IS SHOW APP POLICY
	changeStatusPolicy(policy) {
		this._secondaryService.changeStatusPolicy(policy.companySharePolicyId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initSecondary(policy.secondaryId)
				this.hideDialog();
			}
		});
	}

	// BAT TAT IS SHOW APP POLICY DETAIL
	changeStatusPolicyDetail(policyDetail) {
		this._secondaryService.changeStatusPolicyDetail(policyDetail.companySharePolicyDetailId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initPolicyDetail(policyDetail.companySharePolicyId);
				this.hideDialog();
			}
		});
	}

	// LABEL EDIT BUTTON
	labelButtonEdit() {
		return this.isEdit ? 'Lưu lại' : 'Chỉnh sửa';
	}

	changeEdit() {
		if (this.isEdit) {
			this.save();
		} else {
			this.isEdit = true;
		}
	}

	// INIT DATA
	initData(secondaryId) {
		this.getListPrimary();
		this.initSecondary(secondaryId);
	}

	// GET PRIMARY LIST & POLICY TAM
	getListPrimary() {
		forkJoin([this._secondaryService.getAllCompanySharePrimary(), this._policyService.getAllTemporaryNoPaging()]).subscribe(
			([resCompanySharePrimary, resTempPolicy]) => {
				this.listPrimary = resCompanySharePrimary?.data?.items;
				this.search.listPolicy = [...resTempPolicy?.data?.items];
			}
		);
	}

	// GET SECONDARY
	initSecondary(secondaryId) {
		this.isLoading = true;
		this._secondaryService.getById(secondaryId).subscribe(res => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.companyShareSecondary = {
					...res.data,
					openCellDate: new Date(res.data?.openCellDate),
					closeCellDate: new Date(res.data?.closeCellDate),
				};
				//
				if (this.companyShareSecondary?.listBusinessCustomerBanks?.length) {
					this.listBanks = this.companyShareSecondary.listBusinessCustomerBanks.map(bank => {
						bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
						return bank;
					});
				}
				//
				this.minDate = this.companyShareSecondary?.companySharePrimary?.openCellDate ? new Date(this.companyShareSecondary.companySharePrimary.openCellDate) : null;
				this.maxDate = this.companyShareSecondary?.companySharePrimary?.closeCellDate ? new Date(this.companyShareSecondary.companySharePrimary.closeCellDate) : null;
				//
				this.genListAction(this.companyShareSecondary);
				this.genListPolicyAction(this.companyShareSecondary.policies);

			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}	
	
	confirmEditByOnSave(policy) {
		this.setTimeZoneList(this.fieldDatePolicies, policy);
		this._secondaryService.updatePolicy(policy, policy?.companySharePolicyId).subscribe(res => {
			if (this.handleResponseInterceptor(res, "Cập nhật chính sách thành công")) {
				this.initSecondary(this.companyShareSecondary.secondaryId);
				this.hideDialog('modalDialogPolicy');
				// this.updateClick = false;
				// this.createClick = true;
			} else {
				this.resetTimeZoneList(this.fieldDatePolicies, policy);
				this.submitted = false;
			}
		}, (err) => {
			console.log('err----', err);
			this.resetTimeZoneList(this.fieldDatePolicies, policy);
		});
		this.modalDialogPolicy = false;
	}

	createPolicy() {
		this.policy = { ...BASE.POLICY };
		this.policy.tradingProviderId = 1;
		this.submitted = false;
		this.modalDialogPolicy = true;
	}

	editPolicy(policy) {
		this.policy = { 
			...policy,
			startDate: policy.startDate ? new Date(policy.startDate) : null, 
			endDate: policy.endDate ? new Date(policy.endDate) : null, 
		};
		this.submitted = false;
		// this.onPolicyDetailTable = true;
		this.modalDialogPolicy = true;
		this.initPolicyDetail(this.policy.companySharePolicyId)
	}

	createPolicyDetail(policy) {

		this.policyDetail = {
			...BASE.POLICY_DETAIL,
			fakeId: 0,
			policyId: policy.companySharePolicyId,
			tradingProviderId: this.companyShareSecondary.tradingProdviderId,
		};
		this.submitted = false;
		this.modalDialogPolicyDetail = true;
	}


	//component con gọi (create-or-update-policy)
	createPolicyDetailChild(policyId) {
		console.log('policyId-----', policyId);
		this.policyDetail = {
			...BASE.POLICY_DETAIL,
			policyId: policyId,
			tradingProviderId: this.companyShareSecondary.tradingProdviderId,
		}
		this.submitted = false;
		this.modalDialogPolicyDetail = true;
		
	}

	editPolicyDetail(policyDetail) {
		this.policyDetail = { ...policyDetail };
		this.submitted = false;
		this.modalDialogPolicyDetail = true;
	}
	
	saveEditPolicyDetail(){
		this._secondaryService.updatePolicyDetail(this.policyDetail, this.policyDetail.companySharePolicyDetailId).subscribe(res => {
			if (this.handleResponseInterceptor(res, "Cập nhật bán theo kỳ hạn thành công")) {
				// this.initSecondary(this.companyShareSecondary.secondaryId);
				this.hideDialog('modalDialogPolicyDetail');
			} else {
				this.submitted = false;
			}
		}, () => {

		});
	}

	onChangeOpenCellDate($event) {
		if (+new Date($event) > +new Date(this.companyShareSecondary.closeCellDate)) {
			this.companyShareSecondary.closeCellDate = null;
		}
	}

	/**
	 * Lưu thông tin phát hành thứ cấp
	 */
	save() {
		this.submitted = true;
		const { policies, companySharePrimary, ...body } = this.companyShareSecondary;
		//
		this.setTimeZoneList(this.fieldDates, body);
		if (body.secondaryId) {
			this._secondaryService.update(body).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
						this.initSecondary(body?.secondaryId);
						// this.hideDialog('modalDialog');
						this.isEdit = false;
					} else {
						this.resetTimeZoneList(this.fieldDates, body);
						this.submitted = false;
					}
				},
				() => {
					this.submitted = false;
				}
			);
		}
	}

	hideDialog(modalName?: string) {
		this[modalName] = false;
		this.submitted = false;
	}

	validForm(): boolean {
		const validRequired = this.companyShareSecondary?.companySharePrimaryId && this.companyShareSecondary?.quantity;
		return validRequired;
	}

	validFormPolicy(): boolean {
		const validRequired =
			this.policy?.code && this.policy?.name && this.policy?.type && this.policy?.typeInvestor && this.policy?.minMoneny && this.policy?.numberClosePer && this.policy?.incomeTax;
		return validRequired;
	}

	validFormPolicyDetail(): boolean {
		const validRequired =
			this.policyDetail?.code &&
			this.policyDetail?.name &&
			this.policyDetail?.type &&
			this.policyDetail?.typeInvestor &&
			this.policyDetail?.minMoneny &&
			this.policyDetail?.numberClosePer &&
			this.policyDetail?.incomeTax &&
			this.policyDetail?.interestPeriod &&
			this.policyDetail?.interestPeriodType;
		return validRequired;3
	}

	/**
	 * MO DIALOG XOA
	 * @param item \
	 * @param type 
	 */
	openDeleteDialog(item, type) {
		this.deleteItemDialog = true;
		if (type == POLICY) {
			this.policyDetail = {};
			this.policy = { ...item };
		}
		if (type == POLICY_DETAIL) {
			this.policy = {};
			this.policyDetail = { ...item };
		}
	}

	/**
	 * XOA CHINH SACH VA CHINH SACH CON
	 * @param type 
	 */

	// deletePolicy(policy) {
	// 	this.confirmationService.confirm({
	// 		message: `Bạn có chắc chắn xóa chính sách này?`,
	// 		header: 'Cảnh báo',
	// 		icon: 'pi pi-exclamation-triangle',
	// 		acceptLabel: 'Đồng ý',
	// 		rejectLabel: 'Hủy',
	// 		accept: () => {
	// 			this._secondaryService.deletePolicy(policy.companySharePolicyId).subscribe(
	// 				(response) => {
	// 					if (this.handleResponseInterceptor(response, "Xoá chính sách thành công")) {
	// 						this.initSecondary(policy?.secondaryId);
	// 					} else {
	// 						// this.resetTimeZoneList(this.fieldDates, body);
	// 						this.submitted = false;
	// 					}
	// 				},
	// 				() => {
	// 					this.submitted = false;
	// 				}
	// 			);
	// 		},
	// 		reject: () => {

	// 		},
	// 	});
		
	// }

	deletePolicy(policy) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xóa chính sách này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._secondaryService.deletePolicy(policy.companySharePolicyId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa chính sách thành công"
				)
			  ) {
				this.initSecondary(policy?.secondaryId);
			  }
			});
			}
		});
	  }

	confirmDelete(type) {
		this.deleteItemDialog = false;
		if (type == POLICY) {
			if (this.policy) {
				this._secondaryService.deletePolicy(this.policy.companySharePolicyId).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, "Xoá chính sách thành công")) {
							this.initSecondary(this.policy?.secondaryId);
						} else {
							// this.resetTimeZoneList(this.fieldDates, body);
							this.submitted = false;
						}
					},
					() => {
						this.submitted = false;
					}
				);
			}
		} else if (type == POLICY_DETAIL) {
			if (this.policyDetail) {
				this._secondaryService.deletePolicyDetail(this.policyDetail.companySharePolicyDetailId).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, "Xoá bán theo kỳ hạn thành công")) {
							this.initSecondary(this.policyDetail?.secondaryId);
						} else {
							// this.resetTimeZoneList(this.fieldDates, body);
							this.submitted = false;
						}
					},
					() => {
						this.submitted = false;
					}
				);
			}
		}
	}

	expandAll() {
		if (!this.isExpanded) {
			this.companyShareSecondary.policies.forEach((product) => (this.expandedRows[product.fakeId] = true));
		} else {
			this.expandedRows = {};
		}
		this.isExpanded = !this.isExpanded;
	}

	onSavePolicy(data) {
		console.log('emit-data', data);
		this.policy = { ...this.policy, ...data };
		console.log('đây là policy', this.policy);
		
		this.savePolicy();
	}

	savePolicy() {
		this.setTimeZoneList(this.fieldDatePolicies, this.policy);
    	console.log({ policy: this.policy });
		if (this.policy?.fakeId) {
			this.policy.action = this.ProductPolicyConst.ACTION_UPDATE;
			const { details, ...body } = this.policy;
			this._secondaryService.updatePolicy(body, body?.companySharePolicyId).subscribe(res => {
				if (this.handleResponseInterceptor(res, "Cập nhật chính sách thành công")) {
					this.initSecondary(this.companyShareSecondary.secondaryId);
					this.initPolicyDetail(this.policy.companySharePolicyId)
					this.hideDialog('modalDialogPolicy');
				} else {
					this.submitted = false;
					this.resetTimeZoneList(this.fieldDatePolicies, this.policy);
				}
			}, () => {
				this.resetTimeZoneList(this.fieldDatePolicies, this.policy);
			});

			this.modalDialogPolicy = false;
			this.onPolicyDetailTable = false;

		} else {	
			this.policy.action = this.ProductPolicyConst.ACTION_CREATE;

			if (this.ProductPolicyConst.ACTION_CREATE) {
				const body = {
					...this.policy,
					secondaryId: this.companyShareSecondary.secondaryId,
					companySharePolicyTempId: this.policy.companySharePolicyTempId				
				};
				this._secondaryService.addPolicy(body).subscribe(res => {
					if (this.handleResponseInterceptor(res, "Tạo chính sách thành công")) {
						// this.policyInit = res?.data;
						this.policy = res?.data;
						this.setTimeZoneList(this.fieldDatePolicies, this.policy);
						console.log('sau khi them moi', this.policy);
						
						this.policy.details = res?.data?.companySharePolicyDetail;
						this.initPolicyDetail(this.policy.companySharePolicyId)
						this.initSecondary(this.companyShareSecondary?.secondaryId);
						
						this.createClick = false;
						this.updateClick = true;

					} else {
						this.resetTimeZoneList(this.fieldDatePolicies, this.policy);
						this.submitted = false;
					}

				}, () => {
					this.resetTimeZoneList(this.fieldDatePolicies, this.policy);
				});
			}
		}
		this.onPolicyDetailTable = false;
	}
	//cap nhap chinh sach khi nhan nut luu
	editByOnSave(policy) {
		this.policy = { ...policy };
		this.submitted = false;
		console.log('dataPolicyUpdate-------', policy);
		this.confirmEditByOnSave(this.policy)
	}

	/** NHAN SU KIEN TU MODAL POLICY DETAIL */
	onSaveAddPolicyDetail(data) {
		this.policyDetail = {
			...this.policyDetail,
			...data
		};
		
		this.savePolicyDetail();
	}

	// savePolicyDetail() {
	// 	if (this.policyDetail.fakeId) {
	// 		this.policy.action = this.ProductPolicyConst.ACTION_UPDATE;

	// 		this._secondaryService.updatePolicyDetail(this.policyDetail, this.policyDetail.companySharePolicyDetailId).subscribe(res => {
	// 			if (this.handleResponseInterceptor(res, "Cập nhật bán theo kỳ hạn thành công")) {
	// 				this.initSecondary(this.companyShareSecondary.secondaryId);
	// 				this.hideDialog('modalDialogPolicyDetail');
	// 			} else {
	// 				// this.resetTimeZoneList(this.fieldDates, body);
	// 				this.submitted = false;
	// 			}
	// 		}, () => {

	// 		});
	// 	} else {
	// 		this.policy.action = this.ProductPolicyConst.ACTION_CREATE;
	// 		const body = {
	// 			...this.policyDetail,
	// 			secondaryId: this.companyShareSecondary.secondaryId,
	// 			companySharePolicyId: this.policyDetail.policyId,
	// 		};
	// 		this._secondaryService.addPolicyDetail(body).subscribe(res => {
	// 			if (this.handleResponseInterceptor(res, "Thêm bán theo kỳ hạn thành công")) {
	// 				this.initSecondary(this.companyShareSecondary.secondaryId);
	// 				if(this.policyInit){
	// 					this.initPolicyDetail(this.policyInit?.companySharePolicyId);
	// 				}
	// 				this.hideDialog('modalDialogPolicyDetail');
	// 			} else {
	// 				// this.resetTimeZoneList(this.fieldDates, body);
	// 				this.submitted = false;
	// 			}
	// 		}, () => {

	// 		});
	// 	}


	savePolicyDetail() {
		if (this.policyDetail.companySharePolicyDetailId) {
			this.policy.action = this.ProductPolicyConst.ACTION_UPDATE;

			this._secondaryService.updatePolicyDetail(this.policyDetail, this.policyDetail.companySharePolicyDetailId).subscribe(res => {
				if (this.handleResponseInterceptor(res, "Cập nhật kỳ hạn thành công")) {
					// this.initSecondary(this.companyShareSecondary.secondaryId);
					this.initPolicyDetail(this.policyDetail?.companySharePolicyId);
					this.hideDialog('modalDialogPolicyDetail');
					// this.onPolicyDetailTable = true;
				} else {
					// this.resetTimeZoneList(this.fieldDates, body);
					this.submitted = false;
				}
			}, () => {
			});
		} else {
			this.policy.action = this.ProductPolicyConst.ACTION_CREATE;
			const body = {
				...this.policyDetail,
				secondaryId: this.companyShareSecondary.secondaryId,
				companySharePolicyId: this.policyDetail.policyId,
			};
			this._secondaryService.addPolicyDetail(body).subscribe(res => {
				if (this.handleResponseInterceptor(res, "Thêm kỳ hạn thành công")) {
					// this.initSecondary(this.companyShareSecondary.secondaryId);
					// if(this.policyInit){
					// 	this.policyId = {...this.policyInit.companySharePolicyId}
					// }
					// else{
					// 	this.policyId = res.companySharePolicyId
					// }
					this.initPolicyDetail(this.policyDetail?.policyId);
					this.hideDialog('modalDialogPolicyDetail');
				} else {
					// this.resetTimeZoneList(this.fieldDates, body);
					this.submitted = false;
				}
			}, () => {

			});
		}
		this.modalDialogPolicyDetail = false;
	}

	// XU LY FILTER POLICY & POLICY DETAIL
	selectPolicy($event) {
		const { value } = $event;
		this.search.listDetails = [...value.companySharePolicyDetailTemp];

		Object.keys(this.policy).forEach((key) => {
			if (key in value) {
				this.policy[key] = value[key];
			}
		});

	}

	selectPolicyDetail($event) {
		const { value } = $event;

		Object.keys(this.policyDetail).forEach((key) => {
			if (key in value) {
				this.policyDetail[key] = value[key];
			}
		});

	}

	//getall policy by companySharepolicyId
	initPolicyDetail(companySharePolicyId) {	
		this._secondaryService.getAllPolicyDetail(companySharePolicyId).subscribe((res) => {
			this.isLoading = false;
			this.policy.details = res?.data;
			console.log('companySharePolicyId------', companySharePolicyId);
			this.genListPolicyDetailAction(res?.data);
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

	// CHO PHEP GO GIA TRI INPUT CUA SECONDARY
	allowEditSecondary() {
		return true;
	}
}
