import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CreateOrEditPrizeDraw } from '@shared/interface/prize-draw-management/PrizeDrawManagement.model';
import { HelpersService } from '@shared/services/helpers.service';
import { MessageService } from 'primeng/api';
import { TabView } from 'primeng/tabview';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
	selector: 'app-detail-prize-draw',
	templateUrl: './detail-prize-draw.component.html',
	styleUrls: ['./detail-prize-draw.component.scss'],
})
export class DetailPrizeDrawComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _helpersService: HelpersService,
		private breadcrumbService: BreadcrumbService,
		private activatedRouter: ActivatedRoute,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home']},
			{ label: "Danh sách chương trình", routerLink: ['/prize-draw-management']},
			{ label: "Chi tiết chương trình"},
		]);
		this.prizeDraw.id = +(this.cryptDecode(this.activatedRouter.snapshot.paramMap.get('id')));
	}

	prizeDraw: CreateOrEditPrizeDraw = new CreateOrEditPrizeDraw(); 

	ngOnInit(): void {
	}
	
	@ViewChild(TabView) tabView: TabView;
	tabViewActive = {
		thongTinChuongTrinh: true,
		cauHinhChuongTrinh: false,
		lichSu: false,
	}
	
	changeTab(event) {
        let tabHeader = this.tabView.tabs[event.index].header;
		if(!this.tabViewActive[tabHeader]) {
			this.tabViewActive[tabHeader] = true;
		}
    }

	bannerBackground = 'assets/layout/images/add-banner-voucher-bg.svg';
	viewAvatarLink: any;
}
