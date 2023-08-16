import { Component, Inject, Injector, OnInit } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { ExportReportConst, PermissionGarnerConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { UserServiceProxy } from "@shared/service-proxies/service-proxies";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { ExportReportService } from "@shared/services/export-report.service";
import { PermissionsService } from "@shared/services/permissions.service";
import { AppUtilsService } from "@shared/services/utils.service";
import * as moment from "moment";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
  selector: "app-management-report",
  templateUrl: "./management-report.component.html",
  styleUrls: ["./management-report.component.scss"],
})
export class ManagementReportComponent extends AppComponentBase {
  private permissions: PermissionsService;
  PermissionGarnerConst = PermissionGarnerConst;

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
        { label: "Báo cáo quản trị" },
      ]);
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
  }
  private baseUrl: string;
  title: string;
  submitted = false;
  isSend:boolean;

  showApproveBy: boolean = false;
  acceptStatus: boolean = true;
  isGarnerorProf: boolean;
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
  isLoadingPage = false;
  permission: any;

  statusSearch: any[] = [
    
  ];
  ExportReportConst = ExportReportConst;

  ngOnInit(): void {
    this.isLoadingPage = true;
    this._userService.getAllPermission().subscribe(res => {
      this.isLoadingPage = false;
      this.permission = res.data;
      this.statusSearch = [];

      this.addReport(this.PermissionGarnerConst.Garner_BaoCao_QuanTri_TCTDauTu,
              "BÁO CÁO TỔNG CHI TRẢ ĐẦU TƯ",
              this.ExportReportConst.BC_TCT_DAU_TU);
      this.addReport(this.PermissionGarnerConst.Garner_BaoCao_QuanTri_THCKDauTu,
              "BÁO CÁO TỔNG HỢP CÁC KHOẢN ĐẦU TƯ",
              this.ExportReportConst.BC_TH_KHOAN_DAU_TU);
      this.addReport(this.PermissionGarnerConst.Garner_BaoCao_QuanTri_THSanPhamTichLuy,
              "BÁO CÁO TỔNG HỢP CÁC SẢN PHẨM TÍCH LŨY",
              this.ExportReportConst.BC_SP_TICH_LUY);
      this.addReport(this.PermissionGarnerConst.Garner_BaoCao_QuanTri_QuanTriTH, 
              "BÁO CÁO QUẢN TRỊ TỔNG HỢP",
              this.ExportReportConst.BC_QUAN_TRI_TH);
      this.addReport(this.PermissionGarnerConst.Garner_BaoCao_QuanTri_DauTuBanHo, 
              "BÁO CÁO CÁC KHOẢN ĐẦU TƯ BÁN HỘ",
              this.ExportReportConst.BC_DAU_TU_BAN_HO);

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
    this.dataApprove.startDate = this.formatDateTimeView(this.startDate);
    this.dataApprove.endDate = this.formatDateTimeView(this.endDate);
    this.isSend=true;

    const exportFunctions = {
      [this.ExportReportConst.BC_TCT_DAU_TU]: () => this._exportReportService.getTotalInvestmentPayment(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_TH_KHOAN_DAU_TU]: () => this._exportReportService.getTotalInvestment(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_SP_TICH_LUY]: () => this._exportReportService.getTotalProduct(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_QUAN_TRI_TH]: () => this._exportReportService.getAdministration(this.dataApprove.startDate, this.dataApprove.endDate),
      [this.ExportReportConst.BC_DAU_TU_BAN_HO]: () => this._exportReportService.getSaleInvestment(this.dataApprove.startDate, this.dataApprove.endDate),
     
    };
    
    const exportFunction = exportFunctions[this.status];
    
    if (exportFunction) {
      exportFunction().subscribe((res) => {
        this.isSend = false;
        this.handleResponseInterceptor(res);
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
