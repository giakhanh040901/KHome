import { Component, Injector, ViewChild } from "@angular/core";
import { CrudComponentBase } from "@shared/crud-component-base";
import { DashBoardServiceProxy } from "@shared/service-proxies/dashboard-service";
import { MessageService } from "primeng/api";
import { AppConfig } from "../demo/domain/appconfig";
import { ConfigService } from "../demo/service/app.config.service";
import { BreadcrumbService } from "../layout/breadcrumb/breadcrumb.service";
import { listGender, listGeneration } from "@shared/AppConsts";
import { FormatCurrencyPipe } from "@shared/pipes/formatCurrency.pipe";
import { SpinnerService } from "@shared/services/spinner.service";
import { Paginator } from "primeng/paginator";
import { TokenService } from "@shared/services/token.service";
import jwtDecode from "jwt-decode";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
  providers: [],
})
export class HomeComponent extends CrudComponentBase {
  config: AppConfig;

  constructor(
    injector: Injector,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    public configService: ConfigService,
    private dashBoardService: DashBoardServiceProxy,
    private spinnerService: SpinnerService,
    private tokenService: TokenService,
  ) {
    super(injector, messageService);

    this.breadcrumbService.setItems([
      { label: "Dashboard", routerLink: ["/"] },
    ]);

    this.config = this.configService.config;
  }
  listCard: any[] = [];
  listValueAccountOverview: number[] = [];
  dataGeneration: any = {};
  dataGender: any = {};
  optionsBar;
  baseOptions: any = {
    responsive: true,
    maintainAspectRatio: false,
    aspectRatio: 0.6,
    plugins: {
      legend: {
        display: true,
        position: "bottom",
      },
    },
  };
  baseDataBarOptions: any = {
    borderColor: "#FFFFFF",
    borderRadius: 4,
    barThickness: 10,
    barPercentage: 0.5,
    categoryPercentage: 0.5,
    borderSkipped: false,
    datalabels: {
      display: false,
    },
    borderWidth: 1,
  };
  selectedColumns: any[] = [];
  dataTable: any[] = [];
  filterDates: Date[] | undefined = undefined;
  @ViewChild("paginator", { static: true }) paginator: Paginator;

  ngOnInit() {

    let token = this.tokenService.getToken();
    let refreshToken = this.tokenService.getRefreshToken();

    let tokenExp = jwtDecode(token)['exp'];
    // let refreshTokenExp = jwtDecode(refreshToken)['exp'];

    console.log('exp___', tokenExp, refreshToken);

    this.listCard = [
      [
        {
          backgroundInfor: "#5346E0",
          textColorInfor: "#fff",
          contentFirstLevel: "Tổng số lượng KH",
          valueFirstLevel: 0,
          contentSecondLevel: "Số lượng KH giao dịch",
          valueSecondLevel: 0,
          transactionColor: "#9D98ED",
        },
        {
          backgroundInfor: "#EFDEDE",
          textColorInfor: "#1A1D1F",
          contentFirstLevel: "Số lượng KH hôm nay",
          valueFirstLevel: 0,
          contentSecondLevel: "Số lượng KH giao dịch",
          valueSecondLevel: 0,
          transactionColor: "#DFA8A8",
        },
      ],
      [
        {
          backgroundInfor: "#DEEFE4",
          textColorInfor: "#1A1D1F",
          contentFirstLevel: "Số lượng KH lũy kế tháng",
          valueFirstLevel: 0,
          contentSecondLevel: "Số lượng KH giao dịch",
          valueSecondLevel: 0,
          transactionColor: "#6DCD8F",
        },
        {
          backgroundInfor: "#F4F1E5",
          textColorInfor: "#1A1D1F",
          contentFirstLevel: "Số lượng KH lũy kế năm",
          valueFirstLevel: 0,
          contentSecondLevel: "Số lượng KH giao dịch",
          valueSecondLevel: 0,
          transactionColor: "#E4D38F",
        },
      ],
    ];
    this.optionsBar = {
      ...this.baseOptions,
      scales: {
        x: {
          ticks: {
            color: "#232154",
          },
          grid: {
            color: "white",
          },
        },
        y: {
          ticks: {
            color: "#99A2BC",
          },
          grid: {
            color: "#ebedef",
            drawBorder: false,
          },
        },
      },
    };
    this.selectedColumns = [
      { field: "index", header: "STT", width: "5rem" },
      { field: "code", header: "Mã khách hàng" },
      { field: "gender", header: "Giới tính", width: "10rem" },
      { field: "generation", header: "Thế hệ" },
      { field: "productsUsed", fieldSort: "NumberProducts", header: "Số sản phẩm đã sử dụng" },
      { field: "investing", fieldSort: "TotalActiveAmount", header: "Đang đầu tư" },
      { field: "invested", fieldSort: "TotalActivedAmount", header: "Đã đầu tư" },
      { field: "sum", fieldSort: "TotalAmount", header: "Tổng" },
    ];
    this.setPage({ page: this.offset });
    this.subject.isSetPage.subscribe(() => {
      this.setPage();
    });
    this.getDataOverview();
  }

