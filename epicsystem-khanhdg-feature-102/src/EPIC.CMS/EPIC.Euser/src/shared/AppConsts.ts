export class AppConsts {
    static remoteServiceBaseUrlLocal: string;
    static remoteServiceBaseUrl: string;
	static rocketchatUrl: string;
	static rocketchat = {
		api: '',
		iframeSrc: '',
	};
    
    static baseUrlHome: string;
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish
    static redicrectHrefOpenDocs = "https://docs.google.com/viewerng/viewer?url="; 

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
}

export class AppPermissionNames {
    // user
    public readonly Pages_Users = "Pages.Users";
    public readonly Actions_Users_Create = "Actions.Users.Create";
    public readonly Actions_Users_Update = "Actions.Users.Update";
    public readonly Actions_Users_Delete = "Actions.Users.Delete";
    public readonly Actions_Users_Activation = "Actions.Users.Activation";

    // role
    public readonly Pages_Roles = "Pages.Roles";
    public readonly Actions_Roles_Create = "Actions.Roles.Create";
    public readonly Actions_Roles_Update = "Actions.Roles.Update";
    public readonly Actions_Roles_Delete = "Actions.Roles.Delete";
}

export class PermissionTypes {
    // user
    public static readonly Web = 1;
    public static readonly Menu = 2;
    public static readonly Page = 3;
    public static readonly Table = 4;
    public static readonly Tab = 5;
    public static readonly Form = 6;
    public static readonly ButtonTable = 7;
    public static readonly ButtonAction = 8;
    public static readonly ButtonForm = 9;
}

export class WebKeys {
    //
    public static readonly Core = 1;
    public static readonly Bond = 2;
    public static readonly Invest = 3;
    public static readonly Support = 4;
    public static readonly User = 5;
    public static readonly Garner = 6;
    public static readonly RealState = 7;
    public static readonly Loyalty = 8;
    public static readonly Event = 9;
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
export class PartnerConst {
    public static status = [
        {
            name: 'Kích hoạt',
            code: 'A',
            severity: 'success',
        },
        {
            name: 'Không kích hoạt',
            code: 'D',
            severity: 'secondary',
        },
        {
            name: 'Đóng',
            code: 'C',
            severity: 'danger',
        },
    ];

    public static STATUS_ACTIVE = 'A';
    public static STATUS_DISABLE = 'D';
    public static STATUS_CLOSE = 'C';

    public static getStatusInfo(code, field) {
        let status = this.status.find(status => status.code == code);
        if (status) return status[field];
        return '';
    }
}

export class UserTypes {
    //
    public static list = [
        {
            name: 'Epic Root',
            code: 'RE',
            severity: '',
        },
        {
            name: 'Epic',
            code: 'E',
            severity: '',
        },
        {
            name: 'Đối tác root',
            code: 'RP',
            severity: '',
        },
        {
            name: 'Đối tác',
            code: 'P',
            severity: '',
        },
        {
            name: 'Đại lý Root',
            code: 'RT',
            severity: '',
        },
        {
            name: 'User',
            code: 'T',
            severity: '',
        },
    ];

    public static EPIC_ROOT = 'RE';
    public static EPIC = 'E';
    public static PARTNER_ROOT = 'RP';
    public static PARTNER = 'P';
    public static TRADING_PROVIDER_ROOT = 'RT';
    public static TRADING_PROVIDER = 'T'; // User thuộc đại lý (Do đại lý tạo)
    // TYPE GROUP
    public static TYPE_EPICS = ['E', 'RE'];  // PARTNERROOT HOẶC TRADINGROOT
    public static TYPE_PARTNERS = ['P', 'RP'];  // PARTNERROOT HOẶC PARTNER
    public static TYPE_TRADING = ['T', 'RT'];  // PARTNERROOT HOẶC TRADINGROOT
    public static TYPE_ROOTS = ['RP', 'RT', 'RE'];  // PARTNERROOT HOẶC TRADINGROOT

    
    

