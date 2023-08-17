import { Chart } from "chart.js";
import ChartDataLabels from "chartjs-plugin-datalabels";
import { Component, Inject, Injector, ViewChild } from "@angular/core";
import { MenuItem, MessageService } from "primeng/api";
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
import { TabView } from "primeng/tabview";
import { CountryService } from "@shared/services/country.service";
interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}

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
    @Inject(API_BASE_URL) baseUrl?: string,
    //private countryService: CountryService
  ) {
    super(injector, messageService);
    Chart.register(ChartDataLabels);
    this.breadcrumbService.setItems([{ label: "Trang chủ" }]);
    this.baseUrl = baseUrl || "";
  }
  

  tabViewActive: {
		product: boolean;
		project: boolean;
	} = {
		product: true,
		project: false
	};

	@ViewChild(TabView) tabView: TabView;


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
  items: MenuItem[] | undefined;

  countries: any[] | undefined;

  selectedCountry: any;

  filteredCountries: any[] | undefined;
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

    this.items = [
      {
          label: 'Mua',
          icon: 'pi pi-fw pi-file',
          items: [
              {
                  label: 'New',
                  icon: 'pi pi-fw pi-plus',
                  items: [
                      {
                          label: 'Bookmark',
                          icon: 'pi pi-fw pi-bookmark'
                      },
                      {
                          label: 'Video',
                          icon: 'pi pi-fw pi-video'
                      }
                  ]
              },
              {
                  label: 'Delete',
                  icon: 'pi pi-fw pi-trash'
              },
              {
                  separator: true
              },
              {
                  label: 'Export',
                  icon: 'pi pi-fw pi-external-link'
              }
          ]
      },
      {
          label: 'Bán',
          icon: 'pi pi-fw pi-pencil',
          items: [
              {
                  label: 'Left',
                  icon: 'pi pi-fw pi-align-left'
              },
              {
                  label: 'Right',
                  icon: 'pi pi-fw pi-align-right'
              },
              {
                  label: 'Center',
                  icon: 'pi pi-fw pi-align-center'
              },
              {
                  label: 'Justify',
                  icon: 'pi pi-fw pi-align-justify'
              }
          ]
      },
      {
          label: 'Users',
          icon: 'pi pi-fw pi-user',
          items: [
              {
                  label: 'New',
                  icon: 'pi pi-fw pi-user-plus'
              },
              {
                  label: 'Delete',
                  icon: 'pi pi-fw pi-user-minus'
              },
              {
                  label: 'Search',
                  icon: 'pi pi-fw pi-users',
                  items: [
                      {
                          label: 'Filter',
                          icon: 'pi pi-fw pi-filter',
                          items: [
                              {
                                  label: 'Print',
                                  icon: 'pi pi-fw pi-print'
                              }
                          ]
                      },
                      {
                          icon: 'pi pi-fw pi-bars',
                          label: 'List'
                      }
                  ]
              }
          ]
      },
      {
          label: 'Events',
          icon: 'pi pi-fw pi-calendar',
          items: [
              {
                  label: 'Edit',
                  icon: 'pi pi-fw pi-pencil',
                  items: [
                      {
                          label: 'Save',
                          icon: 'pi pi-fw pi-calendar-plus'
                      },
                      {
                          label: 'Delete',
                          icon: 'pi pi-fw pi-calendar-minus'
                      }
                  ]
              },
              {
                  label: 'Archieve',
                  icon: 'pi pi-fw pi-calendar-times',
                  items: [
                      {
                          label: 'Remove',
                          icon: 'pi pi-fw pi-calendar-minus'
                      }
                  ]
              }
          ]
      },
      {
          label: 'Quit',
          icon: 'pi pi-fw pi-power-off'
      }
  ];
  
    this.getCountries().then((countries) => {
      this.countries = countries;
    });
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

  changeTab(event: any) {
		let tabHeader = this.tabView.tabs[event.index].header;
		this.tabViewActive[tabHeader] = true;
	}

  filterCountry(event: AutoCompleteCompleteEvent) {
    let filtered: any[] = [];
    let query = event.query;

    for (let i = 0; i < (this.countries as any[]).length; i++) {
        let country = (this.countries as any[])[i];
        if (country.name.toLowerCase().indexOf(query.toLowerCase()) == 0) {
            filtered.push(country);
        }
    }

    this.filteredCountries = filtered;
  }

  getCountryData() {
    return [
        { name: 'Afghanistan', code: 'AF' },
        { name: 'Albania', code: 'AL' },
        { name: 'Algeria', code: 'DZ' },
        { name: 'American Samoa', code: 'AS' },
        { name: 'Andorra', code: 'AD' },
        { name: 'Angola', code: 'AO' },
        { name: 'Anguilla', code: 'AI' },
        { name: 'Antarctica', code: 'AQ' },
        { name: 'Antigua and Barbuda', code: 'AG' },
        { name: 'Argentina', code: 'AR' },
        { name: 'Armenia', code: 'AM' },
        { name: 'Aruba', code: 'AW' },
        { name: 'Australia', code: 'AU' },
        { name: 'Austria', code: 'AT' },
        { name: 'Azerbaijan', code: 'AZ' },
        { name: 'Bahamas', code: 'BS' },
        { name: 'Bahrain', code: 'BH' },
        { name: 'Bangladesh', code: 'BD' },
        { name: 'Barbados', code: 'BB' },
        { name: 'Belarus', code: 'BY' },
        { name: 'Belgium', code: 'BE' },
        { name: 'Belize', code: 'BZ' },
        { name: 'Benin', code: 'BJ' },
        { name: 'Bermuda', code: 'BM' },
        { name: 'Bhutan', code: 'BT' },
        { name: 'Bolivia', code: 'BO' },
        { name: 'Bosnia and Herzegovina', code: 'BA' },
        { name: 'Botswana', code: 'BW' },
        { name: 'Bouvet Island', code: 'BV' },
        { name: 'Brazil', code: 'BR' },
        { name: 'British Indian Ocean Territory', code: 'IO' },
        { name: 'Brunei Darussalam', code: 'BN' },
        { name: 'Bulgaria', code: 'BG' },
        { name: 'Burkina Faso', code: 'BF' },
        { name: 'Burundi', code: 'BI' },
        { name: 'Cambodia', code: 'KH' },
        { name: 'Cameroon', code: 'CM' },
        { name: 'Canada', code: 'CA' },
        { name: 'Cape Verde', code: 'CV' },
        { name: 'Cayman Islands', code: 'KY' },
        { name: 'Central African Republic', code: 'CF' },
        { name: 'Chad', code: 'TD' },
        { name: 'Chile', code: 'CL' },
        { name: 'China', code: 'CN' },
        { name: 'Christmas Island', code: 'CX' },
        { name: 'Cocos (Keeling) Islands', code: 'CC' },
        { name: 'Colombia', code: 'CO' },
        { name: 'Comoros', code: 'KM' },
        { name: 'Congo', code: 'CG' },
        { name: 'Congo, The Democratic Republic of the', code: 'CD' },
        { name: 'Cook Islands', code: 'CK' },
        { name: 'Costa Rica', code: 'CR' },
        { name: 'Cote D"Ivoire', code: 'CI' },
        { name: 'Croatia', code: 'HR' },
        { name: 'Cuba', code: 'CU' },
        { name: 'Cyprus', code: 'CY' },
        { name: 'Czech Republic', code: 'CZ' },
        { name: 'Denmark', code: 'DK' },
        { name: 'Djibouti', code: 'DJ' },
        { name: 'Dominica', code: 'DM' },
        { name: 'Dominican Republic', code: 'DO' },
        { name: 'Ecuador', code: 'EC' },
        { name: 'Egypt', code: 'EG' },
        { name: 'El Salvador', code: 'SV' },
        { name: 'Equatorial Guinea', code: 'GQ' },
        { name: 'Eritrea', code: 'ER' },
        { name: 'Estonia', code: 'EE' },
        { name: 'Ethiopia', code: 'ET' },
        { name: 'Falkland Islands (Malvinas)', code: 'FK' },
        { name: 'Faroe Islands', code: 'FO' },
        { name: 'Fiji', code: 'FJ' },
        { name: 'Finland', code: 'FI' },
        { name: 'France', code: 'FR' },
        { name: 'French Guiana', code: 'GF' },
        { name: 'French Polynesia', code: 'PF' },
        { name: 'French Southern Territories', code: 'TF' },
        { name: 'Gabon', code: 'GA' },
        { name: 'Gambia', code: 'GM' },
        { name: 'Georgia', code: 'GE' },
        { name: 'Germany', code: 'DE' },
        { name: 'Ghana', code: 'GH' },
        { name: 'Gibraltar', code: 'GI' },
        { name: 'Greece', code: 'GR' },
        { name: 'Greenland', code: 'GL' },
        { name: 'Grenada', code: 'GD' },
        { name: 'Guadeloupe', code: 'GP' },
        { name: 'Guam', code: 'GU' },
        { name: 'Guatemala', code: 'GT' },
        { name: 'Guernsey', code: 'GG' },
        { name: 'Guinea', code: 'GN' },
        { name: 'Guinea-Bissau', code: 'GW' },
        { name: 'Guyana', code: 'GY' },
        { name: 'Haiti', code: 'HT' },
        { name: 'Heard Island and Mcdonald Islands', code: 'HM' },
        { name: 'Holy See (Vatican City State)', code: 'VA' },
        { name: 'Honduras', code: 'HN' },
        { name: 'Hong Kong', code: 'HK' },
        { name: 'Hungary', code: 'HU' },
        { name: 'Iceland', code: 'IS' },
        { name: 'India', code: 'IN' },
        { name: 'Indonesia', code: 'ID' },
        { name: 'Iran, Islamic Republic Of', code: 'IR' },
        { name: 'Iraq', code: 'IQ' },
        { name: 'Ireland', code: 'IE' },
        { name: 'Isle of Man', code: 'IM' },
        { name: 'Israel', code: 'IL' },
        { name: 'Italy', code: 'IT' },
        { name: 'Jamaica', code: 'JM' },
        { name: 'Japan', code: 'JP' },
        { name: 'Jersey', code: 'JE' },
        { name: 'Jordan', code: 'JO' },
        { name: 'Kazakhstan', code: 'KZ' },
        { name: 'Kenya', code: 'KE' },
        { name: 'Kiribati', code: 'KI' },
        { name: 'Korea, Democratic People"S Republic of', code: 'KP' },
        { name: 'Korea, Republic of', code: 'KR' },
        { name: 'Kuwait', code: 'KW' },
        { name: 'Kyrgyzstan', code: 'KG' },
        { name: 'Lao People"S Democratic Republic', code: 'LA' },
        { name: 'Latvia', code: 'LV' },
        { name: 'Lebanon', code: 'LB' },
        { name: 'Lesotho', code: 'LS' },
        { name: 'Liberia', code: 'LR' },
        { name: 'Libyan Arab Jamahiriya', code: 'LY' },
        { name: 'Liechtenstein', code: 'LI' },
        { name: 'Lithuania', code: 'LT' },
        { name: 'Luxembourg', code: 'LU' },
        { name: 'Macao', code: 'MO' },
        { name: 'Macedonia, The Former Yugoslav Republic of', code: 'MK' },
        { name: 'Madagascar', code: 'MG' },
        { name: 'Malawi', code: 'MW' },
        { name: 'Malaysia', code: 'MY' },
        { name: 'Maldives', code: 'MV' },
        { name: 'Mali', code: 'ML' },
        { name: 'Malta', code: 'MT' },
        { name: 'Marshall Islands', code: 'MH' },
        { name: 'Martinique', code: 'MQ' },
        { name: 'Mauritania', code: 'MR' },
        { name: 'Mauritius', code: 'MU' },
        { name: 'Mayotte', code: 'YT' },
        { name: 'Mexico', code: 'MX' },
        { name: 'Micronesia, Federated States of', code: 'FM' },
        { name: 'Moldova, Republic of', code: 'MD' },
        { name: 'Monaco', code: 'MC' },
        { name: 'Mongolia', code: 'MN' },
        { name: 'Montserrat', code: 'MS' },
        { name: 'Morocco', code: 'MA' },
        { name: 'Mozambique', code: 'MZ' },
        { name: 'Myanmar', code: 'MM' },
        { name: 'Namibia', code: 'NA' },
        { name: 'Nauru', code: 'NR' },
        { name: 'Nepal', code: 'NP' },
        { name: 'Netherlands', code: 'NL' },
        { name: 'Netherlands Antilles', code: 'AN' },
        { name: 'New Caledonia', code: 'NC' },
        { name: 'New Zealand', code: 'NZ' },
        { name: 'Nicaragua', code: 'NI' },
        { name: 'Niger', code: 'NE' },
        { name: 'Nigeria', code: 'NG' },
        { name: 'Niue', code: 'NU' },
        { name: 'Norfolk Island', code: 'NF' },
        { name: 'Northern Mariana Islands', code: 'MP' },
        { name: 'Norway', code: 'NO' },
        { name: 'Oman', code: 'OM' },
        { name: 'Pakistan', code: 'PK' },
        { name: 'Palau', code: 'PW' },
        { name: 'Palestinian Territory, Occupied', code: 'PS' },
        { name: 'Panama', code: 'PA' },
        { name: 'Papua New Guinea', code: 'PG' },
        { name: 'Paraguay', code: 'PY' },
        { name: 'Peru', code: 'PE' },
        { name: 'Philippines', code: 'PH' },
        { name: 'Pitcairn', code: 'PN' },
        { name: 'Poland', code: 'PL' },
        { name: 'Portugal', code: 'PT' },
        { name: 'Puerto Rico', code: 'PR' },
        { name: 'Qatar', code: 'QA' },
        { name: 'Reunion', code: 'RE' },
        { name: 'Romania', code: 'RO' },
        { name: 'Russian Federation', code: 'RU' },
        { name: 'RWANDA', code: 'RW' },
        { name: 'Saint Helena', code: 'SH' },
        { name: 'Saint Kitts and Nevis', code: 'KN' },
        { name: 'Saint Lucia', code: 'LC' },
        { name: 'Saint Pierre and Miquelon', code: 'PM' },
        { name: 'Saint Vincent and the Grenadines', code: 'VC' },
        { name: 'Samoa', code: 'WS' },
        { name: 'San Marino', code: 'SM' },
        { name: 'Sao Tome and Principe', code: 'ST' },
        { name: 'Saudi Arabia', code: 'SA' },
        { name: 'Senegal', code: 'SN' },
        { name: 'Serbia and Montenegro', code: 'CS' },
        { name: 'Seychelles', code: 'SC' },
        { name: 'Sierra Leone', code: 'SL' },
        { name: 'Singapore', code: 'SG' },
        { name: 'Slovakia', code: 'SK' },
        { name: 'Slovenia', code: 'SI' },
        { name: 'Solomon Islands', code: 'SB' },
        { name: 'Somalia', code: 'SO' },
        { name: 'South Africa', code: 'ZA' },
        { name: 'South Georgia and the South Sandwich Islands', code: 'GS' },
        { name: 'Spain', code: 'ES' },
        { name: 'Sri Lanka', code: 'LK' },
        { name: 'Sudan', code: 'SD' },
        { name: 'Suriname', code: 'SR' },
        { name: 'Svalbard and Jan Mayen', code: 'SJ' },
        { name: 'Swaziland', code: 'SZ' },
        { name: 'Sweden', code: 'SE' },
        { name: 'Switzerland', code: 'CH' },
        { name: 'Syrian Arab Republic', code: 'SY' },
        { name: 'Taiwan, Province of China', code: 'TW' },
        { name: 'Tajikistan', code: 'TJ' },
        { name: 'Tanzania, United Republic of', code: 'TZ' },
        { name: 'Thailand', code: 'TH' },
        { name: 'Timor-Leste', code: 'TL' },
        { name: 'Togo', code: 'TG' },
        { name: 'Tokelau', code: 'TK' },
        { name: 'Tonga', code: 'TO' },
        { name: 'Trinidad and Tobago', code: 'TT' },
        { name: 'Tunisia', code: 'TN' },
        { name: 'Turkey', code: 'TR' },
        { name: 'Turkmenistan', code: 'TM' },
        { name: 'Turks and Caicos Islands', code: 'TC' },
        { name: 'Tuvalu', code: 'TV' },
        { name: 'Uganda', code: 'UG' },
        { name: 'Ukraine', code: 'UA' },
        { name: 'United Arab Emirates', code: 'AE' },
        { name: 'United Kingdom', code: 'GB' },
        { name: 'United States', code: 'US' },
        { name: 'United States Minor Outlying Islands', code: 'UM' },
        { name: 'Uruguay', code: 'UY' },
        { name: 'Uzbekistan', code: 'UZ' },
        { name: 'Vanuatu', code: 'VU' },
        { name: 'Venezuela', code: 'VE' },
        { name: 'Viet Nam', code: 'VN' },
        { name: 'Virgin Islands, British', code: 'VG' },
        { name: 'Virgin Islands, U.S.', code: 'VI' },
        { name: 'Wallis and Futuna', code: 'WF' },
        { name: 'Western Sahara', code: 'EH' },
        { name: 'Yemen', code: 'YE' },
        { name: 'Zambia', code: 'ZM' },
        { name: 'Zimbabwe', code: 'ZW' }
    ];
}

getCountries() {
    return Promise.resolve(this.getCountryData());
}
}
