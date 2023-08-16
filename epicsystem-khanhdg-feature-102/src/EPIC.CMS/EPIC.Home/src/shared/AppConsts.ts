export class AppConsts {
    static remoteServiceBaseUrlLocal: string;
    static remoteServiceBaseUrl: string;
	static rocketchatUrl: string;
	static rocketchat = {
		api: '',
		iframeSrc: '',
	};
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish
    // 
    static baseUrlEuser: string; 
    static redirectUrlEuser: string; 
    static user = 'User';
    //
    static baseUrlEcore: string; 
    static redirectUrlEcore: string; 
    static core = 'Core';
    //
    static baseUrlEbond: string; 
    static redirectUrlEbond: string; 
    static bond = 'Bond';
    //
    static baseUrlEinvest: string; 
    static redirectUrlEinvest: string;
    static invest = 'Invest'
    //
    static baseUrlEgarner: string; 
    static redirectUrlEgarner: string;
    static garner = 'Garner'
    //
    static baseUrlRealEState: string; 
    static redirectUrlRealEState: string;
    static realEState = 'RealEState'
    //
    static baseUrlEloyalty: string; 
    static redirectUrlEloyalty: string;
    static loyalty = 'Loyalty'
    //
    static baseUrlEsupport: string; 
    static redirectUrlEsupport: string;
    static support = 'Support'
    //
    static baseUrlEvents: string; 
    static redirectUrlEvents: string;
    static events = 'Events'
    //
    static readonly grantType = {
        password: 'password',
        refreshToken: 'refresh_token',
    };

    static clientId: string;
    static clientSecret: string;

    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly authorization = {
        accessToken: 'access_token',
        refreshToken: 'refresh_token',
        encryptedAuthTokenName: 'enc_auth_token'
    };

    static readonly localRefreshAction = {
        setToken: 'setToken',
        doNothing: 'doNothing',
    }

    static readonly folder = 'home';
    static defaultAvatar = "assets/layout/images/topbar/anonymous-avatar.jpg";
}

export class PermissionWebConst {
    private static readonly Web: string = "web_";
    public static readonly CoreModule: string = "core.";
    public static readonly BondModule: string = "bond.";
    public static readonly InvestModule: string = "invest.";
    public static readonly SupportModule: string = "support.";
    public static readonly UserModule: string = "user.";
    public static readonly GarnerModule: string = "garner.";

    public static readonly CoreWebsite: string = `${PermissionWebConst.CoreModule}${PermissionWebConst.Web}`;
    public static readonly BondWebsite: string = `${PermissionWebConst.BondModule}${PermissionWebConst.Web}`;
    public static readonly InvestWebsite: string = `${PermissionWebConst.InvestModule}${PermissionWebConst.Web}`;
    public static readonly SupportWebsite: string = `${PermissionWebConst.SupportModule}${PermissionWebConst.Web}`;
    public static readonly UserWebsite: string = `${PermissionWebConst.UserModule}${PermissionWebConst.Web}`;
    public static readonly GarnerWebsite: string = `${PermissionWebConst.GarnerModule}${PermissionWebConst.Web}`;
}

export class DataTableConst {
    public static message = {
        emptyMessage: 'Không có dữ liệu',
        totalMessage: 'bản ghi',
        selectedMessage: 'chọn'
    }
}

export class StatusResponseConst {
    public static list = [
        {
            value: false,
            status: 0,
        },
        {
            value: true,
            status: 1,
        },
    ]

    public static RESPONSE_TRUE = 1;
    public static RESPONSE_FALSE = 0;

}

