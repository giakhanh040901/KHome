import { ChangeDetectorRef, Component, ElementRef, Inject, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { StatusBondInfoFileConst, AppConsts, AtributionConfirmConst, TableConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ProjectServiceProxy } from "@shared/services/project-manager-service";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { DistributionService } from "@shared/services/distribution.service";
import { AppUtilsService } from "@shared/services/utils.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { Page } from "@shared/model/page";
import { IColumn } from "@shared/interface/p-table.model";

@Component({
    selector: 'app-project-file',
    templateUrl: './project-file.component.html',
    styleUrls: ['./project-file.component.scss']
})

export class ProjectFileComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private _contractTemplateService: DistributionService,
        private _projectManager: ProjectServiceProxy,
        private _utilsService: AppUtilsService,
        private routeActive: ActivatedRoute,
        private ref: ChangeDetectorRef,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.projectId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    AppConsts = AppConsts;

    baseUrl: string;
    projectId: string;
    
    modalDialog: boolean;
    modalDialogPDF: boolean;

    deleteItemDialog: boolean = false;

    StatusBondInfoFileConst = StatusBondInfoFileConst;

    rows: any[] = [];

    contractFile: any = {
        'id': 0,
        'name': null,
        'url': null,
        'projectId': 0,
    };

    submitted: boolean;
    urlfilePDF:string = '';

    columns: IColumn[] = [];

    @Input() contentHeight: number;
    idHeader:string = "project-file";

    ngOnInit() {
        this.setPage();
        this.columns = [
            {field: 'id', header: '#ID', width: 5},
            {field: 'name', header: 'Tên hồ sơ', width: 25, isResize: true},
            {field: 'dowloadFile', header: 'Tải file', width: 6, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-download'},
            {field: 'viewFile', header: 'Xem file', width: 6, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-eye' },
            {field: 'update', header: 'Cập nhật', width: 8, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-pencil' },
            {field: 'delete', header: 'Xóa', width: 5, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-trash' },
        ];
    }

    setPage(event?: Page) {
        if(!event) 
        this.isLoading = true;
        this._projectManager.getAllJuridicalFile(this.page, this.projectId).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.page.totalItems = res.data.totalItems;
                    this.rows = res.data.items;
                    this.setData(this.rows);
                }
            },() => {
                this.isLoading = false;
            }
        );
    }

    setData(rows) {
        this.rows = rows.map(row => {
            row.dowloadFile = (row) => this.downloadFile(row);
            row.viewFile = (row) => this.viewFile(row);
            row.delete = (row) => this.delete(row);
            row.update = (row) => this.update(row);
            return row;
        });
    }

    resetContractTemplateObject() {
        this.contractFile = {
            id: 0,
            name: null,
            url: null,
            projectId: +this.projectId,
        };
    }

    create() {
        this.resetContractTemplateObject();
        this.submitted = false;
        this.modalDialog = true;
    }

    update(file){
        this.submitted = false;
        this.modalDialog = true;
        this.contractFile = { ...file};
    }

    confirmDelete() {
        this.deleteItemDialog = false;
        this._contractTemplateService.delete(this.contractFile.id).subscribe(
            (response) => {
                if (this.handleResponseInterceptor(response, "Xóa thành công")) {
                    this.setPage();
                    this.resetContractTemplateObject();
                }
            },() => {
                this.messageError(`Không xóa được hồ sơ ${this.contractFile.displayName}`);
            }
        );
    }

    

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
        this.modalDialogPDF = false;
    }

    save() {
        this.submitted = true;
        if (this.contractFile.id) {
            this._projectManager.updateJuridicalFile(this.contractFile).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                        this.hideDialog();
                        this.setPage();
                    } 
                    this.submitted = false;
                }, () => {
                    this.submitted = false;
                }
            );
        } else {
            this._projectManager.createJuridicalFile(this.contractFile).subscribe((response) => {
                if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                    this.hideDialog();
                    this.setPage();
                }
                //
                this.submitted = false;
            },() => {
                this.submitted = false;
            });
        }

    }

    validForm(): boolean {
        return this.contractFile?.url?.trim() && this.contractFile?.name?.trim();
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "project").subscribe((response) => {
                    if(this.handleResponseInterceptor(response)) {
                        this.contractFile.url = response.data;
                    }
                },(err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    delete(file) {
        const data = file;
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn xóa hồ sơ này?',
            ...AtributionConfirmConst,
            accept: () => {
                this._projectManager.deleteJuridicalFile(data.id).subscribe((response) => {
                    console.log('----', this.contractFile.id)
                    if (this.handleResponseInterceptor(response, "Xóa thành công")) {
                        this.setPage();
                    }
                });
            },
            reject: () => {

            },
        });
    }

    downloadFile(row) {
        const url = this.baseUrl + "/" + row.url;
        this._utilsService.makeDownload("", url);
    }

    viewFile(row) {
        let fileUrl = row.url;
        this.urlfilePDF = '/' + fileUrl;
        if(!fileUrl){
            this.messageError("Không có file hồ sơ", "")
        }else{
            if(this.utils.isPdfFile(fileUrl)){
                // console.log('file truyen', this.urlfilePDF);
                // this.modalDialogPDF = true;
                this._projectManager.viewFilePDF(this.urlfilePDF + '&download=true').subscribe();
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF", "")
            }
        }

        // console.log('-----', this.AppConsts.redicrectHrefOpenDocs);
        // window.open(url, "_blank");
    }

    header(): string {
        return this.contractFile?.id ? 'Sửa hồ sơ pháp lý' : 'Thêm hồ sơ pháp lý';
      }

}

