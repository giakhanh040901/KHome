import {Chart, ChartComponent, ChartOptions, ChartType, ChartDataset} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { MessageService, SelectItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
import { options } from 'preact';
import { ProductBondSecondaryServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Router } from '@angular/router';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { DashBoardServiceProxy } from '@shared/service-proxies/dashboard-service';
import * as moment from 'moment';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: []
})
export class HomeComponent extends CrudComponentBase {

    constructor(
        private breadcrumbService: BreadcrumbService,
        injector: Injector,
        private _productBondSecondary: ProductBondSecondaryServiceProxy,
        private _dashBoardService: DashBoardServiceProxy,
        messageService: MessageService,) {
        // this.breadcrumbService.setItems([
        //     {label: 'Dashboard', routerLink: ['/']}
        // ]);
        super(injector, messageService);
        Chart.register(ChartDataLabels);
    } 

    refreshTemplate = true;
    refreshDuChiTemplate = true;

    cashFlowByDayInfo: any;
    cashFlowByDay: any;
    cashFlowByDayOptions: any;
    //
    policy: any;
    policyOptions: any;
    danhSachTheoKyHanSP: any;
    //
    widthReport: any;
    widthPolicy: any;
    widthCashFlow: any;
    reportTradingProvider: any;
    reportTradingProviderOptions: any;

    bondSecondarys: any[] = [];

    //
    report: any;
    //
    duChiInfo: any;
    reportDuChi: any;
    reportDuChiOptions: any;

    subscription: Subscription;

    dataFilter = {
        firstDate: null,
        endDate: null,
        bondSecondaryId: null,
    }

    //
    cashInInfo : any= {};
    upColumnColor: string = '#6C63F0';
    downColumnColor: string = '#ED589D'; 

    //
    reportTradingProviderPlugins: any;
    configReportTradingProvider: any;

    olderX = null;
    olderY = null;

    //
    doanhSoVaSLBanInfo: any;

    //
    maxDate: Date;
    firstDate: Date;
    endDate: Date;
    reportDuChiDate: Date;
    duChiMonth: any;
    duChiYear: any;


    @ViewChild('reportTradingProviderChart') private parentRef: ElementRef<HTMLElement>;


