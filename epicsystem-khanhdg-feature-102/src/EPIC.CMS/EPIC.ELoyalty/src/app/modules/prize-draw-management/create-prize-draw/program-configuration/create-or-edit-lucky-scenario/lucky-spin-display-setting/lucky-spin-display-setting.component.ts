

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { LuckyRotationInterface } from '@shared/interface/prize-draw-management/PrizeDrawManagement.model';
import { HelpersService } from '@shared/services/helpers.service';
// import { ColorPickerService } from 'ngx-color-picker';

@Component({
  selector: 'app-lucky-spin-display-setting',
  templateUrl: './lucky-spin-display-setting.component.html',
  styleUrls: ['./lucky-spin-display-setting.component.scss'],
//   providers: [ColorPickerService]

})
export class LuckySpinDisplaySettingComponent implements OnInit {
    constructor(
        private _helpersService: HelpersService,
    ) { }

    color = '#2883e9';
    endPoint = 'assets/layout/images/lucky-spin';
    endPointTemplateGreen = 'assets/layout/images/lucky-spin/template-green';
    endPointTemplateDarkRed = 'assets/layout/images/lucky-spin/template-dark-red';
    endPointTemplateBrightRed = 'assets/layout/images/lucky-spin/template-bright-red';
    endPointTemplateDarkPurple = 'assets/layout/images/lucky-spin/template-dark-purple';
    endPointTemplateBrightPurple = 'assets/layout/images/lucky-spin/template-bright-purple';
    endPointTemplateYellow = 'assets/layout/images/lucky-spin/template-yellow';

    AppConsts = AppConsts

    @Input() luckySpinSetting: LuckyRotationInterface;
    @Output() luckySpinSettingChange = new EventEmitter<LuckyRotationInterface>();

    iconButton = [
        {
            keyIcon: 'iconPlay',
            keyButton: 'buttonPlay',
            dessciption: 'Nút chơi game',
        },
        {
            keyIcon: 'iconHistory',
            keyButton: 'buttonHistory',
            dessciption: 'Nút lịch sử trúng quà'
        },
        {
            keyIcon: 'iconRank',
            keyButton: 'buttonRank',
            dessciption: 'Nút xếp hạng'
        }
    ];

    imageDefault = {
        iconHome: `${this.endPoint}/iconHome.png`,
        iconSignal: `${this.endPoint}/icon-signal.png`,
        iconBattery: `${this.endPoint}/icon-battery.png`,
        iconPlay: `${this.endPoint}/icon-play.png`,
        iconHistory: `${this.endPoint}/icon-history.png`,
        iconRank: `${this.endPoint}/icon-rank.png`,
        banner: `${this.endPoint}/banner.jpg`,
        rotation: `${this.endPoint}/rotation.png`,
        spin: `${this.endPoint}/spin.png`,
        background: `${this.endPoint}/background.png`,
        borderIphone: `${this.endPoint}/border-iphone.png`,
        rotationBackgroundGif: `${this.endPoint}/rotation-background-gif.gif`,
        rotationBackgroundStatic: `${this.endPoint}/rotation-background-static.png`,
    }

