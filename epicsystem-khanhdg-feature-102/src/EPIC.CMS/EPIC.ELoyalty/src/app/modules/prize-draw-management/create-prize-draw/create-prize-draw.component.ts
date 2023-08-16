import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { PrizeDrawShareService } from '@shared/service-proxies/prize-draw-service';
import { MenuItem, MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-create-prize-draw',
  templateUrl: './create-prize-draw.component.html',
  styleUrls: ['./create-prize-draw.component.scss'],
})
export class CreatePrizeDrawComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private _prizeDrawShareService: PrizeDrawShareService
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink:['/home'] },
			{ label: 'Chương trình trúng thưởng',  routerLink: ['prize-draw-management']},
			{ label: 'Thêm mới'},
		  ]);
	}

	items: MenuItem[];
	subscription: Subscription;

	ngOnInit(): void {		
		this.items = [
			{
			  label: 'Thông tin chương trình',
			  routerLink: '/prize-draw-management/prize-draw/create/program-infomation'
			},
			{
			  label: 'Cấu hình chương trình',
			  routerLink: '/prize-draw-management/prize-draw/create/program-configuration'
			},
		];
		  this.subscription = this._prizeDrawShareService.prizeDrawComplete$.subscribe((personalInformation) =>{
		});
	}
}
