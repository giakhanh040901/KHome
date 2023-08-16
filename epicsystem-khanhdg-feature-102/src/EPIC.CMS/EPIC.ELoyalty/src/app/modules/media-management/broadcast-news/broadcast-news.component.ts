import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { AppConsts, MediaNewsConst, MediaConst, FormNotificationConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import {decode} from 'html-entities';
import { BroadcastService } from '@shared/services/broadcast.service';
import { AddNewsComponent } from './add-news/add-news.component';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';

@Component({
  selector: 'app-broadcast-news',
  templateUrl: './broadcast-news.component.html',
  styleUrls: ['./broadcast-news.component.scss']
})
export class BroadcastNewsComponent extends CrudComponentBase implements OnInit {

  page = new Page()
  rows: any[] = [];
  MediaConst = MediaConst;
  showAddNewModel: Boolean
  addNewModelSubmitted: Boolean;
  newsMedia: any;
  uploadedFiles: any[] = []
  baseImgUrl: String;
  baseUrl: string;
  listAction:any [] = []
  statusSearch: any[] = [
    {
      name: "Tất cả",
      code: ''
    },

    ...MediaNewsConst.statusList
  ]
  constructor(
    injector: Injector,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private broadcastService: BroadcastService,
    private dialogService: DialogService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.showAddNewModel = false;
    this.addNewModelSubmitted = false;
  }

  ngOnInit(): void {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    this.setPage({ page: this.offset });
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Danh sách tin tức' }
    ]);

  }

  changeStatus() {
    this.setPage({ page: this.offset })
  }

  setLengthStringInScreen(ratio) {
    return (this.screenWidth/ratio).toFixed();
  }
  
  create() {
    let modal = this.dialogService.open(AddNewsComponent, {
      data: {
        inputData: null
      },
      header: 'Thêm mới tin tức',
      width: '100%',
      style: {'max-height': '100%', 'border-radius': '0px'},
      contentStyle: { "overflow": "auto", "margin-bottom": "60px" },
      baseZIndex: 10000,
      footer: ""
    })
    modal.onClose.subscribe(result => {
      if(result) {
        this.messageSuccess('Thêm thành công');
        this.offset = 0;
        this.setPage({ page: this.offset });
      }
    })
  }

  genListAction(data = []) {
		this.listAction = data.map((item) => {
			const status = item?.approve?.status;

			const actions = [];

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyTinTuc_CapNhat]) && (item?.status == MediaConst.NHAP || item?.status == MediaConst.TRINH_DUYET)) {
        actions.push({
					data: item,
					label: "Chỉnh sửa",
					icon: "pi pi-user-edit",
					command: ($event) => {
						this.edit($event.item.data);
					},
				});
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyTinTuc_Xoa]) && item?.status == MediaConst.NHAP ) {
        actions.push({
					data: item,
					label: "Xoá",
					icon: "pi pi-trash",
					command: ($event) => {
						this.remove($event.item.data);
					},
				});
      }
          
      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyTinTuc_PheDuyetOrHuyDang]) && (item?.status == MediaConst.NHAP || item?.status == MediaConst.TRINH_DUYET)) {
        actions.push({
          data: item,
          label: "Duyệt đăng",
          icon: "pi pi-check-circle",
          command: ($event) => {
            this.approve($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyTinTuc_PheDuyetOrHuyDang]) && item?.status == MediaConst.ACTIVE) {
        actions.push({
          data: item,
          label: "Bỏ duyệt đăng",
          icon: "pi pi-check-circle",
          command: ($event) => {
            this.approve($event.item.data);
          },
        });
      }

			return actions;
		});
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

  setPage(pageInfo?: any) {

    this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.page.keyword = this.keyword;
    this.isLoading = true;
    this.broadcastService.getAll(this.page, this.status).subscribe((res) => {
      this.isLoading = false;
      this.page.totalItems = res.totalResults;
      this.rows = res.results.map(this.detectVideo);
      this.genListAction(this.rows);
      console.log("đầu ra của get all : ", this.rows);
      
      console.log("res", this.page.totalItems);

    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  hideDialog() {
    this.showAddNewModel = false;
    this.addNewModelSubmitted = false;
  }

  getTypes() {
    let keys: any = [];
    for (let key in MediaConst.newsTypes) {
      keys.push({ key: key, value: MediaConst.newsTypes[key] });
    }
    return keys;
  }

  getStatus() {
    let keys: any = [];
    for (let key in MediaConst.mediaStatus) {
      keys.push({ key: key, value: MediaConst.mediaStatus[key] });
    }
    return keys;
  }

  header() {
    return "Thêm mới tin tức"
  }


  save() {
    console.log(this.newsMedia);
  }

  edit(row) {
    let modal = this.dialogService.open(AddNewsComponent, {
      data: {
        inputData: row,
        unDisableStatus: true
      },
      header: 'Sửa tin tức',
      width: '100%',
      style: { 'max-height': '100%', 'border-radius': '0px'},
      contentStyle: { "overflow": "auto", "margin-bottom": "60px" },
      baseZIndex: 10000,
      footer: ""
    })
    modal.onClose.subscribe(result => {
      if(result) {
        this.messageSuccess('Cập nhật thành công');
        this.offset = 0;
        this.setPage({ page: this.offset });
      }
    })
  }

  remove(row) {
    this._helpersService.dialogConfirmRef(
      "Xác nhận xóa tin tức?",
      IconConfirm.DELETE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        row.status = "DELETED";
        this.broadcastService.saveNews(row).subscribe(res => {
          this.messageSuccess('Xóa thành công!');
          this.setPage({ page: 0 })
        })
      }
    });
  }

  approve(row) {
    var message = "Xác nhận duyệt đăng tin tức?";
    if (row.status == 'ACTIVE') {
      message = "Xác nhận bỏ duyệt đăng tin tức?"
    }
    //
    this._helpersService.dialogConfirmRef(
      message,
      IconConfirm.DELETE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        row.status = row.status == 'ACTIVE' ? 'PENDING' : 'ACTIVE';
        this.broadcastService.saveNews(row).subscribe(res => {
          this.messageSuccess("Thao tác thành công!");
          this.setPage({ page: 0 })
        })
      }
    })
  }
}