export const KeyFilter = {
    blockSpecial: new RegExp(/^[^~!@#$%^&*><:;+=_]+$/),
    numberOnlyBlockSpecial: new RegExp(/^[^\sA-Za-záàảãạâấầẩẫậăắằẳẵặóòỏõọôốồổỗộơớờởỡợéèẻẽẹêếềểễệúùủũụưứừửữựíìỉĩịýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÉÈẺẼẸÊẾỀỂỄỆÚÙỦŨỤƯỨỪỬỮỰÍÌỈĨỊÝỲỶỸỴĐ~!@#$%^&*><:;+=_,/-]+$/),
    stringOnlyBlockSpecial: new RegExp(/^[^0-9~!@#$%^&*><:;+=_,/-]+$/),
    decisionNoBlockSpecial: new RegExp(/^[^\sáàảãạâấầẩẫậăắằẳẵặóòỏõọôốồổỗộơớờởỡợéèẻẽẹêếềểễệúùủũụưứừửữựíìỉĩịýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÉÈẺẼẸÊẾỀỂỄỆÚÙỦŨỤƯỨỪỬỮỰÍÌỈĨỊÝỲỶỸỴĐ~!@#$%^&*><:;+=_,]+$/),
}

export class UserConst {

	public static STATUS = {
		DEACTIVE: 'D',
		ACTIVE: 'A',
		TEMPORARY: 'T',
	}

	public static STATUS_YES = 'Y';
	public static STATUS_NO = 'N'; 
		
	public static STATUS_NAME = {
		[this.STATUS.DEACTIVE]: 'Bị khoá',
		[this.STATUS.ACTIVE]: 'Hoạt động',
		[this.STATUS.TEMPORARY]: 'Tạm',
	}

	public static STATUS_SEVERITY = {
		[this.STATUS.DEACTIVE]: 'cancel',
		[this.STATUS.ACTIVE]: 'success',
		[this.STATUS.TEMPORARY]: 'warning',
	}

}

export class BondSecondaryConst {

    public static getAllowOnlineTradings(code) {
        for (let item of this.allowOnlineTradings) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static types = [
        { name: "Cá nhân", code: "I" },
        { name: "Doanh nghiệp", code: "B" },
    ]  

    public static allowOnlineTradings = [
        {
            name: 'Cho phép',
            code: 'Y'
        },
        {
            name: 'Không cho phép',
            code: 'N'
        }
    ];

    public static STATUS = {
        NHAP: 1,
        TRINH_DUYET: 2,
        HOAT_DONG: 3,
        CANCEL: 4,
        CLOSED: 5,
    }

    public static KICH_HOAT = 'A';
    public static KHOA = 'D';

    public static statusList = [
        {
            name: 'Khởi tạo',
            severity: 'help',
            code: this.STATUS.NHAP,
        },
        {
            name: 'Trình duyệt',
            code: this.STATUS.TRINH_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hoạt động',
            code: this.STATUS.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.STATUS.CANCEL,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.STATUS.CLOSED,
            severity: 'secondary'
        }
    ];

    public static getStatusName(code, isClose) {
        if (isClose == 'Y') return 'Đóng';
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusSeverity(code, isClose) {
        if (isClose == 'Y') return 'secondary';
        for (let item of this.statusList) {
            if (item.code == code) return item.severity;
        }
        return '';
    }

    public static getType(code) {
        for (let item of this.types) {
            if (item.code == code) return item.name;
        }
        return '';
    }

}

export const sloganWebConst = [
    // "Việc gì khó, đã có EMIR lo!",
    "Đừng bao giờ sợ thất bại. Bạn chỉ cần đúng có một lần trong đời thôi - CEO của Starbucks!",
    "Luôn bắt đầu với mong đợi những điều tốt đẹp sẽ xảy ra – Tom Hopkins!",
    "Nơi nào không có cạnh tranh, nơi đó không có thị trường!",
    "Kẻ chiến thắng không bao giờ bỏ cuộc; kẻ bỏ cuộc không bao giờ chiến thắng!",
    // "Nhất cận thị, nhì cận giang, muốn giàu sang thì…cận sếp!",
    "Công việc quan trọng nhất luôn ở phía trước, không bao giờ ở phía sau bạn!",
    "Chúng ta có thể gặp nhiều thất bại, nhưng chúng ta không được bị đánh bại!",
    "Điều duy nhất vượt qua được vận may là sự chăm chỉ. – Harry Golden!",
    "Muốn đi nhanh thì đi một mình. Muốn đi xa thì đi cùng nhau!",
    "Đôi lúc bạn đối mặt với khó khăn không phải vì bạn làm điều gì đó sai mà bởi vì bạn đang đi đúng hướng!",
    "Điều quan trọng không phải vị trí đứng mà hướng đi. Mỗi khi có ý định từ bỏ, hãy nghĩ đến lý do mà bạn bắt đầu!",
    "Cách tốt nhất để dự đoán tương lai là hãy tạo ra nó!",
    "Kỷ luật là cầu nối giữa mục tiêu và thành tựu!",
    "Di chuyển nhanh và phá vỡ các quy tắc. Nếu vẫn chưa phá vỡ cái gì, chứng tỏ bạn di chuyển chưa đủ nhanh!",
    "Đằng nào thì bạn cũng phải nghĩ, vì vậy sao không nghĩ lớn luôn? - Donald Trump!",
    "Những khách hàng không hài lòng sẽ là bài học tuyệt vời cho bạn - Bill Gates!",
    "Nếu mọi người thích bạn, họ sẽ lắng nghe bạn, nhưng nếu họ tin tưởng bạn, họ sẽ làm kinh doanh với bạn!",
    "Hoàn cảnh thuận lợi luôn chứa đựng những yếu tố nguy hiểm. Hoàn cảnh khó khăn luôn giúp ta vững vàng hơn!",
];