    ngOnInit() { 
        // Select Date
        this.firstDate = new Date();
        this.firstDate.setDate( this.firstDate.getDate() - 7 );
        this.endDate = new Date();
        this.reportDuChiDate = new Date();
        this.duChiMonth = this.reportDuChiDate.getMonth() + 1;
        this.duChiYear = this.reportDuChiDate.getFullYear();

        this.getSecondary();

        // Dòng tiền theo ngày
        this.cashFlowByDay = {
            labels: '',
            datasets: [
                {
                    label: 'Dòng tiền theo ngày (tỉ đồng)',
                    backgroundColor: '#6C63F0',
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    data: [],
                    datalabels: {
                        color: '#200E32',
                        anchor: 'end',
                        align: 'end',
                        offset: 0,
                    },
                    clamp: false,
                },                
            ],
        };
            
        this.cashFlowByDay.datasets[0].data.forEach((value, index) => {
            if(value < 0) {
                this.cashFlowByDay.datasets[0].backgroundColor[index] = this.downColumnColor;
                this.cashFlowByDay.datasets[0].datalabels.anchor[index] = 'start';
                this.cashFlowByDay.datasets[0].datalabels.align[index] = 'start';
                this.cashFlowByDay.datasets[0].data.align[index] = 'start';
            }
        });

        console.log("data fix cung", this.cashFlowByDay);        

        // Danh sách theo kỳ hạn sản phẩm
        this.policy = {
            labels: ['1 tháng', '3 tháng', '6 tháng', '9 tháng', '12 tháng'],

            datasets: [
                {
                    label: 'Danh sách theo kỳ hạn sản phẩm (tỉ đồng)',
                    backgroundColor: this.upColumnColor,
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

        let checkLengthPolicy = this.policy.labels.length;
        if(checkLengthPolicy <= 9){
            this.widthPolicy = "100%";
        }  else {
            this.widthPolicy = "100rem";
        }

        // Báo cáo doanh số và số lượng bán theo từng đại lý
        this.reportTradingProvider = {
            labels: ['Đại lý A', 'Đại lý B', 'Đại lý C', 'Đại lý D', 'Đại lý E', 'Đại lý F', 'Đại lý G', 'Đại lý H', 'Đại lý I', 'Đại lý K'],
            datasets: [
                {
                    label: 'Doanh số (tỉ đồng)',
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
                    label: 'Số lượng sales',
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

        // Báo cáo dự chi
        this.reportDuChi = {
            labels: ['T1','T2','T3','T4','T5','T6','T7','T8','T9','T10','T11','T12'],
            datasets: [
                {
                    label: 'Dự chi (tỉ đồng)',
                    backgroundColor: '#6C63F0',
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    datalabels: {
                        color: '#200E32',
                        anchor: 'end',
                        align: 'end',
                        offset: 0,
                    },
                    clamp: false,
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
            tooltips: {
                callbacks: {
                   label: function(tooltipItem) {
                          return tooltipItem.yLabel;
                   }
                }
            },

            scales: {
                x: {
                    ticks: {
                        // color: '#232154',
                        display: false
                        // padding: 50,
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
                        // padding: 50,
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
                        // display: false
                        // padding: 50,
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
                    max: 200,
                    ticks: {
                        color: '#232154',
                        // display: false
                        // padding: 50,
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
                        // color: '#232154',
                        display: false
                        // padding: 50,
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

        this.setPage();
        this.genDataDuChi(this.duChiMonth, this.duChiYear);
    }

    getSecondary() {
        // Lấy List BondSecondary
        this._productBondSecondary.getAllSecondary().subscribe((res) => {
            this.isLoadingPage = false;
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                if (res?.data) {
                    this.bondSecondarys = res?.data;
                    console.log('bondSecondarys: ',this.bondSecondarys);
                }
            }
        }, (err) => {
            this.isLoadingPage = false;
            this.isLoading = false;
            console.log('Error-------', err);
            
        }); 
    }

    changeBondSecondary(bondSecondaryId) {
        this.dataFilter.bondSecondaryId = bondSecondaryId;
        if (this.firstDate && this.endDate){
            this.setPage();
        }
    }

    changeDateFilter(){
        if (this.firstDate && this.endDate) {
            this.setPage();
        }
    }

    checkLengthCashFlow() {
        let checkLength = this.cashFlowByDay.labels.length;
        if(checkLength <= 35 || screen.width *2/3 > checkLength*30){
            this.widthCashFlow = "100%";
        }  else {
            this.widthCashFlow = `${checkLength*30}px`;
        }
    }

    checkLengthPolicy() {
        let checkLengthPolicy = this.policy.labels.length;
        // Kiếm tra số cột trong đồ thị || (độ dài màn hình / 2) với (số cột * 125)
        if(checkLengthPolicy <= 9 || screen.width/2 > checkLengthPolicy*65 ){
            this.widthPolicy = "100%";
        }  else {
            this.widthPolicy = `${checkLengthPolicy*65}px`;
        }
    }

    checkLengthTrading(){
        let checkLength = this.reportTradingProvider.labels.length;
        // Kiếm tra số cột trong đồ thị || (độ dài màn hình) với (số cột * 115)
        if(checkLength <= 12 || screen.width > checkLength*70){
            this.widthReport = "100%";
        }  else {
            this.widthReport = `${checkLength*70}px`;
        }
    }

    changeDateReport() {
        this.duChiMonth = this.reportDuChiDate.getMonth() + 1;
        this.duChiYear = this.reportDuChiDate.getFullYear();
        this.genDataDuChi(this.duChiMonth, this.duChiYear);
    }


    UpdateCashFlowByDayInfo(){
        this.cashFlowByDay.datasets[0].data = this.cashFlowByDayInfo.map(element => (element.totalValue/ 1000000000).toFixed(1) ?? 0);
        this.cashFlowByDay.labels = this.cashFlowByDayInfo.map(element => this.formatDate(element.date));
        
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
        //                           
        this.refreshTemplate = false;
        setTimeout(() => {
            this.refreshTemplate = true;
        }, 0);
        console.log("dong tien theo ngay", this.cashFlowByDay);
    }

    UpdateDanhSachTheoKyHanSP() {
        this.policy.datasets[0].data = this.danhSachTheoKyHanSP.map(element => element.totalValue);
        // this.policy.datasets[0].data = [...this.policy.datasets[0].data, ...this.policy.datasets[0].data, ...this.policy.datasets[0].data, ...this.policy.datasets[0].data]
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
        // this.policy.labels = [...this.policy.labels, ...this.policy.labels, ...this.policy.labels, ...this.policy.labels]
        console.log("Danh sách theo ký hạn sản phẩm: ", this.policy);
        
    };
    //
    UpdatedoanhSoVaSLBan(){
        this.reportTradingProvider.labels = this.doanhSoVaSLBanInfo.map(element => element.departmentName ?? element.name);
        // this.reportTradingProvider.labels = [...this.reportTradingProvider.labels, ...this.reportTradingProvider.labels, ...this.reportTradingProvider.labels];
        // this.reportTradingProvider.labels = [...this.reportTradingProvider.labels, ...this.reportTradingProvider.labels, ...this.reportTradingProvider.labels];
        this.reportTradingProvider.datasets[0].data = this.doanhSoVaSLBanInfo.map(element => (element.totalValue/ 1000000000).toFixed(2) ?? 0);
        // this.reportTradingProvider.datasets[0].data = [...this.reportTradingProvider.datasets[0].data , ...this.reportTradingProvider.datasets[0].data, ...this.reportTradingProvider.datasets[0].data];
        // this.reportTradingProvider.datasets[0].data = [...this.reportTradingProvider.datasets[0].data , ...this.reportTradingProvider.datasets[0].data, ...this.reportTradingProvider.datasets[0].data];
        this.reportTradingProvider.datasets[1].data = this.doanhSoVaSLBanInfo.map(element => element.countSale);
        // this.reportTradingProvider.datasets[1].data = [...this.reportTradingProvider.datasets[1].data, ...this.reportTradingProvider.datasets[1].data, ...this.reportTradingProvider.datasets[1].data];
        // this.reportTradingProvider.datasets[1].data = [...this.reportTradingProvider.datasets[1].data, ...this.reportTradingProvider.datasets[1].data, ...this.reportTradingProvider.datasets[1].data];
        console.log('labels', this.reportTradingProvider.labels );
        console.log('doanh so', this.reportTradingProvider.datasets[0].data );
        console.log('so sales', this.reportTradingProvider.datasets[1].data );
    }

    updateBaoCaoDuChi(){
        this.reportDuChi.labels = this.duChiInfo.map(element => this.formatDate(element.date));
        this.reportDuChi.datasets[0].data = this.duChiInfo.map(element => (element.duChi/ 1000000000).toFixed(1) ?? 0);
        console.log('this.reportDuChi.labels', this.reportDuChi.labels);
        console.log('this.reportDuChi.datasets[0].data', this.reportDuChi.datasets[0].data);
        this.refreshDuChiTemplate = false;
        setTimeout(() => {
            this.refreshDuChiTemplate = true;
        }, 0);
    }

    genDataDuChi(month, year){
        this.isLoading = true;        
        this._dashBoardService.getDuChiDashBoard(month, year).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.duChiInfo = res.data;
                console.log("!!! data du chi: ", this.duChiInfo);
            }
            this.isLoadingPage = true;
            this.updateBaoCaoDuChi()
            this.isLoadingPage = false;
        })
    }


    setPage(){
        this.isLoading = true;
        this.dataFilter.firstDate = moment( this.firstDate).format('YYYY-MM-DD');
        this.dataFilter.endDate = moment( this.endDate).format('YYYY-MM-DD');

        this._dashBoardService.getInfoDashBoard(this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.cashInInfo = res.data;
                console.log('this.cashInInfo: ', this.cashInInfo);

                this.cashFlowByDayInfo = [...this.cashInInfo?.dongTienTheoNgay];
                this.danhSachTheoKyHanSP = [...this.cashInInfo?.danhSachTheoKyHanSP];
                console.log('danhSachTheoKyHanSP', this.danhSachTheoKyHanSP);
                this.doanhSoVaSLBanInfo = this.cashInInfo?.doanhSoVaSLBanTheoPhongBan ?? this.cashInInfo?.doanhSoVaSLBanTheoDLSC
                
                this.UpdateCashFlowByDayInfo();
                this.UpdateDanhSachTheoKyHanSP();
                this.UpdatedoanhSoVaSLBan();
                // this.initChart();
                this.checkLengthPolicy();
                this.checkLengthTrading();
                this.checkLengthCashFlow();
            }

        }, (err) => {
            this.isLoading = false;
            
            console.log('Error-------', err);
        });
    }
}
