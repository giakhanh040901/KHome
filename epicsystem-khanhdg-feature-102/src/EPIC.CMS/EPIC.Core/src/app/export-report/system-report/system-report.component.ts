import { Component, Inject, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ExportReportConst, PermissionCoreConst } from '@shared/AppConsts';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ExportReportService } from '@shared/services/export-report.service';
import { PermissionsService } from '@shared/services/permissions.service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-system-report',
  templateUrl: './system-report.component.html',
  styleUrls: ['./system-report.component.scss']
})
export class SystemReportComponent extends AppComponentBase {

  permissionName: PermissionCoreConst;
  private permissions: PermissionsService;
  PermissionCoreConst = PermissionCoreConst;

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _exportReportService: ExportReportService,
    private breadcrumbService: BreadcrumbService,
    private _userService: UserServiceProxy,
    @Inject(API_BASE_URL) baseUrl?: string
  ) { 
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: "Trang chủ", routerLink: ["/home"] },
      { label: "Báo cáo quản trị" },
    ]);
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
  }
  private baseUrl: string;
  title: string;
  submitted = false;
  isSend: boolean;

  showApproveBy: boolean = false;
  acceptStatus: boolean = true;
  isInvestorProf: boolean;
  check_approve: boolean;

  isLoadingPage = false;

  dataApprove = {
    id: 0,
    userApproveId: 1,
    approveNote: null,
    startDate: null,
    endDate: null,
  };

  startDate: any;
  endDate: any; 

  status: any = null;
  permission: any;

  statusSearch: any[] = [];

  actions: any;
  ExportReportConst = ExportReportConst;
  ngOnInit(): void {
    this.isLoadingPage = true;
    this._userService.getAllPermission().subscribe(res => {
      this.isLoadingPage = false;
      this.permission = res.data;
      this.statusSearch = [];

      this.addReport(this.PermissionCoreConst.Core_BaoCao_HeThong_DSKhachHangRoot, 
                      "BÁO CÁO THAY ĐỔI THÔNG TIN KHÁCH HÀNG",
                      this.ExportReportConst.BC_DS_KHACH_HANG_ROOT);
      this.addReport(this.PermissionCoreConst.Core_BaoCao_QuanTri_TDTTKhachHangRoot, 
                      "BÁO CÁO THAY ĐỔI THÔNG TIN KHÁCH HÀNG ROOT",
                      this.ExportReportConst.BC_THAY_DOI_TT_KHACH_HANG_ROOT);
    }, () => {

    });
  }

  addReport(role ,name, code) {
    if (this.isGranted([role])) {
      this.statusSearch.push({
        name: name,
        code: code
      });
    }
  }

  isGranted(permissionNames = []): boolean {

    return this.isGrantedRootReport(permissionNames);

  }

  isGrantedRootReport(permissionNames = []): boolean {
    for(let permissionName of permissionNames) {
        if(this.permission?.includes(permissionName)) return true;
    }
    return false;
  }

  formatDateTimeView(value) {
    return (moment(value).isValid() && value) ? moment(value).format('YYYY-MM-DDTHH:mm:ss') : '';
  }

  onAccept() {
    console.log("this.status", this.status);
    this.isSend = true;
    this.dataApprove.startDate = this.formatDateTimeView(this.startDate);
    this.dataApprove.endDate = this.formatDateTimeView(this.endDate);

    const exportFunctions = {
      [this.ExportReportConst.BC_DS_KHACH_HANG_ROOT]: () => this._exportReportService.getExportListCustomerRoot(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_THAY_DOI_TT_KHACH_HANG_ROOT]: () => this._exportReportService.getExportCustomerRootInfoChange(this.dataApprove.startDate, this.dataApprove.endDate),
    };
    
    const exportFunction = exportFunctions[this.status];
    
    if (exportFunction) {
      exportFunction().subscribe(() => {
        this.isSend = false;
      });
    } else {
      this.messageError("Vui lòng chọn loại hình thông báo");
      this.isSend = false;
    }
  }

  validForm(): boolean {
    let validRequired;
    validRequired = this.startDate && this.endDate;

    return validRequired;
  }

}
