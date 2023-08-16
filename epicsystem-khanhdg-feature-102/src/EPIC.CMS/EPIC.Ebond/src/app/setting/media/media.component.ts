import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts, FormNotificationConst, MediaConst, MediaNewsConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MediaService } from '@shared/service-proxies/setting-service';
import { SimpleModalService } from 'ngx-simple-modal';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { UploadImageComponent } from 'src/app/components/upload-image/upload-image.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AddMediaComponent } from './add-media/add-media.component';
import {decode} from 'html-entities'
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
@Component({
  selector: 'app-media',
  templateUrl: './media.component.html',
  styleUrls: ['./media.component.scss']
})
export class MediaComponent extends CrudComponentBase implements OnInit {

  page = new Page()
  rows: any[] = [];
  MediaConst = MediaConst;
  MediaNewsConst = MediaNewsConst;
  showAddNewModel: Boolean
  addNewModelSubmitted: Boolean;
  newsMedia: any;
  uploadedFiles: any[] = []
  listAction: any[] = [];
  baseImgUrl: String;
  baseUrl: string;
  position:any;
  type:any;
  statusSearch = [
    {
      name:"Tất cả",
      code: ''
    },
    ...MediaNewsConst.statusList];
  positionSearch = [
    {
      name:"Tất cả",
      code: ''

    },
    ...MediaNewsConst.positionList];
  typeSearch = [
    {
      name:"Tất cả",
      code:''
    },
    ...MediaNewsConst.typeList
  ];
  
  constructor(
    private broadcastService: MediaService,
    injector: Injector,
    messageService: MessageService,
    public dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private confirmationService: ConfirmationService,
    private _dialogService: DialogService,
  ) {
    super(injector, messageService);
    this.showAddNewModel = false;
    this.addNewModelSubmitted = false;
  }

