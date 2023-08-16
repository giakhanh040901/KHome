import { ChangeDetectorRef, Component, ElementRef, Inject, Injector, Input, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { AppConsts, FormNotificationConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { MessageService } from "primeng/api";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { ProjectServiceProxy } from "@shared/services/project-manager-service";
import { DialogService } from "primeng/dynamicdialog";
import { AddProjectImageComponent } from "./add-project-image/add-project-image.component";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";

@Component({
    selector: 'app-project-image',
    templateUrl: './project-image.component.html',
    styleUrls: ['./project-image.component.scss']
  })
export class ProjectImageComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _guaranteeAssetService: ProjectServiceProxy,
        private dialogService: DialogService,
        private routeActive: ActivatedRoute,
        private ref: ChangeDetectorRef,
    ) {
        super(injector, messageService);
        this.projectId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
    }

    projectId: string;

    collateral = {
        'projectId': 0,
        'projectImages': []
    }
    
    images: any[] = [];

    AppConsts = AppConsts;
    FormNotificationConst = FormNotificationConst;

    @Input() contentHeight: number;
    scrollHeight: number = 0;

    ngOnInit() {
        this.setPage();
    }

    @ViewChild('pageHeader') pageHeader: ElementRef<HTMLElement>
    ngAfterViewInit() {
        this.scrollHeight = this.contentHeight - this.pageHeader.nativeElement.offsetHeight ;
        this.ref.detectChanges();
    }

    removeFile(fileToDelete) {
        const ref = this.dialogService.open(
			FormNotificationComponent,
			{
				header: "Xóa hình ảnh đầu tư",
				width: '600px',
				data: {
					title : "Bạn có chắc chắn xóa hình ảnh này không?",
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._guaranteeAssetService.deleteProjectImage(fileToDelete?.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, "Xóa thành công")) {
                        this.setPage();
                    }
                });
            }
        });
    }

    selectImg() {
        const ref = this.dialogService.open(AddProjectImageComponent, {
            header: 'Thêm hình ảnh',
            width: '500px',
            data: {
                projectId: this.projectId,
            },
        });
        ref.onClose.subscribe(response => {
            if(response) {
                this.setPage();
            } 
        });
    }

    create() {
        this.collateral.projectId = +this.projectId;
        this.submitted = false;
    }

    setPage() {
        this.isLoading = true;
        this._guaranteeAssetService.getAllProjectImage(this.projectId).subscribe((res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.images = res.data;
                }
            },() => {
                this.isLoading = false;
            }
        );
        this.isLoading = false;
    }
}
