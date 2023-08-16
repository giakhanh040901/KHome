import { Component, EventEmitter, Injector, Input, OnInit, Output } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { CompanyShareInterestConst, CompanySharePolicyDetailTemplateConst, ProductPolicyConst } from "@shared/AppConsts";
import { OJBECT_SECONDARY_CONST } from "@shared/base-object";
import { CrudComponentBase } from "@shared/crud-component-base";
import { CompanyShareSecondaryServiceProxy } from "@shared/service-proxies/company-share-manager-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { last } from "rxjs/operators";

const { POLICY, POLICY_DETAIL, BASE } = OJBECT_SECONDARY_CONST;
@Component({
	selector: "app-create-or-edit-policy",
	templateUrl: "./create-or-edit-policy.component.html",
	styleUrls: ["./create-or-edit-policy.component.scss"],
})
export class CreateOrEditPolicyComponent extends CrudComponentBase {
	/* PROP */
	@Input() modalDialog: boolean = false;
	@Input() header: string = '';
	@Input() policy: any = {...BASE.POLICY};
	@Input() policyInit:any = { ...BASE.POLICY };
	@Input() isLoading: boolean;
	@Input() listPolicyDetailAction:any[] = [];
	@Input() search = {
		policy: {
			companySharePolicyDetailTemp: [],
		},
		detail: {},
		listPolicy: [],
		listDetails: [],
	};
	@Input() onPolicyDetailTable:boolean = false;
	@Input() createClick: boolean ;
	@Input() updateClick: boolean = false ;
	@Input() allowEdit = false;
	@Input() policyDetails:any[] = [];
	/* EVENT */
	@Output() createPolicyDetailChild = new EventEmitter<any>();
	@Output() onSavePolicy = new EventEmitter<any>();
	@Output() editByOnSave = new EventEmitter<any>();
	@Output() onClose = new EventEmitter<any>();
	
	/* CONST */
	listAction:any[] = [];

	CompanyShareInterestConst = CompanyShareInterestConst;
	ProductPolicyConst = ProductPolicyConst;
	CompanySharePolicyDetailTemplateConst = CompanySharePolicyDetailTemplateConst;

	constructor(
		injector: Injector,
		private _secondaryService: CompanyShareSecondaryServiceProxy,
		messageService: MessageService,
		private confirmationService: ConfirmationService
	) {	
		super(injector, messageService)
	}

	submitted = false;
	blockText: RegExp = /[0-9,.]/;

	ngOnInit(): void {
		const policy: any = {};
		if(!this.policy.companySharePolicyId) this.policy = {...BASE.POLICY};
		this.search.policy = policy;
	}

	hideDialog() {
		this.onClose.emit();

		this.policyInit = {};
		this.updateClick = false;
		this.createClick = true;
	}
	
	onChangeStartDate($event) {
		if(+new Date($event) > +new Date(this.policy.endDate)) {
			this.policy.endDate = null;
		}
	}

	delete(policy){

	}

	createPolicyDetail() {
		this.createPolicyDetailChild.emit(this.policy.companySharePolicyId);
		console.log({policyId: this.policy.companySharePolicyId});
	}

	savePolicy() {
		let policyData = {...this.search.policy, ...this.policy}
		console.log('policyData: ', policyData);
		
		this.onSavePolicy.emit(policyData);
	}

	editByOnClickSave() {
		this.editByOnSave.emit(this.policy);
	}

	// XU LY FILTER POLICY
	selectPolicy($event) {
		console.log({ policyChange : $event });
		
		const { value } = $event;
		this.search.listDetails = [...value.companySharePolicyDetailTemp];
		console.log("policyTempDetail: ", this.search.listDetails);
		
		this.policy = {...value};
		this.policy.companySharePolicyDetailTemp = [];
	
		Object.keys(this.policy).forEach((key) => {
			if (key in value) {
				this.policy[key] = value[key];
			}
		});
	}

}
