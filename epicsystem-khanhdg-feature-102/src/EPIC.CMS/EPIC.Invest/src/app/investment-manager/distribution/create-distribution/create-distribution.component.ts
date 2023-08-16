import { ProjectFilterComponent } from './project-filter/project-filter.component';
import { Component, Injector, OnInit } from "@angular/core";
import { DistributionConst, ProductBondInterestConst, ProductPolicyConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { DynamicDialogRef, DialogService } from "primeng/dynamicdialog";
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
		private dialogService: DialogService,
    	public ref: DynamicDialogRef,
	) {
		super(injector, messageService);
	}

  	/* CONST */
	ProductBondInterestConst = ProductBondInterestConst;
	ProductPolicyConst = ProductPolicyConst;
	DistributionConst = DistributionConst;
	
	distribution: any = { ...BASE.DISTRIBUTION };

	policyTemp: any = {};

  	fieldErrors: any = {};
  
	listProject: any[] = [];
	listBanks: any[] = [];
	projectInfo: any = {};

	submitted = false;

	minDate = null; // type new Date()
	maxDate = null;	// type new Date()

	selectedProject: any = {};

	page = new Page();
	modalDialog: boolean;
  	fieldDates = ["openCellDate", "closeCellDate"];

	projects: any[] = [];

	ngOnInit(): void {
		this._distributionService.getBankList().subscribe((res) => {
			if(this.handleResponseInterceptor(res, '')) {
				if(res?.data?.length) {
					this.listBanks = res.data.map(bank => {
						bank.labelName = bank?.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
						return bank;
					})
				}
			}
		}, () => {
			this.messageError('Không lấy được danh sách ngân hàng');
		});

		this.filterProject();
	}

	filterProject() {
		const ref = this.dialogService.open(ProjectFilterComponent,
			{
				header: 'Tìm kiếm dự án',
				width: '1000px',
				contentStyle: { "height": "auto", "overflow": "hidden", "padding": "0px" },
				style: { "top": "-20%" },
				styleClass:'p-dialog-custom',
			});

		ref.onClose.subscribe((project) => {
			if(project) {
				this.showProjectInfo(project);
			}
		});
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	showProjectInfo(project) {
		this.projectInfo = {...project};
		this.distribution.projectId = this.projectInfo?.id;
	}

	save() {
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
	}

	close() {
		this.ref.close();
	}

	onChangeOpenCellDate($event) {
		if(+new Date($event) > +new Date(this.distribution.closeCellDate)) {
			this.distribution.closeCellDate = null;
		}
	}

	onChangeProject($event) {
		const primaryId = $event.value;
		this.selectedProject = this.findPrimary(primaryId);
		if(this.selectedProject) {
			this.listBanks = this.selectedProject?.tradingProvider?.businessCustomer?.businessCustomerBanks;
			if(this.listBanks?.length) {
				this.listBanks = this.listBanks.map(bank => {
					bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
					return bank;
				});
			}
			this.minDate = this.selectedProject.openCellDate ? new Date(this.selectedProject.openCellDate) : null;
			this.maxDate = this.selectedProject.closeCellDate ? new Date(this.selectedProject.closeCellDate) : null;
		}
	}

	findPrimary(id) {
		const found = this.listProject.find(p => p.bondPrimaryId === id);
		return found || {};
	}

	validForm(): boolean {
		const validRequired = this.distribution.projectId
                        && this.distribution.openCellDate
                        && this.distribution.closeCellDate
                        && this.distribution.tradingBankAcc
                        && this.distribution.methodInterest;
		return validRequired;
	}
}
