import { Component, Inject, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ExportReportConst, PermissionInvestConst } from '@shared/AppConsts';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ExportReportService } from '@shared/services/export-report.service';
import { PermissionsService } from '@shared/services/permissions.service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-business-report',
  templateUrl: './business-report.component.html',
  styleUrls: ['./business-report.component.scss']
})
export class BusinessReportComponent extends AppComponentBase {

  // permissionName: PermissionInvestConst;
  private permissions: PermissionsService;
  PermissionInvestConst = PermissionInvestConst;

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _exportReportService: ExportReportService,
    private breadcrumbService: BreadcrumbService,
    private _userService: UserServiceProxy,
    @Inject(API_BASE_URL) baseUrl?: string,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
        { label: "Trang chủ", routerLink: ["/home"] },
        { label: "Báo cáo kinh doanh" },
      ]);
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
   }

   private baseUrl: string;
   title: string;
   submitted = false;
   isSend:boolean;

   showApproveBy: boolean = false;
   acceptStatus: boolean = true;
   isInvestorProf: boolean;
   check_approve: boolean;
 
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
   isLoading: boolean = false;
   permission: any;

   statusSearch: any[] = [
   ];
   ExportReportConst = ExportReportConst;

  ngOnInit(): void {
    this.isLoading = true;
    this._userService.getAllPermission().subscribe(res => {
      this.isLoading = false;
      this.permission = res.data;
      this.statusSearch = [];

      this.addReport(this.PermissionInvestConst.Invest_BaoCao_KinhDoanh_THCKDauTu,
        "BÁO CÁO TỔNG HỢP CÁC KHOẢN ĐẦU TƯ",
        this.ExportReportConst.BC_THCK_DAU_TU);
      this.addReport(this.PermissionInvestConst.Invest_BaoCao_KinhDoanh_DCDenHan,
              "BÁO CÁO DỰ CHI ĐẾN HẠN",
              this.ExportReportConst.BC_DC_DEN_HAN);
      this.addReport(this.PermissionInvestConst.Invest_BaoCao_KinhDoanh_ThucChi, 
              "BÁO CÁO THỰC CHI",
              this.ExportReportConst.BC_THUC_CHI);
      this.addReport(this.PermissionInvestConst.Invest_BaoCao_KinhDoanh_TCTDauTu, 
              "BÁO CÁO TÍNH TỔNG CHI TRẢ ĐẦU TƯ",
              this.ExportReportConst.BC_TTCT_DAU_TU);

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
    for (let permissionName of permissionNames) {
      if (this.permission?.includes(permissionName)) return true;
    }
    return false;
  }

  formatDateTimeView(value) {
    return (moment(value).isValid() && value) ? moment(value).format('YYYY-MM-DDTHH:mm:ss') : '';
  }

  onAccept() {
    console.log("this.status", this.status);
    this.isSend=true;
    this.dataApprove.startDate = this.formatDateTimeView(this.dataApprove.startDate);
    this.dataApprove.endDate = this.formatDateTimeView(this.dataApprove.endDate);
    const exportFunctions = {
      [this.ExportReportConst.BC_THCK_DAU_TU]: () => this._exportReportService.getExportInvestment(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_DC_DEN_HAN]: () => this._exportReportService.getDueExpend(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_THUC_CHI]: () => this._exportReportService.getActualExpend(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_TTCT_DAU_TU]: () => this._exportReportService.getActualTotal(this.dataApprove.startDate, this.dataApprove.endDate),
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
    validRequired =this.startDate && this.endDate;             
    
    return validRequired;
  }

}
