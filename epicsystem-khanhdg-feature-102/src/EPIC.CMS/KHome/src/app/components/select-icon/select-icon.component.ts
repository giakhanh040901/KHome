import { Component, Injector, OnInit } from "@angular/core";
import { SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { IconConst } from "@shared/IconConst";
import { DistributionService } from "@shared/services/distribution.service";
import { MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";

@Component({
selector: "app-select-icon",
templateUrl: "./select-icon.component.html",
styleUrls: ["./select-icon.component.scss"],
})
export class SelectIconComponent extends CrudComponentBase implements OnInit {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _distributionService: DistributionService,
		public ref: DynamicDialogRef,
		public config: DynamicDialogConfig
	) {
		super(injector, messageService);
	}

	IconConst = IconConst;

	filter = {
		keyword: '',
		type: [],
		groupUtilityId: []
	}
	listIcon: any = [];

	listType = [...IconConst.ListType];
	listGroupUtility = [...IconConst.ListGroup];
	imageStyle: any = {objectFit: 'cover', 'border-radius': '12px', 'background-color': '#D9D9D9'};

	selectedIcon: any = [];

	isUpdate: boolean;

	checked = false;
	ngOnInit(): void {
		this.isUpdate = this.config?.data?.isUpdate;
		this.listIcon = IconConst.getIcon();
	}

	hideDialog() {
		this.ref.close();
	}

	save() {
		if (this.selectedIcon.length > 0){
			let result = [];
			if (!this.isUpdate){
				this.selectedIcon.forEach(item => {
					result.push({iconName: item.icon, path: item.path})
				});
			} else {
				result.push({iconName: this.selectedIcon[0].icon, path: this.selectedIcon[0].path})
			}
			this.ref.close(result);

		}
		else {
			this.ref.close();
		}
	}

	changeFilter(){
		this.listIcon = IconConst.getIcon(this.filter);
	}
}
