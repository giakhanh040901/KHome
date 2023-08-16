import { ProductService } from '@shared/services/product.service';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DialogService } from 'primeng/dynamicdialog';
import { ActivatedRoute, Router } from '@angular/router';
import { OBJECT_PRODUCT } from '@shared/base-object';
import { ProductConst, StatusApprove } from '@shared/AppConsts';
import { TabView } from 'primeng/tabview';
import { FormRequestComponent } from 'src/app/form-general/form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from 'src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent extends CrudComponentBase {

  constructor(
		_injector: Injector,
		_messageService: MessageService,
		private _router: Router,
		private _routeActive: ActivatedRoute,
		private _breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
		private _confirmationService: ConfirmationService,
		private productService: ProductService,
	) {
		super(_injector, _messageService);
		this._breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Sản phẩm tích lũy" }
		]);
		this.productId = +this.cryptDecode(this._routeActive.snapshot.paramMap.get('id'));
	}

	@ViewChild(TabView) tabView: TabView;

	// CONST
	ProductConst = ProductConst;
	StatusApprove = StatusApprove;

	productId: number;
	productDetail: any;

	isEdit: boolean = false;

	fieldDates = ['startDate', 'endDate'];

	interfaceShare = {...OBJECT_PRODUCT.GENERAL,...OBJECT_PRODUCT.SHARE};
	productUpdate = {
		"id": 0,
		"productType": null,
		"icon": null,
		"name": null,
		"code": null,
		"startDate": null,
		"endDate": null,
		"maxInvestor": null,
		"minInvestDay": null,
		"countType": null,
		"guaranteeOrganization": null,
		"isPaymentGurantee": null,
		"isCollateral": null,
		"cpsIssuerId": null,
		"cpsDepositProviderId": null,
		"cpsParValue": null,
		"cpsPeriod": null,
		"cpsPeriodUnit": null,
		"cpsInterestRate": null,
		"cpsInterestRateType": null,
		"cpsInterestPeriod": null,
		"cpsInterestPeriodUnit": null,
		"cpsNumberClosePer": null,
		"cpsQuantity": null,
		"cpsIsListing": null,
		"cpsIsAllowSBD": null,
		"invOwnerId": null,
		"invGeneralContractorId": null,
		"invTotalInvestmentDisplay": null,
		"invTotalInvestment": null,
		"invArea": null,
		"invLocationDescription": null,
		"invLatitude": null,
		"invLongitude": null,
		"invProductTypes": null,
		"summary": null,
	}

	tabViewActive = {
		'thongTinChung': true,
		'daiLyPhanPhoi': false,
		'taiSanDamBao': false,
		'hoSoPhapLy': false,
        'lichSu': false,
	};

	ngOnInit() {
		this.getDetail();
	}

	changeTab(e) {
        let tabHeader = this.tabView.tabs[e.index].header;
		this.tabViewActive[tabHeader] = true;
    }

	getDetail(isLoading = true) {
		this.isLoading = isLoading;
		this.productService.getById(this.productId).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res) && res.data) {
				this.productDetail = {
					...res.data,
					startDate: new Date(res.data?.startDate),
					endDate: new Date(res.data?.endDate),
					summary: this.productDetail?.summary,
				};
			}
			//
			console.log('res___', res);
		});
	}

	setStatusEdit() {
        this.isEdit = !this.isEdit;
    }

	changeEdit() {
        if (this.isEdit) {
            let body = this.formatCalendar(this.fieldDates,
				{...this.filterField(this.productDetail, this.productUpdate)}
			);
            this.productService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    console.log("order data ...", this.productDetail);
                    this.setStatusEdit();
                } 
            });
        } else {
            this.setStatusEdit();
        }
    }

	request(product) {
		const params = {
			id: product.id,
			summary: ProductConst.getTypeName(product?.productType) + ': ' + product?.code + ' - ' + product?.name + ' ( ID: ' + product.id + ' )',
		}
		const ref = this._dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", params)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this.productService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.getDetail(false);
					}
				});
			}
		});
	}

	approveSharing(product){
		const ref = this._dialogService.open(
			FormApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					id: product.id
				},
			}
		);
		console.log("abc", product);

		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				const body = {
					approveNote: dataCallBack?.data?.approveNote,
					id: product.id,
					cancelNote: dataCallBack?.data?.approveNote,
				}
				if ( dataCallBack?.checkApprove == true) {
					this.productService.approve(body).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
							this.getDetail();
						}
					});
				} 
			}
		});
	}

}
