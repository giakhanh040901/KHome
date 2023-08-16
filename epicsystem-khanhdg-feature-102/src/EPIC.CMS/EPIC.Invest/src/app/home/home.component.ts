import {Chart} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Component, ElementRef, Inject, Injector, ViewChild } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
import { CrudComponentBase } from '@shared/crud-component-base';
import { DashBoardServiceProxy } from '@shared/services/dashboard-service';
import { DistributionService } from '@shared/services/distribution.service';
import * as moment from 'moment';
import { AppConsts, DashboardConst, UserTypes } from '@shared/AppConsts';
import { ProjectServiceProxy } from '@shared/services/project-manager-service';
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
        private breadcrumbService: BreadcrumbService,
        private _distributionService: DistributionService,
        private _projectService: ProjectServiceProxy,
        private _dashBoardService: DashBoardServiceProxy,
		private router: Router,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.userLogin = this.getUser();
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
        Chart.register(ChartDataLabels);
    } 
    userLogin: any = {};
    private baseUrl: string;

    refreshTemplate = true;
    refreshDuChiTemplate = true;
    refreshReportTradingProviderChart = true; 
    //
    distributions : any;
    widthReport:any;
    widthPolicy:any;
    widthCashFlow:any;

    DashboardConst = DashboardConst;

    cashFlowByDayInfo: any;
    cashFlowByDay: any;
    cashFlowByDayOptions: any;
    //
    policy: any;
    policyOptions: any;
    danhSachTheoKyHanSP: any;

    //
    reportTradingProvider: any;
    reportTradingProviderOptions: any;

    //
    report: any;
    //
    duChiInfo: any;
    reportDuChi: any;
    reportDuChiOptions: any;

    //
    reportThucChi: any;
    reportThucChiOptions: any;
    baoCaoThucChi: any;

    subscription: Subscription;

    rangeDates: Date[];

    dataFilter = {
        firstDate: null,
        endDate: null,
        distributionId: null,
        tradingProviderId: null,
    }

    //
    cashInInfo : any= {};
    positiveValueColor: string = '#6C63F0';
    negativeValueColor: string = '#ED589D'; 

    //
    reportTradingProviderPlugins: any;
    configReportTradingProvider: any;

    olderX = null;
    olderY = null;

    //
    doanhSoVaSLBanInfo: any;

    //
    listProduct: any;
    listTrading: any;

    // danh sách hoạt động gần nhất
    listAction:[] = [];
    isPartner: boolean = false;

    @ViewChild('reportTradingProviderChart') private parentRef: ElementRef<HTMLElement>;

    convertPriceDisplay(value: number, decimal?: number) {
        let negativeValue: boolean = false || value < 0, unit: number, unitSymbol: string;
        //
        if(value === 0 || (value > -1000 && value < 1000)) return 0;
        if(value < 0) value = -1*value;
        //
        if(value >= 1000 && value < 1000000) { unit = 1000; unitSymbol = 'K'; } // Đơn vị nghìn
        if(value >= 1000000 && value < 1000000000) { unit = 1000000; unitSymbol = 'M'; } // Đơn vị triệu
        if(value >= 1000000000 && value < 1000000000000) { unit = 1000000000; unitSymbol = 'G'; } // Đơn vị tỷ
        if(value >= 1000000000000) { unit = 1000000000000; unitSymbol = 'T'; } // Đơn vị nghìn tỷ
        //
        let priceConvert: number = (value/unit);
        let priceDisplay: string = ((decimal && !Number.isInteger(priceConvert)) ? (priceConvert.toFixed(decimal)) : Math.floor(priceConvert)) + unitSymbol;
        //
        if(negativeValue) priceDisplay = '-' + priceDisplay;
        //
        return priceDisplay;
    }

    ngOnInit() {
        //
        this.isPartner = UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type);
        this.initDate();
        //
        if (this.isPartner) this.getListTrading();
        this.getListProducts();
       
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
        //
        this.getDistribution();

        // Dòng tiền theo ngày
        this.cashFlowByDay = {
            labels: '',
            datasets: [
                {
                    label: 'Dòng tiền',
                    backgroundColor: this.positiveValueColor,
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    // borderWidth: 10,
                    padding: 20, 
                    data: [],
                    datalabels: {
                        display: true,
                        formatter: (value, chart_obj) => {
                            return this.convertPriceDisplay(value);
                          },
                          font: {
                            size: 9,
                            style: 'italic',
                        },
                    },
                    clamp: false,
                },
            ],
        };

        this.cashFlowByDay.datasets[0].data.forEach((value, index) => {
            if(value < 0) {
                this.cashFlowByDay.datasets[0].backgroundColor[index] = '#ED589D';
                this.cashFlowByDay.datasets[0].datalabels.anchor[index] 
                = this.cashFlowByDay.datasets[0].datalabels.align[index] 
                = this.cashFlowByDay.datasets[0].data.align[index]
                = 'start';
            }
        });

        this.cashFlowByDayOptions = {
            responsive: true,
            plugins: {
                legend: {
                    display: true, 
                }
            },
            hoverRadius: 30,
            scales: {
                x: {
                    display: true,
                    ticks: {
                        color: '#232154',
                        font: {
                            size: 9,
                            style: 'italic',
                        },
                    },
                    grid: {
                        color: 'white',
                        padding: 50,
                        beginAtZero: true,
                        // min: 0,
                        // max: 200,
                    }
                },
                y: {
                    display: true,
                    ticks: {
                        color: '#99A2BC',
                        padding: 12,
                        callback: (value, index, ticks) => {
                            return this.convertPriceDisplay(value, 1);
                        }
                    },
                    grid: {
                        color: '#ebedef',
                        borderDash: [8, 4],
                        drawBorder: true,
                        beginAtZero: false,
                    }
                },
            },
        };

        // Danh sách theo kỳ hạn sản phẩm
        this.policy = {
            labels: ['1 tháng', '3 tháng', '6 tháng', '9 tháng', '12 tháng'],
            datasets: [
                {
                    label: 'Tiền theo kỳ hạn sản phẩm',
                    backgroundColor: this.positiveValueColor,
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0],
                    datalabels: {
                        display: false
                    },
                },
            ]
        };
        //
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

        let checkLength = this.reportTradingProvider.labels.length;
        if(checkLength <= 10){
            this.widthReport = "100%";
        }  else {
            this.widthReport = "150rem";
        }

        // Báo cáo thực chi
        this.reportThucChi = {
            labels: ['T1','T2','T3','T4','T5','T6','T7','T8','T9','T10','T11','T12'],
            datasets: [
                {
                    label: 'Số tiệu thực chi',
                    backgroundColor: '#6C63F0',
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    datalabels: {
                        display: false
                    }
                },
            ]
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
                        // padding: 50,
                        beginAtZero: true,
                        min: 0,
                        max: 200,
                    }
                },
                y: {
                    display: true,
                    ticks: {
                        color: '#99A2BC',
                        // padding: 50,
                        callback: (value, index, ticks) => {
                            return this.convertPriceDisplay(value, 1);
                        }
                    },
                    grid: {
                        color: '#ebedef',
                        // padding: 50,
                        borderDash: [8, 4],
                        drawBorder: false,
                    }
                },
            }
        };

        this.reportDuChiOptions = {
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
                        // padding: 50,
                        beginAtZero: true,
                        min: 0,
                        max: 200,
                    }
                },
                y: {
                    display: true,
                    ticks: {
                        color: '#99A2BC',
                        // padding: 50,
                        callback: (value, index, ticks) => {
                            return this.convertPriceDisplay(value, 1);
                        }
                    },
                    grid: {
                        color: '#ebedef',
                        // padding: 50,
                        borderDash: [8, 4],
                        drawBorder: false,
                    }
                },
            }
        }

        this.reportTradingProviderOptions = {
            responsive: true,
            plugins: {
                legend: {
                  display: false
                }
            },
            layout: {
                padding: {
                    right: 18,
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
                        // padding: 50,
                        beginAtZero: true,
                        min: 0,
                        max: 200,
                    }
                },
                y: {
                    display: true,
                    ticks: {
                        color: '#99A2BC',
                        // padding: 50,
                        callback: (value, index, ticks) => {
                            return this.convertPriceDisplay(value, 1);
                        }
                    },
                    grid: {
                        color: '#ebedef',
                        // padding: 50,
                        borderDash: [8, 4],
                        drawBorder: false,
                    }
                },
            }
        };

        this.reportThucChiOptions = {
            responsive: true,
            plugins: {
                legend: {
                  display: true,
                },
            },
            scales: {
                x: {
                    ticks: {
                        color: '#232154',
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
                        callback: (value, index, ticks) => {
                            return this.convertPriceDisplay(value, 1);
                        }
                    },
                    grid: {
                        color: '#ebedef',
                        borderDash: [8, 4],
                        drawBorder: false,
                    }
                },
            },
        }
    }

    initDate() {
        const today = moment();
        const thirtyDaysAgo = moment().subtract(30, 'days');
        this.rangeDates = [thirtyDaysAgo.toDate(), today.toDate()];
        this.SelectDays();
    }

    SelectDays() {
        if (this.rangeDates[1] != null){
            this.dataFilter.firstDate = this.rangeDates[0];
            this.dataFilter.endDate = this.rangeDates[1];
            this.setPage()
        };
    }

    checkLengthCashFlow() {
        let checkLength = this.cashFlowByDay.labels.length;
        if(checkLength <= 15){
            this.widthCashFlow = "100%";
        }  else {
            this.widthCashFlow = `${checkLength*3}rem`;
        }
    }

    checkLengthPolicy() {
        let checkLengthPolicy = this.policy.labels.length;
        if(checkLengthPolicy <= 10 ){
            this.widthPolicy = "100%";
        }  else {
            this.widthPolicy = `${checkLengthPolicy*6}rem`;
        }
    }

    checkLengthTrading(){
        let checkLength = this.reportTradingProvider.labels.length;
        if (UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)){
            if(checkLength < 10){
                this.widthReport = "100%";
            } else {
                this.widthReport = `${checkLength*11}rem`;
            }
        }else {
            if(checkLength < 14){
                this.widthReport = "100%";
            }  else {
                this.widthReport = `${checkLength*6}rem`;
            }
        }
    }

    changeTrading(tradingProviderId) {
        this.getListProducts(tradingProviderId);
        this.setPage();
    }

    changeProduct(productId) {
        if (this.dataFilter.firstDate != null && this.dataFilter.endDate){
            this.setPage();
        }
    }

    getDistribution() {
        this._distributionService.getDistributionsOrder().subscribe((res) => {
            if (this.handleResponseInterceptor(res, '')) {
                if (res?.data) {
                    this.distributions = res?.data;
                    console.log('distributions: ',this.distributions);
                }
            }
        })
    }

    changeDistribute(distributionId) {
        this.dataFilter.distributionId = distributionId;
        if (this.dataFilter.firstDate && this.dataFilter.endDate){
            this.setPage();
        }
    }

    getListProducts(tradingProviderId?:number){
        this._dashBoardService.getProduct(tradingProviderId).subscribe((res) => {
            if (this.handleResponseInterceptor(res, '')) {
                this.listProduct = res?.data;
            }
        })
    }

    getListTrading() {
        this._projectService.getAllTradingProvider().subscribe((res) => {
            if (this.handleResponseInterceptor(res, '')) {
                this.listTrading = res?.data;
                console.log('list Trading ', this.listTrading);
            }
        })
    }

    changeDateFilter(){
        if (this.dataFilter?.firstDate && this.dataFilter?.endDate) {
            this.setPage();
        }
    }

    UpdateCashFlowByDayInfo() {
        this.cashFlowByDay.datasets[0].data = this.cashFlowByDayInfo.map(element => element.totalValue ?? 0);
        this.cashFlowByDay.labels = this.cashFlowByDayInfo.map(element => this.formatDateMonth(element.date));
        
        this.cashFlowByDay.datasets[0].backgroundColor = [];
        this.cashFlowByDay.datasets[0].datalabels.anchor = [];
        this.cashFlowByDay.datasets[0].datalabels.align = [];

        this.cashFlowByDay.datasets[0].data.forEach((value, index) => {
            this.cashFlowByDay.datasets[0].backgroundColor[index] = value >= 0 ? this.positiveValueColor : this.negativeValueColor;
            this.cashFlowByDay.datasets[0].datalabels.anchor[index] = this.cashFlowByDay.datasets[0].datalabels.align[index]= value >= 0 ? 'end' : 'start';
        });                                 
        this.refreshTemplate = false;
        this.checkLengthCashFlow();
        
        setTimeout(() => this.refreshTemplate = true);
    }

    UpdateDanhSachTheoKyHanSP() {
        this.policy.datasets[0].data = this.danhSachTheoKyHanSP.map(element => element.totalValue);
        // this.policy.datasets[0].data = [...this.policy.datasets[0].data, ...this.policy.datasets[0].data]
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
            return ""
        });
        this.checkLengthPolicy();
        // this.policy.labels = [...this.policy.labels, ...this.policy.labels]
        console.log("Danh sách theo ký hạn sản phẩm: ", this.policy);
        
    };

    UpdatedoanhSoVaSLBan(){
        this.reportTradingProvider.labels = this.doanhSoVaSLBanInfo.map(element => element.shortName ? element.shortName : element.name);
        this.reportTradingProvider.datasets[0].data = this.doanhSoVaSLBanInfo.map(element => element.amount ?? 0 ); 
        this.reportTradingProvider.datasets[1].data = this.doanhSoVaSLBanInfo.map(element => element.remain ?? 0);
        this.checkLengthTrading();
    }

    updateBaoCaoDuChi() {
        this.reportDuChi.labels = this.duChiInfo?.map(element => this.formatDate(element?.date));
        this.reportDuChi.datasets[0].data = this.duChiInfo?.map(element => (element?.duChi/ 1000000000));
        // this.reportDuChi.datasets[0].data = [...this.reportDuChi.datasets[0].data, Math.max(...this.reportDuChi.datasets[0].data)*2]   // sau nay fix sau vay....
        this.reportDuChi.labels = [...this.reportDuChi?.labels, "Label Test"]
        this.reportDuChi.datasets[0].backgroundColor = [];
        
        this.reportDuChi.datasets[0].data.forEach((value, index) => {
            this.reportDuChi.datasets[0].backgroundColor[index] = this.positiveValueColor;
            if(this.reportDuChi.labels[index] == "Label Test") {
                this.reportDuChi.datasets[0].backgroundColor[index]  = "#fff"    
            }                        
            });
        this.reportDuChi.datasets[0].data = this.reportDuChi.datasets[0].data.map(element => element.toFixed(1) ?? 0);
        this.refreshDuChiTemplate = false;
        setTimeout(() => this.refreshDuChiTemplate = true);
    }

    UpdateThucChi() {
        this.reportThucChi.datasets[0].data = this.baoCaoThucChi?.map(element => element.amount ?? 0);
        this.reportThucChi.label = this.baoCaoThucChi?.map(element => `Tháng ${element.month}`);
    }

    checkNavi(item) {
        if (UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)){
            switch (item.action) {
                case this.DashboardConst.DAT_LENH_MOI:
                    this.router.navigate(["/trading-contract/order"]);
                    break;
                case this.DashboardConst.TAI_TUC:
                    this.router.navigate(["/trading-contract/expire-contract"]);
                    break;
                case this.DashboardConst.RUT_TIEN:
                    this.router.navigate(["/trading-contract/manager-withdraw"]);
                    break;
                case this.DashboardConst.NHAN_HOP_DONG:
                    this.router.navigate(["/trading-contract/delivery-contract"]);
                    break;
                default:
                    break;
            }
        }else {
            this.messageError('Tài khoản của bạn không được cấp quyền xem thông tin')
        }
    }

    setPage() {
        this.isLoading = true; 
        this._dashBoardService.getInfoDashBoard(this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.cashInInfo = res.data;
                this.cashFlowByDayInfo = [...this.cashInInfo?.dongTienTheoNgay];
                this.danhSachTheoKyHanSP = [...this.cashInInfo?.danhSachTheoKyHanSP];
                if (this.cashInInfo?.dashboardCashInDepartment){
                    this.doanhSoVaSLBanInfo = [...this.cashInInfo?.dashboardCashInDepartment];
                }
                if (this.cashInInfo?.dashboardCashTradingByPartner){
                    this.doanhSoVaSLBanInfo = [...this.cashInInfo?.dashboardCashTradingByPartner];
                }
                // this.doanhSoVaSLBanInfo = this.cashInInfo?.doanhSoVaSLBanTheoPhongBan ?? this.cashInInfo?.doanhSoVaSLBanTheoDLSC
                this.baoCaoThucChi = [...this.cashInInfo?.cashOutByMonthInYear];
                console.log('doanhSoVaSLBanInfo', this.doanhSoVaSLBanInfo);
                this.listAction = this.cashInInfo?.actions?.slice(0, 6);
                
                this.UpdateCashFlowByDayInfo();
                this.UpdateDanhSachTheoKyHanSP();
                this.UpdatedoanhSoVaSLBan();
                this.UpdateThucChi();
                // this.initChart();
            }

        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }
}