    templates = [
        {
            image: `${this.endPointTemplateDarkPurple}/demo.png`,
            background: `${this.endPointTemplateDarkPurple}/background.png`,
            banner: `${this.endPoint}/banner.jpg`,
            rotationBackground: `${this.endPointTemplateDarkPurple}/rotation-background.gif`,
            rotationImage: `${this.endPointTemplateDarkPurple}/rotation.png`,
            needleImage: `${this.endPointTemplateDarkPurple}/spin.png`,
            iconPlay:  `${this.endPoint}/icon-play-gr-1.png`,
            iconHistory:  `${this.endPoint}/icon-history.png`,
            iconRank:  `${this.endPoint}/icon-rank.png`,
            buttonColor: '#ff9800',
            key: 'darkPurple',
        },
        {
            image: `${this.endPointTemplateGreen}/demo.png`,
            background: `${this.endPointTemplateGreen}/background.png`,
            banner: `${this.endPoint}/banner.jpg`,
            rotationBackground: `${this.endPointTemplateGreen}/rotation-background.gif`,
            rotationImage: ``,
            needleImage: `${this.endPointTemplateGreen}/spin.png`,
            buttonColor: '#ff9800',
            iconPlay:  `${this.endPoint}/icon-play-gr-1.png`,
            iconHistory:  `${this.endPoint}/icon-history.png`,
            iconRank:  `${this.endPoint}/icon-rank.png`,
            key: 'green',
        },
        {
            image: `${this.endPointTemplateDarkRed}/demo.png`,
            background: `${this.endPointTemplateDarkRed}/background.png`,
            banner: `${this.endPoint}/banner.jpg`,
            rotationImage: ``,
            needleImage: `${this.endPointTemplateDarkRed}/spin.png`,
            buttonColor: '#ff9800',
            iconPlay:  `${this.endPoint}/icon-play-gr-1.png`,
            iconHistory:  `${this.endPoint}/icon-history.png`,
            iconRank:  `${this.endPoint}/icon-rank.png`,
            key: 'darkRed',
        },
        {
            image: `${this.endPointTemplateBrightRed}/demo.png`,
            background: `${this.endPointTemplateBrightRed}/background.png`,
            banner: `${this.endPoint}/banner.jpg`,
            rotationBackground: `${this.endPointTemplateBrightRed}/rotation-background.png`,
            rotationImage: `${this.endPointTemplateBrightRed}/rotation.png`,
            needleImage: `${this.endPointTemplateBrightRed}/spin.png`,
            buttonColor: '#DD1D1D',
            iconPlay:  `${this.endPoint}/icon-play-gr-2.png`,
            iconHistory:  `${this.endPoint}/icon-gift-box.png`,
            iconRank:  `${this.endPoint}/icon-rank.png`,
            key: 'brightRed',
        },
        
        {
            image: `${this.endPointTemplateBrightPurple}/demo.png`,
            background: `${this.endPointTemplateBrightPurple}/background.jpg`,
            banner: `${this.endPoint}/banner.jpg`,
            rotationImage: `${this.endPointTemplateBrightPurple}/rotation.png`,
            needleImage: `${this.endPointTemplateBrightPurple}/spin.png`,
            buttonColor: '#FFC821',
            iconPlay:  `${this.endPoint}/icon-play-gr-2.png`,
            iconHistory:  `${this.endPoint}/icon-gift-box.png`,
            iconRank:  `${this.endPoint}/icon-rank.png`,
            key: 'brightPurple',
        },
        {
            image: `${this.endPointTemplateYellow}/demo.png`,
            background: `${this.endPointTemplateYellow}/background.png`,
            banner: `${this.endPoint}/banner.jpg`,
            rotationImage: `${this.endPointTemplateYellow}/rotation.png`,
            needleImage: `${this.endPointTemplateYellow}/spin.png`,
            buttonColor: '#FFC821',
            iconPlay:  `${this.endPoint}/icon-play-gr-2.png`,
            iconHistory:  `${this.endPoint}/icon-gift-box.png`,
            iconRank:  `${this.endPoint}/icon-rank.png`,
            key: 'yellow',
        },
    ]

    iconUpload = {
        banner: `${this.endPoint}/upload-banner.png`,
        rotationImage: `${this.endPoint}/upload-rotation.png`,
        needleImage: `${this.endPoint}/upload-spin.png`,
        rotationBackground: `${this.endPoint}/upload-rotation-background.png`,
        background: `${this.endPoint}/upload-background.png`,
    }

    uploadRotation = [
        {
            propertyName: 'rotationBackground',
            description: 'Ảnh nền vòng quay'
        },
        {
            propertyName: 'rotationImage',
            description: 'Ảnh vòng quay'
        },
        {
            propertyName: 'needleImage',
            description: 'Ảnh kim quay'
        },
    ];

    // DANH SÁCH TOÀN BỘ GIẢI THƯỞNG
    prizeValues = ['2star', '3star', 'LoRi','X2', '2The', 'TheV','None', '1star'];
    // prizeValues = ['2star', '1star', 'None', 'TheV', '2The','X2', 'LoRi', '3star'];
    // DANH SÁCH GIẢI THƯỞNG SET ĐƯỢC TRÚNG
    prizeActives = [];
    // DANH SÁCH GIẢI THƯỞNG BỊ LOẠI TRỪ KHÔNG CHO PHÉP QUAY VÀO
    prizeDisable = ['2star', '3star'];
    // GIẢI THƯỞNG TRÚNG Ở THỜI ĐIỂM GÂN NHẤT
    prizeCurent: string = '1star';
    // SET GIẢI THƯỞNG CHÍNH XÁC SẼ TRỦNG
    prizeResult: string;
    // GÓC 360 ỨNG VỚI 1 VÒNG TRÒN
    perigon = 360;
    // SETTING CSS ANIMATION KHI QUAY TRÚNG THƯỞNG
    rotateAnimation: any;
    // GIÁ TRỊ VỊ TRÍ QUAY TRÚNG THƯỞNG
    rotateTransformCss: number = 0;
    // VỊ TRÍ TRÚNG THƯỞNG TRONG DANH SÁCH GIẢI THƯỞNG KEY OF ARRAY
    positionWin: number;
    // GIÁ TRỊ 1 GÓC CỦA GIẢI THƯỞNG (GÓC TRONG TOÁN HỌC 30, 45, 90 ....)
    prizeCorner: number;

