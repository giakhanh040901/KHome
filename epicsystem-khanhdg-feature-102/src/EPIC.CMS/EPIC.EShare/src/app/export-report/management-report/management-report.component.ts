import { Component, Inject, Injector, OnInit } from "@angular/core";
import { CrudComponentBase } from "@shared/crud-component-base";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { ExportReportService } from "@shared/services/export-report.service";
import { AppUtilsService } from "@shared/services/utils.service";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
  selector: "app-management-report",
  templateUrl: "./management-report.component.html",
  styleUrls: ["./management-report.component.scss"],
})
export class ManagementReportComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private _exportReportService: ExportReportService,
    private _utilsService: AppUtilsService,
    private breadcrumbService: BreadcrumbService,
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
  isSend:boolean;

  showApproveBy: boolean = false;
  acceptStatus: boolean = true;
  isInvestorProf: boolean;
  check_approve: boolean;

  dataApprove = {
    id: 0,
    userApproveId: 1,
    approveNote: null,
    date1: null,
    date2: null,
  };
  status: any = null;

  statusSearch: any[] = [
    {
      name: "BÁO CÁO TỔNG HỢP CÁC GÓI CỔ PHẦN",
      code: "1",
    },
    // {
    //   name: "BÁO CÁO TỔNG HỢP CÁC GÓI ĐẦU TƯ",
    //   code: "2",
    // },
    // {
    //   name: "BÁO CÁO TÍNH GỐC LÃI ĐẾN HẠN",
    //   code: '3'
    // }
  ];

  ngOnInit(): void {}

  hideDialog() {}

  onAccept() {
    console.log("this.status", this.status);

    this.dataApprove.date1 = this.formatDateTimeView(this.dataApprove.date1);
    this.dataApprove.date2 = this.formatDateTimeView(this.dataApprove.date2);
    if (this.status == 1) {
      this.isSend=true;
      this._exportReportService
        .getExportInvest(this.dataApprove.date1, this.dataApprove.date2)
        .subscribe((res) => {
          this.isSend=false;
        });
    } else if (this.status == 2) {
      this._exportReportService
        .getExportInvestment(this.dataApprove.date1, this.dataApprove.date2)
        .subscribe((res) => {});
    } else if(this.status == 3) {
      this._exportReportService.getExportCompanyShareDue( this.dataApprove.date1, this.dataApprove.date2).subscribe((res) => {
     
      });
    }
    else if (this.status == null) {
      this.messageError("Vui lòng chọn loại hình thông báo");
    }
  }

  validForm(): boolean {
    let validRequired;
    validRequired = this.dataApprove?.date1 && this.dataApprove?.date2;

    return validRequired;
  }
}
