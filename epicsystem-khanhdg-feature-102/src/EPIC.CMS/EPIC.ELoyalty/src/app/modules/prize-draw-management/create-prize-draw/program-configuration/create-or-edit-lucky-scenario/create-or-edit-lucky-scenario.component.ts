import { Component, EventEmitter, Inject, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppConsts, PrizeDrawManagement } from '@shared/AppConsts';
import { IconConfirm } from '@shared/consts/base.const';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CreateOrEditLuckyScenario } from '@shared/interface/prize-draw-management/PrizeDrawManagement.model';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { HelpersService } from '@shared/services/helpers.service';
import { LuckyScenarioService } from '@shared/services/lucky-scenario-service';
import { VoucherManagementService } from '@shared/services/voucher-management-service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { TabView } from 'primeng/tabview';
import { forkJoin } from 'rxjs';

@Component({
	selector: 'app-create-or-edit-lucky-scenario',
	templateUrl: './create-or-edit-lucky-scenario.component.html',
	styleUrls: ['./create-or-edit-lucky-scenario.component.scss'],
})
export class CreateOrEditLuckyScenarioComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private dialogConfig: DynamicDialogConfig,
		private _voucherManagementService: VoucherManagementService,
		private _helpersService: HelpersService,
		public ref: DynamicDialogRef,
		public _luckyScenarioService: LuckyScenarioService,
		@Inject(API_BASE_URL) baseUrl?: string,
	) {
		super(injector, messageService);
		this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
	}

	avatarBackground = 'assets/layout/images/add-banner-voucher-bg.svg';
	viewAvatarLink: any;
	listVoucher: any[] = [];

	luckyScenario: CreateOrEditLuckyScenario = new CreateOrEditLuckyScenario();

	tabViewActive = {
		'caiDatKichBan': true,
		'giaoDienVongQuay': false,
	};

	luckyProgramId: number;
	@ViewChild(TabView) tabView: TabView;
	private baseUrl: string;
	
	ngOnInit(): void {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		if(this.dialogConfig.data?.item && this.dialogConfig.data?.luckyProgramId) {
			this.luckyProgramId = this.dialogConfig.data?.luckyProgramId;
			this.getLuckyScenarioInfo(this.dialogConfig.data?.item?.id);
		} else if(this.dialogConfig.data?.luckyProgramId) {
			this.luckyProgramId = this.dialogConfig.data?.luckyProgramId;
			this.init();
		} else {
			this.init();
			this.luckyScenario = {
				...new CreateOrEditLuckyScenario(),
				prizeQuantity: 1
			}
			//
			for(let i = 0; i < this.luckyScenario.prizeQuantity; i++) {
				this.luckyScenario.luckyScenarioDetails.push({});
			}
		}
	}

	getLuckyScenarioInfo(id: number){
		this.isLoading = true;
		forkJoin([this._voucherManagementService.getAllActiveVoucher(), this._luckyScenarioService.findById(id)]).subscribe(([res, resScenario]) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.listVoucher = res?.data?.items.map(item => {
					item.label = item.name,
					item.value = +item.voucherId
					return item;
				});
				this.listVoucher.push(PrizeDrawManagement.DEFAULT_VOUCHER);
			}

			if (this.handleResponseInterceptor(resScenario, '')) {
				this.luckyScenario = resScenario?.data;
				this.viewAvatarLink = `${this.baseUrl}/${this.luckyScenario.avatarImageUrl}`;
				this.luckyScenario?.luckyScenarioDetails?.forEach(item => {
					if (!item.voucherId) item.voucherId = PrizeDrawManagement.DEFAULT_VOUCHER.value;
					return item;
				});
				console.log('luckyScenario.luckyScenarioDetails ', this.luckyScenario.luckyScenarioDetails);
				
			}
		}, err => {
			this.isLoading = false;
		}) 
	}

	init() {
		this.isLoading = true;
		this._voucherManagementService.getAllActiveVoucher().subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.listVoucher = res?.data?.items.map(item => {
					item.label = item.name,
					item.value = +item.voucherId
					return item;
				});
				this.listVoucher.push(PrizeDrawManagement.DEFAULT_VOUCHER);
			}
		}, (err) => {
			this.isLoading = false;
		})
	}
	
	changePrizeQuantity(prizeQuantity) {
		if(prizeQuantity){
			console.log({prizeQuantity: prizeQuantity}, {luckyScenario: this.luckyScenario});
			if (this.luckyScenario.luckyScenarioDetails?.length < prizeQuantity){
				let length = prizeQuantity - this.luckyScenario.luckyScenarioDetails?.length
				for(let i = 0; i < length; i++){
					this.luckyScenario.luckyScenarioDetails.push({sortOrder: i});
				}
			} else if (this.luckyScenario.luckyScenarioDetails?.length > prizeQuantity){
				let lengthCut = prizeQuantity - this.luckyScenario.luckyScenarioDetails?.length
				this.luckyScenario.luckyScenarioDetails = this.luckyScenario.luckyScenarioDetails.slice(0, lengthCut)
			}
		}		
	}

	insertImage(event){
		if(event){
			const ref = this._helpersService.dialogUploadImagesRef(
				"prize-draw",
				{
					uploadServer: false,
					multiple: false,
				}
			);
			ref.onClose.subscribe((images) => {
				if (images){
					this.viewAvatarLink = this._helpersService.getBlobUrlImage(images[0]);	
					this.luckyScenario.avatarImageUrl = images[0];	
				}
			})
		}
	}

	changeTabview(e) {
		let tabHeader = this.tabView.tabs[e.index].header;
		if(!this.tabViewActive[tabHeader]) {
			this.tabViewActive[tabHeader] = true;
		}
	}

	close(){
		this.ref.close();
	}

	removeElement(index) {
		const ref = this._helpersService.dialogConfirmRef(
			"Xác nhận xóa phần quà này?",
			IconConfirm.DELETE,
		);
		ref.onClose.subscribe((accept: boolean) => {
			if(accept) {
				this.luckyScenario.luckyScenarioDetails.splice(index, 1);
				this.luckyScenario.prizeQuantity -= 1;
			}
		})
	}

	save() {
		console.log('settingLuckySpin', this.luckyScenario.luckyRotationInterface);
		for(let i=0; i < this.luckyScenario?.luckyScenarioDetails?.length; i++){
			this.luckyScenario.luckyScenarioDetails[i].sortOrder = i;
			if (this.luckyScenario.luckyScenarioDetails[i].voucherId == PrizeDrawManagement.DEFAULT_VOUCHER.value){
				this.luckyScenario.luckyScenarioDetails[i].name = PrizeDrawManagement.DEFAULT_VOUCHER.label;
			}
		}

		if(this.luckyScenario.id && this.luckyProgramId){
			let scenarioBody = {
				id: this.luckyScenario.id,
				luckyProgramId: this.luckyProgramId,
				name: this.luckyScenario.name,
				prizeQuantity: this.luckyScenario.prizeQuantity,
				avatarImageUrl: this.luckyScenario.avatarImageUrl,
				luckyScenarioDetails: this.luckyScenario.luckyScenarioDetails,
			} 
			
			forkJoin([this._luckyScenarioService.updateScenario(scenarioBody), this._luckyScenarioService.updateRotationInterface(this.luckyScenario.luckyRotationInterface)]).subscribe(([res, resRotation]) => {
				if(this.handleResponseInterceptor(res) && this.handleResponseInterceptor(resRotation)){
					this.messageSuccess('Cập nhật thành công');
					this.ref.close();
				}
			});
		} else if(this.luckyProgramId){
			let body = {
				luckyProgramId: this.luckyProgramId,
				name: this.luckyScenario.name,
				luckyScenarioType: PrizeDrawManagement.VONG_QUAY_MAY_MAN,
				prizeQuantity: this.luckyScenario.prizeQuantity,
				avatarImageUrl: this.luckyScenario.avatarImageUrl,
				luckyScenarioDetails: this.luckyScenario.luckyScenarioDetails,
				luckyRotationInterface: this.luckyScenario.luckyRotationInterface
			}
			this._luckyScenarioService.addScenario(body).subscribe((res) =>{
				if(this.handleResponseInterceptor(res)){
					this.messageSuccess('Thêm thành công');
					this.ref.close();
				}
			})
		}
		else{
			this.ref.close(this.luckyScenario);
		}
	}
}