    templateDefault: any = {};

    prizeDraw: any = {}

    ngOnInit(): void {
        const templateDefault = "darkPurple";
        const templateIndex = this.templates.findIndex(t => t.key === templateDefault);
        this.templateDefault = this.templates[templateIndex];
        //
        if(!this.luckySpinSetting) {
            this.luckySpinSetting = new LuckyRotationInterface();
            // SET TEMPLATE DEFAULT, KHỞI TẠO TEMPLATE
            this.changeTemplate(templateDefault, templateIndex);
        } 
        //
        this.prizeCorner = this.perigon/this.prizeValues.length;
        let index = this.prizeValues.findIndex(p => p === this.prizeCurent);
        if(index !== -1) {
            this.rotateTransformCss = this.perigon - index*(this.prizeCorner);
            // SET GÓC QUAY KHỞI TẠO
            this.rotateAnimation = {
                transform: `rotate(${this.rotateTransformCss}deg)`,
            };
        }
    }
   
    /**
     * THAY ẢNH THEO KEY
     * @param key: KEY ỨNG VỚI VỊ TRÍ CẦN THAY ẢNH 
     */
    chooseFile(key) {
        const ref = this._helpersService.dialogUploadImagesRef(
            "image-icon-ferris-wheel",
            {
                previewBeforeUpload: false,
                multiple: false,
                // accept: EAcceptFile.IMAGE,
                uploadServer: false,
            }
        );
        //
        ref.onClose.subscribe((urls: File[]) => {
            if(urls) {
                console.log('urls', urls);
                this.luckySpinSetting[key] = urls[0];
            }
        })
    }

    /**
     * XÓA ẢNH THEO KEY
     * @param key: KEY ỨNG VỚI VỊ TRÍ CẦN THAY ẢNH 
     */
    removeImage(key) {
        console.log('key', key);
        this.luckySpinSetting[key] = '';
    }

    // CUSTOM COLOR PICKER
    focusInputColor(id: string) {
        document.getElementById(id).focus();
    }

    // BẮT ĐẦU QUAY TRÚNG THƯỞNG
    playing: boolean = false;
    prize: string;
    playStart() {
        if(!this.playing) {
            // DISABLE NÚT QUAY KHI ĐANG QUAY
            this.playing = true;
            // LẤY GIÁ TRỊ TRÚNG THƯỞNG (RANDOM)
            this.setPositionWin();
            // TÍNH TOÁN VỊ TRÍ TRÚNG THƯỞNG CẦN QUAY ĐẾN
            let positionWin = this.perigon - this.positionWin*(this.perigon/this.prizeValues.length);
            
            // SET CHÍNH XÁC GIẢI THƯỞNG ĐƯỢC NHẬN
            if(this.prizeResult) {
                let indexPrize = this.prizeValues.findIndex(p => p === this.prizeResult)
                if(indexPrize !== -1) {
                    positionWin = this.perigon - (indexPrize*45);
                } 
            }
            //
            this.prize = this.prizeValues[(positionWin - 45)/(this.perigon/this.prizeValues.length) - 1];
            console.log('positionWin', positionWin);
            
            let animationTime = 7500;
            // BƯỚC NHẢY MỚI LẦN QUAY LÀ 7200 + VỊ TRỊ GÓC CỦA GIẢI THƯỞNG
            // SET VỊ TRÍ GIẢI THƯỞNG ĐƯỢC NHẬN + ACTIVE ANIMATION (ROTATE_DEG)
            this.rotateTransformCss += 7200 + positionWin + this.supplementtaryAngle360;
            this.rotateAnimation = {
                transform: `rotate(${this.rotateTransformCss}deg)`,
                transition: `transform ${animationTime}ms ease-in-out`
            };
            
            // SET THỜI GIAN SAU KHI QUAY XONG THÌ CHO PHÉP CLICK LẠI NÚT QUAY TRÚNG THƯỞNG
            setTimeout(() => {
                this.playing = false;
            }, animationTime);
        }
    }

