import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { AppConsts, MediaNewsConst, MediaConst, SearchConst, FormNotificationConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { debounceTime } from "rxjs/operators";
import { decode } from 'html-entities';
import { BroadcastService } from '@shared/services/broadcast.service';
import { AddMediaComponent } from './add-media/add-media.component';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';
@Component({
  selector: 'app-media',
  templateUrl: './media.component.html',
  styleUrls: ['./media.component.scss']
})
export class MediaComponent extends CrudComponentBase {

  page = new Page()
  rows: any[] = [];
  MediaConst = MediaConst;
  MediaNewsConst = MediaNewsConst;
  showAddNewModel: Boolean
  addNewModelSubmitted: Boolean;
  newsMedia: any;
  uploadedFiles: any[] = []
  baseImgUrl: String;
  baseUrl: string;
  position: any;
  type: any;
  listAction:any[] = [];
  rowExcute:any[] = [];
  listFormatMedia: any[] = [];
  formatVideo: boolean;
  statusSearch = [
    {
      name: "Tất cả",
      code: ''
    },
    ...MediaNewsConst.statusList];
  positionSearch = [
    {
      name: "Tất cả",
      code: ''

    },
    ...MediaNewsConst.positionList];
  typeSearch = [
    {
      name: "Tất cả",
      code: ''
    },
    ...MediaNewsConst.typeList
  ];

  constructor(
    private broadcastService: BroadcastService,
    injector: Injector,
    messageService: MessageService,
    public dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    public _dialogService: DialogService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.showAddNewModel = false;
    this.addNewModelSubmitted = false;
  }

  ngOnInit(): void {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Danh sách hình ảnh' }
    ]);
  }

  genListAction(data = []) {
    this.listAction = data.map((item) => {
      const status = item?.approve?.status;

      const actions = [];

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyHinhAnh_CapNhat]) && item?.status != MediaConst.ACTIVE ) {
        actions.push({
          data: item,
          label: "Chỉnh sửa",
          icon: "pi pi-user-edit",
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyHinhAnh_Xoa])) {
        actions.push({
          data: item,
          label: "Xoá",
          icon: "pi pi-trash",
          command: ($event) => {
            this.remove($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyHinhAnh_PheDuyetOrHuyDang]) && item?.status == MediaConst.ACTIVE) {
        actions.push({
          data: item,
          label: "Bỏ duyệt đăng",
          icon: "pi pi-check-circle",
          command: ($event) => {
            this.approve($event.item.data);
          },
        });
      }
      
      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyHinhAnh_PheDuyetOrHuyDang]) && (item?.status == MediaConst.TRINH_DUYET || item?.status == MediaConst.NHAP)) {
        actions.push({
          data: item,
          label: "Duyệt đăng",
          icon: "pi pi-check-circle",
          command: ($event) => {
            this.approve($event.item.data);
          },
        });
      }
          
      return actions;
    });
  }

  changeStatus() {
    this.setPage({ page: this.offset });
  }

  changeType() {
    this.setPage({ page: this.offset });
  }

  changePosition() {
    this.setPage({ page: this.offset });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.page.keyword = this.keyword;
    this.isLoading = true;
    // this.page.pageSize = 20;
    this.broadcastService.getAllMedia(this.page, this.status, this.type, this.position).subscribe((res) => {
      this.page.totalItems = res.totalResults;
      this.rows = res.results.map(this.detectVideo);
      this.genListAction(this.rows);
      this.isLoading = false;
    }, (err) => {
      this.isLoading = false;
      console.log('Error', err);
      
    });
  }

  setLengthStringInScreen(ratio) {
    return (this.screenWidth/ratio).toFixed();
  }

  detectVideo(row) {
    row.content = decode(row.content);
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

  create() {
    const ref = this.dialogService.open(AddMediaComponent, {
      data: {
        inputData: null
      },
      header: 'Thêm mới hình ảnh',
      width: '100%',
      style: {'max-height': '100%', 'border-radius': '0px'},
      contentStyle: { "overflow": "auto", "margin-bottom": "60px" },
      baseZIndex: 10000,
      footer: ""
    }).onClose.subscribe(result => {
      if(result) {
        this.messageSuccess('Thêm mới thành công');
        this.offset = 0;
        this.setPage({ page: this.offset });
      }
    })
  }
  //
  edit(row) {
    const ref = this.dialogService.open(AddMediaComponent, {
      data: {
        inputData: row,
        unDisableStatus: true
      },
      header: 'Chỉnh sửa hình ảnh',
      width: '100%',
      style: {'max-height': '100%', 'border-radius': '0px'},
      contentStyle: { "overflow": "auto", "margin-bottom": "60px" },
      baseZIndex: 10000,
      footer: ""
    }).onClose.subscribe((result) => {
      if(result) {
        this.messageSuccess('Cập nhật thành công');
        this.offset = 0;
        this.setPage({ page: this.offset });
      }
    })
  }

  remove(row) {
    this._helpersService.dialogConfirmRef(
      "Xác nhận xóa hình ảnh?",
      IconConfirm.DELETE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        row.status = "DELETED";
        this.broadcastService.saveMedia(row).subscribe( res => {
          this.messageSuccess('Xóa thành công!');
          this.setPage({ page: 0 })
        })
      }
    })
  }

  approve(row) {
    var message = "Xác nhận duyệt đăng hình ảnh?";
    if (row.status == 'ACTIVE') {
      message = "Xác nhận bỏ duyệt đăng hình ảnh?"
    }
    //
    this._helpersService.dialogConfirmRef(
      "Xác nhận xóa tin tức?",
      IconConfirm.DELETE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        row.status = row.status == 'ACTIVE' ? 'PENDING' : 'ACTIVE';
        this.broadcastService.saveMedia(row).subscribe(res => {
          this.messageSuccess("Thao tác thành công!");
          this.setPage({ page: 0 })
        })
      }
    })
  }
}
