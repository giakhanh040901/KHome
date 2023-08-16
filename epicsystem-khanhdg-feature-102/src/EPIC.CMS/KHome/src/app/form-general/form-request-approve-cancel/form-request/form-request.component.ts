import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { AppConsts, ApproveConst, IDropdown } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ProjectOverviewService } from '@shared/services/project-overview.service';
import { MessageService } from 'primeng/api';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {DynamicDialogConfig} from 'primeng/dynamicdialog';

@Component({
  selector: 'app-form-request',
  templateUrl: './form-request.component.html',
  styleUrls: ['./form-request.component.scss']
})
export class FormRequestComponent extends CrudComponentBase {

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
  public listOwner: IDropdown[] = [];

  title: string;
  submitted = false;

  acceptStatus: boolean = true;

  dataRequest = {
    id: 0,
    actionType: ApproveConst.ACTION_ADD,
    userApproveId: 0,
    requestNote: null,
    summary: null,
  }

  type: string;
  dataInfo: any = {};

  // type = distributionProductInfo : Thông tin phân phối sản phẩm

  ngOnInit(): void {
    this.dataRequest.id = this.configDialog.data.id;
    this.dataRequest.summary = this.configDialog.data.summary;
    
    // Thông tin hiển thị theo type
    this.type = this.configDialog.data?.type;
    this.dataInfo = this.configDialog.data?.data;
    //
    if(this.configDialog.data?.actionType) this.dataRequest.actionType = this.configDialog.data?.actionType;
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
    this.onAccept();
  }

  cancel() {
    this.acceptStatus = false;
    this.onAccept();
  }

  onAccept() {
    this.ref.close({ data: this.dataRequest, accept: this.acceptStatus});
  }

}