    // RANDOM GIẢI THƯỜNG VÀ LOẠI TRỪ CÁC GIẢI THƯỞNG TRONG MẢNG PRIZEDISABLES
    // PRIZE_ACTIVES LÀ MẢNG CÁC GIẢI THƯỞNG ĐƯỢC NHẬN NẾU SET VÀ KHÔNG QUAN TÂM ĐẾN PRIZEDISABLES
    setPositionWin() {
        let prizeActives = [...this.prizeValues];
        if(this.prizeActives.length) {  
            prizeActives = this.prizeActives;
        }
        // 
        let random = Math.random();
        let valueWin = prizeActives[Math.floor(random*prizeActives.length)];
        if(this.prizeDisable.includes(valueWin) && !this.prizeActives.length) {
            this.setPositionWin();
        } else {
            let indexWin = this.prizeValues.findIndex(p => p === valueWin);
            this.positionWin = indexWin;
        }
    }
   
    // TÍNH TOÁN GÓC BÙ 360 CỦA LẦN TRÚNG THƯỞNG TRƯỚC ĐÓ 
    get supplementtaryAngle360(): number {
        let value: number;
        // TỶ LỆ GÓC QUAY THỰC TẾ SO VỚI 360 ĐỘ
        let cornerPercentage = this.rotateTransformCss/this.perigon;
        // TÍNH TOÁN GÓC QUAY CỦA LẦN QUAY TRƯỚC
        // NẾU TỶ LỆ > 1 THÌ LẤY PHẦN DƯ*360
        // NẾU TỶ LỆ < 1 THÌ LẤY CHÍNH NÓ * 360 
        // SAU KHI TÍNH XONG THÌ TÍNH GÓC BÙ_360 = 360 - GÓC_ĐÃ_TÍNH_ĐƯỢC
        value = this.perigon - (
                                cornerPercentage > 1 
                                ? (cornerPercentage - Math.floor(cornerPercentage))*this.perigon 
                                : cornerPercentage*this.perigon
                            );
        return value;
    }

    /**
     * 
     * @param file : File có thể là dạng url hoặc là File
     * @returns 
     */
    genUrl = (file: string | File, key?: string): any => {
        if(typeof file === 'string') {
            if(file) {
                if(file.search(this.endPoint) !== -1) {
                    return file;
                } 
            }
            return AppConsts.remoteServiceBaseUrl+'/'+file;
        } else if(file instanceof File) {
            let blobUrl: any = this._helpersService.getBlobUrlImage(file);
            if(key === 'background-image') {
                blobUrl = blobUrl.changingThisBreaksApplicationSecurity;
            }
            return blobUrl;
        }
        return '';
    }

    changeTemplate(key: string, index) {
        this.luckySpinSetting.template = key;
        const propertyChanges = [
            'background', 'banner', 
            'rotationBackground', 'rotationImage', 'needleImage', 
            'iconPlay', 'iconHistory', 'iconRank', 'buttonColor'
        ];
        //
        propertyChanges.forEach(property => {
            this.luckySpinSetting[property] = this.templates[index][property] || this.templateDefault[property];
        });
    }

    close() {

    }

    displayDiv = {
        template: true,
        displayButton: true,
        iconButton: true,
    }

    hiddenDiv(key) {
        this.displayDiv[key] = !this.displayDiv[key];
    }

    zoomValue: number = 100;
    zoomIn() {  
        if(this.zoomValue > 5) {
            this.zoomValue -= 5;
        }
    }

    zoomOut() {
        if(this.zoomValue <= 110) {
            this.zoomValue += 5;
        }
    }

    hoverChanges = {
        buttonPlay: false,
        buttonHistory: false,
        buttonRank: false,
        buttonColor: false,
        prizeContent: false,
        prizeContentBackground: false,
    }
    //
    mouseHover(key: string) {
        this.resetHoverChange();
        this.hoverChanges[key] = true;
    }

    mouseLeave(key: string) {
        this.hoverChanges[key] = false;
    }

    changeStyleColor(key: string) {
        this.resetHoverChange();
        this.hoverChanges[key] = true;
    }

    resetHoverChange() {
        for(const [key, value] of Object.entries(this.hoverChanges)) {
            this.hoverChanges[key] = false;
        }
    }

    async mouseOut() {
        let body = { ...this.luckySpinSetting };
        for(const [key, value] of Object.entries(body)) {
            if(typeof value === 'string' && value.search(this.endPoint) !== -1) {
                body[key] = await this._helpersService.createFileFromPath(value);
            }
        }
        this.luckySpinSettingChange.emit(body);
    }
}

  
