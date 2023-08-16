import { ChangeDetectorRef, Component, ElementRef, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { ActiveDeactiveConst, AppConsts, FormNotificationConst, ProjectShareConst, SearchConst, TableConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ProjectShareService } from "@shared/services/project-share.service";
import { MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { ProjectShareDetailComponent } from "./project-share-detail/project-share-detail.component";
import { IAction, IColumn } from "@shared/interface/p-table.model";
import { BasicFilter } from "@shared/interface/filter.model";

@Component({
	selector: "app-project-share",
	templateUrl: "./project-share.component.html",
	styleUrls: ["./project-share.component.scss"],
})
export class ProjectShareComponent extends CrudComponentBase {
	constructor(
		injector: Injector, 
		messageService: MessageService,
		private _dialogService: DialogService,
		private _projectShareService: ProjectShareService,
		private ref: ChangeDetectorRef,
	) {
		super(injector, messageService);
	}

	ActiveDeactiveConst = ActiveDeactiveConst;
	ProjectShareConst = ProjectShareConst;

	dataFilter: BasicFilter = new BasicFilter();

	isLoading: boolean;
	page: Page = new Page();
	listAction: IAction[][] = [];

	rows: any[] = [];
	columns: IColumn[] = [];

	@Input() projectId;
	@Input() contentHeight: number;
	idHeader:string = "project-share";

	ngOnInit(): void {
    	// Xử lý ẩn hiện cột trong bảng
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, class: 'b-border-frozen-left', isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT },
			{ field: 'title', header: 'Tiêu đề', width: 18, isPin: true },
			{ field: 'createdDate', header: 'Ngày đăng', width: 12 },
			{ field: 'createdBy', header: 'Người đăng', width: 12 },
			{ field: 'overviewContent', header: 'Nội dung', width: 26, isResize: true },
			{ field: 'status', header: 'Trạng thái', width: 8, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'b-border-frozen-right' },
			{ field: '', header: '', width: 3, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
		];
		//
		this.setPage();
	}
	
	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this._projectShareService.findAll(this.page, this.projectId, this.dataFilter).subscribe((res) => {
		  	this.isLoading = false;
			if(this.handleResponseInterceptor(res)) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res?.data?.items;
				if(this.rows?.length) { 
					this.setData(this.rows);
					this.genListAction(this.rows);
				}
			}
		}), (err) => {
		  this.isLoading = false;
		  console.log('Error-------', err);
		}
	};

	setData(rows) {
		for (let row of rows) {
		  row.createdDate = row.createdDate ? this.formatDate(row.createdDate) : "";
		  row.statusElement = ProjectShareConst.getInfo(row.status);
		}
	}

	genListAction(data = []) {
		this.listAction = data.map( (item, index) => {
			const actions = [];
			// if(this.isGranted([this.PermissionRealStateConst.RealStatePDYCRV_ChiTietHD])) {
				actions.push({
					data: item,
					label: "Chi tiết",
					icon: "pi pi-info-circle",
					command: ($event) => {
						this.detail($event.item.data);
					},
				});
			// }

			// if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ChinhSach_CapNhat])) {
				actions.push({
			  data: item,
			  index: index,
			  label: "Sửa",
			  icon: "pi pi-pencil",
			  command: ($event) => {
				this.edit($event.item.data);
			  },
			});
		  	// }
		  
		  	// if (item.status == ActiveDeactiveConst.ACTIVE || item.status == ActiveDeactiveConst.DEACTIVE) {
				actions.push({
				data: item,
				index: index,
				label: item.status == ActiveDeactiveConst.ACTIVE ? "Hủy kích hoạt" : 'Kích hoạt',
				icon: item.status == ActiveDeactiveConst.ACTIVE ? "pi pi-lock" : "pi pi-check-circle",
				command: ($event) => {
					this.changeStatus($event.item.data);
				},
				});
		  	// }
		  
		  	// if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ChinhSach_CapNhat])) {
			actions.push({
			  data: item,
			  label: "Xoá",
			  icon: "pi pi-trash",
			  command: ($event) => {
				this.delete($event.item.data);
			  },
			});
		  	// }
		  return actions;
		});
	}

	detail(item) {
		const ref = this._dialogService.open(ProjectShareDetailComponent, {
			header: "Chi tiết chia sẻ dự án",
			width: '1000px',
			data: {
			  projectId: this.projectId,
			  item: item,
			  isEdit: false
			},
		});
			ref.onClose.subscribe((statusResponse) => {
				if(statusResponse) {
				this.setPage();
			}
		});
	}
	edit(item) {
		const ref = this._dialogService.open(ProjectShareDetailComponent, {
			header: "Chỉnh sửa chia sẻ dự án",
			width: '1000px',
			data: {
			  projectId: this.projectId,
			  item: item,
			  isEdit: true
			},
		});
			ref.onClose.subscribe((statusResponse) => {
				if(statusResponse) {
				this.setPage();
			}
		});
	}

	changeStatus(item){
		this._projectShareService.changeStatus(item.id).subscribe((res) => {
			if (this.handleResponseInterceptor(res, item.status == ActiveDeactiveConst.ACTIVE ? "Hủy kích hoạt thành công" : "Kích hoạt thành công")) {
				this.setPage();
			}
		},(err) => {
			console.log("err----", err);
			this.messageError(AppConsts.messageError);
		  })
	}

	delete(item) {
		const ref = this._dialogService.open(FormNotificationComponent, {
			header: "Thông báo",
			width: "600px",
			data: {
				title: `Bạn có chắc chắn muốn xóa chia sẻ dự án?`,
				icon: FormNotificationConst.IMAGE_CLOSE,
			},
		});
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._projectShareService.delete(item.id).subscribe((res) => {
					if ( this.handleResponseInterceptor(res, "Xóa thành công")) {
						this.setPage();
					}
					}, (err) => {
					console.log('err____', err);
					this.messageError(AppConsts.messageError);
				});
			} 
		});
	}

	create(event: any){
		if (event) {
			const ref = this._dialogService.open(ProjectShareDetailComponent, {
				header: "Thêm mới chia sẻ dự án",
				width: '1000px',
				data: {
				  projectId: this.projectId
				},
			});
				ref.onClose.subscribe((statusResponse) => {
					if(statusResponse) {
					this.setPage();
				}
			});
		}
	}	
}
