import {Chart, ChartComponent, ChartOptions, ChartType, ChartDataset} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { MessageService, SelectItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
import { options } from 'preact';
import { CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
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
        private _companyShareSecondary: CompanyShareSecondaryServiceProxy,
        private _dashBoardService: DashBoardServiceProxy,
        messageService: MessageService,) {
        // this.breadcrumbService.setItems([
        //     {label: 'Dashboard', routerLink: ['/']}
        // ]);
        super(injector, messageService);
        Chart.register(ChartDataLabels);
    } 

    refreshTemplate = true;

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

    companyShareSecondarys: any[] = [];

    //
    report: any;
    reportOptions: any;

    subscription: Subscription;

    rangeDates: Date[];

    dataFilter = {
        firstDate: null,
        endDate: null,
        companyShareSecondaryId: null,
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

    @ViewChild('reportTradingProviderChart') private parentRef: ElementRef<HTMLElement>;


    ngOnInit() { 

        // Dòng tiền theo ngày
        this.cashFlowByDay = {
            labels: ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
            datasets: [
                {
                    label: 'Dòng tiền theo ngày (tỉ đồng)',
                    backgroundColor: ['#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0','#6C63F0',],
                    borderColor: '#FFFFFF',
                    borderRadius: 4,
                    barThickness: 12,   // Độ rộng của cột #ED589D
                    borderSkipped: false,
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    datalabels: {
                        color: '#200E32',
                        anchor: ['end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end'],
                        align: ['end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end','end'],
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

        // Báo cáo dự chi
        this.report = {
            labels: ['T1','T2','T3','T4','T5','T6','T7','T8','T9','T10','T11','T12'],
            datasets: [
                {
                    label: 'My First dataset',
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
            layout: {
                padding: {
                    right: 18
                }
            },
            scales: {
                x: {
                    min: 0,
                    max: 9,
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


        //moveChart plugin block
        let self = this;

        const moveChart = {
            id: 'moveChart',
            afterEvent(chart, args) {
                const { ctx, canvas , chartArea: {left, right, top , bottom, width, height} } = chart;
                canvas.addEventListener('mousemove', (event) => {                    
                    // console.log('mousemove', event);
                    const x = args.event.x;
                    const y = args.event.y;
                    // console.log('mousemove: ' + x + '-' + y);
                    
                    if(x >=  left - 15 && x <=  left + 15 && y >= height / 2  + top - 15 
                        && y <= height / 2  + top + 15){
                        canvas.style.cursor = 'pointer';
                    } else if (x >=  right - 15 && x <=  right + 15 && y >= height / 2  + top - 15 
                    && y <= height / 2  + top + 15){
                        canvas.style.cursor = 'pointer';
                    }                    
                    else {
                        canvas.style.cursor = 'default';
                    }
                }),
                canvas.addEventListener('click' , (event) => {
                    // console.log('click', event);
                    const x = event.layerX;
                    const y = event.layerY;

                    // console.log('click: ' + x + '-' + y);
                    if(x >=  left - 15 && x <=  left + 15 && y >= height / 2  + top - 15 && y <= height / 2  + top + 15 && x != self.olderX && y != self.olderY){
                        // console.log("Trái");                        
                        self.nextChart(-10);
                    } else if (x >=  right - 15 && x <=  right + 15 && y >= height / 2  + top - 15 && y <= height / 2  + top + 15 && x != self.olderX && y != self.olderY){
                        // console.log("Phải");
                        self.nextChart(10);
                    }                    
                    else {
                        // console.log("Click trượt");
                    }
                    self.olderX = x;
                    self.olderY = y;
                })
            },

            afterDraw(chart, args, pluginOptions){
                // console.log("chart chart chart", chart);
                const { ctx , chartArea: {left, right, top , bottom, width, height} } = chart;
                
                class CircleChevron {
                    // constructor (x1, y1) {

                    // }
                    draw(ctx, x1, pixel, min ,max){
                        const angle = Math.PI / 180;
                        ctx.beginPath();
                        ctx.lineWidth = 3;
                        ctx.strokeStyle = 'rgba(102, 102, 102, 0.5)';
                        ctx.fillStyle = 'white';
                        ctx.arc(x1, height / 2  + top , 15 , angle * 0, angle * 360, false);
                        ctx.stroke();
                        ctx.fill();
                        ctx.closePath();
        
                        // Arrow left
                        ctx.beginPath();
                        ctx.lineWidth = 3;
                        if (min == 0){
                            ctx.strokeStyle = 'rgba(255, 26, 104, 1)';
                        }else{
                            ctx.strokeStyle = 'rgba(255, 26, 104, 1)';
                        }
                        ctx.moveTo(x1 + pixel, height / 2  + top - 7.5);
                        ctx.lineTo(x1 - pixel, height / 2  + top);
                        ctx.lineTo(x1 + pixel, height / 2  + top + 7.5);
                        ctx.stroke();
                        ctx.closePath();
                    }
                }

                let drawCircleLeft = new CircleChevron();
                drawCircleLeft.draw(ctx, left, 5, self.reportTradingProviderOptions.scales.x.min, self.reportTradingProviderOptions.scales.x.max);

                let drawCircleRight = new CircleChevron();
                drawCircleRight.draw(ctx, right, -5, self.reportTradingProviderOptions.scales.x.min, self.reportTradingProviderOptions.scales.x.max);
            }

        }  

        this.reportTradingProviderPlugins = [moveChart];
    }

    getSecondary(){
        // Lấy List CompanyShareSecondary
        this._companyShareSecondary.getAllSecondary().subscribe((res) => {
            this.isLoadingPage = false;
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                if (res?.data) {
                    this.companyShareSecondarys = res?.data;
                    console.log('companyShareSecondarys: ',this.companyShareSecondarys);
                }
            }
        }, (err) => {
            this.isLoadingPage = false;
            this.isLoading = false;
            console.log('Error-------', err);
            
        }); 
    }

    //
    nextChart(valueNextPosition){
        let minNew = this.reportTradingProviderOptions.scales.x.min + valueNextPosition;
        let maxNew = this.reportTradingProviderOptions.scales.x.max + valueNextPosition;
        console.log('minNew', minNew);
        console.log('maxNew', maxNew);
        
        if (maxNew >= this.reportTradingProvider.labels.length){
            this.reportTradingProviderOptions.scales.x.min = this.reportTradingProvider.labels.length -10;
            this.reportTradingProviderOptions.scales.x.max = this.reportTradingProvider.labels.length -1;    

        } else if (minNew < 0){
            this.reportTradingProviderOptions.scales.x.min = 0;
            this.reportTradingProviderOptions.scales.x.max = 9;
 
        } else {
            this.reportTradingProviderOptions.scales.x.min = minNew;
            this.reportTradingProviderOptions.scales.x.max = maxNew;    

        }
        this.refreshTemplate = false;
        setTimeout(() => {
            this.refreshTemplate = true;
            }, 0);
        // this.initChart();
    }

    changeCompanyShareSecondary(companyShareSecondaryId) {
        this.dataFilter.companyShareSecondaryId = companyShareSecondaryId;
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

    UpdateCashFlowByDayInfo(){
        this.cashFlowByDay.datasets[0].data = this.cashFlowByDayInfo.map(element => (element.totalValue/ 100000000000).toFixed(2) ?? 0);
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
        this.refreshTemplate = false;
        setTimeout(() => {
            this.refreshTemplate = true;
        }, 0);
        console.log("dong tien theo ngay", this.cashFlowByDay);
    }

    UpdateDanhSachTheoKyHanSP(){
        this.policy.datasets[0].data = this.danhSachTheoKyHanSP.map(element => element.totalValue);
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
        console.log("Danh sách theo ký hạn sản phẩm: ", this.policy);
        
    };
    
    UpdatedoanhSoVaSLBan(){
        this.reportTradingProvider.labels = this.doanhSoVaSLBanInfo.map(element => element.departmentName ?? element.name);
        this.reportTradingProvider.datasets[0].data = this.doanhSoVaSLBanInfo.map(element => (element.totalValue/ 100000000000).toFixed(2) ?? 0);
        this.reportTradingProvider.datasets[1].data = this.doanhSoVaSLBanInfo.map(element => element.countSale);
        console.log('labels', this.reportTradingProvider.labels );
        console.log('doanh so', this.reportTradingProvider.datasets[0].data );
        console.log('so sales', this.reportTradingProvider.datasets[1].data );

    }

    // initChart(){
    //     const parentElement = this.parentRef.nativeElement;
    //     const pChartTag = parentElement.querySelector('p-chart');
    //     const pChartTagChild = pChartTag.querySelector('div');
    //     const canvasTag = pChartTagChild.querySelector('canvas');

    //     console.log("canvasTag", canvasTag);
        
    //     this.configReportTradingProvider = {
    //         type: 'bar',
    //         data: this.reportTradingProvider,
    //         options: this.reportTradingProviderOptions,
    //         plugins: this.reportTradingProviderPlugins
    //     }

    //     // canvasTag.destroi
    //     const tradingOrDepartment = new Chart(
    //         canvasTag,
    //         this.configReportTradingProvider
    //     ) 
    //     tradingOrDepartment.update();
    //     // tradingOrDepartment.ctx.onclick
    //     console.log("Day la tradingOrDepartment: ", tradingOrDepartment);
    // }
    
    setPage(){
        this.isLoading = true;
        this.dataFilter.firstDate = moment( this.dataFilter.firstDate).format('YYYY-MM-DD');
        this.dataFilter.endDate = moment( this.dataFilter.endDate).format('YYYY-MM-DD');
        this.reportTradingProviderOptions.scales.x.min = 0;
        this.reportTradingProviderOptions.scales.x.max = 9;
        this._dashBoardService.getInfoDashBoard(this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.cashInInfo = res.data;
                this.cashFlowByDayInfo = [...this.cashInInfo?.dongTienTheoNgay];
                this.danhSachTheoKyHanSP = [...this.cashInInfo?.danhSachTheoKyHanSP];
                console.log('danhSachTheoKyHanSP', this.danhSachTheoKyHanSP);
                this.doanhSoVaSLBanInfo = this.cashInInfo?.doanhSoVaSLBanTheoPhongBan ?? this.cashInInfo?.doanhSoVaSLBanTheoDLSC
                
                this.UpdateCashFlowByDayInfo();
                this.UpdateDanhSachTheoKyHanSP();
                this.UpdatedoanhSoVaSLBan();
                // this.initChart();
            }

        }, (err) => {
            this.isLoading = false;
            
            console.log('Error-------', err);
        });
    }
}
