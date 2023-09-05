import { ChangeDetectorRef, Component, Inject, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IDropdown, ProjectOverviewConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ProjectOverviewService } from '@shared/services/project-overview.service';
import { MenuItem, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-project-find',
  templateUrl: './project-find.component.html',
  styleUrls: ['./project-find.component.scss']
})
export class ProjectFindComponent extends CrudComponentBase  {

  constructor(
    private breadcrumbService: BreadcrumbService,
    injector: Injector,
    messageService: MessageService,
    @Inject(API_BASE_URL) baseUrl?: string,
    private projectOverviewService?: ProjectOverviewService,
    private router?: Router,
    private changeDetectorRef?: ChangeDetectorRef,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([{ label: "Trang chủ" }]);
    this.baseUrl = baseUrl || "";
  }
  ownerFilters: IDropdown[] = [];
  ProjectOverviewConst = ProjectOverviewConst;
  filter: {
    keyword: string;
    ownerId: number | undefined;
    projectType: number | undefined;
    productTypes: number[] | undefined;
    status: number | undefined;
  } = {
    keyword: "",
    ownerId: undefined,
    projectType: undefined,
    productTypes: undefined,
    status: undefined,
  };
  rows = [];
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
    this.setPage({ page: this.offset });
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

  ngAfterViewInit() {
    this.projectOverviewService._listOwner$.subscribe((res: any) => {
        if (res) {
            this.ownerFilters = res;
        }
    });

    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  setPage(pageInfo?: any) {
    this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.filter.keyword;
    this.projectOverviewService.getAllProject(this.page, this.filter, this.sortData).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, "")) {
            this.page.totalItems = res.data.totalItems;
            if (res.data?.items) {
                this.rows = res.data.items
                console.log('!!! rows', this.rows);
                
                // this.rows = res.data.items.map(
                // 	(item: any) =>
                // 		({
                // 			id: item.id,
                // 			code: item.code,
                // 			name: item.name,
                // 			productType: item?.productTypes
                // 				? ProjectOverviewConst.getNameProductTypes(
                // 						item?.productTypes
                // 					)
                // 				: "",
                // 			ownerName: item.ownerName,
                // 			createdDate:
                // 				item.createdDate && item.createdDate.length
                // 					? this.formatDate(item.createdDate)
                // 					: "",
                // 			createdBy: item.createdBy,
                // 			status: item.status,
                // 		} as ProjectOverviewModel)
                // );
            }
            if (this.rows?.length) {
                // this.genListAction(this.rows);
            }
        }
    },
    (err) => {
        this.isLoading = false;
    }
);
}
    onProjectBtnClick(id){
    // Navigate to /products page
        this.router.navigate(['/project/' + this.cryptEncode(id)]);
    }

    onClickHome() {
        this.router.navigate(['/home']);
    }

    changeFilter(value) {
		this.setPage({ page: this.offset });
	}

    public get listProductType() {
		return ProjectOverviewConst.productTypes;
	}

    public get productTypes() {
		return ProjectOverviewConst.productTypes;
	}

    public get statusFilters() {
		return ProjectOverviewConst.statusFilters;
	}
}
