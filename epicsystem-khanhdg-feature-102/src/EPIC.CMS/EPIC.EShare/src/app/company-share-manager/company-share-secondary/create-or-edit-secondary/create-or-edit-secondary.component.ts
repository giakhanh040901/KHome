import { Component, EventEmitter, Injector, Input, OnInit, Output } from "@angular/core";
import { Router } from "@angular/router";
import { CompanyShareInterestConst, ProductPolicyConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { CompanyShareInfoServiceProxy } from "@shared/service-proxies/company-share-manager-service";
import * as moment from "moment";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
	selector: "app-create-or-edit-secondary",
	templateUrl: "./create-or-edit-secondary.component.html",
	styleUrls: ["./create-or-edit-secondary.component.scss"],
})
export class CreateOrEditSecondaryComponent extends CrudComponentBase {
	/* PROP */
	@Input() modalDialog: boolean = false;
	@Input() header: string = '';
	@Input() companyShareSecondary: any = {};
	@Input() listPrimary = [];
	@Input() allowEdit = false;

	/* EVENT */
	@Output() onSave = new EventEmitter<any>();
	@Output() onClose = new EventEmitter<any>();
	@Output() onCreatePolicyDetail = new EventEmitter<any>();
	@Output() onEditPolicy = new EventEmitter<any>();
	@Output() onDelete = new EventEmitter<any>();
	@Output() onCreatePolicy = new EventEmitter<void>();
	@Output() onEditPolicyDetail = new EventEmitter<any>();
	
	/* CONST */
	CompanyShareInterestConst = CompanyShareInterestConst;
	ProductPolicyConst = ProductPolicyConst;

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _companyShareInfoService: CompanyShareInfoServiceProxy,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private router: Router,
		private confirmationService: ConfirmationService,
	) {
		super(injector, messageService);
	}

	submitted = false;
	/** EXPAND COLLAPSE */
	expandedRows = {};
	isExpanded: boolean = false;

	minDate = null; // type new Date()
	maxDate = null;	// type new Date()

	selectedPrimary: any = {};
	listBanks: any[] = [];

	ngOnInit(): void {}

	hideDialog() {
		this.onClose.emit();
	}

	save() {
		this.onSave.emit(this.companyShareSecondary);
	}

	/**
	 * EMIT { policy, policyIndex }
	 * @param policy 
	 * @param policyIndex 
	 */
	createPolicyDetail(policy,policyIndex) {
		this.onCreatePolicyDetail.emit({ policy, policyIndex });
	}

	editPolicy(policy) {
		this.onEditPolicy.emit(policy);
	}

	/**
	 * EMIT { item, type }
	 * @param item 
	 * @param type policy | policyDetail
	 */
	delete(item, type) {
		this.onDelete.emit({ item, type });
	}

	createPolicy() {
		this.onCreatePolicy.emit();
	}

	editPolicyDetail(policyDetail) {
		this.onEditPolicyDetail.emit(policyDetail);
	}

	onChangeOpenCellDate($event) {
		if(+new Date($event) > +new Date(this.companyShareSecondary.closeCellDate)) {
			this.companyShareSecondary.closeCellDate = null;
		}
	}

	onChangePrimary($event) {
		const primaryId = $event.value;

		this.selectedPrimary = this.findPrimary(primaryId);

		if(this.selectedPrimary) {
			this.listBanks = this.selectedPrimary?.tradingProvider?.businessCustomer?.businessCustomerBanks;
			if(this.listBanks?.length) {
				this.listBanks = this.listBanks.map(bank => {
					bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
					return bank;
				});
			}
			this.minDate = this.selectedPrimary.openCellDate ? new Date(this.selectedPrimary.openCellDate) : null;
			this.maxDate = this.selectedPrimary.closeCellDate ? new Date(this.selectedPrimary.closeCellDate) : null;
		}
		console.log({ selectedPrimary: this.selectedPrimary, minDate: this.minDate, maxDate: this.maxDate });
	}

	findPrimary(id) {
		const found = this.listPrimary.find(p => p.companySharePrimaryId === id);
		return found || {};
	}

	/**
	 * Mở hết
	 */
	expandAll() {
		if (!this.isExpanded) {
			this.companyShareSecondary.policies.forEach((product) => (this.expandedRows[product.fakeId] = true));
		} else {
			this.expandedRows = {};
		}
		this.isExpanded = !this.isExpanded;
	}

	validForm(): boolean {
		const validRequired = this.companyShareSecondary.openCellDate
							&& this.companyShareSecondary.closeCellDate
							&& this.companyShareSecondary.businessCustomerBankAccId;
		return validRequired;
	}
}
