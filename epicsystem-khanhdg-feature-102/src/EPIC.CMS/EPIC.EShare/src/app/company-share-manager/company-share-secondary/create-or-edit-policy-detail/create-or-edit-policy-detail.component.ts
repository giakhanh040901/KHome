import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { CompanyShareInterestConst, CompanySharePolicyDetailTemplateConst, ProductPolicyConst } from "@shared/AppConsts";
import { MessageService } from "primeng/api";

@Component({
	selector: "app-create-or-edit-policy-detail",
	templateUrl: "./create-or-edit-policy-detail.component.html",
	styleUrls: ["./create-or-edit-policy-detail.component.scss"],
})
export class CreateOrEditPolicyDetailComponent implements OnInit {
	/* PROP */
	@Input() modalDialog: boolean = false;
	@Input() header: string = '';
	@Input() policy: any = {};
	@Input() policyDetail: any = {};

	@Input() search = {
		policy: {
			companySharePolicyDetailTemp: [],
		},
		detail: {},
		listPolicy: [],
		listDetails: [],
	};
	@Input() allowEdit = false;
	@Input() onPolicyDetailTable = false;

	/* EVENT */
	@Output() saveAddPolicyDetail = new EventEmitter<any>();
	@Output() closeModalDialog = new EventEmitter<any>();
	
	/* CONST */
	CompanyShareInterestConst = CompanyShareInterestConst;
	ProductPolicyConst = ProductPolicyConst;
	CompanySharePolicyDetailTemplateConst = CompanySharePolicyDetailTemplateConst;


	constructor(
		private messageService: MessageService, 
	) {

	}


	blockText: RegExp = /[0-9,.]/;
	submitted = false;

	ngOnInit(): void {}

	onChangeIntestType($event) {
		const { value } = $event;
		if (CompanyShareInterestConst.isDinhKy(value)) {
			this.policyDetail.interestPeriodType = CompanyShareInterestConst.interestPeriodTypes[0].code;
		} else {
			this.policyDetail.interestPeriodQuantity = null;
			this.policyDetail.interestPeriodType = null;
		}
	}

	hideDialog() {
		this.closeModalDialog.emit();
	}

	save() {
		this.saveAddPolicyDetail.emit(this.policyDetail);
	}

	// XU LY FILTER POLICY & POLICY DETAIL
	selectPolicy($event) {
		const { value } = $event;
		this.search.listDetails = [...value.companySharePolicyDetailTemp];

		Object.keys(this.policy).forEach((key) => {
			if (key in value) {
				this.policy[key] = value[key];
			}
		});
	}

	selectPolicyDetail($event) {
		const { value } = $event;

		Object.keys(this.policyDetail).forEach((key) => {
			if (key in value) {
				this.policyDetail[key] = value[key];
			}
		});
		console.log('policyDetail', this.policyDetail);
		
	}

    validForm(): boolean {
        const validRequired = this.policyDetail?.periodQuantity 
							&& this.policyDetail?.stt 
							&& this.policyDetail?.periodType 
							&& this.policyDetail?.shortName
							&& this.policyDetail?.name 
							&& this.policyDetail?.profit
							&& this.policyDetail?.interestType 
							&& this.policyDetail?.status
							&& ((this.policyDetail?.interestType == CompanySharePolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIODIC && this.policyDetail?.interestPeriodQuantity && this.policyDetail?.interestPeriodType) || this.policyDetail?.interestType == CompanySharePolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END);
        return validRequired;
    }

}
