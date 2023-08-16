import { ChangeDetectorRef, Component, ElementRef, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts, AtributionConfirmConst, MediaConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AddProjectMediaComponent } from './add-project-media/add-project-media.component';
import { MediaService } from '@shared/services/media.service';
import { IAction, IColumn } from '@shared/interface/p-table.model';
import { MediaFilter } from '@shared/interface/filter.model';

@Component({
  selector: 'app-project-media',
  templateUrl: './project-media.component.html',
  styleUrls: ['./project-media.component.scss']
})
export class ProjectMediaComponent extends CrudComponentBase implements OnInit {

    page = new Page()
    rows: any[] = [];
    status: string;

    MediaConst = MediaConst;

    showAddNewModel: Boolean
    addNewModelSubmitted: Boolean;
    newsMedia: any;
    uploadedFiles: any[] = []
    
    baseImgUrl: String;
    baseUrl: string;
    position: any;
    type: any;
    
    constructor(

        injector: Injector,
        messageService: MessageService,
        private broadcastService: MediaService,
        public dialogService: DialogService,
        private breadcrumbService: BreadcrumbService,
        private confirmationService: ConfirmationService,
        private routeActive: ActivatedRoute,
        private ref: ChangeDetectorRef,
    ) {
        super(injector, messageService);
        this.showAddNewModel = false;
        this.addNewModelSubmitted = false;
        this.projectId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: "Sản phẩm đầu tư", routerLink: ['/invest-manager/project'] },
            { label: "Chi tiết sản phẩm đầu tư" },
        ]);
    }

    projectId: number;
    columns: IColumn[] = [];
    listAction: IAction[][] = [];

    @Input() contentHeight: number = 0;
    idHeader:string = "project-media";

    ngOnInit(): void {
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
        this.setPage();
        this.columns = [
            {field: 'code', header: 'Mã', width: 6, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT},
            {field: 'mainImg', header: 'Ảnh', width: 5, isPin: true, type: TableConst.columnTypes.IMAGE, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left'},
            {field: 'title', header: 'Tiêu đề', width: 25,  },
            {field: 'content', header: 'Nội dung', width: 20, isResize: true },
            {field: 'createdAt', header: 'Ngày tạo', width: 12, },
            {field: 'status', header: 'Trạng thái', width: 10, type: TableConst.columnTypes.STATUS, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT},
            {field: '', header: '', width: 3, type: TableConst.columnTypes.ACTION_DROPDOWN, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT },
        ]
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        let dataFilter: MediaFilter = {
            type: 'invest_product',
            productId: this.projectId,
        }

        this.broadcastService.getAllMedia(this.page, dataFilter).subscribe((res) => {
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
        this.rows = rows.map(row => {
            row.createdAt = this.formatDate(row.createdAt);
            row.statusElement = MediaConst.getStatusInfo(row.status);
            return row;
        });
    }

    genListAction(data = []) {
        this.listAction = data.map(item => {
            const actions = [];
            if (item.status != this.MediaConst.status.DELETED && this.isGranted([this.PermissionInvestConst.InvestSPDT_TTSP_CapNhat])) {
                actions.push({
                    data: item,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                });
            }

            if (item.status == this.MediaConst.status.DELETED && this.isGranted([this.PermissionInvestConst.InvestSPDT_TTSP_DanhSach])) {
                actions.push({
                    data: item,
                    label: 'Xem chi tiết',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.detailDelete($event.item.data);
                    }
                });
            }

            if (item.status != this.MediaConst.status.DELETED && this.isGranted([this.PermissionInvestConst.InvestSPDT_TTSP_Xoa])) {
                actions.push({
                    data: item,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.remove($event.item.data);
                    }
                });
            }

            if ((item.status == this.MediaConst.status.PENDING || item.status == this.MediaConst.status.DRAFT) && this.isGranted([this.PermissionInvestConst.InvestSPDT_TTSP_PheDuyetOrHuy])) {
                actions.push({
                    data: item,
                    label: 'Duyệt đăng',
                    icon: 'pi pi-check',
                    command: ($event) => {
                        this.approve($event.item.data);
                    }
                });
            }

            if (item.status == this.MediaConst.status.ACTIVE && this.isGranted([this.PermissionInvestConst.InvestSPDT_TTSP_PheDuyetOrHuy])) {
                actions.push({
                    data: item,
                    label: 'Bỏ duyệt đăng',
                    icon: 'pi pi-times',
                    command: ($event) => {
                        this.approve($event.item.data);
                    }
                });
            }
            return actions;
        });
    }

    detectVideo(row) {
        // console.log("Đầu ra khi đã decode xong",row.content);
        if (row.mainImg) {
            var isVideo = false;
            const images = ["jpg", "gif", "png"]
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
    
    

    detailDelete(row) {
        this.dialogService.open(AddProjectMediaComponent, {
            header: 'Xem hình ảnh',
            width: '800px',
            data: {
                inputData: row,
                projectId: this.projectId,
            },
        }).onClose.subscribe(() => {
            this.setPage(this.page);
        })
    }

    create() {
        this.dialogService.open(AddProjectMediaComponent, {
            header: 'Thêm mới hình ảnh',
            width: '800px',
            data: {
                projectId: this.projectId
            },
        }).onClose.subscribe(result => {
            this.setPage(this.page);
        })
    }

    edit(row) {
        this.dialogService.open(AddProjectMediaComponent, {
            header: 'Chỉnh sửa hình ảnh',
            width: '800px',
            data: {
                inputData: row,
                projectId: this.projectId,
            },
        }).onClose.subscribe(() => {
            this.setPage(this.page);
        })
    }

    remove(row) {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn xóa hình ảnh này?',
            ...AtributionConfirmConst,
            accept: () => {
                row.status = "DELETED";
                this.broadcastService.saveMedia(row).subscribe((response) => {
                    if (row.status == "DELETED") {
                        this.messageSuccess('Xoá hình ảnh thành công!');
                        this.setPage();
                    }
                }, () => {
                    this.messageSuccess('Xoá hình ảnh thất bại!');
                });
            },
        });
    }

    approve(row) {
        var messageMedia = "Bạn có muốn duyệt đăng hình ảnh?";
        if(row.status == 'ACTIVE') {
            messageMedia = "Bạn có muốn bỏ duyệt đăng hình ảnh?";
        }
        //
        this.confirmationService.confirm({
            message: messageMedia,
            ...AtributionConfirmConst,
            accept: () => {
                row.status = row.status == 'ACTIVE' ? 'PENDING': 'ACTIVE';
                this.broadcastService.saveMedia(row).subscribe(res => {
                    if (row.status != "DELETED") {
                        this.messageSuccess(row.status == 'PENDING' ? "Bỏ đăng thành công" : "Đăng hình ảnh thành công");
                        this.setPage();
                    }
                },() => {
                    this.messageError("Đăng hình ảnh thất bại");
                });
            },
        });
    }
}

