import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AppConsts, FormNotificationConst, GeneralDescriptionConst, MessageErrorConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { GeneralDescriptionService } from "@shared/services/general-description.service";
import { AppUtilsService } from "@shared/services/utils.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { concat } from "rxjs";
import { UploadImageComponent } from "src/app/components-general/upload-image/upload-image.component";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { CreateFeatureComponent } from "../create-feature/create-feature.component";
import { CreateProductImageComponent } from "../create-product-image/create-product-image.component";

@Component({
  selector: "app-general-description-detail",
  templateUrl: "./general-description-detail.component.html",
  styleUrls: ["./general-description-detail.component.scss"],
})
export class GeneralDescriptionDetailComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		private fb: FormBuilder,
		private router: Router,
		private _dialogService: DialogService,
		private routeActive: ActivatedRoute,
		private _utilsService: AppUtilsService,
		protected messageService: MessageService,
		private confirmationService: ConfirmationService,
		private _generalDescriptionService: GeneralDescriptionService

	) {
		super(injector, messageService);
		// this.distributionId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}

	inputData: any;
	postForm: FormGroup;
	baseUrl: string;
	caretPos: number = 0;

	listActionImage:any[] = [];
	listActionFeature:any[] = [];


	htmlMarkdownOptions: any = [
		{
		value: "MARKDOWN",
		name: "MARKDOWN",
		},
		{
		value: "HTML",
		name: "HTML",
		},
	];

	GeneralDescriptionConst = GeneralDescriptionConst;

	// EDIT
	isEdit: boolean = false;
	fieldErrors: any = [];
	modalDialog: boolean;
	modalDialogOrgs: boolean;
	submitted: boolean;

	image = {
		id:  0,
		order: 0,
		position: '',
		path: '',
		status: '',
	}

	feature = {
		// id: 0,
		order: 0,
		iconUri: '',
		description: '',
		status: 'DRAFT',
	}

	descriptionContent = {
		description: "",
		descriptionContentType: null,
		status: 'ACTIVE'
	}

	generalDescription: any = {
		//
		images: [],
		//
		descriptionContent: {
			description: "",
			descriptionContentType: null
		},
		//
	  	features: [],
	  	status: "",
	}

	ngOnInit(): void {
		this.getDetail();

		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.descriptionContent.descriptionContentType = this.generalDescription?.descriptionContent?.descriptionContentType ?? "HTML";
		this.descriptionContent.description = this.generalDescription?.descriptionContent?.description ?? "";
		
	}

    // LABEL EDIT BUTTON
	labelButtonEdit() {
		return this.isEdit ? 'Lưu lại' : 'Chỉnh sửa';
	}

	setStatusEdit() {
		this.isEdit = !this.isEdit;
	}

	getCaretPos(oField) {
        if (oField.selectionStart || oField.selectionStart == '0') {
          this.caretPos = oField.selectionStart;
          console.log("this.caretPos",this.caretPos);
        }
    }

	insertImage() {
        const ref = this._dialogService.open(UploadImageComponent, {
          data: {
            inputData: [],
            showOrder: false
          },
          header: 'Chèn hình ảnh',
          width: '600px',
          footer: ""
        });
        ref.onClose.subscribe(images => {
          let imagesUrl = "";
		  if(images){
			images.forEach(image => {
				imagesUrl += `![](${this.baseUrl}/${image.data}) \n`;
			  })
		  }
          let oldContentValue = this.descriptionContent.description;
          let newContentValue = oldContentValue.slice(0, this.caretPos) + imagesUrl + oldContentValue.slice(this.caretPos); 
          this.descriptionContent.description = newContentValue;
    
        })
    }

	changeEdit() {
        this.setStatusEdit();
        this.generalDescription.descriptionContent.descriptionContentType = this.descriptionContent.descriptionContentType;
        this.generalDescription.descriptionContent.description = this.descriptionContent.description;

		if (!this.isEdit) {
			// IMAGE
			for (let i = 0; i<this.generalDescription.images.length; i++){
				if (this.generalDescription.images[i]._id){
					delete this.generalDescription.images[i]._id;
				}
			}

			// FEATURE
			for (let i = 0; i<this.generalDescription.features.length; i++){
				if (this.generalDescription.features[i]._id){
					delete this.generalDescription.features[i]._id;
				}
			}
			//
			let requestCompleteCount = 0, requestComplete = 3;
			concat(
				this._generalDescriptionService.updateImage({data: this.generalDescription.images}), // request 1
				this._generalDescriptionService.updateDescription(this.descriptionContent), // request 2
				this._generalDescriptionService.updateFeature({data: this.generalDescription.features}), // request 3
			).subscribe((res) => {
				requestCompleteCount++;
				if (requestCompleteCount == requestComplete) {
					this.messageSuccess('Cập nhật thành công!');
				} 
			}, (err) => {
				this.setStatusEdit();
				let responseErr = JSON.parse(err?.response);
				this.messageError(responseErr?.message);
			});
		} 
    }

	genlistActionImageProduct(data = []) {
		this.listActionImage = data.map((image, index) => {
		  const actions = [];

		  if (this.isGranted([this.PermissionGarnerConst.GarnerMTC_ThongTinMTC_Xem])) {
			actions.push({
			  data: image,
			  label: 'Xem chi tiết',
			  icon: 'pi pi-eye',
			  command: ($event) => {
				console.log("$event.item.data", $event.item.data);
				this.viewImage($event.item.data);
			  }
			})
		  }
	
		  if (this.isGranted([this.PermissionGarnerConst.GarnerMTC_ThongTinMTC_Sua])) {
			actions.push({
			  data: image,
			  label: 'Chỉnh sửa',
			  icon: 'pi pi-pencil',
			  command: ($event) => {
				console.log("$event.item.data", $event.item.data);
				this.editImage($event.item.data);
			  }
			})
		  }

		  if (this.isGranted([this.PermissionGarnerConst.GarnerMTC_ThongTinMTC_Sua])) {
			actions.push({
			  data: image,
			  index: index,
			  label: 'Xoá tệp tin',
			  icon: 'pi pi-trash',
			  command: ($event) => {
				this.deleteImage($event.item.data, $event.item.index);
			  }
			})
		  }

		  return actions;
		});
		console.log('listActionsImage', this.listActionImage);
	}

	genlistActionFeature(data = []) {
		this.listActionFeature = data.map((feature, index) => {
		  const actions = [];

		  if (this.isGranted([this.PermissionGarnerConst.GarnerMTC_ThongTinMTC_Xem])) {
			actions.push({
			  data: feature,
			  label: 'Xem chi tiết',
			  icon: 'pi pi-eye',
			  command: ($event) => {
				console.log("$event.item.data", $event.item.data);
				this.viewFeature($event.item.data);
			  }
			})
		  }
	
		  if (this.isGranted([this.PermissionGarnerConst.GarnerMTC_ThongTinMTC_Sua])) {
			actions.push({
			  data: feature,
			  label: 'Chỉnh sửa',
			  icon: 'pi pi-pencil',
			  command: ($event) => {
				console.log("$event.item.data", $event.item.data);
				this.editFeature($event.item.data);
			  }
			})
		  }

		  if ( this.isGranted([this.PermissionGarnerConst.GarnerMTC_ThongTinMTC_Xoa])) {
			actions.push({
			  data: feature,
			  index: index,
			  label: 'Xoá tệp tin',
			  icon: 'pi pi-trash',
			  command: ($event) => {
				this.deleteFeature($event.item.data, $event.item.index);
			  }
			})
		  }
	

		  return actions;
		});
		console.log('listActionsFeature', this.listActionImage);
	}



	createImage() {
		const ref = this._dialogService.open(
			CreateProductImageComponent,
			{
			  header: 'Thêm mới hình ảnh sản phẩm',
			  width: '700px',
			  contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			  baseZIndex: 10000,
			});
			//
			ref.onClose.subscribe((image) => {
				if(image) {
					if (!image?.id) {
						for (let i = 0; i<this.generalDescription.images.length; i++){
							this.generalDescription.images[i].order = i+1;
						}
						this.generalDescription?.images.push({order: this.generalDescription?.images.length + 1, ...image});
						this.genlistActionImageProduct(this.generalDescription?.images);
					}
				}
			});

	}

	createFeatures() {
		const ref = this._dialogService.open(
		CreateFeatureComponent,
		{
			header: 'Thêm mới tiêu chí nổi bật',
			width: '700px',
			contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			baseZIndex: 10000,
		});
		//
		ref.onClose.subscribe((feature) => {
			if(feature) {
				if (!feature?.id) {
					for (let i = 0; i<this.generalDescription.features.length; i++){
						this.generalDescription.features[i].order = i+1;
					}
					
					this.generalDescription?.features.push({order: this.generalDescription?.features.length + 1, ...feature});
					this.genlistActionFeature(this.generalDescription?.features);
				}
			}
		});
	}

	viewImage(image) {
		const ref = this._dialogService.open(
			CreateProductImageComponent,
			{
			  header: 'Xem chi tiết hình ảnh sản phẩm',
			  width: '700px',
			  contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			  baseZIndex: 10000,
			  data: {
				image: image,
				isView: true
			  },
			}
		  );
		  //
		  ref.onClose.subscribe((image) => {

		  });
	}

	viewFeature(feature) {
		const ref = this._dialogService.open(
			CreateFeatureComponent,
			{
			  header: 'Xem chi tiết tiêu chí nổi bật',
			  width: '700px',
			  contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			  baseZIndex: 10000,
			  data: {
				feature: feature,
				isView: true
			  },
			}
		  );
		  //
		  ref.onClose.subscribe((feature) => {

		  });
	}

	editImage(image) {
		const ref = this._dialogService.open(
		CreateProductImageComponent,
		  {
			header: 'Sửa hình ảnh sản phẩm',
			width: '700px',
			contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			baseZIndex: 10000,
			data: {
				image: image
			},
		  }
		);
		//
		ref.onClose.subscribe((image) => {
		  //
		});
	}

	editFeature(feature) {
		const ref = this._dialogService.open(
			CreateFeatureComponent,
		  {
			header: 'Sửa tiêu chí nổi bật',
			width: '700px',
			contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			baseZIndex: 10000,
			data: {
				feature: feature
			},
		  }
		);
		//
		ref.onClose.subscribe((feature) => {
		  //
		});
	}

	deleteImage(image, index) {
		this.confirmationService.confirm({
		  message: `Bạn có chắc chắn muốn xóa hình ảnh sản phẩm ?`,
		  header: 'Xóa hình ảnh sản phẩm',
		  icon: 'pi pi-info-circle',
		  acceptLabel: 'Đồng ý',
		  rejectLabel: 'Hủy bỏ',
		  accept: () => {
			// image.status = GeneralDescriptionConst.XOA;
			this.generalDescription.images.splice(index, 1);
			for (let i = 0; i<this.generalDescription.images.length; i++){
				if (this.generalDescription.images[i]._id){
					delete this.generalDescription.images[i]._id;
				}
				// if (this.generalDescription.images[i].id){
				// 	delete this.generalDescription.images[i].id;
				// }
			}
			this._generalDescriptionService.updateImage({data: this.generalDescription.images}).subscribe((response) => {
				if (response.status = 'OK') {
					this.messageSuccess('Thành công!');
				}
			});

		  }
		});
	}

	deleteFeature(feature, index) {
		this.confirmationService.confirm({
		  message: `Bạn có chắc chắn muốn xóa tiêu chí nổi bật ?`,
		  header: 'Xóa hình tiêu chí nổi bật',
		  icon: 'pi pi-info-circle',
		  acceptLabel: 'Đồng ý',
		  rejectLabel: 'Hủy bỏ',
		  accept: () => {
			this.generalDescription.features.splice(index, 1);
			
			// feature.status = GeneralDescriptionConst.XOA;
			for (let i = 0; i<this.generalDescription.features.length; i++){
				if (this.generalDescription.features[i]._id){
					delete this.generalDescription.features[i]._id;
				}
			}
			this._generalDescriptionService.updateFeature({data: this.generalDescription.features}).subscribe((response) => {
				this.messageSuccess('Xóa tiêu chí thành công');
				this.getDetail()
			},err => {
				this.messageError('Xóa tiêu chí thất bại');
			  });
		  }
		});
	}
	//
	getDetail() {
		this.isLoading = true;
		this._generalDescriptionService.getGeneralDescriptionById().subscribe((res) => {
			this.isLoading = false;
			this.generalDescription.images = res.images;
			this.generalDescription.features = res.features;
			this.descriptionContent.descriptionContentType = res.descriptionContentType;
			this.descriptionContent.description = res.description;

			this.genlistActionFeature(this.generalDescription?.features);
			this.genlistActionImageProduct(this.generalDescription?.images);
		});
	}

	onRowReorderFeature(event) {
		this.generalDescription.features.map((item, index) => {
			item.order = index + 1;
		})	
	}
	
	onRowReorderImage(event) {
		this.generalDescription.images.map((item, index) => {
			item.order = index + 1;
		})
	}

	requestFeature(row) {
		if (row.status == GeneralDescriptionConst.NHAP){
			row.status = GeneralDescriptionConst.TRINH_DUYET;
		
			for (let i = 0; i<this.generalDescription.features.length; i++){
				if (this.generalDescription.features[i]._id){
					delete this.generalDescription.features[i]._id;
				}
			}
			this._generalDescriptionService.updateFeature({data: this.generalDescription.features}).subscribe((response) => {
				this.messageSuccess('Trình duyệt tiêu chí thành công');
				this.getDetail()
			},err => {
				this.messageError('Đăng tiêu chí thất bại');
			  });
		}
	}

	requestImage(row) {
		if (row.status == GeneralDescriptionConst.NHAP){
			row.status = GeneralDescriptionConst.TRINH_DUYET;
		
			for (let i = 0; i<this.generalDescription.images.length; i++){
				if (this.generalDescription.images[i]._id){
					delete this.generalDescription.images[i]._id;
				}
			}
			//
			this._generalDescriptionService.updateImage({data: this.generalDescription.images}).subscribe((response) => {
				this.messageSuccess('Trình duyệt hình ảnh thành công');
				this.getDetail()
			},(err) => {
				this.messageError('Trình duyệt hình ảnh thất bại');
			});
		}
	}

	approveFeature(row) {
		var message = "Bạn có muốn duyệt đăng tiêu chí?";
		if (row.status == 'ACTIVE') {
		  message = "Bạn có muốn bỏ duyệt đăng tiêu chí?"
		}
		//
		const ref = this._dialogService.open(
		  FormNotificationComponent,
		  {
			header: "Thông báo",
			width: '600px',
			contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
			styleClass: 'p-dialog-custom',
			baseZIndex: 10000,
			data: {
			  title: message,
			  icon: FormNotificationConst.IMAGE_APPROVE,
			},
		  }
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
		  console.log({ dataCallBack });
		  if (dataCallBack?.accept) {
			row.status = row.status == 'ACTIVE' ? 'PENDING' : 'ACTIVE';

			for (let i = 0; i<this.generalDescription.features.length; i++){
				if (this.generalDescription.features[i]._id){
					delete this.generalDescription.features[i]._id;
				}
			}
			this._generalDescriptionService.updateFeature({data: this.generalDescription.features}).subscribe((response) => {
				this.messageSuccess(row.status == 'PENDING' ? "Bỏ đăng tiêu chí thành công" : "Đăng tiêu chí thành công");
				this.getDetail()
			},err => {
				this.messageError('Đăng tiêu chí thất bại');
			  });
		  }
		});
	  }

	  approveImage(row) {
		var message = "Bạn có muốn duyệt đăng hình ảnh?";
		if (row.status == 'ACTIVE') {
		  message = "Bạn có muốn bỏ duyệt đăng hình ảnh?"
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
			  title: message,
			  icon: FormNotificationConst.IMAGE_APPROVE,
			},
		  }
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
		  console.log({ dataCallBack });
		  if (dataCallBack?.accept) {
			row.status = row.status == 'ACTIVE' ? 'PENDING' : 'ACTIVE';

			for (let i = 0; i<this.generalDescription.images.length; i++){
				if (this.generalDescription.images[i]._id){
					delete this.generalDescription.images[i]._id;
				}
			}
			this._generalDescriptionService.updateImage({data: this.generalDescription.images}).subscribe((response) => {
				this.messageSuccess(row.status == 'PENDING' ? "Bỏ đăng hình ảnh thành công" : "Đăng hình ảnh thành công");
				this.getDetail()
			},err => {
				this.messageError('Đăng hình ảnh thất bại');
			  });
		  }
		});
	}
}
