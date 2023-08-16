import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Router } from '@angular/router';
import { OBJECT_PRODUCT } from '@shared/base-object';
import { ProductService } from '@shared/services/product.service';
import { AppConsts, MessageErrorConst, ProductConst, SearchConst, UnitDateConst, YesNoConst } from '@shared/AppConsts';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { UploadImageComponent } from 'src/app/components-general/upload-image/upload-image.component';

@Component({
	selector: 'app-create-invest',
	templateUrl: './create-invest.component.html',
	styleUrls: ['./create-invest.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService]
})
export class CreateInvestComponent extends CrudComponentBase {

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
	YesNoConst = YesNoConst; 	// Yes No
	MessageErrorConst = MessageErrorConst;

	invest: any = {...{...OBJECT_PRODUCT.GENERAL, ...OBJECT_PRODUCT.INVEST }};

	customers = [];

	customerOwners = [];
	ownerInfo: any;

	customerGeneralContractors = [];
	generalContractorInfo:any;

	fieldDates = ['startDate', 'endDate'];

	keywordOwner = '';
	keywordGeneralContractor = '';

	serachDebounceTime = {
		keywordOwner: new Subject(),
		keywordGeneralContractor: new Subject(),
	};
	baseUrl: string;

	ngOnInit() {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.invest.productType = ProductConst.INVEST;
		this.invest.summary = 'Thêm mới đầu tư BĐS';

		//
		this.setupSearchDebounceTime();
	}

	selectImg() {
		const ref = this._dialogService.open(UploadImageComponent, {
		data: {
			inputData: [this.invest.icon]
		},
		header: 'Tải hình ảnh',
		width: '500px',
		footer: ""
		});
		ref.onClose.subscribe(images => { 
		  if(images && images.length > 0) {
			this.invest.icon = images[0].data;
		  }
		});
	  }

	setupSearchDebounceTime() {
		this.serachDebounceTime.keywordOwner.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((typeSearch: string) => {
            if (this.keywordOwner.trim()) {
                this.getInfoCustomer(typeSearch);
            } else {
                this.ownerInfo = {};
            }
        });
		//
		this.serachDebounceTime.keywordGeneralContractor.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((typeSearch: string) => {
            if (this.keywordGeneralContractor.trim()) {
                this.getInfoCustomer(typeSearch);
            } else {
                this.generalContractorInfo = {};
            }
        });
	}

	search(typeSearch) {
		this.serachDebounceTime[typeSearch].next(typeSearch);
	}

	getInfoCustomer(typeSearch) {
		this.isLoading = false;
		let keyword;
		keyword = this[typeSearch].trim();
		console.log("key word: "+this.keyword);
		if(keyword != "") {
			this.productService.getBusinessCustomer(keyword).subscribe((res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, '')) {
					console.log('res___', res);
					if (!res.data) {
						this.messageError(MessageErrorConst.message.DataNotFound);
						this.customerOwners = [];
						this.customerGeneralContractors = [];
					} else {
						if(typeSearch == 'keywordOwner') {
							this.customerOwners = [res.data];
						} else {
							this.customerGeneralContractors = [res.data];
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

	isChooseOwnerCustomer(item) {
		this.ownerInfo = {...item};
		this.invest.invOwnerId = this.ownerInfo.businessCustomerId;
	}

	isChooseGeneralContractorCustomer(item) {
		this.generalContractorInfo = {...item};
		this.invest.invGeneralContractorId = this.generalContractorInfo.businessCustomerId;
	}

	save() {
		if(this.validForm()) {
		this.submitted = true;
		console.log({ invest: this.invest });
		
		let body = this.formatCalendar(this.fieldDates, {...this.invest});

		this.productService.create(body).subscribe((response) => {
				if (this.handleResponseInterceptor(response)) {
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
			console.log("this.invest",this.invest);
		}
	}

	clearOwner() {
		this.keywordOwner = null;
		this.ownerInfo = null;
		this.customerOwners = [];
	}

	clearGeneralContractor() {
		this.keywordGeneralContractor = null;
		this.generalContractorInfo = null;
		this.customerGeneralContractors = [];
	}

	validForm(): boolean {
		const validRequired = this.invest?.invGeneralContractorId 
							&& this.invest?.invOwnerId 
							&& this.invest?.code 
							&& this.invest?.name;
		return validRequired;
	}

	close() {
		this._ref.close();
	}

}
