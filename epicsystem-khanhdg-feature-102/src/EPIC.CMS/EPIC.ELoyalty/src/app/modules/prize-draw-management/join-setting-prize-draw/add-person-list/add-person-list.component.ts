import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { GenderCost, PrizeDrawManagement } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { IndividualCustomerService } from '@shared/services/individual-customer-service';
import { LuckyProgramInvestorService } from '@shared/services/lucky-program-investor.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
	selector: 'app-add-person-list',
	templateUrl: './add-person-list.component.html',
	styleUrls: ['./add-person-list.component.scss'],
	})
export class AddPersonListComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private configDialog: DynamicDialogConfig,
		private _luckyProgramInvestorService: LuckyProgramInvestorService,
		public ref: DynamicDialogRef,
		private _individualCustomerService: IndividualCustomerService,
		private changeDetectorRef: ChangeDetectorRef,
	) {
		super(injector, messageService);
	}

	rows: any[] = [];

	public filter = {
		keyword: "",
		customerType: null,
		rankId: null,
		sex: null,
	};

	selectedItems = [];
	prizeDrawId: number;

	ranks: IDropdown[] = [];

	PrizeDrawManagement = PrizeDrawManagement;
	GenderCost = GenderCost;
	
	ngOnInit(): void {
		this.prizeDrawId = this.configDialog.data?.prizeDrawId;
		this.setPage();
	}

	getGenderName = (code) => {
		return GenderCost.getGenderName(code);
	}

	ngAfterViewInit() {
		this._individualCustomerService.getListClass().subscribe((res: any) => {
		  if (this.handleResponseInterceptor(res, '')) {
			if (res.data.items) {
			  this.ranks = res.data.items.map(
				(item: any) =>
				  ({
					value: item.id,
					label: item.name,
				  } as IDropdown)
			  );
			}
		  }
		});
	
		this.changeDetectorRef.detectChanges();
		this.changeDetectorRef.markForCheck();
	  }
	setPage(pageInfo?:any){
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._luckyProgramInvestorService.getAllInvestor(this.prizeDrawId, this.page, this.filter).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
			}
		}, () => {
			this.isLoading = false;
		})
	}

	close(event){}

	save(event){
		if(event){
			let investorIds = this.selectedItems.map(item => item.investorId)
			let body = {
				luckyProgramId: this.prizeDrawId,
				investorIds: investorIds
			}
			this._luckyProgramInvestorService.create(body).subscribe((res) => {
				if(this.handleResponseInterceptor(res)) {
					this.ref.close(true);
				}
			})
		}

		
	}
}
