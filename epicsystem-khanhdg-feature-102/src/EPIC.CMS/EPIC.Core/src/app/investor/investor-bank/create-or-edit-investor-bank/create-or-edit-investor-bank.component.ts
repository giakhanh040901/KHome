import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorBankConst, InvestorConst, KeyFilter, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { BankServiceProxy } from '@shared/service-proxies/bank-service';
import { BusinessCustomerBankServiceProxy } from '@shared/service-proxies/business-customer-service';
import { InvestorServiceProxy } from '@shared/service-proxies/investor-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-edit-investor-bank',
  templateUrl: './create-or-edit-investor-bank.component.html',
  styleUrls: ['./create-or-edit-investor-bank.component.scss']
})
export class CreateOrEditInvestorBankComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _businessCustomerBankService: BusinessCustomerBankServiceProxy,
		private _investorService: InvestorServiceProxy,
		private router: Router,
		private _bankService: BankServiceProxy,
		public confirmationService: ConfirmationService,
		public ref: DynamicDialogRef,
		public configDialog: DynamicDialogConfig,
		private routeActive: ActivatedRoute,
	) { 
		super(injector, messageService);
		this.investorId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
		this.isTemp = +this.routeActive.snapshot.paramMap.get("isTemp");
	}

	investorId: number;
	isTemp = InvestorConst.TEMP.YES;
	fieldErrors = {};
	investorBank: any = {};
	KeyFilter = KeyFilter;
	banks: any = {};
	investorDetail: any = {};

	ngOnInit(): void {
		this.investorDetail = this.configDialog?.data?.investorDetail;
		if (this.configDialog?.data?.investorBank) this.investorBank = this.configDialog?.data?.investorBank;
		this.getAllBank();
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.investorBank.bankId) {
				this.keyupBankAccount();
			} 
		});
	}

	keyupBankAccount() {
		this.isLoading = true;
		console.log("this.investorBank",this.investorBank);
		this.investorBank.ownerAccount ='';
		this._investorService.getBankAccount(this.investorBank.bankId,this.investorBank.bankAccount ).subscribe(
			(res) => {
				this.isLoading = false;
				if(res.code === ErrorBankConst.LOI_KET_NOI_MSB|| res.code === ErrorBankConst.SO_TK_KHONG_TON_TAI) {
					this.messageService.add({
						severity: 'error',
						summary: '',
						detail: 'Không tìm thấy thông tin chủ tài khoản, vui lòng kiểm tra lại (FE)',
						life: 3000,
					});
					this.investorBank.ownerAccount = res?.data;
				} else
				if (this.handleResponseInterceptor(res)) {
					
					console.log("res",res);
					this.investorBank.ownerAccount = res?.data;
				}
			},
			() => {
				this.isLoading = false;
			}
		);
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
					this.banks = this.banks.map(bank => {
						bank.labelName = bank.bankName + ' - ' + bank.fullBankName;
						return bank;
					});
					console.log("this.bankFullName",this.banks);
					
					console.log({ banks: res.data.items, totalItems: res.data.totalItems });

				}
			},
			() => {
				this.isLoading = false;
			}
		);
	}

	changeBankId(value) {
		this.investorBank.bankAccount= ''
		this.investorBank.ownerAccount= ''
		console.log("value",value);
		this.investorBank.bankId = value;
	}

	resetValid(field) {
		this.fieldErrors[field] = false;
	}

	save() {
		this.submitted = true;

		console.log({ investorBank: this.investorBank }, {isTemp: this.isTemp});
		if (this.validForm()){
			this.investorBank.ownerAccount = this.removeVietnameseTones(this.investorBank?.ownerAccount)?.toUpperCase( );
			if (this.investorBank.id) {
				let body = {
					investorBankAccId : this.investorBank.id,
					investorId : this.investorDetail.investorId,
					investorGroupId : this.investorBank.investorGroupId,
					bankAccount : this.investorBank.bankAccount,
					ownerAccount : this.investorBank.ownerAccount,
					isTemp : this.isTemp == InvestorConst.TEMP.YES
				}
				
				this._investorService.updateBank(body).subscribe(
					(response) => {
						this.callTriggerFiledError(response, this.fieldErrors);
						if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
							this.submitted = false;
							if (this.isTemp === InvestorConst.TEMP.NO) {
								this.router.navigate(["/customer/investor/" + this.cryptEncode(response.data) + "/temp/1"]).then(() => {
									window.location.reload();
								});
							}
							this.ref.close({ data: response, accept: true });

						} else {
							this.callTriggerFiledError(response, this.fieldErrors);
							this.submitted = false;
						}
					},
					() => {
						this.submitted = false;
					}
				);
			} else {
				const body = {
					...this.investorBank,
					investorId: this.investorDetail.investorId,
					investorGroupId: this.investorDetail.investorGroupId,
					isTemp: this.isTemp == InvestorConst.TEMP.YES,
				};
				this._investorService.createBank(body).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, "Thêm thành công")) {
							this.submitted = false;
							if (this.isTemp === InvestorConst.TEMP.NO) {
								this.router.navigate(["/customer/investor/" + this.cryptEncode(response.data) + "/temp/1"]).then(() => {
									window.location.reload();
								});
							}
							this.ref.close({ data: response, accept: true });
						} else {
							this.callTriggerFiledError(response, this.fieldErrors);
							this.submitted = false;
						}
					},
					() => {
						this.submitted = false;
					}
				);
			}
		} else {
			this.messageError('Vui lòng nhập đủ thông tin cho các trường có dấu (*)');
		}

	}

	close() {
		this.ref.close();
	}

	validForm(): boolean {
		const validRequired = this.investorBank?.bankAccount?.trim() && this.investorBank?.ownerAccount?.trim();

		return validRequired;
	}
}
