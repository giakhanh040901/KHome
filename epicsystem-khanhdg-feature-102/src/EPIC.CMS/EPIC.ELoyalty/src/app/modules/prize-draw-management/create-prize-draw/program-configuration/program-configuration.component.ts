import { Component, Injector, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActiveDeactiveConst, EPositionFrozenCell, EPositionTextCell, ETypeDataTable, ETypeFormatDate, PrizeDrawManagement } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IActionTable, IHeaderColumn, IValueFormatter } from '@shared/interface/InterfaceConst.interface';
import { CreateOrEditLuckyScenario, CreateOrEditPrizeDraw } from '@shared/interface/prize-draw-management/PrizeDrawManagement.model';
import { PrizeDrawShareService } from '@shared/service-proxies/prize-draw-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { CreateOrEditLuckyScenarioComponent } from './create-or-edit-lucky-scenario/create-or-edit-lucky-scenario.component';
import { formatDate } from '@shared/function-common';
import { AppSessionService } from '@shared/session/app-session.service';
import { PrizeDrawService } from '@shared/services/prize-draw.service';
import { forkJoin } from 'rxjs';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';
import { LuckyScenarioService } from '@shared/services/lucky-scenario-service';

@Component({
	selector: 'app-program-configuration',
	templateUrl: './program-configuration.component.html',
	styleUrls: ['./program-configuration.component.scss'],
})
export class ProgramConfigurationComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
        private router: Router,
		private _prizeDrawShareService: PrizeDrawShareService,
		private dialogService: DialogService,
		private _appSessionService: AppSessionService,
		private _prizeDrawService: PrizeDrawService,
		private _helpersService: HelpersService,
		private _luckyScenarioService: LuckyScenarioService,

	) {
        super(injector, messageService);
	}

	userInfo: any = {};

	PrizeDrawManagement = PrizeDrawManagement;
	ActiveDeactiveConst = ActiveDeactiveConst;
	
	isJoinTimeSetting: boolean = false;
	isTimeLine: boolean = false;
	isTimeCustomerJoin: boolean;
	isMaxNumberOfTurnByIp: boolean;

	prizeDrawProgramInfo: CreateOrEditPrizeDraw;

	cols: any[];

	listAction: IActionTable[][] = [];
	headerColumns: IHeaderColumn[] = [];

	isEdit: boolean = false;
	@Input() prizeDrawId: number = null;
	ngOnInit(): void {
		this.userInfo = this._appSessionService.user;
		this.prizeDrawProgramInfo = this._prizeDrawShareService.getPrizeDrawProgramInfo();
		if (!this.prizeDrawProgramInfo?.name && !this.prizeDrawId) {
			this.router.navigate(['/prize-draw-management/prize-draw/create/program-infomation']);
		}
		this.cols = [
			{ field: 'name', header: 'Kịch bản', width: '20rem', isPin: true},
			{ field: 'luckyScenarioTypeName', header: 'Loại kịch bản', width: '16rem', isPin: true},
			{ field: 'createdDate', header: 'Thời gian', width: '10rem' },
			{ field: 'createdBy', header: 'Người tạo', width: '12rem', cutText: 'b-cut-text-12'},
			{ field: 'columnResize', header: '', type:'hidden' },
		];

		if(this.prizeDrawId){
			this.init();
		}
	}

	init(){
		this.isLoading = true;
		forkJoin([
			this._prizeDrawService.getById(this.prizeDrawId),
			this._prizeDrawService.getAllLuckyScenario(this.prizeDrawId)
		]).subscribe(([res, resScenario]) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res) && res?.data) {
				this.prizeDrawProgramInfo = res?.data;
				if(this.prizeDrawProgramInfo.joinTimeSetting){
					this.isJoinTimeSetting = true;
					this.prizeDrawProgramInfo.joinTimeSetting == PrizeDrawManagement.TIMELINE ? this.isTimeLine = true : this.isTimeCustomerJoin = false;
				}
				if(this.prizeDrawProgramInfo.maxNumberOfTurnByIp) this.isMaxNumberOfTurnByIp = true
			} 
			if(this.handleResponseInterceptor(resScenario)){
				this.prizeDrawProgramInfo.luckyScenarios = resScenario.data;
				this.setData(this.prizeDrawProgramInfo.luckyScenarios);
				this.genListAction();

			}
			console.log('prizeDrawProgramInfo ', this.prizeDrawProgramInfo);
			
		}, err => {
			this.isLoading = false;
		})

	}

	changeEdit() {
		this.isEdit = !this.isEdit;
		if(!this.isEdit) {
			// Lưu chỉnh sửa
			this._prizeDrawService.update(this.prizeDrawProgramInfo).subscribe((res) => {
				if(this.handleResponseInterceptor(res, 'Chỉnh sửa thành công')){
					this.init();
				}
			})
		}
	}
	genListAction() {
		this.listAction = this.prizeDrawProgramInfo.luckyScenarios.map((row, index) => {
			let actions: IActionTable[] = [];

			actions.push({
				data: row,
				label: "Thông tin chi tiết",
				icon: "pi pi-info-circle",
				command: ($event) => {
					this.createLuckyScenario($event.item.data, index);
				}
			})

			actions.push({
				data: row,
				label: row.status == ActiveDeactiveConst.ACTIVE ? "Huỷ kích hoạt" : "Kích hoạt",
				icon: row.status == ActiveDeactiveConst.ACTIVE ? "pi pi-times" : "pi pi-check",
				command: ($event) => {
					this.changeStatus($event.item.data);
				}
			})

			actions.push({
				data: row,
				label: "Xoá",
				icon: "pi pi-trash",
				command: ($event) => {
					this.delete($event.item.data, index);
				}
			})
			//
			return actions;
		})
	}

	delete(item, index){
		this._helpersService.dialogConfirmRef("Xác nhận xoá kịch bản", IconConfirm.DELETE).onClose.subscribe((accept: boolean) => {
			if(accept){
				if (item.id){
					this._luckyScenarioService.delete(item.id).subscribe((res) => {
						if(this.handleResponseInterceptor(res, 'Thao tác thành công')){
							this.init();
						}
					})
				} else{
					this.prizeDrawProgramInfo.luckyScenarios.splice(index, 1);
				}

			}
		})
	}

	changeStatus(item){
		this._helpersService.dialogConfirmRef(
			item.status == ActiveDeactiveConst.ACTIVE ? "Xác nhận huỷ kích hoạt" : "Xác nhận kích hoạt",
			item.status == ActiveDeactiveConst.ACTIVE ? IconConfirm.QUESTION : IconConfirm.APPROVE
		).onClose.subscribe((accept: boolean) => {
			if(accept){
				this._luckyScenarioService.changeStatus(item.id).subscribe((res) => {
					if(this.handleResponseInterceptor(res, 'Thao tác thành công')){
						this.init();
					}
				})
			}
		})
	}

	changeTimeSetting(event){
		if(!event.checked){
			// Reset thông tin giới hạn tham gia chương trình
			this.isTimeCustomerJoin = false;
			this.isTimeLine = false;
			this.prizeDrawProgramInfo.joinTimeSetting = undefined; 
			this.prizeDrawProgramInfo.numberOfTurn = undefined; 
			this.prizeDrawProgramInfo.startTurnDate = ''; 
			this.prizeDrawProgramInfo.resetTurnQuantity = undefined; 
			this.prizeDrawProgramInfo.resetTurnType = ''; 
		}
	}
	
	changeJoinTimeSetting(event, joinTimeSettingType){
		// Reset thông tin giới hạn tham gia chương trình
		this.prizeDrawProgramInfo.joinTimeSetting = undefined; 
		this.prizeDrawProgramInfo.numberOfTurn = undefined; 
		this.prizeDrawProgramInfo.startTurnDate = ''; 
		this.prizeDrawProgramInfo.resetTurnQuantity = undefined; 
		this.prizeDrawProgramInfo.resetTurnType = ''; 

		if(event.checked){
			this.prizeDrawProgramInfo.joinTimeSetting = joinTimeSettingType
			if (joinTimeSettingType == PrizeDrawManagement.TIMELINE){
				this.isTimeCustomerJoin = false;
			}
			if (joinTimeSettingType == PrizeDrawManagement.CUSTOMER_JOIN){
				this.isTimeLine = false;
			}
		} 
	}

	createLuckyScenario(item?: CreateOrEditLuckyScenario, index?: number) {
		const ref = this.dialogService.open(CreateOrEditLuckyScenarioComponent,
			{
				header: 'Tạo kịch bản vòng quay may mắn',
				width: innerWidth > 1366 ? '60%' : '75%',
                height: '100vh',
                styleClass: 'dialog-right full-height',
				data: {
					item: item,
					luckyProgramId: this.prizeDrawId
				}
		})
		ref.onClose.subscribe((dataLuckyScenario: CreateOrEditLuckyScenario) => {
			if (dataLuckyScenario) {
				if(item) {
					this.prizeDrawProgramInfo.luckyScenarios[index] = dataLuckyScenario;
				} else {
					this.prizeDrawProgramInfo.luckyScenarios.push(dataLuckyScenario);
				}
				this.setData(this.prizeDrawProgramInfo.luckyScenarios);
				this.genListAction();
			} else {
				this.init();
			}
			console.log('SIUU prizeDrawProgramInfo', this.prizeDrawProgramInfo);
		})
	}

	prevPage() {
        this._prizeDrawShareService.prizeDrawProgramInfo = this.prizeDrawProgramInfo;
        this.router.navigate(['/prize-draw-management/prize-draw/create/program-infomation']);
    }

	setData(rows = []){
		for (let row of rows) {
			row.luckyScenarioType = PrizeDrawManagement.VONG_QUAY_MAY_MAN;
			row.luckyScenarioTypeName = PrizeDrawManagement.getNameLuckyScenarioType(row.luckyScenarioType),
			row.createdDate = row?.createdDate ? formatDate( row.createdDate, ETypeFormatDate.DATE) : formatDate(new Date(), ETypeFormatDate.DATE),
			row.createdBy = this.userInfo.userName
		}
	}

	validForm(): boolean {
		const validRequired = this.prizeDrawProgramInfo?.luckyScenarios?.length > 0;
		return validRequired;
	}

	complete(){
		if(this.validForm()){
			this._prizeDrawShareService.complete();
			this._prizeDrawService.create(this.prizeDrawProgramInfo).subscribe((res) => {
				if (this.handleResponseInterceptor(res, '')) {
					this.router.navigate(['/prize-draw-management']);
				}
			})
		} else{
			this.messageError('Vui lòng cấu hình kịch bản');
		}

	}
}
