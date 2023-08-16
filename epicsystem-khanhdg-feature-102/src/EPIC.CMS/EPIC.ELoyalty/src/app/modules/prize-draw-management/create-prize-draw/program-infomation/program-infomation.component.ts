import { Component, Inject, Injector, Input, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts, MARKDOWN_OPTIONS, MessageErrorConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDescriptionContent, IImage } from '@shared/interface/InterfaceConst.interface';
import { CreateOrEditPrizeDraw } from '@shared/interface/prize-draw-management/PrizeDrawManagement.model';
import { PrizeDrawShareService } from '@shared/service-proxies/prize-draw-service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { HelpersService } from '@shared/services/helpers.service';
import { PrizeDrawService } from '@shared/services/prize-draw.service';
import { MessageService } from 'primeng/api';

export const DEFAULT_IMAGE = 160;

export const DEFAULT_IMAGE_BANNER = {
	width: DEFAULT_IMAGE * 2,
	height: DEFAULT_IMAGE,
};

@Component({
	selector: 'app-program-infomation',
	templateUrl: './program-infomation.component.html',
	styleUrls: ['./program-infomation.component.scss'],
})
export class ProgramInfomationComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
        private router: Router,
		private _prizeDrawShareService: PrizeDrawShareService,
		private _prizeDrawervice: PrizeDrawService,
		private _helpersService: HelpersService,
		private activeRouter: ActivatedRoute,
		@Inject(API_BASE_URL) baseUrl?: string,
	) {
        super(injector, messageService);
		this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
		const isCreateNew = this.activeRouter.snapshot.queryParamMap.get('isCreateNew');
        if(isCreateNew) this._prizeDrawShareService.reset();
	}

	private baseUrl: string;


	public bannerBackground = 'assets/layout/images/add-banner-voucher-bg.svg';
	public selectedBannerVoucher: IImage;

	public get MARKDOWN_OPTIONS() {
		return MARKDOWN_OPTIONS;
	}
	
	@Input() prizeDrawId: number;
	prizeDrawProgramInfo: CreateOrEditPrizeDraw = new CreateOrEditPrizeDraw(); 
	viewAvatarLink: any;

	isEdit: boolean = false;

	ngOnInit(): void {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.selectedBannerVoucher = {
			src: this.bannerBackground,
			...DEFAULT_IMAGE_BANNER,
		  };
		this.prizeDrawProgramInfo.descriptionContentType = MARKDOWN_OPTIONS.MARKDOWN;
		for (const [key, value] of Object.entries(this.prizeDrawProgramInfo)) {
            this.prizeDrawProgramInfo[key] = this._prizeDrawShareService.prizeDrawProgramInfo[key];
        }
		if (this.prizeDrawProgramInfo.avatarImageUrl as File){
			this.viewAvatarLink = this._helpersService.getBlobUrlImage(this.prizeDrawProgramInfo.avatarImageUrl as File);	
		}
		if(this.prizeDrawId){
			this.init();
		}
	}

	init(){
		this.isLoading = true;
		this._prizeDrawervice.getById(this.prizeDrawId).subscribe((res) => {
			this.isLoading = false;
			this.prizeDrawProgramInfo = res.data;
			this.viewAvatarLink = `${this.baseUrl}/${this.prizeDrawProgramInfo.avatarImageUrl}`;
		}, err => {
			this.isLoading = false;
		})
	}

	ngOnDestroy(): void {
        // this._orderShareService.filterCustomer = false;
        // this._orderShareService.filterProject = false;
        // this._orderShareService.filterView = false;
    }

	changeEdit() {
		this.isEdit = !this.isEdit;
		if(!this.isEdit) {
			// Lưu chỉnh sửa
			this._prizeDrawervice.update(this.prizeDrawProgramInfo).subscribe((res) => {
				if(this.handleResponseInterceptor(res, 'Chỉnh sửa thành công')){
					this.init();
				}
			})
		}
	}

	public get widthDivBanner() {
		return `calc(${DEFAULT_IMAGE * 2}px + 1rem)`;
	}

	public get DEFAULT_IMAGE_BANNER() {
		return DEFAULT_IMAGE_BANNER;
	}

	// public onChangeBanner(event: IImage | undefined) {
	// 	if (event) {
	// 	  this.selectedBannerVoucher = event;
	// 	  this.prizeDrawProgramInfo.avatar = event.src;
	// 	}
	// }

	insertImage(event){
		if(event && (this.isEdit || !this.prizeDrawProgramInfo?.id)){
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
					this.prizeDrawProgramInfo.avatarImageUrl = images[0];	
				}
			})
		}
	}

	validForm(): boolean {
		const validRequired = this.prizeDrawProgramInfo?.name
							&& this.prizeDrawProgramInfo?.code
							&& this.prizeDrawProgramInfo?.avatarImageUrl
							&& this.prizeDrawProgramInfo?.startDate
							&& this.prizeDrawProgramInfo?.endDate;
		return validRequired;
	}
   
	nextPage() {
		if(this.validForm()){
			this._prizeDrawShareService.filterProgramConfig = true;
			this._prizeDrawShareService.prizeDrawProgramInfo = this.prizeDrawProgramInfo;
			this.router.navigate(['/prize-draw-management/prize-draw/create/program-configuration']);
		} else {
			this.messageError(MessageErrorConst.message.Validate);
		}

	}
}