  public selectDays(event: any) {
    if (event && this.filterDates && this.filterDates[1]) {
      this.changeDataTable();
      this.getDataOverview();
    }
  }

  public onClearDays(event: any) {
    if (event) {
      this.changeDataTable();
      this.getDataOverview();
    }
  }

  private changeDataTable() {
    if (this.page.getPageNumber() === this.page.pageNumberFirst) {
      this.setPage({ page: this.offset });
    } else {
      this.paginator.changePageToFirst(new PointerEvent("button"));
    }
  }

  public setPage(pageInfo?: any) {
    this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.dashBoardService
      .getDataTabel(this.page, this.filterDates, this.sortData)
      .subscribe((result: any) => {
        if (this.handleResponseInterceptor(result)) {
          this.isLoading = false;
          this.page.totalItems = result.data.totalItems;
          this.dataTable = result.data.items.map((e: any) => ({
            index: e.stt,
            code: e.cifCode,
            gender:
              listGender.find((gender: any) => gender.value === e.gender)
                ?.name || "",
            generation:
              listGeneration.find(
                (generation: any) => generation.value === e.generation
              )?.name || "",
            productsUsed: e.numberProducts || 0,
            investing: this.formatMoneyVND(e.totalActiveAmount),
            invested: this.formatMoneyVND(e.totalActivedAmount),
            sum: this.formatMoneyVND(e.totalAmount),
          }));
        }
      });
  }

  public getDataOverview() {
    this.spinnerService.isShow = true;
    this.dashBoardService
      .getDataOverview(this.filterDates)
      .subscribe((resOverview: any) => {
        this.spinnerService.isShow = false;
        if (this.handleResponseInterceptor(resOverview)) {
          const {
            dashboardOverview,
            funnelCharts,
            generationCustomerCharts,
            genderCustomerCharts,
          } = resOverview.data;
          this.listCard[0][0] = {
            ...this.listCard[0][0],
            valueFirstLevel: dashboardOverview.totalInvestor,
            valueSecondLevel: dashboardOverview.totalDealInvestor,
          };
          this.listCard[0][1] = {
            ...this.listCard[0][1],
            valueFirstLevel: dashboardOverview.totalTodayInvestor,
            valueSecondLevel: dashboardOverview.totalTodayDealInvestor,
          };
          this.listCard[1][0] = {
            ...this.listCard[1][0],
            valueFirstLevel: dashboardOverview.totalMonthInvestor,
            valueSecondLevel: dashboardOverview.totalMonthDealInvestor,
          };
          this.listCard[1][1] = {
            ...this.listCard[1][1],
            valueFirstLevel: dashboardOverview.totalYearInvestor,
            valueSecondLevel: dashboardOverview.totalYearDealInvestor,
          };
          this.listValueAccountOverview = Object.values(funnelCharts).map((e) =>
            Number(e)
          );
          this.dataGeneration = {
            labels: generationCustomerCharts.map((e: any) => e.month),
            datasets: [
              {
                ...this.baseDataBarOptions,
                label: "Generation X",
                backgroundColor: "#5346E0",
                data: generationCustomerCharts.map((e: any) => e.generationX),
              },
              {
                ...this.baseDataBarOptions,
                label: "Millennial",
                backgroundColor: "#CFD5EA",
                data: generationCustomerCharts.map((e: any) => e.millennial),
              },
              {
                ...this.baseDataBarOptions,
                label: "Generation Z",
                backgroundColor: "#E3FAE6",
                data: generationCustomerCharts.map((e: any) => e.generationZ),
              },
              {
                ...this.baseDataBarOptions,
                label: "Baby Boomer",
                backgroundColor: "#F1698A",
                data: generationCustomerCharts.map((e: any) => e.babyBoomer),
              },
            ],
          };
          this.dataGender = {
            labels: genderCustomerCharts.map((e: any) => e.month),
            datasets: [
              {
                ...this.baseDataBarOptions,
                label: "Nam",
                backgroundColor: "#5346E0",
                data: genderCustomerCharts.map((e: any) => e.male),
              },
              {
                ...this.baseDataBarOptions,
                label: "Nữ",
                backgroundColor: "#F1698A",
                data: genderCustomerCharts.map((e: any) => e.female),
              },
              {
                ...this.baseDataBarOptions,
                label: "Tổng",
                backgroundColor: "#CFD5EA",
                data: genderCustomerCharts.map((e: any) => e.total),
              },
            ],
          };
        }
      });
  }

  private formatMoneyVND(value) {
    return FormatCurrencyPipe.prototype.transform(value) + " VNĐ";
  }
}
