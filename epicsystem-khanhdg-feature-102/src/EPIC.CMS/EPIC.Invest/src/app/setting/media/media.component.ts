import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts, FormNotificationConst, MediaConst, MediaNewsConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AddMediaComponent } from './add-media/add-media.component';
import { decode } from 'html-entities'
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { MediaService } from '@shared/services/media.service';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { BasicFilter, MediaFilter } from '@shared/interface/filter.model';
@Component({  
  selector: 'app-media',
  templateUrl: './media.component.html',
  styleUrls: ['./media.component.scss']
})
export class MediaComponent extends CrudComponentBase implements OnInit {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private broadcastService: MediaService,
        public dialogService: DialogService,
        private breadcrumbService: BreadcrumbService,
        private _dialogService: DialogService,
    ) {
        super(injector, messageService);
    }

    page = new Page()
    rows: any[] = [];
    
    MediaConst = MediaConst;
    MediaNewsConst = MediaNewsConst;

    listAction: any[] = [];
    position: any;

    columns: IColumn[] = [];
    dataTableEmit: DataTableEmit = new DataTableEmit();

    dataFilter: MediaFilter = {
        type: 'estate_invest'
    };

    ngOnInit(): void {
        this.setPage();
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Danh sách hình ảnh hiển thị' }
        ]);

        this.columns = [
            { field: 'code', header: 'Mã', width: 6, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, isSort: true },
            { field: 'mainImg', header: 'Ảnh', width: 6.5, isPin: true, type: TableConst.columnTypes.IMAGE, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', },
            { field: 'title', header: 'Tiêu đề', width: 20, isPin: true, isSort: true, isResize: true },
            { field: 'displayText', header: 'Nội dung', width: 25, isPin: true, isSort: true },
            { field: 'createdBy', header: 'Người tạo', width: 18, isSort: true },
            { field: 'createdAt', header: 'Ngày tạo', width: 12, type: TableConst.columnTypes.DATETIME, isSort: true },
            { field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true,type: TableConst.columnTypes.STATUS, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'justify-content-left b-border-frozen-right' },
            { field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
        ];
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this.broadcastService.getAllMedia(this.page, this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            this.page.totalItems = res.totalResults;
            this.rows = res.results.map(this.detectVideo);
            this.genListAction(this.rows);
            this.setData(this.rows);
        }, (err) => {
            this.isLoading = false;
            console.log('Error', err);
        });
    }

    setData(rows) {
        for (let row of rows) {
            row.statusElement = MediaConst.getStatusInfo(row?.status);
        };
    }

    genListAction(data = []) {
        this.listAction = data.map(item => {
            const actions = [];
            if (item.status != this.MediaConst.status.DELETED && this.isGranted([this.PermissionInvestConst.InvestHinhAnh_Sua])) {
                actions.push({
                    data: item,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                });
            }

            if (item.status == this.MediaConst.status.DELETED && this.isGranted([this.PermissionInvestConst.InvestHinhAnh_ChiTiet])) {
                actions.push({
                    data: item,
                    label: 'Xem chi tiết',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.detailDelete($event.item.data);
                    }
                });
            }

            if ((item.status == this.MediaConst.status.PENDING || item.status == this.MediaConst.status.DRAFT) && this.isGranted([this.PermissionInvestConst.InvestHinhAnh_DuyetDang])) {
                actions.push({
                    data: item,
                    label: 'Duyệt đăng',
                    icon: 'pi pi-check',
                    command: ($event) => {
                        this.approve($event.item.data);
                    }
                });
            }

            if (item.status == this.MediaConst.status.ACTIVE && this.isGranted([this.PermissionInvestConst.InvestHinhAnh_DuyetDang])) {
                actions.push({
                    data: item,
                    label: 'Bỏ duyệt đăng',
                    icon: 'pi pi-times',
                    command: ($event) => {
                        this.approve($event.item.data);
                    }
                });
            }

            if (item.status != this.MediaConst.status.DELETED && this.isGranted([this.PermissionInvestConst.InvestHinhAnh_Xoa])) {
                actions.push({
                    data: item,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.remove($event.item.data);
                    }
                });
            }
            return actions;
        });
    }

    detailDelete(row) {
        this.dialogService.open(AddMediaComponent, {
            header: 'Xem hình ảnh',
            width: '800px',
            data: {
                inputData: row
            },
        }).onClose.subscribe(() => {
            this.setPage();
        })
    }

    onSort(event) {
        this.dataFilter.sortFields = event;
        this.setPage(this.page);
    }

    setLengthStringForScreen(ratio) {
        return (this.screenWidth/ratio).toFixed();
    }

    detectVideo(row) {
        row.content = decode(row.content);
        if (row.mainImg) {
        var isVideo = false;
        let videos = ["mp4", "3gp", "ogg", "mkv"]
        for (var i = 0; i < videos.length; i++) {
            if (row.mainImg.search(videos[i]) > -1) {
            isVideo = true;
            break;
            }
        }
        }
        return { ...row, isVideo }
    }

    create() {
        this.dialogService.open(AddMediaComponent, {
            header: 'Thêm mới hình ảnh',
            width: '1200px',
            data: {
                inputData: null
            },
        }).onClose.subscribe(response => {
            if(response) {
                alert(response);
                this.setPage();
            }
        })
    }

    edit(row) {
        this.dialogService.open(AddMediaComponent, {
            header: 'Chỉnh sửa hình ảnh',
            width: '800px',
            data: {
                inputData: row
            },
        }).onClose.subscribe((response) => {
            if(response) {
                this.setPage();
            }
        })
    }

    remove(row) {
        const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {
                    title: "Bạn có chắc chắn xóa hình ảnh?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                row.status = "DELETED";
                this.broadcastService.saveMedia(row).subscribe((response) => {
                    if (row.status == "DELETED") {
                        this.messageSuccess('Xoá hình ảnh thành công!');
                        this.setPage();
                    }
                },() => {
                    this.messageError('Xoá hình ảnh thất bại!');
                });
            }
        });
    }

    approve(row) {
        let messageMedia = row.status == 'ACTIVE' ? "Bạn có muốn bỏ duyệt đăng hình ảnh?" : "Bạn có muốn duyệt đăng hình ảnh?";
        this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {
                    title: messageMedia,
                    icon: FormNotificationConst.IMAGE_APPROVE,
                },
            }
        ).onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                row.status = row.status == 'ACTIVE' ? 'PENDING': 'ACTIVE';
                this.broadcastService.saveMedia(row).subscribe(res => {
                    if (row.status != "DELETED") {
                        this.messageSuccess(row.status == 'PENDING' ? "Bỏ đăng thành công" : "Đăng hình ảnh thành công");
                        this.setPage();
                    }
                });
            }
        });
    }
}
