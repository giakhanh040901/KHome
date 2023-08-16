import { ProductBondPolicyServiceProxy, ProductBondSecondaryServiceProxy } from "../../../../shared/service-proxies/bond-manager-service";
import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import { FormNotificationConst, PermissionBondConst, ProductBondInterestConst, ProductBondSecondaryConst, ProductPolicyConst, YesNoConst } from "@shared/AppConsts";
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
import { FormBondInfoApproveComponent } from "src/app/approve-manager/approve/form-bond-info-approve/form-bond-info-approve.component";
const { POLICY, POLICY_DETAIL, BASE } = OJBECT_SECONDARY_CONST;

@Component({
	selector: "app-product-bond-secondary-update",
	templateUrl: "./product-bond-secondary-update.component.html",
	styleUrls: ["./product-bond-secondary-update.component.scss"],
	providers: [DialogService],
})
export class ProductBondSecondaryUpdateComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		private route: ActivatedRoute,
		private routeActive: ActivatedRoute,
		private _secondaryService: ProductBondSecondaryServiceProxy,
		private _policyService: ProductBondPolicyServiceProxy,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Bán theo kỳ hạn", routerLink: ['/bond-manager/product-bond-secondary'] },
			{ label: "Chi tiết bán theo kỳ hạn" },
		]);
		this.bondSecondaryId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));

	}

	ref: DynamicDialogRef;

	bondSecondaryId: number;

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
	ProductSecondaryConst = ProductBondSecondaryConst;
	ProductPolicyConst = ProductPolicyConst;
	ProductBondInterestConst = ProductBondInterestConst;
	YesNoConst = YesNoConst;

	// Row Expand
	expandedRows = {};
	isExpanded: boolean = false;

	// EDIT
	isEdit: boolean = false;

	// TAB VIEW
	activeIndex = 0;
	// MAIN
	productBondSecondary: any = { ...BASE.SECODARY };
	policy: any = { ...BASE.POLICY };
	policyDetail: any = { ...BASE.POLICY_DETAIL };
	/**
	 * Dùng cho search policy và detail base
	 *  */
	search = {
		policy: {
			productBondPolicyDetailTemp: [],
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
			this.productBondSecondary.bondSecondaryId = this.cryptDecode(params?.id);
			this.initData(this.cryptDecode(params?.id));
		});
	}

	checkEpic(productBondSecondary) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô trái phiếu này không?',
			header: 'Thông báo',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-check-circle',
			accept: () => {
				this._secondaryService.check({ id: productBondSecondary?.bondSecondaryId }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.initData(productBondSecondary.bondSecondaryId);
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

	approveBond(bondData) {
		console.log("bondDataaa",bondData);
		
		const ref = this.dialogService.open(
			FormBondInfoApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: bondData
			}
		);

		ref.onClose.subscribe((dataCallBack) => {

			if (dataCallBack?.accept) {
				if (dataCallBack.checkApprove == true) {
					this._secondaryService.approve(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
							this.initData(bondData.bondSecondaryId);
						}
					});
				}
				else {
					this._secondaryService.cancel(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Huỷ duyệt thành công!")) {
							this.initData(bondData.bondSecondaryId);
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
		// 			this.approve($event.item.data?.bondSecondaryId);
		// 		}
		// 	});
		// }

		if (secondaryItem.status == this.ProductSecondaryConst.STATUS.NHAP ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Trình duyệt',
				icon: 'pi pi-forward',
				command: ($event) => {
					this.submit($event.item.data?.bondSecondaryId);
				}
			});
		}

		// if (secondaryItem.status == this.ProductSecondaryConst.STATUS.TRINH_DUYET && this.isGranted()) {
		// 	this.actions.push({
		// 		data: secondaryItem,
		// 		label: 'Huỷ duyệt',
		// 		icon: 'pi pi-forward',
		// 		command: ($event) => {
		// 			this.cancel($event.item.data?.bondSecondaryId);
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
					this.toggleClosed($event.item.data?.bondSecondaryId, YesNoConst.STATUS_YES);
				}
			});
		}

		if (secondaryItem.isClose == YesNoConst.STATUS_YES ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Mở',
				icon: 'pi pi-unlock',
				command: ($event) => {
					this.toggleClosed($event.item.data?.bondSecondaryId, YesNoConst.STATUS_NO);
				}
			});
		}

		if (secondaryItem.isShowApp == YesNoConst.STATUS_NO ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Bật show app',
				icon: 'pi pi-eject',
				command: ($event) => {
					this.toggleIsShowApp($event.item.data?.bondSecondaryId, YesNoConst.STATUS_YES);
				}
			});
		}

		if (secondaryItem.isShowApp == YesNoConst.STATUS_YES ) {
			this.actions.push({
				data: secondaryItem,
				label: 'Tắt show app',
				icon: 'pi pi-eye-slash',
				command: ($event) => {
					this.toggleIsShowApp($event.item.data?.bondSecondaryId, YesNoConst.STATUS_NO);
				}
			});
		}
	}

	genListPolicyAction(data = []) {
		if(data) {
			this.listPolicyAction = data.map(policyItem => {
				const actions = [];
				
				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_KyHan_ThemMoi])){
					actions.push({
						data: policyItem,
						label: 'Thêm kỳ hạn',
						icon: 'pi pi-plus-circle',
						command: ($event) => {
							this.createPolicyDetail($event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_CapNhat])){
					actions.push({
						data: policyItem,
						label: 'Sửa',
						icon: 'pi pi-pencil',
						command: ($event) => {
							this.editPolicy($event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_Xoa])){
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
				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_BatTatShowApp])) {
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
				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_KichHoatOrHuy])) {
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

				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_KyHan_CapNhat])){
					actions.push({
						data: policyDetailItem,
						label: 'Sửa',
						icon: 'pi pi-pencil',
						command: ($event) => {
							this.editPolicyDetail($event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_KyHan_Xoa])){
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

				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_KyHan_BatTatShowApp])) {
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

				if (this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_KyHan_KichHoatOrHuy])) {
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
				
				this._secondaryService.deletePolicyDetail(policyDetail.bondPolicyDetailId).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Xoá thành công")) {
						this.initPolicyDetail(policyDetail.bondPolicyId)
					}
				});
			},
			reject: () => {
			},
		});
	}

	approve(secondaryId) {
		this.processChangeStatus(secondaryId, ProductBondSecondaryConst.STATUS.HOAT_DONG, "Duyệt thành công");
	}

	cancel(productBondSecondary) {
		const ref = this.dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC("Hủy duyệt", productBondSecondary?.bondSecondaryId)
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._secondaryService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
						this.initSecondary(productBondSecondary.bondSecondaryId);
					}
				});
			}
		});
	}

	check(productBondSecondary) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô trái phiếu này không?',
			header: 'Phê duyệt',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-exclamation-triangle',
			accept: () => {
				this._secondaryService.check({ id: productBondSecondary?.bondSecondaryId }).subscribe((response) => {

					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.initSecondary(productBondSecondary.bondSecondaryId);
					}
				});
			},
			reject: () => {
			},
		});
	}

	unapprove(secondaryId) {
		this.processChangeStatus(secondaryId, ProductBondSecondaryConst.STATUS.NHAP, "Đã từ chối duyệt");
	}

	processChangeStatus(secondaryId, status, msg) {
		this._secondaryService.changeStatus(secondaryId, status).subscribe((response) => {
			if (this.handleResponseInterceptor(response, msg)) {
				this.hideDialog();
				this.initSecondary(this.productBondSecondary.bondSecondaryId);
			}
		});
	}

	request(productBondSecondary) {
		console.log("productBondSecondaryyy",productBondSecondary);
		
		const summary = 'Bán theo kỳ hạn: ' + productBondSecondary?.productBondInfo?.bondCode + ' - ' + productBondSecondary?.productBondInfo?.bondName + '( ID: ' + productBondSecondary.bondSecondaryId + ')';
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", productBondSecondary.bondSecondaryId, summary)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);

			if (dataCallBack?.accept) {
				this._secondaryService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.initSecondary(productBondSecondary.bondSecondaryId);
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
				this.initSecondary(this.productBondSecondary.bondSecondaryId);
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
		this._secondaryService.toggleIsShowAppPolicy(policy.bondPolicyId, value).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initSecondary(policy.bondSecondaryId)
				this.hideDialog();
			}
		});
	}

	// BAT TAT IS SHOW APP POLICY DETAIL
	toggleIsShowAppPolicyDetail(policyDetail, value) {
		this._secondaryService.toggleIsShowAppPolicyDetail(policyDetail.bondPolicyDetailId, value).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initPolicyDetail(policyDetail.bondPolicyId);
				this.hideDialog();
			}
		});
	}
	// BAT TAT IS SHOW APP POLICY
	changeStatusPolicy(policy) {
		this._secondaryService.changeStatusPolicy(policy.bondPolicyId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initSecondary(policy.bondSecondaryId)
				this.hideDialog();
			}
		});
	}

	// BAT TAT IS SHOW APP POLICY DETAIL
	changeStatusPolicyDetail(policyDetail) {
		this._secondaryService.changeStatusPolicyDetail(policyDetail.bondPolicyDetailId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.initPolicyDetail(policyDetail.bondPolicyId);
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
		forkJoin([this._secondaryService.getAllBondPrimary(), this._policyService.getAllTemporaryNoPaging()]).subscribe(
			([resBondPrimary, resTempPolicy]) => {
				this.listPrimary = resBondPrimary?.data?.items;
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
				this.productBondSecondary = {
					...res.data,
					openCellDate: new Date(res.data?.openCellDate),
					closeCellDate: new Date(res.data?.closeCellDate),
				};
				//
				if (this.productBondSecondary?.listBusinessCustomerBanks?.length) {
					this.listBanks = this.productBondSecondary.listBusinessCustomerBanks.map(bank => {
						bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
						return bank;
					});
				}
				//
				this.minDate = this.productBondSecondary?.productBondPrimary?.openCellDate ? new Date(this.productBondSecondary.productBondPrimary.openCellDate) : null;
				this.maxDate = this.productBondSecondary?.productBondPrimary?.closeCellDate ? new Date(this.productBondSecondary.productBondPrimary.closeCellDate) : null;
				//
				this.genListAction(this.productBondSecondary);
				this.genListPolicyAction(this.productBondSecondary.policies);

			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}	
	
	confirmEditByOnSave(policy) {
		let body = this.formatCalendar(this.fieldDatePolicies, {...policy});
		this._secondaryService.updatePolicy(body, body?.bondPolicyId).subscribe(res => {
			if (this.handleResponseInterceptor(res, "Cập nhật chính sách thành công")) {
				this.initSecondary(this.productBondSecondary.bondSecondaryId);
				this.hideDialog('modalDialogPolicy');
				// this.updateClick = false;
				// this.createClick = true;
			} else {
				this.submitted = false;
			}
		}, (err) => {
			console.log('err----', err);
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
		this.initPolicyDetail(this.policy.bondPolicyId)
	}

	createPolicyDetail(policy) {

		this.policyDetail = {
			...BASE.POLICY_DETAIL,
			fakeId: 0,
			policyId: policy.bondPolicyId,
			tradingProviderId: this.productBondSecondary.tradingProdviderId,
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
			tradingProviderId: this.productBondSecondary.tradingProdviderId,
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
		this._secondaryService.updatePolicyDetail(this.policyDetail, this.policyDetail.bondPolicyDetailId).subscribe(res => {
			if (this.handleResponseInterceptor(res, "Cập nhật bán theo kỳ hạn thành công")) {
				// this.initSecondary(this.productBondSecondary.bondSecondaryId);
				this.hideDialog('modalDialogPolicyDetail');
			} else {
				this.submitted = false;
			}
		}, () => {

		});
	}

	onChangeOpenCellDate($event) {
		if (+new Date($event) > +new Date(this.productBondSecondary.closeCellDate)) {
			this.productBondSecondary.closeCellDate = null;
		}
	}

	/**
	 * Lưu thông tin phát hành thứ cấp
	 */
	save() {
		this.submitted = true;
		let { policies, productBondPrimary, ...body } = this.productBondSecondary;
		//
		this.formatCalendar(this.fieldDates, body);
		if (body.bondSecondaryId) {
			this._secondaryService.update(body).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
						this.initSecondary(body?.bondSecondaryId);
						// this.hideDialog('modalDialog');
						this.isEdit = false;
					} else {
						this.submitted = false;
					}
				},() => {
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
		const validRequired = this.productBondSecondary?.bondPrimaryId && this.productBondSecondary?.quantity;
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
	// 			this._secondaryService.deletePolicy(policy.bondPolicyId).subscribe(
	// 				(response) => {
	// 					if (this.handleResponseInterceptor(response, "Xoá chính sách thành công")) {
	// 						this.initSecondary(policy?.bondSecondaryId);
	// 					} else {
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
			this._secondaryService.deletePolicy(policy.bondPolicyId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa chính sách thành công"
				)
			  ) {
				this.initSecondary(policy?.bondSecondaryId);
			  }
			});
			}
		});
	  }

	confirmDelete(type) {
		this.deleteItemDialog = false;
		if (type == POLICY) {
			if (this.policy) {
				this._secondaryService.deletePolicy(this.policy.bondPolicyId).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, "Xoá chính sách thành công")) {
							this.initSecondary(this.policy?.bondSecondaryId);
						} else {
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
				this._secondaryService.deletePolicyDetail(this.policyDetail.bondPolicyDetailId).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, "Xoá bán theo kỳ hạn thành công")) {
							this.initSecondary(this.policyDetail?.bondSecondaryId);
						} else {
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
			this.productBondSecondary.policies.forEach((product) => (this.expandedRows[product.fakeId] = true));
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
		this.formatCalendar(this.fieldDatePolicies, this.policy);
    	console.log({ policy: this.policy });
		if (this.policy?.fakeId) {
			this.policy.action = this.ProductPolicyConst.ACTION_UPDATE;
			const { details, ...body } = this.policy;
			this._secondaryService.updatePolicy(body, body?.bondPolicyId).subscribe(res => {
				if (this.handleResponseInterceptor(res, "Cập nhật chính sách thành công")) {
					this.initSecondary(this.productBondSecondary.bondSecondaryId);
					this.initPolicyDetail(this.policy.bondPolicyId)
					this.hideDialog('modalDialogPolicy');
				} else {
					this.submitted = false;
				}
			}, (err) => {
				console.log('err__', err);
			});

			this.modalDialogPolicy = false;
			this.onPolicyDetailTable = false;

		} else {	
			this.policy.action = this.ProductPolicyConst.ACTION_CREATE;

			if (this.ProductPolicyConst.ACTION_CREATE) {
				const body = {
					...this.policy,
					bondSecondaryId: this.productBondSecondary.bondSecondaryId,
					bondPolicyTempId: this.policy.bondPolicyTempId				
				};
				this._secondaryService.addPolicy(body).subscribe(res => {
					if (this.handleResponseInterceptor(res, "Tạo chính sách thành công")) {
						// this.policyInit = res?.data;
						this.policy = res?.data;
						this.policy = this.formatCalendar(this.fieldDatePolicies, {...this.policy});
						console.log('sau khi them moi', this.policy);
						
						this.policy.details = res?.data?.productBondPolicyDetail;
						this.initPolicyDetail(this.policy.bondPolicyId)
						this.initSecondary(this.productBondSecondary?.bondSecondaryId);
						
						this.createClick = false;
						this.updateClick = true;

					} else {
						this.submitted = false;
					}

				}, (err) => {
					console.log('err__', err);
					
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

	// 		this._secondaryService.updatePolicyDetail(this.policyDetail, this.policyDetail.bondPolicyDetailId).subscribe(res => {
	// 			if (this.handleResponseInterceptor(res, "Cập nhật bán theo kỳ hạn thành công")) {
	// 				this.initSecondary(this.productBondSecondary.bondSecondaryId);
	// 				this.hideDialog('modalDialogPolicyDetail');
	// 			} else {
	// 				this.submitted = false;
	// 			}
	// 		}, () => {

	// 		});
	// 	} else {
	// 		this.policy.action = this.ProductPolicyConst.ACTION_CREATE;
	// 		const body = {
	// 			...this.policyDetail,
	// 			bondSecondaryId: this.productBondSecondary.bondSecondaryId,
	// 			bondPolicyId: this.policyDetail.policyId,
	// 		};
	// 		this._secondaryService.addPolicyDetail(body).subscribe(res => {
	// 			if (this.handleResponseInterceptor(res, "Thêm bán theo kỳ hạn thành công")) {
	// 				this.initSecondary(this.productBondSecondary.bondSecondaryId);
	// 				if(this.policyInit){
	// 					this.initPolicyDetail(this.policyInit?.bondPolicyId);
	// 				}
	// 				this.hideDialog('modalDialogPolicyDetail');
	// 			} else {
	// 				this.submitted = false;
	// 			}
	// 		}, () => {

	// 		});
	// 	}


	savePolicyDetail() {
		if (this.policyDetail.bondPolicyDetailId) {
			this.policy.action = this.ProductPolicyConst.ACTION_UPDATE;

			this._secondaryService.updatePolicyDetail(this.policyDetail, this.policyDetail.bondPolicyDetailId).subscribe(res => {
				if (this.handleResponseInterceptor(res, "Cập nhật kỳ hạn thành công")) {
					// this.initSecondary(this.productBondSecondary.bondSecondaryId);
					this.initPolicyDetail(this.policyDetail?.bondPolicyId);
					this.hideDialog('modalDialogPolicyDetail');
					// this.onPolicyDetailTable = true;
				} else {
					this.submitted = false;
				}
			}, () => {
			});
		} else {
			this.policy.action = this.ProductPolicyConst.ACTION_CREATE;
			const body = {
				...this.policyDetail,
				bondSecondaryId: this.productBondSecondary.bondSecondaryId,
				bondPolicyId: this.policyDetail.policyId,
			};
			this._secondaryService.addPolicyDetail(body).subscribe(res => {
				if (this.handleResponseInterceptor(res, "Thêm kỳ hạn thành công")) {
					// this.initSecondary(this.productBondSecondary.bondSecondaryId);
					// if(this.policyInit){
					// 	this.policyId = {...this.policyInit.bondPolicyId}
					// }
					// else{
					// 	this.policyId = res.bondPolicyId
					// }
					this.initPolicyDetail(this.policyDetail?.policyId);
					this.hideDialog('modalDialogPolicyDetail');
				} else {
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
		this.search.listDetails = [...value.productBondPolicyDetailTemp];

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

	//getall policy by bondpolicyId
	initPolicyDetail(bondPolicyId) {	
		this._secondaryService.getAllPolicyDetail(bondPolicyId).subscribe((res) => {
			this.isLoading = false;
			this.policy.details = res?.data;
			console.log('bondPolicyId------', bondPolicyId);
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
