import { Chart } from "chart.js";
import ChartDataLabels from "chartjs-plugin-datalabels";
import { Component, Inject, Injector } from "@angular/core";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "../layout/breadcrumb/breadcrumb.service";
import { CrudComponentBase } from "@shared/crud-component-base";
import { DashBoardServiceProxy } from "@shared/service-proxies/dashboard-service";
import * as moment from "moment";
import {
  DashboardConst,
  IConstant,
  IDropdown,
  ProductConst,
} from "@shared/AppConsts";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
  providers: [],
})
export class HomeComponent extends CrudComponentBase {
  constructor(
    private breadcrumbService: BreadcrumbService,
    injector: Injector,
    private _dashBoardService: DashBoardServiceProxy,
    messageService: MessageService,
    @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(injector, messageService);
    Chart.register(ChartDataLabels);
    this.breadcrumbService.setItems([{ label: "Trang chủ" }]);
    this.baseUrl = baseUrl || "";
  }

  public baseUrl: string = "";
  public dataOverview: {
    totalProduct: number;
    saleProject: number;
    totalProductSell: number;
    ratioProductSell: number;
    totalCustomer: number;
    customerMaxsell: number;
  } = {
    totalProduct: 0,
    saleProject: 0,
    totalProductSell: 0,
    ratioProductSell: 0,
    totalCustomer: 0,
    customerMaxsell: 0,
  };
  public listProject: IDropdown[] = [];
  public filter: {
    project: number | undefined;
    dates: Date[] | undefined;
  } = {
    project: undefined,
    dates: undefined,
  };
  public productFilter: number | undefined = undefined;
  public listGalleria: {
    srcImg: string;
    title: string;
    project: {
      name: string;
      position: string;
      quantity: number;
      sold: number;
      acreage: string;
    };
  }[] = [];
  public listRecentActivity: {
    avatar: string;
    name: string;
    infor: string;
    time: string;
  }[] = [];

