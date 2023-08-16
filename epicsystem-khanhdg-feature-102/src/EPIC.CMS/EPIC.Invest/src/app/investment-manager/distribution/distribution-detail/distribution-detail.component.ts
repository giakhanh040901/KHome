import { DistributionService } from '@shared/services/distribution.service';
import { Component, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { AppConsts, AtributionConfirmConst, DistributionConst, ProductBondInterestConst, ProductBondSecondaryConst, ProductPolicyConst, ProjectConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DialogService } from "primeng/dynamicdialog";
import { ActivatedRoute, Params } from "@angular/router";
import { OJBECT_DISTRIBUTION_CONST } from "@shared/base-object";
import { TabView } from 'primeng/tabview';
import { FormApproveComponent } from 'src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component';
import { FormRequestComponent } from 'src/app/form-general/form-request-approve-cancel/form-request/form-request.component';
import { FormCancelComponent } from 'src/app/form-general/form-request-approve-cancel/form-cancel/form-cancel.component';
import { NationalityConst } from '@shared/nationality-list';
import { ContractTemplateService } from '@shared/services/contract-template.service';

const { POLICY, POLICY_DETAIL, BASE } = OJBECT_DISTRIBUTION_CONST;

@Component({
	selector: 'app-distribution-detail',
	templateUrl: './distribution-detail.component.html',
	styleUrls: ['./distribution-detail.component.scss']
})
export class DistributionDetailComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private route: ActivatedRoute,
		private routeActive: ActivatedRoute,
		private _distributionService: DistributionService,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		private breadcrumbService: BreadcrumbService,
		private _contractTemplateService: ContractTemplateService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Phân phối đầu tư", routerLink: ['/invest-manager/distribution'] },
			{ label: "Chi tiết phân phối đầu tư" },
		]);
		this.distributionId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}
	
	distributionId: any;
	distribution: any = { ...BASE.DISTRIBUTION };

	actions: any[] = [];

	// CONST DATA
	ProductBondInterestConst = ProductBondInterestConst;
	ProductSecondaryConst = ProductBondSecondaryConst;
	ProductPolicyConst = ProductPolicyConst;
	NationalityConst = NationalityConst;
	ProjectConst = ProjectConst;
	YesNoConst = YesNoConst;
	DistributionConst = DistributionConst;
	// EDIT
	isEdit: boolean = false;
	// TAB VIEW
	activeIndex = 0;

	listBanks: any[] = [];
	fieldDates = ["openCellDate", "closeCellDate"];

	distributionUpdate = {
		"id": 0,
		"projectId": null,
		"tradingProviderId": null,
		"tradingBankAcc": null,
		"tradingBankAccPays": null,
		"openCellDate": null,
		"closeCellDate": null,
		"image": null,
		"methodInterest": null
	}

	tabViewActive = {
		'thongTinChung': true,
		'tongQuan': false,
		'chinhSach': false,
		'fileChinhSach': false,
		'mauHopDong': false,
		'hopDongPhanPhoi': false,
		'mauGiaoNhanHD': false,
	};

	imageDefault = AppConsts.imageDefault;
	@ViewChild(TabView) tabView: TabView;

	contentHeights: number[];

	ngOnInit() {
		this.route.params.subscribe((params: Params) => {
			this.distributionId = this.cryptDecode(params?.id);
			this.getDetail(this.cryptDecode(params?.id));
		});

		this._distributionService.getBankList().subscribe((res) => {
			if(this.handleResponseInterceptor(res, '')) {
				if(res?.data?.length) {
					this.listBanks = res.data.map(bank => {
						bank.labelName = bank?.bankAccNo + ' - ' + bank?.bankAccName + ' - ' + bank.bankName;
						bank.labelSelected = bank.bankName;
						return bank;
					})
				}
			}
		}, () => {
			this.messageError('Không lấy được danh sách ngân hàng');
		});

	}

	myUploader(event) {
		if (event?.files[0]) {
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "distribution").subscribe((response) => {
				if (this.handleResponseInterceptor(response, "")) {
					this.distribution.image = response?.data;
				}
			}, (err) => {
				console.log('err-----', err);
			});
		}
	}

	approveSharing(distribution) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				data: {
					id: distribution.id
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				const body1 = {
					approveNote: dataCallBack?.data?.approveNote,
					id: distribution.id,
					cancelNote: dataCallBack?.data?.approveNote,
				}
				if ( dataCallBack?.checkApprove == true) {
					this._distributionService.approve(body1).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
							this.getDetail(distribution.id);
						}
					});
				} else if (dataCallBack?.checkApprove == false) {
					this._distributionService.cancel(body1).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
							this.getDetail(distribution.id);
						}
					});
				}
			}
		});
		
	}

	openThongTinChung: boolean = true;
	changeTabview(e) {
		let tabHeader = this.tabView.tabs[e.index].header;
		this.openThongTinChung = (tabHeader == 'thongTinChung');
		if(!this.tabViewActive[tabHeader]) {
			this.tabViewActive[tabHeader] = true;
		}
	}


	genListAction(distribution) {
		this.actions = [];
		if (distribution.status == this.ProductSecondaryConst.STATUS.NHAP && this.isGranted([this.PermissionInvestConst.InvestPPDT_TrinhDuyet])) {
			this.actions.push({
				data: distribution,
				label: 'Trình duyệt',
				icon: 'pi pi-arrow-up',
				command: ($event) => {
					this.request($event.item.data?.id);
				}
			});
		}

		if (distribution.status == this.ProductSecondaryConst.STATUS.HOAT_DONG && distribution.isCheck == "N" && this.isGranted([this.PermissionInvestConst.InvestPPDT_EpicXacMinh])) {
			this.actions.push({
				data: distribution,
				label: 'Phê duyệt (Epic)',
				icon: 'pi pi-check',
				command: ($event) => {

					this.check($event.item.data?.id);
				}
			});
		}

		if (this.isGranted([this.PermissionInvestConst.InvestPPDT_DongTam])) {
			this.actions.push({
				data: distribution,
				label: distribution.isClose == YesNoConst.NO ? 'Đóng tạm' : 'Mở',
				icon: distribution.isClose == YesNoConst.NO ? 'pi pi-lock' : 'pi pi-lock-open',
				command: ($event) => {
					this.toggleClosed($event.item.data?.id);
				}
			});
		}

		if (this.isGranted([this.PermissionInvestConst.InvestPPDT_BatTatShowApp])) {
			this.actions.push({
				data: distribution,
				label: distribution.isShowApp == YesNoConst.NO ? 'Bật show app' : 'Tắt show app',
				icon: distribution.isShowApp == YesNoConst.NO ? 'pi pi-eye' : 'pi pi-eye-slash',
				command: ($event) => {
					this.toggleIsShowApp($event.item.data?.id);
				}
			});
		}
	}

	request(distributionId) {
		const params = {
			id: distributionId,
			summary: 'Phân phối đầu tư bán theo kỳ hạn',
		}
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", params)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._distributionService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.getDetail(distributionId);
					}
				});
			}
		});
	}

	approve(distributionId) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", { id: distributionId })
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._distributionService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.getDetail(distributionId);
					}
				});
			}
		});
	}

	cancel(distributionId) {
		const ref = this.dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC("Hủy duyệt", { id: distributionId })
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._distributionService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
						this.getDetail(distributionId);
					}
				});
			}
		});
	}

	check(distributionId) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt phân phối đầu từ phiếu này không?',
			...AtributionConfirmConst,
			accept: () => {
				this._distributionService.check({ id: distributionId }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.getDetail(distributionId);
					}
				});
			},
			reject: () => {
			},
		});
	}

	// KHOA TAM
	toggleClosed(distributionId) {
		this._distributionService.toggleIsClosed(distributionId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật trạng thái đóng thành công")) {
				this.getDetail(distributionId);
			}
		});
	}

	// BAT TAT IS SHOW APP
	toggleIsShowApp(distributionId) {
		this._distributionService.toggleIsShowApp(distributionId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Cập nhật show app thành công")) {
				this.getDetail(distributionId);
			}
		});
	}

	// LABEL EDIT BUTTON
	labelButtonEdit() {
		return this.isEdit ? 'Lưu lại' : 'Chỉnh sửa';
	}

	// GET SECONDARY
	getDetail(distributionId, isLoading = true) {
		this.isLoading = isLoading;
		this._distributionService.getById(distributionId).subscribe(res => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.distribution = {
					...res.data,
					openCellDate: new Date(res.data?.openCellDate),
					closeCellDate: new Date(res.data?.closeCellDate),
				};
				//
				this.distribution.project.typeNames = '';
				for(let item of ProjectConst.projectTypes) {
					if(this.distribution?.project?.projectTypes) {
						if(this.distribution?.project?.projectTypes.includes(item.type)) {
							this.distribution.project.typeNames += item.name + ', ';
						}
					}
				}
				this.genListAction(this.distribution);
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

	onChangeOpenCellDate($event) {
		if (+new Date($event) > +new Date(this.distribution.closeCellDate)) {
			this.distribution.closeCellDate = null;
		}
	}

	setStatusEdit() {
		this.isEdit = !this.isEdit;
	}

	changeEdit() {
		if (this.isEdit) {

            let body = this.formatCalendar(this.fieldDates, 
				{...this.filterField(this.distribution, this.distributionUpdate)}
			);
			this._distributionService.update(body).subscribe((response) => {
				if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
					this.setStatusEdit();
					this.getDetail(this.distributionId, false);
				} 
			}, (err) => {
				console.log('err---', err);
			});
		} else {
			this.setStatusEdit();
		}
	}

	getDisplayNameBank(businessCustomerBankAccId) {
		let bankInfo = this.listBanks.find(b => b.businessCustomerBankAccId == businessCustomerBankAccId);
		return bankInfo ? (bankInfo?.bankAccNo + ' - ' + bankInfo?.bankName) : null;
	}

}
