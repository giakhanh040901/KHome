import { Component, Inject, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ExportReportConst, PermissionBondConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ExportReportService } from '@shared/services/export-report.service';
import { PermissionsService } from '@shared/services/permissions.service';
import { AppUtilsService } from '@shared/services/utils.service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-business-report',
  templateUrl: './business-report.component.html',
  styleUrls: ['./business-report.component.scss']
})
export class BusinessReportComponent extends AppComponentBase {

  permissionName: PermissionBondConst;
  private permissions: PermissionsService;
  PermissionBondConst = PermissionBondConst;

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
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Báo cáo kinh doanh' },
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
  }
  status: any= null;
  isLoadingPage = false;
  permission: any;

  statusSearch: any[] = [
    // {
    //   name: "BÁO CÁO TỔNG HỢP CÁC GÓI TRÁI PHIẾU",
    //   code: "1",
    // },
    // {
    //   name: "BÁO CÁO TỔNG HỢP CÁC GÓI ĐẦU TƯ",
    //   code: "2",
    // },
    // {
    //   name: "BÁO CÁO TÍNH GỐC LÃI ĐẾN HẠN",
    //   code: '3'
    // }
  ];
  ExportReportConst = ExportReportConst;

  ngOnInit(): void {
    this.isLoadingPage = true;
    this._userService.getAllPermission().subscribe(res => {
      this.isLoadingPage = false;
      this.permission = res.data;
      this.statusSearch = [];

      if (this.isGranted([this.PermissionBondConst.Bond_BaoCao_QuanTri_THCGTraiPhieu])) {
        this.statusSearch.push(
          {
            name: "BÁO CÁO TỔNG HỢP CÁC GÓI TRÁI PHIẾU",
            code: this.ExportReportConst.BC_THCG_TRAI_PHIEU,
          },
        )
      }

      if (this.isGranted([this.PermissionBondConst.Bond_BaoCao_QuanTri_THCGDauTu])) {
        this.statusSearch.push(
          {
            name: "BÁO CÁO TỔNG HỢP CÁC GÓI ĐẦU TƯ",
            code: this.ExportReportConst.BC_THCG_DAU_TU,
          },
        )
      }

      if (this.isGranted([this.PermissionBondConst.Bond_BaoCao_QuanTri_TGLDenHan])) {
        this.statusSearch.push(
          {
            name: "BÁO CÁO TÍNH GỐC LÃI ĐẾN HẠN",
            code: this.ExportReportConst.BC_TGL_DEN_HAN,
          }
        )
      }
    }, () => {

    });
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

    this.dataApprove.startDate = this.formatDateTimeView(this.dataApprove.startDate);
    this.dataApprove.endDate = this.formatDateTimeView(this.dataApprove.endDate);
    if (this.status == this.ExportReportConst.BC_THCG_TRAI_PHIEU) {
      this.isSend=true;
      this._exportReportService
        .getExportInvest(this.dataApprove.startDate, this.dataApprove.endDate)
        .subscribe((res) => {
          this.isSend=false;
        });
    } else if (this.status == this.ExportReportConst.BC_THCG_DAU_TU) {
      this._exportReportService
        .getExportInvestment(this.dataApprove.startDate, this.dataApprove.endDate)
        .subscribe((res) => {
          this.isSend = false;
        });
    } else if(this.status == this.ExportReportConst.BC_TGL_DEN_HAN) {
      this._exportReportService.getExportBondDue( this.dataApprove.startDate, this.dataApprove.endDate).subscribe((res) => {
        this.isSend = false;
      });
    }
    else if (this.status == null) {
      this.messageError("Vui lòng chọn loại hình thông báo");
    }
  }

  validForm(): boolean {
    let validRequired;
    validRequired = this.dataApprove?.startDate && this.dataApprove?.endDate;

    return validRequired;
  }

}
