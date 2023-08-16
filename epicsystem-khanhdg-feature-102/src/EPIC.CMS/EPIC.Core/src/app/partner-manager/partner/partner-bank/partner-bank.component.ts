import { Component, Injector, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ErrorBankConst, FormNotificationConst, KeyFilter, MessageErrorConst, SearchConst, TradingProviderConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { BankServiceProxy } from "@shared/service-proxies/bank-service";
import { InvestorServiceProxy } from "@shared/service-proxies/investor-service";
import { PartnerBankService } from "@shared/services/partner-bank-account.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from "src/app/form-notification/form-notification.component";

@Component({
	selector: "app-partner-bank",
	templateUrl: "./partner-bank.component.html",
	styleUrls: ["./partner-bank.component.scss"],
	providers: [ConfirmationService, MessageService]
})
export class PartnerBankComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _bankService: BankServiceProxy,
		private _partnerBankService: PartnerBankService,
		private _investorService: InvestorServiceProxy,
		private _dialogService: DialogService,
		private confirmationService: ConfirmationService,
		private router: Router,
	) {
		super(injector, messageService);
	}

	@Input() partnerDetail: any = {};

	ref: DynamicDialogRef;
	YesNoConst = YesNoConst;
	TradingProviderConst = TradingProviderConst;
	KeyFilter = KeyFilter;

	modalDialog: boolean;
  
	rows: any[] = [];
	banks: any = {};
	isLoadingBank: boolean;

	partnerBank: any = {
		"id": 0,
		"bankId": null,
		"bankAccNo": null,
		"bankAccName": null,
	}

	itemTradingProviderInfo = {};
	fieldErrors = {};
	submitted: boolean;

	isDetail = false;
	actionsDisplay: any[] = [];
	actions: any[] = [];
	
	ngOnInit(): void {
		this.setPage();
		this.getAllBank();
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.partnerBank.bankId) {
				this.keyupBankAccount();
			} 
		});
	}

	keyupBankAccount() {
		this.isLoadingBank = true;
		this._investorService.getBankAccount(this.partnerBank.bankId,this.partnerBank.bankAccNo ).subscribe(
			(res) => {
				this.isLoadingBank = false;
				if (this.handleResponseInterceptor(res)) {
					if(res.code === ErrorBankConst.LOI_KET_NOI_MSB|| res.code === ErrorBankConst.SO_TK_KHONG_TON_TAI) {
						this.messageError('Không tìm thấy thông tin chủ tài khoản, vui lòng kiểm tra lại (FE)');
					} 
					if (res?.data){
						this.partnerBank.bankAccName = res?.data;
					}
				}
			},
			() => {
				this.isLoadingBank = false;
			}
		);
	}

	changeBankId(value) {
		this.partnerBank.bankAccNo= ''
		this.partnerBank.bankAccName= ''
    	this.partnerBank.bankId = value
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
		  () => {
			this.isLoading = false;
		  }
		);
	}

	genListAction(data = []) {
		this.actions = data.map(item => {
		  	const action = [];

			if (item.isDefault !== YesNoConst.YES) {
				action.push({
					data: item,
					label: 'Sửa',
					icon: 'pi pi-pencil',
					command: ($event) => {
						this.edit($event.item.data);
					}
				});	
			}
	
			if (item.isDefault !== YesNoConst.YES && item.status == TradingProviderConst.KICH_HOAT ) {
				action.push({
				data: item,
				label: 'Chọn mặc định',
				icon: 'pi pi-check',
				command: ($event) => {
					this.setDefault($event.item.data);
				}
				});
			}

			if (item.isDefault !== YesNoConst.YES) {
				action.push({
					data: item,
					label: item.status == TradingProviderConst.KICH_HOAT ? 'Hủy kích hoạt' : 'Kích hoạt',
					icon: item.status == TradingProviderConst.KICH_HOAT ? 'pi pi-lock' : 'pi pi-check-circle',
					command: ($event) => {
						this.changeStatus($event.item.data);
					}
				});	
			}

			if (item.isDefault !== YesNoConst.YES) {
				action.push(
				{
					data: item,
					label: 'Xóa',
					icon: 'pi pi-trash',
					command: ($event) => {
						this.delete($event.item.data);
					}
				}
				);

			}
		  	return action;
		});
	}


	setPage( pageInfo?: any){
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		//
		this._partnerBankService.findAll(this.page, this.partnerDetail.partnerId).subscribe((res) => {
		  this.isLoading = false;
		  if (this.handleResponseInterceptor(res, '')) {
			this.page.totalItems = res.data.totalItems;
			this.rows = res.data.items;
			this.genListAction(this.rows);
			console.log({ rowsPayment: res.data.items, totalItems: res.data.totalItems });
		  }
		}, () => {
		  this.isLoading = false;
		});
	}
	
	header(): string {
		return !this.partnerBank?.id ? 'Thêm tài khoản ngân hàng' : 'Sửa tài khoản ngân hàng';
	}
	
	create() {
		this.isLoadingBank = true;
		this.partnerBank = {};
		this.submitted = false;
		this.modalDialog = true;
	}
	
	edit(row) {
		this.partnerBank = {
		  ...row,
		};
		console.log({ partnerBank: this.partnerBank });
		this.modalDialog = true;
	}

	changeStatus(bank) {
		let body = {
			id: bank.id,
			partnerId: bank.partnerId 
		}
		this._partnerBankService.changeStatus(bank.id, bank.partnerId).subscribe((res) => {
			if (this.handleResponseInterceptor(res, 
				bank.status == TradingProviderConst.KICH_HOAT ? "Hủy kích hoạt thành công" : "Kích hoạt thành công"
			)) {
				this.setPage();
			}
		})
	}
	
	setDefault(bank) {
		this._partnerBankService.setDefault(bank.id, bank.partnerId).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Chọn tài khoản này thành mặc định thành công")) {
				this.setPage();
			}
		});
	}

	
	
	detail() {
		this.isDetail = true;
		this.partnerBank = {
		  ...this.partnerBank,
		};
	
		console.log({ partnerBank: this.partnerBank });
		this.modalDialog = true;
	}
	
	delete(bank) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn xóa tài khoản ngân hàng này?',
			header: 'Xóa tài khoản ngân hàng',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
		  	icon: 'pi pi-exclamation-triangle',
			accept: () => {
				this._partnerBankService.delete(bank.id, bank.partnerId).subscribe((response) => {
				  if (this.handleResponseInterceptor(response, "Xóa thành công")) {
					this.setPage();
				  }
				});
			},
			reject: () => {
		
			},
		});
	}
	
	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}
	
	save() {
		if (this.validForm()){
			this.submitted = true;
			console.log({ partnerBank: this.partnerBank });
			this.partnerBank.bankAccName = this.removeVietnameseTones(this.partnerBank.bankAccName).toUpperCase();
			if (this.partnerBank.id) {
			  this._partnerBankService.update(this.partnerBank).subscribe((response) => {
				//
				if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
				  this.submitted = false;
				  this.setPage({ page: this.page.pageNumber });
				  this.hideDialog();
				} else {
				  this.submitted = false;
				}
			  }, () => {
				this.submitted = false;
			  }
			  );
			} else {
				this._partnerBankService.create(this.partnerBank).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
					this.submitted = false;
					this.setPage();
					this.hideDialog();
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
		const validRequired = this.partnerBank?.bankAccNo?.trim()
							&& this.partnerBank?.bankAccName?.trim()
							&& this.partnerBank?.bankId;
		return validRequired;
	}
}
