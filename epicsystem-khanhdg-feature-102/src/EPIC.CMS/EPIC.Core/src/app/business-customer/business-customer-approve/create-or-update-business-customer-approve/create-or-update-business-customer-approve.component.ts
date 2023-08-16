import { Component, Injector } from "@angular/core";
import { Router } from "@angular/router";
import { ErrorBankConst, KeyFilter, MessageErrorConst, SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { CreateOrEditBusinessCustomer } from "@shared/interfaces/business-customer/BusinesCustomer.model";
import { NationalityConst } from "@shared/nationality-list";
import { BankServiceProxy } from "@shared/service-proxies/bank-service";
import { BusinessCustomerApproveServiceProxy } from "@shared/service-proxies/business-customer-service";
import { InvestorServiceProxy } from "@shared/service-proxies/investor-service";
import { MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";

@Component({
	selector: "app-create-or-update-business-customer-approve",
	templateUrl: "./create-or-update-business-customer-approve.component.html",
	styleUrls: ["./create-or-update-business-customer-approve.component.scss"],
})
export class CreateOrUpdateBusinessCustomerApproveComponent extends CrudComponentBase {
	constructor(
		injector: Injector, 
		messageService: MessageService,
		public configDialog: DynamicDialogConfig,
		public ref: DynamicDialogRef,
        private _businessCustomerApproveService: BusinessCustomerApproveServiceProxy,
        private _bankService: BankServiceProxy,
        private _investorService: InvestorServiceProxy,
        private router: Router,
	) {
		super(injector, messageService);
	}

    fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];
    fieldErrors = {};
	businessCustomer: CreateOrEditBusinessCustomer = new CreateOrEditBusinessCustomer();

    banks: any[] = [];
    isLoadingBank: boolean = true;

	NationalityConst = NationalityConst;
	KeyFilter = KeyFilter;
	
	ngOnInit(): void {
        this.getAllBank();
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.businessCustomer.bankId) {
				this.keyupBankAccount();
			} 
		});
	}

	getAllBank() {
        this.page.keyword = this.keyword;
        this.isLoading = true;
        this._bankService.getAllBank(this.page).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.page.totalItems = res.data.totalItems;
                    this.banks = res.data.items;
                    console.log({ banks: res.data.items, totalItems: res.data.totalItems });
                }
            },
            (err) => {
                this.isLoading = false;
                console.log('Error-------', err);
                
            }
        );
    }

	changeBankId(value) {
		this.businessCustomer.bankAccNo= ''
		this.businessCustomer.bankAccName= ''
        this.businessCustomer.bankId = value
	}

	keyupBankAccount() {
        this.isLoadingBank = true;
		this.businessCustomer.bankAccName ='';
			this._investorService.getBankAccount(this.businessCustomer.bankId,this.businessCustomer.bankAccNo ).subscribe(
				(res) => {
					this.isLoadingBank = false;
                    if(res.code === ErrorBankConst.LOI_KET_NOI_MSB|| res.code === ErrorBankConst.SO_TK_KHONG_TON_TAI) {
						this.messageService.add({
							severity: 'error',
							summary: '',
							detail: 'Không tìm thấy thông tin chủ tài khoản, vui lòng kiểm tra lại (FE)',
							life: 3000,
						});
						this.businessCustomer.bankAccName = res?.data;
					} else
					if (this.handleResponseInterceptor(res)) {
						console.log("res",res);
						this.businessCustomer.bankAccName = res?.data;
					}
				},
				() => {
					this.isLoadingBank = false;
				}
			);
	}

	resetValid(field) {
        this.fieldErrors[field] = false;
    }

	close() {
		this.ref.close();
	}

	save() {
		if (this.validForm()){
			this.submitted = true;
			console.log({ businessCustomerBBB: this.businessCustomer });
			this.businessCustomer.bankAccName = this.removeVietnameseTones(this.businessCustomer.bankAccName).toUpperCase();

			let body = this.formatCalendarSendApi(this.fieldDates, {...this.businessCustomer});
			if (this.businessCustomer.businessCustomerTempId) {
				this._businessCustomerApproveService.update(body).subscribe((response) => {
					if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
						this.submitted = false;
						this.ref.close({ data: response, accept: true });
					} else {
						this.submitted = false;
					}
				}, () => {
					this.submitted = false;
					}
				);
			} else {
				this._businessCustomerApproveService.create(body).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
							this.submitted = false;
							this.router.navigate(['/customer/business-customer/business-customer-approve/detail/' + this.cryptEncode(response?.data?.businessCustomerTempId)]);
							this.ref.close({ data: response, accept: true });
						} else {
							this.submitted = false;
						}
					}, () => {
						this.submitted = false;
					}
				);
			}
		} else {
			this.messageError(MessageErrorConst.message.Validate);
		}
    }

    validForm(): boolean {
        const validRequired = this.businessCustomer?.code?.trim() && this.businessCustomer?.taxCode?.trim()
                            && this.businessCustomer?.licenseIssuer?.trim() && this.businessCustomer?.tradingAddress?.trim()
                            && this.businessCustomer?.name?.trim() && this.businessCustomer?.shortName?.trim()
                            && this.businessCustomer?.email?.trim() && this.businessCustomer?.address?.trim()
                            && this.businessCustomer?.nation && this.businessCustomer?.repName?.trim()
                            && this.businessCustomer?.repPosition?.trim() && this.businessCustomer?.bankAccNo?.trim()
                            && this.businessCustomer?.bankAccName?.trim() && this.businessCustomer?.bankId != null;
        return validRequired;
    }

	// save() {
		// if (this.validForm()){
		// 	this.submitted = true;
		// 	console.log({ businessCustomerBank: this.businessCustomerBank });
		// 	this.businessCustomerBank.bankAccName = this.removeVietnameseTones(
		// 		this.businessCustomerBank.bankAccName
		// 	).toUpperCase();
		// 	//
		// 	if (this.businessCustomerBank.businessCustomerBankId) {
		// 		this._businessCustomerBankService
		// 			.update(this.businessCustomerBank)
		// 			.subscribe(
		// 				(response) => {
		// 					//
		// 					if (
		// 						this.handleResponseInterceptor(response, "Cập nhật thành công")
		// 					) {
		// 						this.submitted = false;
		// 						this.ref.close({ data: response, accept: true });
		// 					} else {
		// 						this.submitted = false;
		// 					}
		// 				},
		// 				() => {
		// 					this.submitted = false;
		// 				}
		// 			);
		// 	} else {
		// 		this.businessCustomerBank.businessCustomerId =
		// 			this.businessCustomerDetail.businessCustomerId;
		// 		this._businessCustomerBankService
		// 			.create(this.businessCustomerBank)
		// 			.subscribe(
		// 				(response) => {
		// 					if (this.handleResponseInterceptor(response, "Thêm thành công")) {
		// 						this.submitted = false;
		// 						setTimeout(() => {
		// 							this.router.navigate([
		// 								"/customer/business-customer/business-customer-approve/detail/" +
		// 									this.cryptEncode(response.data.businessCustomerTempId),
		// 							]);
		// 						}, 800);
		// 						this.ref.close({ data: response, accept: true });
	
		// 					} else {
		// 						this.submitted = false;
		// 					}
		// 				},
		// 				() => {
		// 					this.submitted = false;
		// 				}
		// 			);
		// 	}
		// } else {
		// 	this.messageError(MessageErrorConst.message.Validate);
		// }
	// }


}