  baseOptions: any = {
    responsive: true,
    maintainAspectRatio: false,
    aspectRatio: 0.6,
    plugins: {
      legend: {
        display: false,
      },
    },
  };
  dataLine;
  optionsLine;
  dataBar;
  optionsBar;
  dataCombo;
  optionsCombo;
  ngOnInit() {
    // get default filter.dates
    this.getDefaultFilterDate();

    // get data
    this.getListProject();
    this.getData();

    // init options
    this.optionsLine = {
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
            color: "#757575",
            precision: 0,
          },
          min: 0,
          grid: {
            drawBorder: false,
          },
        },
      },
    };

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

    this.optionsCombo = {
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
  }

  public onChangeProjectFilter(event: any) {
    if (event) {
      this.getData();
    }
  }

  public selectDays(event: any) {
    if (event && this.filter.dates && this.filter.dates[1]) {
      this.getDataChangeTime();
    }
  }

  public onClearDays(event: any) {
    this.getDefaultFilterDate();
    this.getDataChangeTime();
  }

  public getData() {
    this._dashBoardService.getData(this.filter).subscribe((res: any) => {
      if (this.handleResponseInterceptor(res)) {
        const { dashboardOverView, numberOfApartmentsChart } = res.data;
        this.dataOverview = {
          totalProduct: dashboardOverView.totalProductItem,
          saleProject: dashboardOverView.projectSell,
          totalProductSell: dashboardOverView.totalProductItemSell,
          ratioProductSell: Math.round(dashboardOverView.ratio),
          totalCustomer: dashboardOverView.totalCustomer,
          customerMaxsell: dashboardOverView.customerMaxsell,
        };
        this.listRecentActivity = res.data.rstDashboardActions.map(
          (e: any) => ({
            avatar: e.avatar,
            name: e.fullname,
            infor:
              DashboardConst.listAction.find(
                (action: IConstant) => e.action === action.id
              ).value || "",
            time: moment(e.createdDate).format("DD/MM/YYYY HH:mm"),
          })
        );
        this.listGalleria = res.data.dashboardOutstandingProject.map(
          (e: any) => ({
            srcImg: e.urlImage ? `${this.baseUrl}/${e.urlImage}` : "",
            title: "Dự án nổi bật",
            project: {
              name: e.name,
              position: e.address,
              quantity: e.totalProductItem,
              sold: e.totalProductItemSell,
              acreage: e.landArea,
            },
          })
        );
        this.fillDataChartChangeTime(res.data);
        this.dataCombo = {
          labels: numberOfApartmentsChart.map(
            (e: any) =>
              ProductConst.classifyTypes.find(
                (item: any) => item.code === e.classifyType
              )?.name || ""
          ),
          datasets: [
            {
              type: "line",
              label: "SL căn đã bán",
              data: numberOfApartmentsChart.map(
                (e: any) => e.totalProductItemSell
              ),
              fill: false,
              borderColor: "#EF4770",
              tension: 0,
              pointBackgroundColor: "#FFFFFF",
              pointBorderColor: "#01058A",
              borderWidth: 2,
              pointRadius: 5,
              pointHoverRadius: 7,
              datalabels: {
                display: false,
              },
            },
            {
              type: "bar",
              label: "SL căn",
              data: numberOfApartmentsChart.map((e: any) => e.totalProductItem),
              backgroundColor: "#C7CBEC",
              borderColor: "#FFFFFF",
              borderRadius: 4,
              barThickness: 40, // Độ rộng của cột
              barPercentage: 0.5,
              categoryPercentage: 0.5,
              borderSkipped: false,
              datalabels: {
                display: false,
              },
              borderWidth: 1,
            },
          ],
        };
      }
    });
  }

  public getDataChangeTime() {
    this._dashBoardService.getData(this.filter).subscribe((res: any) => {
      if (this.handleResponseInterceptor(res)) {
        this.fillDataChartChangeTime(res.data);
      }
    });
  }

  private fillDataChartChangeTime(dataSource: any) {
    const { sellChartOverTime, chartRatioSell } = dataSource;
    this.dataLine = {
      labels: sellChartOverTime.map(
        (e: any) => `${moment(e.depositDate).format("DD/MM/YYYY")}`
      ),
      datasets: [
        {
          label: "Tổng giá trị trong ngày",
          data: sellChartOverTime.map((e: any) => e.total),
          fill: false,
          borderColor: "#EF4770",
          tension: 0.4,
          pointBackgroundColor: "#FFFFFF",
          pointBorderColor: "#01058A",
          borderWidth: 2,
          pointRadius: 5,
          pointHoverRadius: 7,
          datalabels: {
            display: false,
          },
        },
      ],
    };
    const formatCurrency = this.formatCurrency.bind(this);
    this.optionsLine.plugins = {
      ...this.optionsLine.plugins,
      tooltip: {
        callbacks: {
          label: function (context) {
            const totalPrice = sellChartOverTime[context.dataIndex]
              ? sellChartOverTime[context.dataIndex].totalPrice
              : 0;
            return `Giá trị trong ngày: ${
              context.formattedValue
            } căn hộ - ${formatCurrency(totalPrice)}`;
          },
        },
      },
    };

    this.dataBar = {
      labels: chartRatioSell.map((e: any) => e.name),
      datasets: [
        {
          label: "Lượt bán",
          data: chartRatioSell.map((e: any) => e.totalSell),
          backgroundColor: "#6C63F0",
          borderColor: "#FFFFFF",
          borderRadius: 4,
          barThickness: 40, // Độ rộng của cột
          barPercentage: 0.5,
          categoryPercentage: 0.5,
          borderSkipped: false,
          datalabels: {
            display: false,
          },
          borderWidth: 1,
        },
      ],
    };
  }

  private getListProject() {
    this._dashBoardService.getListProject().subscribe((res: any) => {
      if (this.handleResponseInterceptor(res)) {
        this.listProject = res.data.map(
          (e: any) =>
            ({
              name: e.name,
              code: e.projectId,
            } as IDropdown)
        );
      }
    });
  }

  private getDefaultFilterDate() {
    // mặc định cách 1 tuần
    let date = moment();
    let sevenDayAgo = moment().subtract(7, 'days');
    this.filter.dates = [sevenDayAgo.toDate(), date.toDate()];
  }
}
