export class CreateOrEditPrizeDraw {
    public id?: number = undefined;
    public name?: string = '';
    public code?: string = '';
    public startDate: any = '';
    public endDate: any = '';
    public descriptionContentType?: string = 'MARKDOWN';
    public descriptionContent?: string = '';
    public avatarImageUrl: File | string = undefined;
    public joinTimeSetting?: number = undefined;
    public numberOfTurn?: number = undefined;
    public startTurnDate?: any = '';
    public resetTurnQuantity?: number = undefined;
    public resetTurnType?: string = ''
    public maxNumberOfTurnByIp?: number = undefined;
    public luckyScenarios?: CreateOrEditLuckyScenario[] = [];
}

export class CreateOrEditLuckyScenario{
    public id?:number | undefined= undefined;
    public name?:string = '';
    public prizeQuantity?: number = undefined;
    public avatarImageUrl?: File | undefined = undefined;
    public luckyScenarioDetails?: LuckyScenarioDetail[] = [];
    public luckyRotationInterface?:LuckyRotationInterface;
    public status?:string = '';
}

export class LuckyScenarioDetail{ 
    public voucherId?: number | undefined = undefined;
    public sortOrder?: number | undefined = undefined;
    public name?: string = '';
    public quantity?: number | undefined = undefined;
    public probability?: number | undefined = undefined;
}

export class LuckyRotationInterface{
    buttonPlay?:boolean = true;   // Nút chơi game
    buttonHistory?:boolean = true;    // Nút lịch sử
    buttonRank?: boolean = true;   // Nút xếp hạng
    buttonColor?:string = '';    // Mã màu các nút button Chơi game; lịch sử; xếp hạng
    iconHome?: boolean = true; // Hiện icon Home hay ko
    winText?: boolean = true;  // Hiện thông báo trúng thưởng hay không
    showBanner?: boolean = true;   // Hiện banner hay ko
    iconTopColorWhite?:boolean = false;   // Bỏ qua
    iconPlay?:any = '';   // ảnh icon nút Chơi game
    iconHistory?:any = '';    // ảnh icon nút lịch sử chơi
    iconRank?:any = '';   // ảnh icon nút xếp hạng
    winTextColor?:string = '#ffffff'; // string - mã màu chữ thông báo trúng thưởng
    winTextBackgroundColor?:string = '#ffffff4d'; // string - mã màu nền thông báo trúng thưởng
    banner?:any = ''; // ảnh banner
    rotationImage?:any = '';  // ảnh vòng quay
    rotationBackground?:any = ''; // ảnh nền vòng quay
    needleImage?:any = ''; // Ảnh kim quay
    background?:any = ''; // Hình nền
    template?:string = '';   // string 
}