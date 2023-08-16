import { Component, Injector, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DistributionConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-distribution-history',
  templateUrl: './distribution-history.component.html',
  styleUrls: ['./distribution-history.component.scss']
})
export class DistributionHistoryComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
		messageService: MessageService,
		private routeActive: ActivatedRoute,
		private _distributionService: DistributionService,
  ) { 
		super(injector, messageService);
  }

  DistributionConst = DistributionConst;
  @Input() distributionId;
	page = new Page();
	rows: any[] = [];

  ngOnInit(): void {
    this.setPage();
  }

  setPage(pageInfo?:any) {
		this.isLoading = true;
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this._distributionService.getHistory(this.page,this.distributionId).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res?.data?.items
				console.log({ coupon: res });
			}
		}, () => {
			this.isLoading = false;
		});
	}
}
