import { Component, OnInit, ViewChild } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Observable, forkJoin, of } from 'rxjs';
import { FileService } from '@shared/services/file-service';
import { FileUpload } from 'primeng/fileupload';
import { HelpersService } from '@shared/services/helpers.service';
import { ContentTypeEView, EAcceptFile } from '@shared/consts/base.const';
import { IResponseItem } from '@shared/interface/respose.interface';


@Component({
  selector: 'e-upload',
  templateUrl: './e-upload.component.html',
  styleUrls: ['./e-upload.component.scss']
})
export class EUploadComponent implements OnInit {

    constructor(
        public ref: DynamicDialogRef, 
        public config: DynamicDialogConfig,
        private _fileService: FileService,
        private _helpersService: HelpersService,
    ) { 
        
    }

    isUploading: boolean = false;
    dialogData: any = {};
    
    ngOnInit(): void {
        let data: any = this.config.data;
        this.dialogData  = {
            ...data,
            folderUpload: data.folderUpload,
            uploadServer: !(data.uploadServer === false),
            multiple: !(data.multiple === false),
            accept: data.accept || EAcceptFile.ALL,
            previewBeforeUpload: !(data.previewBeforeUpload === false),
        };
        //
        if(!this.dialogData.folderUpload) {
            this._helpersService.messageError('','Dev chưa thêm Tên thư mục upload!', 60000);
        }

        // ẨN MODAL KHI CHỌN FILE
        const elements: any = document.querySelectorAll('.p-dialog-mask-scrollblocker');
        elements[elements.length - 1].style.opacity = 0;
    }

    @ViewChild('pUpload') pUpload: FileUpload;
    
    fileInput: HTMLElement;
    ngAfterViewInit() {
        this.pUpload.choose();
        // ẨN DIALOG_MODAL KHI CLICK CANCEL KHÔNG CHỌN FILE
        const elementPUpload = document.getElementsByClassName("e-file-upload");
        this.fileInput = elementPUpload[0].getElementsByTagName("input")[0];
        this.fileInput.addEventListener("cancel", this.hideDialogListen);
    }

    hideDialogListen = () => {
        this.hideDialog()
    }
    
    hideDialog(): any {
        this.ref.close();
    }

    onSelectedFiles(event?: any) {
        // console.log('event', event);
        this.files = event.currentFiles;
        if(this.dialogData.previewBeforeUpload === false) {
            this.config.data.isLoading = true;
            this.onUpload();
            
        } else {
            // HIỆN MODEL SAU KHI CHỌN XONG FILE
            const elements: any = document.querySelectorAll('.p-dialog-mask-scrollblocker');
            elements[elements.length - 1].style.opacity = 1;
            this.fileInput.removeEventListener("cancel", this.hideDialogListen);
        }
    }

    files: File[]= [];
    preview(image) {
        this._helpersService.dialogViewerRef(
            image,
            ContentTypeEView.IMAGE,
        );
    }

    removeFile(index: number) {
        this.files.splice(index, 1);
    }

    onUpload() {
        if(this.dialogData.uploadServer) {
            let uploadFilesProcess: Observable<IResponseItem<string>>[] = [];
            //
            let quantity: number = +this.dialogData.quantity;
            quantity = quantity > 0 ? quantity : undefined;  
            //
            this.files.forEach((file, index) => {
                if((quantity && index < quantity) || !quantity) {
                    uploadFilesProcess.push(this._fileService.uploadFile(file, this.dialogData.folderUpload));
                }
            });
            //
            this.isUploading = true;
            if(this.dialogData?.callback) {
                // Gọi lại component gốc để tương tác
                this.dialogData.callback();
            }//
            if(uploadFilesProcess?.length && this.dialogData.folderUpload) {
                    forkJoin(uploadFilesProcess).subscribe(results => {
                        let fileUploads: string[] = [];
                        this.isUploading = false;
                        if(results && results?.length) {
                            fileUploads = results.map(file => file?.data)
                        } 
                        this.ref.close(fileUploads);
                    }, (err) => {
                        this.isUploading = false;
                    }
                );
            }
        } else {
            this.ref.close(this.files);
        }
    }

}
