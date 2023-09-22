import { Component, Inject, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ProductService } from '@shared/services/product.service';
import { MenuItem, MessageService } from 'primeng/api';
import { TabView } from 'primeng/tabview';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent extends CrudComponentBase {

  constructor(
    private breadcrumbService: BreadcrumbService,
    injector: Injector,
    messageService: MessageService,
    @Inject(API_BASE_URL) baseUrl?: string,
    public productService?: ProductService,
    private _routeActive?: ActivatedRoute,
    private router?: Router,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([{ label: "Trang chủ" }]);
    this.baseUrl = baseUrl || "";
    this.productId = +this.cryptDecode(
        this._routeActive.snapshot.paramMap.get("id")
    );
  }
  loanPrice: number;
  loanRate: number;
  interestRate= 10;
  loanDuration = 35;
  monthlyPay: number;
  ProductConst = ProductConst;
  @ViewChild(TabView) tabView: TabView;
  @ViewChild(TabView) tabViewRecent: TabView;
  public baseUrl: string = "";
  items: MenuItem[] | undefined;
  images: any[] | undefined;
  tabViewActive: {
		product: boolean;
		project: boolean;
	} = {
		product: true,
		project: false
	};
  tabViewRecentActive: {
    recently: boolean;
    favourite: boolean;
  } = {
    recently: true,
    favourite: false
  };
  responsiveOptions: any[] | undefined;
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
  productId: number;
//   productId = 2782;
  productInfo: any = {};

  public order: {
    cartId: number;
    openSellDetailId: number;
    paymentType: number;
    investorIdenId: number;
    contractAddressId: number;
    orderCoOwners: string;
    id: number;
  } = {
    cartId: null,
    openSellDetailId: 0,
    paymentType: 0,
    investorIdenId: 0,
    contractAddressId: null,
    orderCoOwners: null,
    id: 0
  };

  ngOnInit(): void {
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
    this.items = [
      {
          label: 'Mua',
          // icon: 'pi pi-fw pi-file',
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
          // icon: 'pi pi-fw pi-pencil',
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
          // icon: 'pi pi-fw pi-user',
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
          // icon: 'pi pi-fw pi-calendar',
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
          // icon: 'pi pi-fw pi-power-off'
      }
    ];
    this.isLoading = true;
    this.productService.findById(this.productId).subscribe((res) => {
      if (this.handleResponseInterceptor(res)) {
        this.productInfo = res?.data;
        console.log('!!! loanPrice', this.loanPrice);
        console.log('Giá căn hộ', this.productInfo.price);
        this.calculateLoanRateFromLoanPrice(this.loanPrice);
        this.calculateLoanPriceFromLoanRate(this.loanRate);
        this.calculateLoanPriceFromPrice(this.productInfo.price);
        this.calculateLoanDuration(this.loanDuration);
        this.calculateInterestRate(this.interestRate);
      }
    });
    
  }

  calculateLoanRateFromLoanPrice(loanPrice) {
    // let value = name == 'unitPrice' ? +event.value : +event.target.value;
    // let onUnitPrice = Boolean(name == 'unitPrice' && this.productInfo.priceArea);
    // let onPriceArea = Boolean(name == 'priceArea' && this.productInfo.unitPrice);
    // if((onUnitPrice || onPriceArea) && value >= 0) {
    //   this.productInfo.price = value * (onUnitPrice ? this.productInfo.priceArea : this.productInfo.unitPrice);
    // }
    
    console.log("Loan Price", loanPrice);
    let months = this.loanDuration * 12;
    this.loanRate = (loanPrice/ this.productInfo.price) * 100
    if(this.loanRate < 1) {
        this.loanRate = 0
    }
    else if(this.loanRate > 100) {
        this.loanRate = 100
    }
    console.log('Tỷ lệ vay %', this.loanRate);
    let rate = (this.interestRate / 12) / 100
    console.log(rate);
    this.monthlyPay = loanPrice *(rate*(1+rate)**months) / ((1+rate)**months - 1)
    console.log('Hàng tháng', this.monthlyPay)
    
  }

  calculateLoanPriceFromLoanRate(loanRate) {
    // let value = name == 'unitPrice' ? +event.value : +event.target.value;
    // let onUnitPrice = Boolean(name == 'unitPrice' && this.productInfo.priceArea);
    // let onPriceArea = Boolean(name == 'priceArea' && this.productInfo.unitPrice);
    // if((onUnitPrice || onPriceArea) && value >= 0) {
    //   this.productInfo.price = value * (onUnitPrice ? this.productInfo.priceArea : this.productInfo.unitPrice);
    // }
    
    
    this.loanPrice = (loanRate * this.productInfo.price) / 100
    let months = this.loanDuration * 12;
    let rate = (this.interestRate / 12) / 100
    console.log(rate);
    this.monthlyPay = this.loanPrice *(rate*(1+rate)**months) / ((1+rate)**months - 1)
    console.log('Hàng tháng', this.monthlyPay)
  }
  calculateLoanPriceFromPrice(price) {
    // let value = name == 'unitPrice' ? +event.value : +event.target.value;
    // let onUnitPrice = Boolean(name == 'unitPrice' && this.productInfo.priceArea);
    // let onPriceArea = Boolean(name == 'priceArea' && this.productInfo.unitPrice);
    // if((onUnitPrice || onPriceArea) && value >= 0) {
    //   this.productInfo.price = value * (onUnitPrice ? this.productInfo.priceArea : this.productInfo.unitPrice);
    // }
    let months = this.loanDuration * 12;
    this.loanRate = 70
    let rate = (this.interestRate / 12) / 100
    console.log(rate);
    this.loanPrice = (price * this.loanRate) / 100
    this.monthlyPay = this.loanPrice *(rate*(1+rate)**months) / ((1+rate)**months - 1)
    console.log('Hàng tháng', this.monthlyPay)
    
  }
  calculateLoanDuration(loanDuration) {
    // if (loanDuration == 0) {
    //     this.monthlyPay = 0
    // }
    let months = loanDuration * 12;
    let rate = (this.interestRate / 12) / 100
    console.log(rate);
    this.monthlyPay = this.loanPrice *(rate*(1+rate)**months) / ((1+rate)**months - 1)
    console.log('Hàng tháng', this.monthlyPay)
  }
  calculateInterestRate(interestRate) {
    // if (loanDuration == 0) {
    //     this.monthlyPay = 0
    // }
    let months = this.loanDuration * 12;
    let rate = (interestRate / 12) / 100
    console.log(rate);
    this.monthlyPay = this.loanPrice *(rate*(1+rate)**months) / ((1+rate)**months - 1)
    console.log('Hàng tháng', this.monthlyPay)
  }
  getImages() {
    return Promise.resolve(this.getDatas());
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

  changeTab(event: any) {
		let tabHeader = this.tabView.tabs[event.index].header;
		this.tabViewActive[tabHeader] = true;
	}
  changeTabRecent(event: any) {
    let tabHeader = this.tabViewRecent.tabs[event.index].header;
    this.tabViewRecentActive[tabHeader] = true;
  }
  onClickHome() {
    this.router.navigate(['/home']);
  }
  onBtnProductItemClick(id){
    // Navigate to /products page
    this.router.navigate(['/product/' + this.cryptEncode(id)]);
  }

  onOrder(event?: any) {
    this.order.id = this.productInfo.id;
    console.log('vào rồi', this.order);
    this.productService.createOrder(this.order).subscribe((res) => {
      if (this.handleResponseInterceptor(res, "Thêm thành công")) {
        console.log('thêm được');
        this.router.navigate(['/order']);
      }
      else {
        this.submitted = false;
      }
    }) 
	}
}
