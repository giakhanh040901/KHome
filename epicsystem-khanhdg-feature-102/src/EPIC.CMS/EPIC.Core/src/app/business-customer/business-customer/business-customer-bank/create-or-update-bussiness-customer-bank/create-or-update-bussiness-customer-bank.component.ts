import { Component, Injector, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ErrorBankConst, KeyFilter, MessageErrorConst, SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { BankServiceProxy } from "@shared/service-proxies/bank-service";
import { BusinessCustomerBankServiceProxy } from "@shared/service-proxies/business-customer-service";
import { InvestorServiceProxy } from "@shared/service-proxies/investor-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";

@Component({
	selector: "app-create-or-update-bussiness-customer-bank",
	templateUrl: "./create-or-update-bussiness-customer-bank.component.html",
	styleUrls: ["./create-or-update-bussiness-customer-bank.component.scss"],
	providers: [ConfirmationService, MessageService],
})
export class CreateOrUpdateBussinessCustomerBankComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _businessCustomerBankService: BusinessCustomerBankServiceProxy,
		private _investorService: InvestorServiceProxy,
		private router: Router,
		private _bankService: BankServiceProxy,
		public confirmationService: ConfirmationService,
		public ref: DynamicDialogRef,
		public configDialog: DynamicDialogConfig
	) {
		super(injector, messageService);
	}

	businessCustomerBank: any = {
		id: 0,
		businessCustomerId: null,
		businessCustomerTempId: null,
		bankId: null,
		bankBranchName: null,
		bankAccNo: null,
		bankAccName: null,
		isTemp: false,
	};
	banks: any = [];
	fieldErrors = {};
	isLoadingBank: boolean = false;
	businessCustomerDetail: any = {};
	businessCustomerId: number;
	isApprove: boolean;
	KeyFilter = KeyFilter;

	ngOnInit(): void {
		this.businessCustomerDetail = this.configDialog?.data?.businessCustomerDetail;
		this.isApprove = this.configDialog?.data?.isApprove;
		if (this.isApprove) this.businessCustomerBank.isTemp = true;
		this.businessCustomerId = this.businessCustomerDetail.businessCustomerId;
		this.getAllBank();
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.businessCustomerBank.bankId) {
				this.keyupBankAccount();
			} 			
		});
	}

	keyupBankAccount() {
		this.isLoadingBank = true;
			console.log("this.investorBank",this.businessCustomerBank);
			this.businessCustomerBank.ownerAccount ='';
				this._investorService.getBankAccount(this.businessCustomerBank.bankId,this.businessCustomerBank.bankAccNo ).subscribe(
					(res) => {
						this.isLoadingBank = false;
			  if(res.code === ErrorBankConst.LOI_KET_NOI_MSB|| res.code === ErrorBankConst.SO_TK_KHONG_TON_TAI) {
							this.messageService.add({
								severity: 'error',
								summary: '',
								detail: 'Không tìm thấy thông tin chủ tài khoản, vui lòng kiểm tra lại (FE)',
								life: 3000,
							});
							this.businessCustomerBank.bankAccName = res?.data;
						} else
						if (this.handleResponseInterceptor(res)) {
							console.log("res",res);
							this.businessCustomerBank.bankAccName = res?.data;
						}
					},
					() => {
						this.isLoadingBank = false;
					}
				);
		}
		
	changeBankId(value) {
		this.businessCustomerBank.bankAccNo = "";
		this.businessCustomerBank.bankAccName = "";
		console.log("value", value);
		this.businessCustomerBank.bankId = value;
	}

	getAllBank() {
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._bankService.getAllBank(this.page).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) {
					this.page.totalItems = res.data.totalItems;
					this.banks = res?.data?.items;
					if (this.banks){
						this.banks.map(bank => {
							bank.labelName = bank.bankName + ' - ' + bank.fullBankName;
							return bank;
						})
					}
					console.log({
						banks: res.data.items,
						totalItems: res.data.totalItems,
					});
				}
			},
			() => {
				this.isLoading = false;
			}
		);
	}

	resetValid(field) {
		this.fieldErrors[field] = false;
	}

	save() {
		if (this.validForm()){
			this.submitted = true;
			console.log({ businessCustomerBank: this.businessCustomerBank }, { businessCustomer: this.businessCustomerDetail });
			this.businessCustomerBank.bankAccName = this.removeVietnameseTones(
				this.businessCustomerBank.bankAccName
			).toUpperCase();
			//
			if (this.businessCustomerBank.businessCustomerBankId) {
				this._businessCustomerBankService.update(this.businessCustomerBank).subscribe((response) => {
							//
					if (
						this.handleResponseInterceptor(response, "Cập nhật thành công")
					) {
						this.submitted = false;
						this.ref.close({ data: response, accept: true });
					} else {
						this.submitted = false;
					}},
						() => {
							this.submitted = false;
						}
					);
			} else {
				if (!this.isApprove){
					this.businessCustomerBank.businessCustomerId = this.businessCustomerDetail.businessCustomerId;
				} else {
					this.businessCustomerBank.businessCustomerId = this.businessCustomerDetail.businessCustomerTempId
				}
				this._businessCustomerBankService.create(this.businessCustomerBank).subscribe( (response) => {
					if (this.handleResponseInterceptor(response, "Thêm thành công")) {
						this.submitted = false;
						if (!this.isApprove){
							setTimeout(() => {
								this.router.navigate([
									"/customer/business-customer/business-customer-approve/detail/" +
										this.cryptEncode(response.data.businessCustomerTempId),
								]);
							}, 800);
						}
						this.ref.close({ data: response, accept: true });
					} else {
						this.submitted = false;
					}},
					() => {
						this.submitted = false;
					}
				);
			}
			
		} else {
			this.messageError(MessageErrorConst.message.Validate);
		}
	}

	close() {
		this.ref.close();
	}

	validForm(): boolean {
		const validRequired = this.businessCustomerBank?.bankAccNo?.trim()
		  && this.businessCustomerBank?.bankAccName?.trim()
		  && this.businessCustomerBank?.bankId;
		return validRequired;
	  }
}