    public static getUserTypeInfo(code, property) {
        let type = this.list.find(t => t.code == code);
        if (type) return type[property];
        return '';
    }

}

export const KeyFilter = {
    blockSpecial: new RegExp(/^[^~!@#$%^&*><:;+=_]+$/),
    numberOnlyBlockSpecial: new RegExp(/^[^\sA-Za-záàảãạâấầẩẫậăắằẳẵặóòỏõọôốồổỗộơớờởỡợéèẻẽẹêếềểễệúùủũụưứừửữựíìỉĩịýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÉÈẺẼẸÊẾỀỂỄỆÚÙỦŨỤƯỨỪỬỮỰÍÌỈĨỊÝỲỶỸỴĐ~!@#$%^&*><:;+=_,/-]+$/),
    stringOnlyBlockSpecial: new RegExp(/^[^0-9~!@#$%^&*><:;+=_,/-]+$/),
    decisionNoBlockSpecial: new RegExp(/^[^\sáàảãạâấầẩẫậăắằẳẵặóòỏõọôốồổỗộơớờởỡợéèẻẽẹêếềểễệúùủũụưứừửữựíìỉĩịýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÉÈẺẼẸÊẾỀỂỄỆÚÙỦŨỤƯỨỪỬỮỰÍÌỈĨỊÝỲỶỸỴĐ~!@#$%^&*><:;+=_,]+$/),
}

export class SearchConst {
    public static DEBOUNCE_TIME = 800;
}

export class UserConst {

	public static STATUS = {
		DEACTIVE: 'D',
		ACTIVE: 'A',
		TEMPORARY: 'T',
	}

    public static STATUS_YES = 'Y';
	public static STATUS_NO = 'N'; 
    public static STATUS_DELETE = 'X'; 

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

export class ApproveConst {
    public static dataType = [
        {
            name: 'Người dùng',
            code: 1,
        },
        {
            name: 'Nhà đầu tư',
            code: 2,
        },
        {
            name: 'Khách hàng doanh nghiệp',
            code: 3,
        },
        {
            name: 'Lô trái phiếu',
            code: 4,
        },
        {
            name: 'Bán theo kỳ hạn',
            code: 5,
        },
        {
            name: 'Tệp tin hợp đồng',
            code: 6,
        },
        {
            name: 'Sale',
            code: 8,
        },
        {
            name: 'Nhà đầu tư chuyên nghiệp',
            code: 10,
        },
    ];
    public static STATUS_USER = 1;
    public static STATUS_INVESTOR = 2;
    public static STATUS_BUSINESS_CUSTOMER = 3;
    public static STATUS_PRO_BOND_INFO = 4;
    public static STATUS_PRO_BOND_SECONDARY = 5;
    public static STATUS_DISTRI_CONTRACT_FILE = 6;
    public static STATUS_SALE = 8;
    public static STATUS_INVESTOR_PRO = 10;

    public static getDataTypesName(code) {
        let dataType = this.dataType.find(dataType => dataType.code == code);
        if (dataType) return dataType.name;
        return '';
    }

    public static status = [
        {
            name: 'Trình duyệt',
            code: 1,
            severity: 'warning',
        },
        {
            name: 'Đã duyệt',
            code: 2,
            severity: 'success',
        },
        {
            name: 'Hủy',
            code: 3,
            severity: 'danger',
        },
    ];

    public static statusConst = [
        {
            name: 'Trình duyệt',
            code: 1,
            severity: 'warning',
        },
        {
            name: 'Đã duyệt',
            code: 2,
            severity: 'success',
        },
        {
            name: 'Hủy',
            code: 3,
            severity: 'danger',
        },

    ];

    public static STATUS_ACTIVE = 1;
    public static STATUS_DISABLE = 2;
    public static STATUS_CLOSE = 3;

    public static getStatusSeverity(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }

    public static getStatusName(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static actionType = [
        {
            name: 'Thêm',
            code: 1,
            severity: 'success',
        },
        {
            name: 'Sửa',
            code: 2,
            severity: 'warning',
        },
        {
            name: 'Xoá',
            code: 3,
            severity: 'danger',
        },
    ];
    public static actionTypeApprove = [
        {
            name: 'Thêm',
            code: 1,
            severity: 'success',
        },
        {
            name: 'Sửa',
            code: 2,
            severity: 'warning',
        }
    ];

    public static getActionTypeSeverity(code) {
        let actionType = this.actionType.find(actionType => actionType.code == code);
        if (actionType) return actionType.severity;
        return '';
    }
    public static ACTION_ADD = 1;
    public static ACTION_UPDATE = 2;
    public static ACTION_DELETE = 3;

    public static getActionTypeName(code) {
        let actionType = this.actionType.find(actionType => actionType.code == code);
        if (actionType) return actionType.name;
        return '';
    }

    //
    // public static types = [
    //     {
    //         name: 'Tổ chức',
    //         code: 'B',
    //     },
    //     {
    //         name: 'Cá nhân',
    //         code: 'I',
    //     },

    // ];

    // public static getNameType(code) {
    //     let type = this.types.find(type => type.code == code);
    //     if(type) return type.name;
    //     return '';
    // }
}

export class BusinessCustomerApproveConst {
    public static status = {
        KHOI_TAO: 1,
        CHO_DUYET: 2,
        DA_DUYET: 3,
        HUY_DUYET: 4,
    }

    public static statusList = [
        {
            name: 'Khởi tạo',
            severity: 'help',
            code: this.status.KHOI_TAO,
        },
        {
            name: 'Trình duyệt',
            severity: 'warning',
            code: this.status.CHO_DUYET,
        },
        {
            name: 'Đã duyệt',
            code: this.status.DA_DUYET,
            severity: 'success',
        },
        {
            name: 'Huỷ duyệt',
            code: this.status.HUY_DUYET,
            severity: 'danger',
        }
    ];

    public static getStatusName(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusSeverity(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.severity;
        }
        return '';
    }
    // Bộ lọc tìm kiếm nhập keyword theo loại 
    public static fieldFilters = [
        {
            name: 'Mã số thuế',
            code: 2,
            field: 'keyword',
            placeholder: 'Nhập mã số thuế...',
        },
        {
            name: 'Số điện thoại',
            code: 3,
            field: 'phone',
            placeholder: 'Nhập số điện thoại...',
        },
        {
            name: 'Email',
            code: 4,
            field: 'email',
            placeholder: 'Nhập email...',
        },
        {
            name: 'Tên doanh nghiệp',
            code: 1,
            field: 'name',
            placeholder: 'Nhập tên doanh nghiệp...',
        },
    ];
    
    public static getInfoFieldFilter(field, attribute: string) {
        const fieldFilter = this.fieldFilters.find(fieldFilter => fieldFilter.field == field);
        return fieldFilter ? fieldFilter[attribute] : null;
    }

}

export class MessageErrorConst {

    public static message = {
        Error: "ACTIVE",
        Validate: "Vui lòng nhập đủ thông tin cho các trường có dấu (*)",
        DataNotFound: "Không tìm thấy dữ liệu!"
    }
    
}

export class BusinessCustomerConst {
    public static status = {
        HOAT_DONG: 2,
        HUY_DUYET: 3,
    }

    public static isCheckConst = [
        {
            name: 'Đã kiểm tra',
            code: 'Y'

        },
        {
            name: "Chưa kiểm tra",
            code: 'N'
        }
    ]

    public static statusList = [

        {
            name: 'Hoạt động',
            code: this.status.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.status.HUY_DUYET,
            severity: 'danger',
        },
    ];

    public static getStatusName(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusSeverity(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.severity;
        }
        return '';
    }
}

export class FormNotificationConst{
    public static IMAGE_APPROVE = "IMAGE_APPROVE";
    public static IMAGE_CLOSE = "IMAGE_CLOSE";
}

export class YesNoConst {
    public static list = [
        {
            name: 'Có',
            code: 'Y',
        },
        {
            name: 'Không',
            code: 'N',
        },
    ]

    public static getName(code) {
        for (let item of this.list) {
            if (item.code == code) return item.name;
        }
        return '-';
    }

    public static STATUS_YES = 'Y';
    public static STATUS_NO = 'N';
}

export class ContractTypeConst {
    public static list = [
        {
            name: 'PNOTE',
            code: 1,
        },
        {
            name: 'PRO',
            code: 2,
        },
        {
            name: 'PROA',
            code: 3,
        }
    ];

    public static PNOTE = 1;
    public static PRO = 2;
    public static PROA = 3;

    public static getName(code: Number) {
        const rslt = this.list.find(e => e.code.toString() === code.toString());
        return rslt ? rslt.name : '-';
    }
}

export class AccountConst {
    // public static statusName = {
    //     A: { name: "Hoạt động", color: "success" },
    //     D: { name: "Đang khóa", color: "secondary" }
    // }
    public static status = [
        {
            name: 'Hoạt động',
            code: 'A',
            severity: 'success'
        },
        {
            name: 'Đang khóa',
            code: 'D',
            severity: 'secondary'
        }
    ];

    public static getInfoStatus(code, field) {
        const source = this.status.find(type => type.code == code);
        return source ? source[field] : null;
    }
}

export class ActiveDeactiveConst {

    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';
    public static DELETE = 'X';
    public static list = [
        {
            name: 'Kích hoạt',
            code: this.ACTIVE,
            severity: 'success',

        },
        {
            name: 'Khóa',
            code: this.DEACTIVE,
            severity: 'secondary',
                
        },
        {
            name: 'Đã xóa',
            code: this.DELETE,
            severity: 'danger',
                
        },
    ];

    public static listStatus = [
        // {
        //     name: 'Tất cả',
        //     code: null,
        //     severity: 'danger',
        // },
        {
            name: 'Kích hoạt',
            code: this.ACTIVE,
            severity: 'success',

        },
        {
            name: 'Khóa',
            code: this.DEACTIVE,
            severity: 'secondary',
                
        },
    ];

    public static getInfo(code, atribution = 'name') {
        let status = this.list.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static getInfoStatus(code, atribution = 'name') {
        let status = this.listStatus.find(s => s.code == code);
        return status ? status[atribution] : null;
    }
}

export class RoleTypeConst{
    public static MAC_DINH = 1;
    public static PARTNER = 2;
    public static TRADING_PROVIDER = 3;
    public static EPIC = 4;


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