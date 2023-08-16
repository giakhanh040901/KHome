import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Router } from '@angular/router';
import { OBJECT_PRODUCT } from '@shared/base-object';
import { ProductService } from '@shared/services/product.service';
import { Subject } from 'rxjs';
import { AppConsts, MessageErrorConst, ProductConst, SearchConst, UnitDateConst, YesNoConst } from '@shared/AppConsts';
import { debounceTime } from 'rxjs/operators';
import { UploadImageComponent } from 'src/app/components-general/upload-image/upload-image.component';


@Component({
	selector: 'app-create-share',
	templateUrl: './create-share.component.html',
	styleUrls: ['./create-share.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService]
})
export class CreateShareComponent extends CrudComponentBase {

	constructor(
		_injector: Injector,
		_messageService: MessageService,
		private _router: Router,
		private _breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
		private _confirmationService: ConfirmationService,
		public _ref: DynamicDialogRef,
		//
		private productService: ProductService,
	) {
		super(_injector, _messageService);
	}

	// CONST
	ProductConst = ProductConst;
	UnitDateConst = UnitDateConst;	
	YesNoConst = YesNoConst; 
	MessageErrorConst = MessageErrorConst;

	customerDeposits = [];
	depositInfo: any;

	customerIssuers = [];
	issuerInfo:any;
	
	share: any = {...{...OBJECT_PRODUCT.GENERAL,...OBJECT_PRODUCT.SHARE}}

	fieldDates = ['startDate', 'endDate'];

	keywordIssuer = '';
	keywordDeposit = '';

	serachDebounceTime = {
		keywordIssuer: new Subject(),
		keywordDeposit: new Subject(),
	};
	baseUrl: string;

	ngOnInit() {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.share.productType = ProductConst.SHARE;
		this.share.summary = "Thêm sản phẩm cổ phần";

		this.setupSearchDebounceTime();
	}

	selectImg() {
		const ref = this._dialogService.open(UploadImageComponent, {
		data: {
			inputData: [this.share.icon]
		},
		header: 'Tải hình ảnh',
		width: '500px',
		footer: ""
		});
		ref.onClose.subscribe(images => { 
		  if(images && images.length > 0) {
			this.share.icon = images[0].data;
		  }
		});
	  }

	setupSearchDebounceTime() {
		this.serachDebounceTime.keywordIssuer.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((typeSearch: string) => {
            if (this.keywordIssuer.trim()) {
                this.getInfoCustomer(typeSearch);
            } else {
                this.issuerInfo = {};
            }
        });
		//
		this.serachDebounceTime.keywordDeposit.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((typeSearch: string) => {
            if (this.keywordDeposit.trim()) {
                this.getInfoCustomer(typeSearch);
            } else {
                this.depositInfo = {};
            }
        });
	}

	search(typeSearch) {
		this.serachDebounceTime[typeSearch].next(typeSearch);
	}

	changeInterestRateType(type) {
		if(type == ProductConst.CUOI_KY){
			this.share.cpsInterestPeriod = null;
			this.share.cpsInterestPeriodUnit = null
		}
		console.log('this.share.cpsInterestPeriod ', this.share.cpsInterestPeriod);
		console.log('this.share.cpsInterestPeriodUnit', this.share.cpsInterestPeriodUnit);
	}

	getInfoCustomer(typeSearch) {
		this.isLoading = false;
		let keyword;
		keyword = this[typeSearch].trim();
		if(keyword != "") {
			this.productService.getBusinessCustomer(keyword).subscribe((res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, '')) {
					console.log('res___', res);
					if (!res.data) {
						this.messageError(MessageErrorConst.message.DataNotFound);
						if(typeSearch == 'keywordIssuer') this.customerIssuers = [];
						if(typeSearch == 'keywordDeposit') this.customerDeposits = [];
					} else {
						if(typeSearch == 'keywordIssuer') {
							this.customerIssuers = [res.data];
						} else {
							this.customerDeposits = [res.data];
						}
					}
				}
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
			});
		} else {
			this.isLoading = false;
			this.messageError(MessageErrorConst.message.DataNotFound);
		}
    }

	isChooseDepositCustomer(item) {
		this.depositInfo = {...item};
		this.share.cpsDepositProviderId = this.depositInfo.businessCustomerId;
	}

	isChooseIssuerCustomer(item) {
		this.issuerInfo = {...item};
		this.share.cpsIssuerId = this.issuerInfo.businessCustomerId;
	}

	clearIssuer() {
		this.keywordIssuer = null;
		this.issuerInfo = null;
		this.customerIssuers = []
	}

	clearDeposit() {
		this.keywordDeposit = null;
		this.depositInfo = null;
		this.customerDeposits = [];
	}

	save() {
		if(this.validForm()) {
			console.log('share__', this.share);
			this.submitted = true;
			
			let body = this.formatCalendar(this.fieldDates, {...this.share});
			this.productService.create(body).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Thêm thành công")) {
						this.submitted = false;
						this._ref.close(response);
					} else {
						this.submitted = false;
					}
				}, (err) => {
					this.submitted = false;
				}
			);
		} else {
			this.messageError(MessageErrorConst.message.Validate);
		}
	}

	validForm(): boolean {
		const validRequired = 
			// this.share.cpsDepositProviderId 
			this.share.cpsIssuerId 
			&& this.share.code
			// && this.share.cpsPeriod
			// && this.share.cpsPeriodUnit
			&& this.share.cpsInterestRate
			&& this.share.cpsInterestRateType
			&& (this.share.cpsInterestRateType == ProductConst.CUOI_KY || this.share.cpsInterestPeriod)
			&& (this.share.cpsInterestRateType == ProductConst.CUOI_KY || this.share.cpsInterestPeriodUnit) 
			&& this.share.cpsNumberClosePer
			&& this.share.countType
			&& this.share.cpsParValue
			&& this.share.cpsQuantity
			&& this.share.startDate
			// && this.share.endDate; 
	
		return validRequired;
	}

	close() {
		this._ref.close();
	}

}
