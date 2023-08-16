import {Chart, ChartConfiguration, ChartOptions} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Component, ElementRef, Inject, Injector, ViewChild } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { CrudComponentBase } from '@shared/crud-component-base';
import { DashBoardServiceProxy } from '@shared/service-proxies/dashboard-service';
import * as moment from 'moment';
import { ProductService } from '@shared/services/product.service';
import { AppConsts, DashboardConst, UserTypes } from '@shared/AppConsts';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { Router } from '@angular/router';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: []
})
export class HomeComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _dashBoardService: DashBoardServiceProxy,
        private _productService: ProductService,
		private router: Router,
        @Inject(API_BASE_URL) baseUrl?: string,
    ){
        super(injector, messageService);
        this.userLogin = this.getUser();
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
        Chart.register(ChartDataLabels);
    } 

    DashboardConst = DashboardConst; 
    private baseUrl: string;

    userLogin: any = {};
    refreshTemplate = true;
    refreshReportTradingProviderChart = true; 
    //
    products : any;
    widthReport:any;
    widthPolicy:any;
    widthCashFlow:any;

    cashFlowByDayInfo: any;
    cashFlowByDay: any;
    cashFlowByDayOptions: any;
    //
    policy: any;
    policyOptions: any;
    danhSachTheoKyHanSP: any;

    //
    reportTradingProvider: any = {};
    reportTradingProviderOptions: any = {};
    doanhSoVaSLBanInfo: any;

    //
    reportThucChi: any;
    reportThucChiOptions: any;
    baoCaoThucChi: any;
    
    subscription: Subscription;

    rangeDates: Date[];

    dataFilter = {
        firstDate: null,
        endDate: null,
        productId: null,
    }

    //
    cashInFlowInfo : any= {};   // Dòng tiền vào
    cashOutFlowInfo : any= {};  // Dòng tiền ra
    cashRemainInfo : any= {};  // Số dư
    // 
    dashboardInfo: any;
    upColumnColor: string = '#6C63F0';
    downColumnColor: string = '#ED589D'; 

    //
    reportTradingProviderPlugins: any;
    configReportTradingProvider: any;

    olderX = null;
    olderY = null;

    //
    maxDate: Date;

    @ViewChild('reportTradingProviderChart') private parentRef: ElementRef<HTMLElement>;

    //
    listProduct: any;
    listTrading: any;

    // danh sách hoạt động gần nhất
    listAction:[] = [];
    isPartner: boolean = false;

    ngOnInit() { 
        // product
        this.getListProducts();
        if (UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)){
            this.getListTrading()
            this.isPartner = true;
        } else {
            this.getListProducts();
        }
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
        // Dòng tiền theo ngày
        this.cashFlowByDay = {
            labels: ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
            datasets: [
                {
                    label: 'Dòng tiền',
                    backgroundColor: ['#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0',],
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    datalabels: {
                        display: false
                    },
                    clamp: false,
                },
            ],
        };

        this.cashFlowByDay.datasets[0].data.forEach((value, index) => {
            if(value < 0) {
                this.cashFlowByDay.datasets[0].backgroundColor[index] = '#ED589D';
                this.cashFlowByDay.datasets[0].datalabels.anchor[index] = 'start';
                this.cashFlowByDay.datasets[0].datalabels.align[index] = 'start';
                this.cashFlowByDay.datasets[0].data.align[index] = 'start';
            }
        });
        this.checkLenghtCashFlow();

        // Danh sách theo kỳ hạn sản phẩm
        this.policy = {
            labels: ['1 tháng', '3 tháng', '6 tháng', '9 tháng', '12 tháng', '1 tháng', '3 tháng', '6 tháng', '9 tháng'],
            datasets: [
                {
                    label: 'Danh sách theo kỳ hạn sản phẩm',
                    backgroundColor: this.upColumnColor,
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    datalabels: {
                        display: false
                    },
                },
            ]
        };
        this.checkLengthPolicy();

        // Báo cáo doanh số và số lượng bán theo từng đại lý
        this.reportTradingProvider = {
            // 10 cái để width 100%
            // >10 150rem
            labels: ['Đại lý A', 'Đại lý B', 'Đại lý C', 'Đại lý D', 'Đại lý E', 'Đại lý F', 'Đại lý G', 'Đại lý H', 'Đại lý I', 'Đại lý K'],
            datasets: [
                {
                    label: 'Doanh số',
                    backgroundColor: '#6C63F0',
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 14,   // Độ rộng của cột #ED589D
                    barPercentage: 0.5,
                    categoryPercentage: 0.5,
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    datalabels: {
                        display: false
                    },
                    borderWidth: 1,                    
                    beginAtZero: true,
                    layout: {
                        padding: {
                            right: 50
                        }
                    },
                },
                {
                    label: 'Số dư',
                    backgroundColor: '#ED589D',
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 14,   // Độ rộng của cột #ED589D
                    barPercentage: 0.5,
                    categoryPercentage: 0.5,
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    borderWidth: 1,
                    datalabels: {
                        display: false
                    },  
                    beginAtZero: true,
                    layout: {
                        padding: {
                            left: 50
                        }
                    },
                },
            ],
        };
        this.checkLengthTrading()

        // Báo cáo thực chi
        this.reportThucChi = {
            labels: ['Tháng 1','Tháng 2','Tháng 3','Tháng 4','Tháng 5','Tháng 6','Tháng 7','Tháng 8','Tháng 9','Tháng 10','Tháng 11','Tháng 12'],
            datasets: [
                {
                    label: 'Giá trị',
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    fill: true,
                    backgroundColor: 'rgba(83,70,224,0.2)',
                    pointBackgroundColor: '#5346E0',
                    pointBorderColor: '#5346E0',
                    pointRadius: 3,
                    pointHoverRadius: 5, 
                    borderColor: '#ED589D',
                    borderWidth: 2,
                    tension: 0,
                    datalabels: {
                        display: false
                    }
                },
            ]
        };

        this.cashFlowByDayOptions = {
            responsive: true,
            plugins: {
                legend: {
                  display: false
                }
            },
            scales: {
                x: {
                    ticks: {
                        color: '#232154',
                        font: {
                            size: 9,
                            style: 'italic'
                        }
                    },
                    grid: {
                        color: 'white',
                        padding: 50,
                        beginAtZero: true,
                        min: 0,
                        max: 200,
                    }
                },
                y: {
                    display: true,
                    ticks: {
                        color: '#99A2BC',
                        padding: 12,
                    },
                    grid: {
                        color: '#ebedef',
                        borderDash: [8, 4],
                        drawBorder: false,
                    }
                },
            }
        };

        this.policyOptions = {
            responsive: true,
            plugins: {
                legend: {
                  display: false
                }
            },
            scales: {
                x: {
                    ticks: {
                        color: '#232154',
                        font: {
                            size: 9,
                            style: 'italic'
                        }
                    },
                    grid: {
                        color: 'white',
                        beginAtZero: true,
                        min: 0,
                        max: 200,
                    }
                },
                y: {
                    display: true,
                    ticks: {
                        color: '#99A2BC',
                    },
                    grid: {
                        color: '#ebedef',
                        borderDash: [8, 4],
                        drawBorder: false,
                    }
                },
            }
        };

        this.reportTradingProviderOptions = {
            responsive: true,
            plugins: {
                legend: {
                  display: false
                }
            },
            layout: {
                padding: {
                    right: 18
                }
            },
            scales: {
                x: {
                    min: 0,
                    max: 100,
                    ticks: {
                        color: '#232154',
                        font: {
                            size: 9,
                            style: 'italic'
                        }
                    },
                    grid: {
                        color: 'white',
                        beginAtZero: true,
                        min: 0,
                        max: 200,
                    }
                },
                y: {
                    display: true,
                    ticks: {
                        color: '#99A2BC',
                    },
                    grid: {
                        color: '#ebedef',
                        borderDash: [8, 4],
                        drawBorder: false,
                    }
                },
            }
        };

        this.reportThucChiOptions = {
            maintainAspectRatio: false,
            aspectRatio: 0.6,
            plugins: {
                legend: {
                    display: false
                },
                // beforeDatasetsDraw: function(chart) {
                //     var ctx = chart.ctx;
                //     var gradient = ctx.createLinearGradient(0, 0, 0, chart.chartArea.bottom);
                //     gradient.addColorStop(0, '#5346E0');
                //     gradient.addColorStop(1, 'rgba(83, 70, 224, 0)');
                //     ctx.fillStyle = gradient;
                //     ctx.fillRect(chart.chartArea.left, chart.chartArea.top, chart.chartArea.right - chart.chartArea.left, chart.chartArea.bottom - chart.chartArea.top);
                // }
            },
            scales: {
                x: {
                    ticks: {
                        color: '#232154',
                        font: {
                            size: 9,
                            style: 'italic'
                        }
                    },
                    grid: {
                        color: 'white'
                    }
                },
                y: {
                    ticks: {
                        color: '#99A2BC'
                    },
                    grid: {
                        color: '#ebedef'
                    }
                }
            }
        }

        this.initDate();
    }

    initDate(){
        const today = moment();
        const thirtyDaysAgo = moment().subtract(30, 'days');
        this.rangeDates = [thirtyDaysAgo.toDate(), today.toDate()];
        this.SelectDays();
    }

    getListProducts(tradingProviderId?:number){
        this.isLoadingPage = false;
        this.isLoading = true;
        this._dashBoardService.getProduct(tradingProviderId).subscribe((res) => {
            this.isLoadingPage = false;
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.listProduct = res?.data;
            }
        })
    }

    getListTrading(){
        this.isLoadingPage = false;
        this.isLoading = true;
        this._productService.getTradingProvider().subscribe((res) => {
            this.isLoadingPage = false;
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.listTrading = res?.data;
                console.log('list Trading ', this.listTrading);
                
            }
        })
    }

    checkLenghtCashFlow(){
        let checkLengthCashFlow = this.cashFlowByDay.labels.length;
        if(checkLengthCashFlow <= 15){
            this.widthCashFlow = "100%";
        }  else {
            this.widthCashFlow = `${checkLengthCashFlow*3}rem`;
        }
    }

    checkLengthPolicy(){
        let checkLengthPolicy = this.policy.labels.length;
        if(checkLengthPolicy < 10){
            this.widthPolicy = "100%";
        }  else {
            this.widthPolicy = `${checkLengthPolicy*6}rem`;
        }
    }

    checkLengthTrading(){
        let checkLength = this.reportTradingProvider.labels.length;
        if(checkLength <= 10){
            this.widthReport = "100%";
        }  else {
            if (UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)){
                this.widthReport = `${checkLength*12}rem`;
            } else {
                this.widthReport = `${checkLength*6}rem`;
            }
        }
    }

    changeTrading(tradingProviderId) {
        console.log('!!! tradingProviderId ', tradingProviderId);
        this.getListProducts(tradingProviderId);
    }

    changeProduct(productId) {
        if (this.dataFilter.firstDate != null && this.dataFilter.endDate){
            this.setPage();
        }
    }

    SelectDays(){
        if (this.rangeDates[1] != null){
            this.dataFilter.firstDate = this.rangeDates[0];
            this.dataFilter.endDate = this.rangeDates[1];
            this.setPage()
        };
    }

    UpdateCashFlowByDayInfo() {
        this.cashFlowByDay.datasets[0].data = this.cashFlowByDayInfo.map(element => element.value ?? 0);
        this.cashFlowByDay.labels = this.cashFlowByDayInfo.map(element => this.formatDateMonth(element.date));
        
        this.cashFlowByDay.datasets[0].backgroundColor = [];
        this.cashFlowByDay.datasets[0].datalabels.anchor = [];
        this.cashFlowByDay.datasets[0].datalabels.align = [];

        this.cashFlowByDay.datasets[0].data.forEach((value, index) => {
            if(value < 0) {
                this.cashFlowByDay.datasets[0].backgroundColor[index] = this.downColumnColor;
                this.cashFlowByDay.datasets[0].datalabels.anchor[index] = 'start';
                this.cashFlowByDay.datasets[0].datalabels.align[index] = 'start';
            }
            else {
                this.cashFlowByDay.datasets[0].backgroundColor[index] = this.upColumnColor;
                this.cashFlowByDay.datasets[0].datalabels.anchor[index] = 'end';
                this.cashFlowByDay.datasets[0].datalabels.align[index] = 'end';
            }
        });      
        this.checkLenghtCashFlow();                        

        console.log("dong tien theo ngay", this.cashFlowByDay);
    }

    UpdateDanhSachTheoKyHanSP(){
        this.policy.datasets[0].data = this.danhSachTheoKyHanSP.map(element => element.value);
        this.policy.labels = this.danhSachTheoKyHanSP.map(element => {
            if(element.periodType == "D"){
                return `${element.periodQuantity} Ngày`
            } 
            if(element.periodType == "M"){
                return `${element.periodQuantity} Tháng`
            } 
            if(element.periodType == "Y"){
                return `${element.periodQuantity} Năm`
            } 
            return element.name;
        });
        this.checkLengthPolicy();
        console.log("Danh sách theo ký hạn sản phẩm: ", this.policy);
        
    };

    UpdatedoanhSoVaSLBan(){
        this.reportTradingProvider.labels = this.doanhSoVaSLBanInfo.map(element => element.tradingProviderShortName ? element.tradingProviderShortName :  element.tradingProviderName ? element.tradingProviderName : element.departmentName);
        this.reportTradingProvider.datasets[0].data = this.doanhSoVaSLBanInfo.map(element => element.amout ?? 0 );
        this.reportTradingProvider.datasets[1].data = this.doanhSoVaSLBanInfo.map(element => element.remain ?? 0);        
        this.checkLengthTrading();
        console.log('!!! Báo cáo doanh số: ', this.reportTradingProvider);
        

    }

    UpdateThucChi(){
        this.reportThucChi.datasets[0].data = this.baoCaoThucChi.map(element => element.amount ?? 0);
        this.reportThucChi.label = this.baoCaoThucChi.map(element => `Tháng ${element.month}`);
    }

    checkNavi(item){
        switch (item.action) {
            case this.DashboardConst.DAT_LENH_MOI:
                this.router.navigate(["/trading-contract/order"]);
                break;
            case this.DashboardConst.RUT_TIEN:
                this.router.navigate(["/trading-contract/manager-withdraw"]);
                break;
            case this.DashboardConst.YEU_CAU_NHAN_HOP_DONG:
                this.router.navigate(["/trading-contract/delivery-contract"]);
                break;
            default:
                break;
        }
    }

    setPage() {
        //
        this.isLoading = true;
        this.dataFilter.firstDate = this.dataFilter.firstDate ? (this.dataFilter.firstDate) : moment().subtract(30, 'days');
        this.dataFilter.endDate = this.dataFilter.endDate ? (this.dataFilter.endDate) : moment(new Date());    
        
        this._dashBoardService.getInfoDashBoard(this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.cashInFlowInfo = res.data?.cashInFlow;
                this.cashOutFlowInfo = res.data?.cashOutFlow;
                this.cashRemainInfo = res.data?.cashRemain;
                //
                this.dashboardInfo = res.data;
                console.log('dashboardInfo ', this.dashboardInfo);
                this.cashFlowByDayInfo = [...this.dashboardInfo?.timeFlow];
                this.danhSachTheoKyHanSP = [...this.dashboardInfo?.listPolicy];
                if (this.dashboardInfo?.cashInByPartner) this.doanhSoVaSLBanInfo = [...this.dashboardInfo?.cashInByPartner];
                if (this.dashboardInfo?.cashInByTrading) this.doanhSoVaSLBanInfo = [...this.dashboardInfo?.cashInByTrading];
                this.baoCaoThucChi = [...this.dashboardInfo?.cashOutByMonths];
                this.listAction = this.dashboardInfo.actions.slice(0, 6);
                console.log('!!! listAction ', this.listAction);
                
                this.UpdateCashFlowByDayInfo();
                this.UpdateDanhSachTheoKyHanSP();
                this.UpdatedoanhSoVaSLBan();
                this.UpdateThucChi();
                this.refreshTemplate = false;
                setTimeout(() => {
                    this.refreshTemplate = true;
                }, 0);
                // this.initChart();
            }

        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }
}
