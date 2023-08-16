import { Component, ElementRef, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ConfirmationService, MenuItem, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { forkJoin } from "rxjs";
import { Table } from "primeng/table";
import { Page } from "@shared/model/page";
import { CallCenterConfigService } from "@shared/services/call-center-config.service";
import { CallConfigUpdateComponent } from "./call-config-update/call-config-update.component";

@Component({
	selector: 'app-call-center-config',
	templateUrl: './call-center-config.component.html',
	styleUrls: ['./call-center-config.component.scss']
})
export class CallCenterConfigComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _dialogService: DialogService,
		private _breadcrumbService: BreadcrumbService,
		private _confirmationService: ConfirmationService,
		private _callCenterConfigService: CallCenterConfigService,
	) {
		super(injector, messageService);
	}

	rows: any[] = [];
	rowsMerged: any[] = [];
	fieldFilters = {
	}
	page = new Page();

	ngOnInit(): void {
		this.setPage();
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage();
			} else {
				this.setPage();
			}
		});
	}

	showData(rows) {
		for (let row of rows) {

		};
	}

	update() {
		const ref = this._dialogService.open(
			CallConfigUpdateComponent,
			{
				header: 'Cập nhật cấu hình',
				width: '60%',
				height: '85%',
				style: { 'max-height': '100%', 'border-radius': '10px', "overflow": "auto" },
				contentStyle: { "overflow": "auto" },
				data: {
					rows: this.rowsMerged
				}
			}
		);

		ref.onClose.subscribe((statusResponse) => {
			if (statusResponse) {
				this.setPage();
			}
		});
	}

	setPage(event?: any) {
		if (event) {
			this.page.pageNumber = event.page;
			this.page.pageSize = event.rows;
		}

		this.page.keyword = this.keyword;
		this.isLoading = true;
		let rowsSort = [];
		let rowsList = [];

		forkJoin([
			this._callCenterConfigService.getAll(this.page),
			this._callCenterConfigService.getAllUserForConfig()
		]).subscribe(([res, resUser]) => {
			this.isLoading = false;

			if (this.handleResponseInterceptor(res, '')) {
				this.rows = res.data?.items.sort((a, b) => {
					if (a.sortOrder === null) {
					  return 1; 
					}
					if (b.sortOrder === null) {
					  return -1; 
					}
					return a.sortOrder - b.sortOrder; 
				});;
		
				rowsSort = res.data?.items
					.map(o => ({
						userId: o.userId,
						sortOrder: o.sortOrder,
						fullName: o.user?.fullname,
					}))

				this.page.totalItems = res.data?.totalItems;
			}

			if (this.handleResponseInterceptor(resUser, '')) {
				rowsList = resUser.data?.items
					.map(o => ({
						userId: o.userId,
						sortOrder: o.sortOrder ?? null,
						fullName: o.userInfo?.displayName,
					}));
			}

			this.rowsMerged = rowsList.map(itemA => {
				const itemB = rowsSort.find(itemB => itemB.userId === itemA.userId);
				return itemB ? { sortOrder: itemB.sortOrder, userId: itemB.userId, fullName: itemB.fullName } : { sortOrder: null, userId: itemA.userId, fullName: itemA.fullName };
			});
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

	onRowEditSave() {
		console.log("+_+");
		this.submitted = true;
		let body = {
		  details :  this.rows.filter(item => item.sortOrder !== null)
		}
		  this._callCenterConfigService.update(body).subscribe((response) => {
			  if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
				this.setPage();
			  } else {
				this.submitted = false;
			  }
			}, (err) => {
			  console.log('err----', err);
			  this.submitted = false;
			}
		  );
	}


}
