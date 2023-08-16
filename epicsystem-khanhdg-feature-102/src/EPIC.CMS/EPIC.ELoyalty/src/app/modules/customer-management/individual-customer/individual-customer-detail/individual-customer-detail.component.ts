import { Component, Injector } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ITabView } from '@shared/interface/InterfaceConst.interface';
import { IndividualCustomerService } from '@shared/services/individual-customer-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { IndividualCustomerDetailHistoryComponent } from './individual-customer-detail-history/individual-customer-detail-history.component';
import { IndividualCustomerDetailOfferComponent } from './individual-customer-detail-offer/individual-customer-detail-offer.component';
import { IndividualCustomerDetailOverviewComponent } from './individual-customer-detail-overview/individual-customer-detail-overview.component';

@Component({
  selector: 'individual-customer-detail',
  templateUrl: './individual-customer-detail.component.html',
  styleUrls: ['./individual-customer-detail.component.scss'],
})
export class IndividualCustomerDetailComponent extends CrudComponentBase {
  public isEdit: boolean = false; // => view
  public listTabPanel: ITabView[] = [];

  constructor(
    injector: Injector,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private routeActive: ActivatedRoute,
    private individualCustomerService: IndividualCustomerService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý khách hàng', routerLink: ['/home'] },
      { label: 'Khách hàng cá nhân', routerLink: ['/customer-management/individual-customer'] },
      { label: 'Chi tiết khách hàng' },
    ]);
    this.isEdit = !!this.router.url.includes('edit');
    this.individualCustomerService.individualCustomerId = +this.cryptDecode(
      this.routeActive.snapshot.paramMap.get('id')
    );
  }
  isShowTab: boolean = false;

  ngOnInit() {
    this.callGenTabView();
  }

  callGenTabView() {
    // Kiểm tra nếu listTabPanel đã có dữ liệu thì dừng đệ quy
    if (this.listTabPanel.length > 0) {
      return;
    }
  
    // Tạo một biến đếm và hằng số thời gian cho đệ quy
    let retryCount = 0;
    const maxRetries = 30; // 3 giây: 100ms * 30
  
    const recursiveCall = () => {
      // Gọi hàm genTabView sau mỗi khoảng thời gian 100ms
      const timeoutId = setTimeout(() => {
        this.genTabView();
  
        // Kiểm tra nếu listTabPanel đã có dữ liệu thì dừng đệ quy và xóa timeout
        if (this.listTabPanel.length > 0) {
          clearTimeout(timeoutId);
        } else {
          // Nếu chưa có dữ liệu, tăng biến đếm lên và kiểm tra điều kiện đệ quy
          retryCount++;
          if (retryCount < maxRetries) {
            recursiveCall();
          } 
        }
      }, 100);
    };
  
    // Gọi hàm đệ quy lần đầu tiên
    recursiveCall();
  }

  genTabView(){
      if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ThongTinChung])){
        this.listTabPanel.push({
          key: 'overview',
          title: 'Thông tin chung',
          component: IndividualCustomerDetailOverviewComponent,
          isDisabled: false,
        })
      }
      if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_UuDai])){
        this.listTabPanel.push({
          key: 'offer',
          title: 'Danh sách ưu đãi',
          component: IndividualCustomerDetailOfferComponent,
          isDisabled: false,
        })
      }
      if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_SuKienThamGia])){
        this.listTabPanel.push({
          key: 'event',
          title: 'Sự kiện tham gia',
          component: null,
          isDisabled: false,
        })
      }
      if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_LichSuThamGia])){
        this.listTabPanel.push({
          key: 'history',
          title: 'Lịch sử tham gia',
          component: IndividualCustomerDetailHistoryComponent,
          isDisabled: false,
        })
      }
      this.isShowTab = true;
  }
}
