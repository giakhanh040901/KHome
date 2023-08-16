import { CrudComponentBase } from '@shared/crud-component-base';
import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { IDropdown } from '@shared/AppConsts';
import { ProjectOverviewService } from '@shared/services/project-overview.service';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';

@Component({
    selector: 'app-form-approve',
    templateUrl: './form-approve.component.html',
    styleUrls: ['./form-approve.component.scss']
})
export class FormApproveComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        public ref: DynamicDialogRef,
        private projectOverviewService: ProjectOverviewService,
        public changeDetectorRef: ChangeDetectorRef,
        public configDialog: DynamicDialogConfig
    ) {
        super(injector, messageService);
    }

    title: string;
    submitted = false;

    showApproveBy: boolean = false;
    acceptStatus: boolean = true;
    check_approve: boolean;
    dataApprove = {
        id: 0,
        userApproveId: 1,
        approveNote: null,
        cancelNote: null,
    }

    type: string;
    dataInfo: any = {};
    public listOwner: IDropdown[] = [];

    ngOnInit(): void {
        this.dataApprove.id = this.configDialog.data.id;
        this.check_approve = true;

        // Thông tin hiển thị theo type
        this.type = this.configDialog.data?.type;
        this.dataInfo = this.configDialog.data?.data;
    }

    ngAfterViewInit() {
        this.projectOverviewService._listOwner$.subscribe((res: any) => {
          if (res) {
            this.listOwner = res;
          }
        });
        this.changeDetectorRef.detectChanges();
        this.changeDetectorRef.markForCheck();
    }

    hideDialog() {
    }

    accept() {
        this.acceptStatus = true;
        if (!this.check_approve){
            this.dataApprove.cancelNote = this.dataApprove.approveNote;
        }
        this.onAccept();
    }

    cancel() {
        this.acceptStatus = false;
        this.onAccept();
    }

    onAccept() {
        this.ref.close({ data: this.dataApprove, accept: this.acceptStatus, checkApprove: this.check_approve });
    }

    validForm(): boolean {
        let validRequired;
        validRequired = this.dataApprove?.approveNote?.trim();
        return validRequired;
    }

}
