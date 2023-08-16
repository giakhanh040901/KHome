import { BondSecondaryService } from './../../shared/services/bond-secondary.service';
import {Chart, ChartComponent} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { MessageService, SelectItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
import { options } from 'preact';
import { CrudComponentBase } from '@shared/crud-component-base';
import { DistributionService } from '@shared/services/distribution.service';
import * as moment from 'moment';
import { DashboardService } from '@shared/services/dashboard-service';


@Component({
  selector: 'app-overview-bond',
  templateUrl: './overview-bond.component.html',
  styleUrls: ['../overview-invest/overview-invest.component.scss']
})
export class OverviewBondComponent extends CrudComponentBase {

  constructor(
      private breadcrumbService: BreadcrumbService,
      injector: Injector,
      private _bondSecondaryService: BondSecondaryService,
      private _dashBoardService: DashboardService,
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
  widthCashFlowByDay = '100%';
  //
  policy: any;
  policyOptions: any;
  danhSachTheoKyHanSP: any;
  //
  widthReport: any;
  widthPolicy: any;
  reportTradingProvider: any;
  reportTradingProviderOptions: any;

  bondSecondarys: any[] = [];

  //
  report: any;
  reportOptions: any;

  subscription: Subscription;

  rangeDates: Date[];

  dataFilter = {
      firstDate: null,
      endDate: null,
      bondSecondaryId: null,
  }

  //
  cashInfo : any= {};
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

  @ViewChild('reportTradingProviderChart') private parentRef: ElementRef<HTMLElement>;


  ngOnInit() { 

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
                      padding: 8,
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
                  max: 100,
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
  }

  getSecondary() {
      // Lấy List BondSecondary
      this._bondSecondaryService.getAllSecondary().subscribe((res) => {
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
      if (this.dataFilter.firstDate != null && this.dataFilter.endDate){
          this.setPage();
      }
  }

  SelectDays() {
      this.maxDate = new Date(this.rangeDates[0]);
      this.maxDate.setDate( this.maxDate.getDate() + 30 );
      if (this.rangeDates[1] != null){
          this.dataFilter.firstDate = this.rangeDates[0];
          this.dataFilter.endDate = this.rangeDates[1];
          this.setPage()
      };
  }

  UpdateCashFlowByDayInfo(){
      this.cashFlowByDay.datasets[0].data = this.cashFlowByDayInfo.map(element =>  {
        let value = '0';
            if(element.totalValue) {
                value = (element.totalValue/ 1000000000).toFixed(1);
            }

            return value != '0.0' ? value : 0;
      });
      //
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

      // Update chiều rộng biểu đồ theo số phần tử hiển thị, mỗi cột ứng với 22px
      this.widthCashFlowByDay = (this.cashFlowByDay?.datasets[0]?.data?.length*22 > this.screenWidth) ? (this.cashFlowByDay?.datasets[0]?.data?.length*22) + 'px' : '100%';
      //                           
      this.refreshTemplate = false;
      setTimeout(() => {
          this.refreshTemplate = true;
      }, 0);
      console.log("dong tien theo ngay", this.cashFlowByDay);
  }

  UpdateDanhSachTheoKyHanSP() {
      this.policy.datasets[0].data = this.danhSachTheoKyHanSP.map(element => (element.totalValue / 1000000000).toFixed(1));
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
  //
  UpdatedoanhSoVaSLBan(){
      this.reportTradingProvider.labels = this.doanhSoVaSLBanInfo.map(element => element.departmentName ?? element.name);
      this.reportTradingProvider.datasets[0].data = this.doanhSoVaSLBanInfo.map(element => (element.totalValue/ 1000000000).toFixed(1) ?? 0);
      this.reportTradingProvider.datasets[1].data = this.doanhSoVaSLBanInfo.map(element => element.countSale);
      console.log('labels', this.reportTradingProvider.labels );
      console.log('doanh so', this.reportTradingProvider.datasets[0].data );
      console.log('so sales', this.reportTradingProvider.datasets[1].data );

  }

  setPage(){
      this.isLoading = true;
      this.dataFilter.firstDate = moment( this.dataFilter.firstDate).format('YYYY-MM-DD');
      this.dataFilter.endDate = moment( this.dataFilter.endDate).format('YYYY-MM-DD');
      this.reportTradingProviderOptions.scales.x.min = 0;
      this.reportTradingProviderOptions.scales.x.max = 9;
      this._dashBoardService.getInfoDashboardBond(this.dataFilter).subscribe((res) => {
          this.isLoading = false;
          if (this.handleResponseInterceptor(res, '')) {
              this.page.totalItems = res.data.totalItems;
              this.cashInfo = res.data;
              this.cashFlowByDayInfo = [...this.cashInfo?.dongTienTheoNgay];
              this.danhSachTheoKyHanSP = [...this.cashInfo?.danhSachTheoKyHanSP];
              console.log('danhSachTheoKyHanSP', this.danhSachTheoKyHanSP);
              this.doanhSoVaSLBanInfo = this.cashInfo?.doanhSoVaSLBanTheoPhongBan ?? this.cashInfo?.doanhSoVaSLBanTheoDLSC
              
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