  ngOnInit(): void {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl; 
    this.setPage({ page: this.offset });
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink:['/home'] },
      { label: 'Danh sách hình ảnh hiển thị'}
    ]);
  }

  genListAction(data = []) {
    this.listAction = data.map(item => {

      const actions = [
      ];

      if (item.status != this.MediaConst.status.DELETED && this.isGranted([this.PermissionBondConst.BondCaiDat_HinhAnh_Sua])) {
        actions.push({
          data: item,
          label: 'Sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          }
        });
      }

      if (item.status == this.MediaConst.status.DELETED && this.isGranted()) {
        actions.push({
          data: item,
          label: 'Xem chi tiết',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.detailDelete($event.item.data);
          }
        });
      }

      if (item.status != this.MediaConst.status.DELETED && this.isGranted([this.PermissionBondConst.BondCaiDat_HinhAnh_Xoa])) {
        actions.push({
          data: item,
          label: 'Xoá',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.remove($event.item.data);
          }
        });
      }

      if ((item.status == this.MediaConst.status.PENDING || item.status == this.MediaConst.status.DRAFT) && this.isGranted([this.PermissionBondConst.BondCaiDat_HinhAnh_DuyetDang])) {
        actions.push({
          data: item,
          label: 'Duyệt đăng',
          icon: 'pi pi-check',
          command: ($event) => {
            this.approve($event.item.data);
          }
        });
      }

      if (item.status == this.MediaConst.status.ACTIVE && this.isGranted([this.PermissionBondConst.BondCaiDat_HinhAnh_DuyetDang])) {
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
    console.log('listActions', this.listAction);

  }

  changeStatus(){
    this.setPage({ page: this.offset });
  }

  changeType(){
    this.setPage({ page: this.offset });
  }

  changePosition(){
    this.setPage({ page: this.offset });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    // this.page.pageSize = 20;
    this.broadcastService.getAllMedia(this.page, this.status, 'bond',this.position).subscribe((res) => { 
        this.isLoading = false;
        this.page.totalItems = res.totalResults;
        this.rows = res.results.map(this.detectVideo);
        this.genListAction(this.rows);
       
    }, (err) => {
        this.isLoading = false;
        console.log('Error', err);
        
    });
  }

  setLengthStringForScreen(ratio) {
    return (this.screenWidth/ratio).toFixed();
  }

  create() {
    const ref = this.dialogService.open(AddMediaComponent, {
      data: {
        inputData: null
      },
      header: 'Thêm mới hình ảnh',
      width: '800px',
      footer: ""
    }).onClose.subscribe(result => {
      this.offset = 0;
      this.setPage({ page: this.offset });
    })
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

  edit(row) {
    console.log("row",row);
    
    const ref = this.dialogService.open(AddMediaComponent, {
      data: {
        inputData: row
      },
      header: 'Chỉnh sửa hình ảnh',
      width: '800px',
      footer: ""
    }).onClose.subscribe(() => {
      this.offset = 0;
      this.setPage({ page: this.offset });
    })
  }

  detailDelete(row) {
    console.log("row",row);
    
    const ref = this.dialogService.open(AddMediaComponent, {
      data: {
        inputData: row
      },
      header: 'Xem hình ảnh',
      width: '800px',
      footer: ""
    }).onClose.subscribe(() => {
      this.offset = 0;
      this.setPage({ page: this.offset });
    })
  }

  // remove(row) {
  //   this.confirmationService.confirm({
	// 		message: 'Bạn có chắc chắn xóa hình ảnh này?',
	// 		header: 'Xóa hình ảnh',
	// 		icon: 'pi pi-exclamation-triangle',
  //     acceptLabel: 'Đồng ý',
	// 	  rejectLabel: 'Hủy',
	// 		accept: () => {
  //       row.status = "DELETED";
	// 			this.broadcastService.saveMedia(row).subscribe((response) => {
	// 				if (row.status == "DELETED") {
  //           this.messageService.add({
  //             severity: 'success',
  //             summary: '',
  //             detail: `Xoá hình ảnh thành công!`,
  //             life: 3000,
  //           });
	// 					this.setPage();
	// 				}
	// 			},() => {
  //         this.messageService.add({
  //           severity: 'error',
  //           summary: '',
  //           detail: `Xoá hình ảnh thất bại!`,
  //           life: 3000,
  //         });
  //       }
  //       );
	// 		},
	// 		reject: () => {

	// 		},
	// 	});
  // }

  remove(row) {
		const ref = this._dialogService.open(
			FormNotificationComponent,
			{
				header: "Thông báo",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title: "Bạn có chắc chắn xóa hình ảnh?",
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
      if (dataCallBack?.accept) {
        row.status = "DELETED";
              this.broadcastService.saveMedia(row).subscribe((response) => {
              if (row.status == "DELETED") {
                this.messageService.add({
                  severity: 'success',
                  summary: '',
                  detail: `Xoá hình ảnh thành công!`,
                  life: 3000,
                });
                this.setPage();
              }
            },() => {
              this.messageService.add({
                severity: 'error',
                summary: '',
                detail: `Xoá hình ảnh thất bại!`,
                life: 3000,
              });
            }
            );
				  }
			  });
  }

  // approve(row) {
  //   var messageMedia = "Bạn có muốn duyệt đăng hình ảnh?";
  //   var headerMedia = "Đăng hình ảnh"
  //   if(row.status == 'ACTIVE') {
  //     messageMedia = "Bạn có muốn bỏ duyệt đăng hình ảnh?";
  //     headerMedia = "Bỏ đăng hình ảnh"
  //   }
  //   this.confirmationService.confirm({
	// 		message: messageMedia,
	// 		header: headerMedia,
	// 		icon: 'pi pi-exclamation-triangle',
  //     acceptLabel: 'Đồng ý',
	// 	  rejectLabel: 'Hủy',
	// 		accept: () => {
  //       row.status = row.status == 'ACTIVE' ? 'PENDING': 'ACTIVE';
	// 			this.broadcastService.saveMedia(row).subscribe(res => {
	// 				if (row.status != "DELETED") {
  //           this.messageService.add({
  //             severity: 'success',
  //             summary: row.status == 'PENDING' ? "Bỏ đăng thành công" : "Đăng hình ảnh thành công",
  //             detail: '',
  //             life: 3000,
  //           });
	// 					this.setPage();
	// 				}
	// 			},() => {
  //         this.messageService.add({
  //           severity: 'error',
  //           summary: "Đăng hình ảnh thất bại",
  //           detail: "Vui lòng thử lại",
  //           life: 3000,
  //         });
  //       }
  //       );
	// 		},
	// 		reject: () => {

	// 		},
	// 	});
  // }

  approve(row) {
    var messageMedia = "Bạn có muốn duyệt đăng hình ảnh?";
    var headerMedia = "Đăng hình ảnh"
    if(row.status == 'ACTIVE') {
      messageMedia = "Bạn có muốn bỏ duyệt đăng hình ảnh?";
      headerMedia = "Bỏ đăng hình ảnh"
    }
		const ref = this._dialogService.open(
			FormNotificationComponent,
			{
				header: "Thông báo",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title: messageMedia,
					icon: FormNotificationConst.IMAGE_APPROVE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
      if (dataCallBack?.accept) {
        row.status = row.status == 'ACTIVE' ? 'PENDING': 'ACTIVE';
				this.broadcastService.saveMedia(row).subscribe(res => {
              if (row.status != "DELETED") {
                this.messageService.add({
                  severity: 'success',
                  summary: row.status == 'PENDING' ? "Bỏ đăng thành công" : "Đăng hình ảnh thành công",
                  detail: '',
                  life: 3000,
                });
                this.setPage();
              }
            },() => {
              this.messageService.add({
                severity: 'error',
                summary: "Đăng hình ảnh thất bại",
                detail: "Vui lòng thử lại",
                life: 3000,
              });
            }
            );
				  }
			  });
  }
}
