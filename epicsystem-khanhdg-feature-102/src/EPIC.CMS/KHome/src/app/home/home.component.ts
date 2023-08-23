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

interface Product {
  id?: string;
  code?: string;
  name?: string;
  description?: string;
  price?: number;
  quantity?: number;
  inventoryStatus?: string;
  category?: string;
  image?: string;
  rating?: number;
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
  products: Product[] | undefined;

  responsiveOptions: any[] | undefined;
  
  images: any[] | undefined;

  responsiveOption: any[] = [
    {
        breakpoint: '1024px',
        numVisible: 5
    },
    {
        breakpoint: '768px',
        numVisible: 3
    },
    {
        breakpoint: '560px',
        numVisible: 1
    }
];
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

    this.getProductsSmall().then((products) => {
      this.products = products;
  });

  this.responsiveOptions = [
      {
          breakpoint: '1199px',
          numVisible: 1,
          numScroll: 1
      },
      {
          breakpoint: '991px',
          numVisible: 2,
          numScroll: 1
      },
      {
          breakpoint: '767px',
          numVisible: 1,
          numScroll: 1
      }
  ];

  this.getImages().then((images) => {
    this.images = images;
  });
  }

  getSeverity(status: string) {
    switch (status) {
        case 'INSTOCK':
            return 'success';
        case 'LOWSTOCK':
            return 'warning';
        case 'OUTOFSTOCK':
            return 'danger';
    }
    return '';
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


getProductsData() {
  return [
      {
          id: '1000',
          code: 'f230fh0g3',
          name: 'Bamboo Watch',
          description: 'Product Description',
          image: 'bamboo-watch.jpg',
          price: 65,
          category: 'Accessories',
          quantity: 24,
          inventoryStatus: 'INSTOCK',
          rating: 5
      },
      {
          id: '1001',
          code: 'nvklal433',
          name: 'Black Watch',
          description: 'Product Description',
          image: 'black-watch.jpg',
          price: 72,
          category: 'Accessories',
          quantity: 61,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 4
      },
      {
          id: '1002',
          code: 'zz21cz3c1',
          name: 'Blue Band',
          description: 'Product Description',
          image: 'blue-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 2,
          inventoryStatus: 'LOWSTOCK',
          rating: 3
      },
      {
          id: '1003',
          code: '244wgerg2',
          name: 'Blue T-Shirt',
          description: 'Product Description',
          image: 'blue-t-shirt.jpg',
          price: 29,
          category: 'Clothing',
          quantity: 25,
          inventoryStatus: 'INSTOCK',
          rating: 5
      },
      {
          id: '1004',
          code: 'h456wer53',
          name: 'Bracelet',
          description: 'Product Description',
          image: 'bracelet.jpg',
          price: 15,
          category: 'Accessories',
          quantity: 73,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1005',
          code: 'av2231fwg',
          name: 'Brown Purse',
          description: 'Product Description',
          image: 'brown-purse.jpg',
          price: 120,
          category: 'Accessories',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 4
      },
      {
          id: '1006',
          code: 'bib36pfvm',
          name: 'Chakra Bracelet',
          description: 'Product Description',
          image: 'chakra-bracelet.jpg',
          price: 32,
          category: 'Accessories',
          quantity: 5,
          inventoryStatus: 'LOWSTOCK',
          rating: 3
      },
      {
          id: '1007',
          code: 'mbvjkgip5',
          name: 'Galaxy Earrings',
          description: 'Product Description',
          image: 'galaxy-earrings.jpg',
          price: 34,
          category: 'Accessories',
          quantity: 23,
          inventoryStatus: 'INSTOCK',
          rating: 5
      },
      {
          id: '1008',
          code: 'vbb124btr',
          name: 'Game Controller',
          description: 'Product Description',
          image: 'game-controller.jpg',
          price: 99,
          category: 'Electronics',
          quantity: 2,
          inventoryStatus: 'LOWSTOCK',
          rating: 4
      },
      {
          id: '1009',
          code: 'cm230f032',
          name: 'Gaming Set',
          description: 'Product Description',
          image: 'gaming-set.jpg',
          price: 299,
          category: 'Electronics',
          quantity: 63,
          inventoryStatus: 'INSTOCK',
          rating: 3
      },
      {
          id: '1010',
          code: 'plb34234v',
          name: 'Gold Phone Case',
          description: 'Product Description',
          image: 'gold-phone-case.jpg',
          price: 24,
          category: 'Accessories',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 4
      },
      {
          id: '1011',
          code: '4920nnc2d',
          name: 'Green Earbuds',
          description: 'Product Description',
          image: 'green-earbuds.jpg',
          price: 89,
          category: 'Electronics',
          quantity: 23,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1012',
          code: '250vm23cc',
          name: 'Green T-Shirt',
          description: 'Product Description',
          image: 'green-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 74,
          inventoryStatus: 'INSTOCK',
          rating: 5
      },
      {
          id: '1013',
          code: 'fldsmn31b',
          name: 'Grey T-Shirt',
          description: 'Product Description',
          image: 'grey-t-shirt.jpg',
          price: 48,
          category: 'Clothing',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 3
      },
      {
          id: '1014',
          code: 'waas1x2as',
          name: 'Headphones',
          description: 'Product Description',
          image: 'headphones.jpg',
          price: 175,
          category: 'Electronics',
          quantity: 8,
          inventoryStatus: 'LOWSTOCK',
          rating: 5
      },
      {
          id: '1015',
          code: 'vb34btbg5',
          name: 'Light Green T-Shirt',
          description: 'Product Description',
          image: 'light-green-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 34,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1016',
          code: 'k8l6j58jl',
          name: 'Lime Band',
          description: 'Product Description',
          image: 'lime-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 12,
          inventoryStatus: 'INSTOCK',
          rating: 3
      },
      {
          id: '1017',
          code: 'v435nn85n',
          name: 'Mini Speakers',
          description: 'Product Description',
          image: 'mini-speakers.jpg',
          price: 85,
          category: 'Clothing',
          quantity: 42,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1018',
          code: '09zx9c0zc',
          name: 'Painted Phone Case',
          description: 'Product Description',
          image: 'painted-phone-case.jpg',
          price: 56,
          category: 'Accessories',
          quantity: 41,
          inventoryStatus: 'INSTOCK',
          rating: 5
      },
      {
          id: '1019',
          code: 'mnb5mb2m5',
          name: 'Pink Band',
          description: 'Product Description',
          image: 'pink-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 63,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1020',
          code: 'r23fwf2w3',
          name: 'Pink Purse',
          description: 'Product Description',
          image: 'pink-purse.jpg',
          price: 110,
          category: 'Accessories',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 4
      },
      {
          id: '1021',
          code: 'pxpzczo23',
          name: 'Purple Band',
          description: 'Product Description',
          image: 'purple-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 6,
          inventoryStatus: 'LOWSTOCK',
          rating: 3
      },
      {
          id: '1022',
          code: '2c42cb5cb',
          name: 'Purple Gemstone Necklace',
          description: 'Product Description',
          image: 'purple-gemstone-necklace.jpg',
          price: 45,
          category: 'Accessories',
          quantity: 62,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1023',
          code: '5k43kkk23',
          name: 'Purple T-Shirt',
          description: 'Product Description',
          image: 'purple-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 2,
          inventoryStatus: 'LOWSTOCK',
          rating: 5
      },
      {
          id: '1024',
          code: 'lm2tny2k4',
          name: 'Shoes',
          description: 'Product Description',
          image: 'shoes.jpg',
          price: 64,
          category: 'Clothing',
          quantity: 0,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1025',
          code: 'nbm5mv45n',
          name: 'Sneakers',
          description: 'Product Description',
          image: 'sneakers.jpg',
          price: 78,
          category: 'Clothing',
          quantity: 52,
          inventoryStatus: 'INSTOCK',
          rating: 4
      },
      {
          id: '1026',
          code: 'zx23zc42c',
          name: 'Teal T-Shirt',
          description: 'Product Description',
          image: 'teal-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 3,
          inventoryStatus: 'LOWSTOCK',
          rating: 3
      },
      {
          id: '1027',
          code: 'acvx872gc',
          name: 'Yellow Earbuds',
          description: 'Product Description',
          image: 'yellow-earbuds.jpg',
          price: 89,
          category: 'Electronics',
          quantity: 35,
          inventoryStatus: 'INSTOCK',
          rating: 3
      },
      {
          id: '1028',
          code: 'tx125ck42',
          name: 'Yoga Mat',
          description: 'Product Description',
          image: 'yoga-mat.jpg',
          price: 20,
          category: 'Fitness',
          quantity: 15,
          inventoryStatus: 'INSTOCK',
          rating: 5
      },
      {
          id: '1029',
          code: 'gwuby345v',
          name: 'Yoga Set',
          description: 'Product Description',
          image: 'yoga-set.jpg',
          price: 20,
          category: 'Fitness',
          quantity: 25,
          inventoryStatus: 'INSTOCK',
          rating: 8
      }
  ];
}

getProductsWithOrdersData() {
  return [
      {
          id: '1000',
          code: 'f230fh0g3',
          name: 'Bamboo Watch',
          description: 'Product Description',
          image: 'bamboo-watch.jpg',
          price: 65,
          category: 'Accessories',
          quantity: 24,
          inventoryStatus: 'INSTOCK',
          rating: 5,
          orders: [
              {
                  id: '1000-0',
                  productCode: 'f230fh0g3',
                  date: '2020-09-13',
                  amount: 65,
                  quantity: 1,
                  customer: 'David James',
                  status: 'PENDING'
              },
              {
                  id: '1000-1',
                  productCode: 'f230fh0g3',
                  date: '2020-05-14',
                  amount: 130,
                  quantity: 2,
                  customer: 'Leon Rodrigues',
                  status: 'DELIVERED'
              },
              {
                  id: '1000-2',
                  productCode: 'f230fh0g3',
                  date: '2019-01-04',
                  amount: 65,
                  quantity: 1,
                  customer: 'Juan Alejandro',
                  status: 'RETURNED'
              },
              {
                  id: '1000-3',
                  productCode: 'f230fh0g3',
                  date: '2020-09-13',
                  amount: 195,
                  quantity: 3,
                  customer: 'Claire Morrow',
                  status: 'CANCELLED'
              }
          ]
      },
      {
          id: '1001',
          code: 'nvklal433',
          name: 'Black Watch',
          description: 'Product Description',
          image: 'black-watch.jpg',
          price: 72,
          category: 'Accessories',
          quantity: 61,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1001-0',
                  productCode: 'nvklal433',
                  date: '2020-05-14',
                  amount: 72,
                  quantity: 1,
                  customer: 'Maisha Jefferson',
                  status: 'DELIVERED'
              },
              {
                  id: '1001-1',
                  productCode: 'nvklal433',
                  date: '2020-02-28',
                  amount: 144,
                  quantity: 2,
                  customer: 'Octavia Murillo',
                  status: 'PENDING'
              }
          ]
      },
      {
          id: '1002',
          code: 'zz21cz3c1',
          name: 'Blue Band',
          description: 'Product Description',
          image: 'blue-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 2,
          inventoryStatus: 'LOWSTOCK',
          rating: 3,
          orders: [
              {
                  id: '1002-0',
                  productCode: 'zz21cz3c1',
                  date: '2020-07-05',
                  amount: 79,
                  quantity: 1,
                  customer: 'Stacey Leja',
                  status: 'DELIVERED'
              },
              {
                  id: '1002-1',
                  productCode: 'zz21cz3c1',
                  date: '2020-02-06',
                  amount: 79,
                  quantity: 1,
                  customer: 'Ashley Wickens',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1003',
          code: '244wgerg2',
          name: 'Blue T-Shirt',
          description: 'Product Description',
          image: 'blue-t-shirt.jpg',
          price: 29,
          category: 'Clothing',
          quantity: 25,
          inventoryStatus: 'INSTOCK',
          rating: 5,
          orders: []
      },
      {
          id: '1004',
          code: 'h456wer53',
          name: 'Bracelet',
          description: 'Product Description',
          image: 'bracelet.jpg',
          price: 15,
          category: 'Accessories',
          quantity: 73,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1004-0',
                  productCode: 'h456wer53',
                  date: '2020-09-05',
                  amount: 60,
                  quantity: 4,
                  customer: 'Mayumi Misaki',
                  status: 'PENDING'
              },
              {
                  id: '1004-1',
                  productCode: 'h456wer53',
                  date: '2019-04-16',
                  amount: 2,
                  quantity: 30,
                  customer: 'Francesco Salvatore',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1005',
          code: 'av2231fwg',
          name: 'Brown Purse',
          description: 'Product Description',
          image: 'brown-purse.jpg',
          price: 120,
          category: 'Accessories',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1005-0',
                  productCode: 'av2231fwg',
                  date: '2020-01-25',
                  amount: 120,
                  quantity: 1,
                  customer: 'Isabel Sinclair',
                  status: 'RETURNED'
              },
              {
                  id: '1005-1',
                  productCode: 'av2231fwg',
                  date: '2019-03-12',
                  amount: 240,
                  quantity: 2,
                  customer: 'Lionel Clifford',
                  status: 'DELIVERED'
              },
              {
                  id: '1005-2',
                  productCode: 'av2231fwg',
                  date: '2019-05-05',
                  amount: 120,
                  quantity: 1,
                  customer: 'Cody Chavez',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1006',
          code: 'bib36pfvm',
          name: 'Chakra Bracelet',
          description: 'Product Description',
          image: 'chakra-bracelet.jpg',
          price: 32,
          category: 'Accessories',
          quantity: 5,
          inventoryStatus: 'LOWSTOCK',
          rating: 3,
          orders: [
              {
                  id: '1006-0',
                  productCode: 'bib36pfvm',
                  date: '2020-02-24',
                  amount: 32,
                  quantity: 1,
                  customer: 'Arvin Darci',
                  status: 'DELIVERED'
              },
              {
                  id: '1006-1',
                  productCode: 'bib36pfvm',
                  date: '2020-01-14',
                  amount: 64,
                  quantity: 2,
                  customer: 'Izzy Jones',
                  status: 'PENDING'
              }
          ]
      },
      {
          id: '1007',
          code: 'mbvjkgip5',
          name: 'Galaxy Earrings',
          description: 'Product Description',
          image: 'galaxy-earrings.jpg',
          price: 34,
          category: 'Accessories',
          quantity: 23,
          inventoryStatus: 'INSTOCK',
          rating: 5,
          orders: [
              {
                  id: '1007-0',
                  productCode: 'mbvjkgip5',
                  date: '2020-06-19',
                  amount: 34,
                  quantity: 1,
                  customer: 'Jennifer Smith',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1008',
          code: 'vbb124btr',
          name: 'Game Controller',
          description: 'Product Description',
          image: 'game-controller.jpg',
          price: 99,
          category: 'Electronics',
          quantity: 2,
          inventoryStatus: 'LOWSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1008-0',
                  productCode: 'vbb124btr',
                  date: '2020-01-05',
                  amount: 99,
                  quantity: 1,
                  customer: 'Jeanfrancois David',
                  status: 'DELIVERED'
              },
              {
                  id: '1008-1',
                  productCode: 'vbb124btr',
                  date: '2020-01-19',
                  amount: 198,
                  quantity: 2,
                  customer: 'Ivar Greenwood',
                  status: 'RETURNED'
              }
          ]
      },
      {
          id: '1009',
          code: 'cm230f032',
          name: 'Gaming Set',
          description: 'Product Description',
          image: 'gaming-set.jpg',
          price: 299,
          category: 'Electronics',
          quantity: 63,
          inventoryStatus: 'INSTOCK',
          rating: 3,
          orders: [
              {
                  id: '1009-0',
                  productCode: 'cm230f032',
                  date: '2020-06-24',
                  amount: 299,
                  quantity: 1,
                  customer: 'Kadeem Mujtaba',
                  status: 'PENDING'
              },
              {
                  id: '1009-1',
                  productCode: 'cm230f032',
                  date: '2020-05-11',
                  amount: 299,
                  quantity: 1,
                  customer: 'Ashley Wickens',
                  status: 'DELIVERED'
              },
              {
                  id: '1009-2',
                  productCode: 'cm230f032',
                  date: '2019-02-07',
                  amount: 299,
                  quantity: 1,
                  customer: 'Julie Johnson',
                  status: 'DELIVERED'
              },
              {
                  id: '1009-3',
                  productCode: 'cm230f032',
                  date: '2020-04-26',
                  amount: 299,
                  quantity: 1,
                  customer: 'Tony Costa',
                  status: 'CANCELLED'
              }
          ]
      },
      {
          id: '1010',
          code: 'plb34234v',
          name: 'Gold Phone Case',
          description: 'Product Description',
          image: 'gold-phone-case.jpg',
          price: 24,
          category: 'Accessories',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1010-0',
                  productCode: 'plb34234v',
                  date: '2020-02-04',
                  amount: 24,
                  quantity: 1,
                  customer: 'James Butt',
                  status: 'DELIVERED'
              },
              {
                  id: '1010-1',
                  productCode: 'plb34234v',
                  date: '2020-05-05',
                  amount: 48,
                  quantity: 2,
                  customer: 'Josephine Darakjy',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1011',
          code: '4920nnc2d',
          name: 'Green Earbuds',
          description: 'Product Description',
          image: 'green-earbuds.jpg',
          price: 89,
          category: 'Electronics',
          quantity: 23,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1011-0',
                  productCode: '4920nnc2d',
                  date: '2020-06-01',
                  amount: 89,
                  quantity: 1,
                  customer: 'Art Venere',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1012',
          code: '250vm23cc',
          name: 'Green T-Shirt',
          description: 'Product Description',
          image: 'green-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 74,
          inventoryStatus: 'INSTOCK',
          rating: 5,
          orders: [
              {
                  id: '1012-0',
                  productCode: '250vm23cc',
                  date: '2020-02-05',
                  amount: 49,
                  quantity: 1,
                  customer: 'Lenna Paprocki',
                  status: 'DELIVERED'
              },
              {
                  id: '1012-1',
                  productCode: '250vm23cc',
                  date: '2020-02-15',
                  amount: 49,
                  quantity: 1,
                  customer: 'Donette Foller',
                  status: 'PENDING'
              }
          ]
      },
      {
          id: '1013',
          code: 'fldsmn31b',
          name: 'Grey T-Shirt',
          description: 'Product Description',
          image: 'grey-t-shirt.jpg',
          price: 48,
          category: 'Clothing',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 3,
          orders: [
              {
                  id: '1013-0',
                  productCode: 'fldsmn31b',
                  date: '2020-04-01',
                  amount: 48,
                  quantity: 1,
                  customer: 'Simona Morasca',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1014',
          code: 'waas1x2as',
          name: 'Headphones',
          description: 'Product Description',
          image: 'headphones.jpg',
          price: 175,
          category: 'Electronics',
          quantity: 8,
          inventoryStatus: 'LOWSTOCK',
          rating: 5,
          orders: [
              {
                  id: '1014-0',
                  productCode: 'waas1x2as',
                  date: '2020-05-15',
                  amount: 175,
                  quantity: 1,
                  customer: 'Lenna Paprocki',
                  status: 'DELIVERED'
              },
              {
                  id: '1014-1',
                  productCode: 'waas1x2as',
                  date: '2020-01-02',
                  amount: 175,
                  quantity: 1,
                  customer: 'Donette Foller',
                  status: 'CANCELLED'
              }
          ]
      },
      {
          id: '1015',
          code: 'vb34btbg5',
          name: 'Light Green T-Shirt',
          description: 'Product Description',
          image: 'light-green-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 34,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1015-0',
                  productCode: 'vb34btbg5',
                  date: '2020-07-02',
                  amount: 98,
                  quantity: 2,
                  customer: 'Mitsue Tollner',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1016',
          code: 'k8l6j58jl',
          name: 'Lime Band',
          description: 'Product Description',
          image: 'lime-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 12,
          inventoryStatus: 'INSTOCK',
          rating: 3,
          orders: []
      },
      {
          id: '1017',
          code: 'v435nn85n',
          name: 'Mini Speakers',
          description: 'Product Description',
          image: 'mini-speakers.jpg',
          price: 85,
          category: 'Clothing',
          quantity: 42,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1017-0',
                  productCode: 'v435nn85n',
                  date: '2020-07-12',
                  amount: 85,
                  quantity: 1,
                  customer: 'Minna Amigon',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1018',
          code: '09zx9c0zc',
          name: 'Painted Phone Case',
          description: 'Product Description',
          image: 'painted-phone-case.jpg',
          price: 56,
          category: 'Accessories',
          quantity: 41,
          inventoryStatus: 'INSTOCK',
          rating: 5,
          orders: [
              {
                  id: '1018-0',
                  productCode: '09zx9c0zc',
                  date: '2020-07-01',
                  amount: 56,
                  quantity: 1,
                  customer: 'Abel Maclead',
                  status: 'DELIVERED'
              },
              {
                  id: '1018-1',
                  productCode: '09zx9c0zc',
                  date: '2020-05-02',
                  amount: 56,
                  quantity: 1,
                  customer: 'Minna Amigon',
                  status: 'RETURNED'
              }
          ]
      },
      {
          id: '1019',
          code: 'mnb5mb2m5',
          name: 'Pink Band',
          description: 'Product Description',
          image: 'pink-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 63,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: []
      },
      {
          id: '1020',
          code: 'r23fwf2w3',
          name: 'Pink Purse',
          description: 'Product Description',
          image: 'pink-purse.jpg',
          price: 110,
          category: 'Accessories',
          quantity: 0,
          inventoryStatus: 'OUTOFSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1020-0',
                  productCode: 'r23fwf2w3',
                  date: '2020-05-29',
                  amount: 110,
                  quantity: 1,
                  customer: 'Kiley Caldarera',
                  status: 'DELIVERED'
              },
              {
                  id: '1020-1',
                  productCode: 'r23fwf2w3',
                  date: '2020-02-11',
                  amount: 220,
                  quantity: 2,
                  customer: 'Graciela Ruta',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1021',
          code: 'pxpzczo23',
          name: 'Purple Band',
          description: 'Product Description',
          image: 'purple-band.jpg',
          price: 79,
          category: 'Fitness',
          quantity: 6,
          inventoryStatus: 'LOWSTOCK',
          rating: 3,
          orders: [
              {
                  id: '1021-0',
                  productCode: 'pxpzczo23',
                  date: '2020-02-02',
                  amount: 79,
                  quantity: 1,
                  customer: 'Cammy Albares',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1022',
          code: '2c42cb5cb',
          name: 'Purple Gemstone Necklace',
          description: 'Product Description',
          image: 'purple-gemstone-necklace.jpg',
          price: 45,
          category: 'Accessories',
          quantity: 62,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1022-0',
                  productCode: '2c42cb5cb',
                  date: '2020-06-29',
                  amount: 45,
                  quantity: 1,
                  customer: 'Mattie Poquette',
                  status: 'DELIVERED'
              },
              {
                  id: '1022-1',
                  productCode: '2c42cb5cb',
                  date: '2020-02-11',
                  amount: 135,
                  quantity: 3,
                  customer: 'Meaghan Garufi',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1023',
          code: '5k43kkk23',
          name: 'Purple T-Shirt',
          description: 'Product Description',
          image: 'purple-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 2,
          inventoryStatus: 'LOWSTOCK',
          rating: 5,
          orders: [
              {
                  id: '1023-0',
                  productCode: '5k43kkk23',
                  date: '2020-04-15',
                  amount: 49,
                  quantity: 1,
                  customer: 'Gladys Rim',
                  status: 'RETURNED'
              }
          ]
      },
      {
          id: '1024',
          code: 'lm2tny2k4',
          name: 'Shoes',
          description: 'Product Description',
          image: 'shoes.jpg',
          price: 64,
          category: 'Clothing',
          quantity: 0,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: []
      },
      {
          id: '1025',
          code: 'nbm5mv45n',
          name: 'Sneakers',
          description: 'Product Description',
          image: 'sneakers.jpg',
          price: 78,
          category: 'Clothing',
          quantity: 52,
          inventoryStatus: 'INSTOCK',
          rating: 4,
          orders: [
              {
                  id: '1025-0',
                  productCode: 'nbm5mv45n',
                  date: '2020-02-19',
                  amount: 78,
                  quantity: 1,
                  customer: 'Yuki Whobrey',
                  status: 'DELIVERED'
              },
              {
                  id: '1025-1',
                  productCode: 'nbm5mv45n',
                  date: '2020-05-21',
                  amount: 78,
                  quantity: 1,
                  customer: 'Fletcher Flosi',
                  status: 'PENDING'
              }
          ]
      },
      {
          id: '1026',
          code: 'zx23zc42c',
          name: 'Teal T-Shirt',
          description: 'Product Description',
          image: 'teal-t-shirt.jpg',
          price: 49,
          category: 'Clothing',
          quantity: 3,
          inventoryStatus: 'LOWSTOCK',
          rating: 3,
          orders: [
              {
                  id: '1026-0',
                  productCode: 'zx23zc42c',
                  date: '2020-04-24',
                  amount: 98,
                  quantity: 2,
                  customer: 'Bette Nicka',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1027',
          code: 'acvx872gc',
          name: 'Yellow Earbuds',
          description: 'Product Description',
          image: 'yellow-earbuds.jpg',
          price: 89,
          category: 'Electronics',
          quantity: 35,
          inventoryStatus: 'INSTOCK',
          rating: 3,
          orders: [
              {
                  id: '1027-0',
                  productCode: 'acvx872gc',
                  date: '2020-01-29',
                  amount: 89,
                  quantity: 1,
                  customer: 'Veronika Inouye',
                  status: 'DELIVERED'
              },
              {
                  id: '1027-1',
                  productCode: 'acvx872gc',
                  date: '2020-06-11',
                  amount: 89,
                  quantity: 1,
                  customer: 'Willard Kolmetz',
                  status: 'DELIVERED'
              }
          ]
      },
      {
          id: '1028',
          code: 'tx125ck42',
          name: 'Yoga Mat',
          description: 'Product Description',
          image: 'yoga-mat.jpg',
          price: 20,
          category: 'Fitness',
          quantity: 15,
          inventoryStatus: 'INSTOCK',
          rating: 5,
          orders: []
      },
      {
          id: '1029',
          code: 'gwuby345v',
          name: 'Yoga Set',
          description: 'Product Description',
          image: 'yoga-set.jpg',
          price: 20,
          category: 'Fitness',
          quantity: 25,
          inventoryStatus: 'INSTOCK',
          rating: 8,
          orders: [
              {
                  id: '1029-0',
                  productCode: 'gwuby345v',
                  date: '2020-02-14',
                  amount: 4,
                  quantity: 80,
                  customer: 'Maryann Royster',
                  status: 'DELIVERED'
              }
          ]
      }
  ];
}

getProductsMini() {
  return Promise.resolve(this.getProductsData().slice(0, 5));
}

getProductsSmall() {
  return Promise.resolve(this.getProductsData().slice(0, 10));
}

getProducts() {
  return Promise.resolve(this.getProductsData());
}

getProductsWithOrdersSmall() {
  return Promise.resolve(this.getProductsWithOrdersData().slice(0, 10));
}

getProductsWithOrders() {
  return Promise.resolve(this.getProductsWithOrdersData());
}

getDatas() {
  return [
      {
          itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria1.jpg',
          thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria1s.jpg',
          alt: 'Description for Image 1',
          title: 'Title 1'
      },
      {
          itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria2.jpg',
          thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria2s.jpg',
          alt: 'Description for Image 2',
          title: 'Title 2'
      },
      {
          itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria3.jpg',
          thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria3s.jpg',
          alt: 'Description for Image 3',
          title: 'Title 3'
      },
      {
          itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria4.jpg',
          thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria4s.jpg',
          alt: 'Description for Image 4',
          title: 'Title 4'
      },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria5.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria5s.jpg',
      //     alt: 'Description for Image 5',
      //     title: 'Title 5'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria6.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria6s.jpg',
      //     alt: 'Description for Image 6',
      //     title: 'Title 6'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria7.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria7s.jpg',
      //     alt: 'Description for Image 7',
      //     title: 'Title 7'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria8.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria8s.jpg',
      //     alt: 'Description for Image 8',
      //     title: 'Title 8'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria9.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria9s.jpg',
      //     alt: 'Description for Image 9',
      //     title: 'Title 9'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria10.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria10s.jpg',
      //     alt: 'Description for Image 10',
      //     title: 'Title 10'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria11.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria11s.jpg',
      //     alt: 'Description for Image 11',
      //     title: 'Title 11'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria12.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria12s.jpg',
      //     alt: 'Description for Image 12',
      //     title: 'Title 12'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria13.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria13s.jpg',
      //     alt: 'Description for Image 13',
      //     title: 'Title 13'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria14.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria14s.jpg',
      //     alt: 'Description for Image 14',
      //     title: 'Title 14'
      // },
      // {
      //     itemImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria15.jpg',
      //     thumbnailImageSrc: 'https://primefaces.org/cdn/primeng/images/galleria/galleria15s.jpg',
      //     alt: 'Description for Image 15',
      //     title: 'Title 15'
      // }
  ];
}

getImages() {
  return Promise.resolve(this.getDatas());
}
}
