import { Component, Injector, OnInit } from "@angular/core";
import { MessageErrorConst, ProductConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ConfirmationService, MessageService } from "primeng/api";
import { DynamicDialogRef, DialogService } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";
import { Router } from "@angular/router";
import { DistributionService } from "@shared/services/distribution.service";
import { OJBECT_DISTRIBUTION_CONST } from "@shared/base-object";

const { BASE } = OJBECT_DISTRIBUTION_CONST;

@Component({
  selector: 'app-create-distribution',
  templateUrl: './create-distribution.component.html',
  styleUrls: ['./create-distribution.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService]
})
export class CreateDistributionComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _distributionService: DistributionService,
		private _router: Router,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
    	public ref: DynamicDialogRef,
		private router: Router
	) {
		super(injector, messageService);
	}

  	/* CONST */
	ProductConst = ProductConst;
	MessageErrorConst= MessageErrorConst;

	distribution: any = { ...BASE.DISTRIBUTION };

	minDate = null;
	maxDate = null;

	listBanks: any[] = [];

  	fieldDates = ["openCellDate", "closeCellDate"];

	products: any;
	product: any = {};

	ngOnInit(): void {
		//
		this.isLoadingPage = true;
		forkJoin([this._distributionService.getAllProduct(this.page), this._distributionService.getBankList()]).subscribe(([resProduct, resBank]) => {
			this.isLoadingPage = false;
			if(this.handleResponseInterceptor(resProduct, '')) {
				if(resProduct?.data?.length) {
					this.products = resProduct.data.map(product => {
						product.displayName = product?.code + ' - ' + product?.name;
						product.totalInvestment = this.getTotalInvestment(product);
						return product;
					});
				}
			}
			//
			if(this.handleResponseInterceptor(resBank, '')) {
				if(resBank?.data?.length) {
					this.listBanks = resBank.data.map(bank => {
						bank.labelName = bank?.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
						return bank;
					});
				}
			}
		}, (err) => {
			this.isLoadingPage = false;
		});
	}

	// GET GIÁ TRỊ HẠN MỨC PHÂN PHỐI
	getTotalInvestment(product) {
		let totalInvestment: number;
		//
		if(product.productType == ProductConst.SHARE) totalInvestment = product?.cpsParValue * product.cpsQuantity;
		if(product.productType == ProductConst.INVEST) totalInvestment = product?.invTotalInvestment;
		//
		return totalInvestment ? totalInvestment : 0;
	}

	save() {
		if(this.validForm())	{
			this.submitted = true;
		let body = this.formatCalendar(this.fieldDates, {...this.distribution});
		this._distributionService.create(body).subscribe((response) => {
				if (this.handleResponseInterceptor(response, "Thêm thành công")) {
					this.submitted = false;
					this.ref.close(response?.data);
				} else {
					this.submitted = false;
				}
			}, (err) => {
				this.submitted = false;
			}
		);
		} else	{
			this.messageError(MessageErrorConst.message.Validate);
		}
		
	}

	close() {
		this.ref.close();
	}

	onChangeOpenCellDate($event) {
		if(+new Date($event) > +new Date(this.distribution.closeCellDate)) {
			this.distribution.closeCellDate = null;
		}
	}

	onChangeProduct($event) {
		const productId = $event.value;

		this.product = this.findProduct(productId);

		if(this.product) {
			this.minDate = this.product.openCellDate ? new Date(this.product.openCellDate) : null;
			this.maxDate = this.product.closeCellDate ? new Date(this.product.closeCellDate) : null;
		}
	}

	findProduct(id) {
		const found = this.products.find(p => p.id === id);
		return found || {};
	}

	validForm(): boolean {
		const validRequired = this.distribution.productId
							&& this.distribution.openCellDate
							&& this.distribution.closeCellDate
							&& this.distribution.tradingBankAccountCollects?.length;
							// && this.distribution.tradingBankAccountPays?.length;
						
		return validRequired;
	}
}

