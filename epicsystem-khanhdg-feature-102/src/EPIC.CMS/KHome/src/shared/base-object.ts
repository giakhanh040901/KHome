export const OJBECT_SECONDARY_CONST = {
	POLICY: "policy",
	POLICY_DETAIL: "policyDetail",
	BASE: {
		SECODARY: {
			bondSecondaryId: 0,
			bondPrimaryId: 0,
			quantity: null,
			isClose: "N",
			openCellDate: null,
			closeCellDate: null,
			businessCustomerBankAccId: 0,
			status: "N",
			policies: [],
		},
		POLICY: {
			bondPolicyId: 0,
			fakeId: 0,
			tradingProviderId: 0,
			bondPrimaryId: 0,
			bondSecondaryId: 0,
			code: null, // Mã chính sách
			name: null, // Tên chính sách
			type: null, // Kiểu chính sách
			investorType: null, // Loại nhà đầu tư
			minMoney: null, // Số tiền đầu tư tối thổi
			incomeTax: null, // Thuế lợi nhuận
			transferTax: null, // Thuế chuyển nhượng
			isTransfer: "N", // Có cho phép chuyển nhượng không
			classify: 1, // Phân loại
			status: null, // Trạng thái
			details: [], // Chi tiết chính sách
		},
		POLICY_DETAIL: {
			id: 0,
			fakeId: 0,
			tradingProviderId: 0,
			bondSecondaryId: 0,
			policyId: 0,
			policyIndex: 0,
			stt: null, // Số thứ tự
			code: null, // Mã chính sách
			shortName: "", // Tên tắt
			interestType: 1, // Kiểu trả lãi
			interestPeriodQuantity: null, // Số kỳ trả
			interestPeriodType: null, // Kiểu kỳ trả lãi
			profit: null, // Lợi nhuận
			interestDays: null, // Số ngày
			isShowApp: "Y", //
			name: null, // Tên kỳ hạn
			status: null, // Trạng thái
		},
	}
};
export const OJBECT_DISTRIBUTION_CONST = {
	POLICY: "policy",
	POLICY_DETAIL: "policyDetail",
	BASE: {
		DISTRIBUTION: {
			id: 0,
			projectId: 0,
			isClose: "N",
			openCellDate: null,
			closeCellDate: null,
			businessCustomerBankAccId: 0,
			policies: [],
			tradingProviderId: 0,
		},
		POLICY: {
			id: 0,
			distributionId: 0,
			code: null,   // Mã chính sách
			name: null,   // Tên chính sách
			minMoney: null,  // Số tiền đầu tư tối thiểu
			type: null,   // Kiểu chính sách
			minInvestDay: null, // Số ngày đầu tư tối thiểu
			incomeTax: null,  // Thuế lợi nhuận
			calculateType: null,  // Cách tính lợi tức
			policyDisplayOrder: null, // Thứ tự hiển thị
			minTakeContract: null, // Gửi yêu cầu nhận hợp đồng từ (VND) 
			description: null, // Mô tả
			minWithDraw: null, // Số tiền rút vốn tối thiểu 
			maxWithDraw: null, // Số tiền rút vốn tối đa
			exitFee: null,  // Phí rút tiền (%) 
			exitFeeType: null, // Kiểu của phí rút tiền
			transferTax: null, // Phí chuyển đổi tài sản (%)
			remindRenewals: null, // Nhắc tái tục trước (ngày)
			expirationRenewals: null, // Gửi yêu cầu tái tục trước (ngày)
			renewalsType: null, // Loại hợp đồng tái tục
			classify: 1, // Kiểu chính sách
			isTransfer: "N",  // Chuyển đổi tài sản
			policyDetail: [], // Chi tiết chính sách
			startDate: null, // Ngày bắt đầu
			endDate: null, // Ngày hết hiệu lực
			status: 'A', // Trạng thái
		},
		POLICY_DETAIL: {
			id: 0,
			distributionId: 0,
			policyId: 0,
			stt: null, // Số thứ tự
			name: null, // Tên kỳ hạn
			shortName: null, // Tên tắt
			periodQuantity: null, // Số kỳ trả
			periodType: null, // Kiểu kỳ trả lãi
			profit: null, // Lợi nhuận
			interestDays: null, // Số ngày
			status: 'A', // Trạng thái
			interestPeriodQuantity: null,
		},
	}
};

export const OBJECT_INVESTOR_EKYC = {
	DEFAULT_IMAGE: {
		IMAGE_FRONT: 'assets/layout/images/front-image.png',
		IMAGE_BACK: 'assets/layout/images/back-image.png',
		IMAGE_PASSPORT: 'assets/layout/images/front-image.png',
		IMAGE_AVATAR: 'assets/layout/images/avatar.png',
		IMAGE_PROJECT: 'assets/layout/images/project-img-default.jpg',
		IMAGE_STRUCTURE: 'assets/layout/images/structure-default.jpg',
	},
	MODAL_EKYC_TYPE: {
		ADD_IDENTIFICATION: 'ADD_IDENTIFICATION',
		CREATE_INVESTOR: 'CREATE_INVESTOR',
	}
};

export const OBJECT_CONFIRMATION_DIALOG = {
	DEFAULT_IMAGE: {
		IMAGE_APPROVE: 'assets/layout/images/icon-dialog/icon-approve-modalDialog.svg',
		IMAGE_CLOSE: 'assets/layout/images/icon-dialog/icon-close-modalDialog.svg',
	},
};

export const DEFAULT_MEDIA_RST = {
	DEFAULT_IMAGE: {
		IMAGE_ADD: 'assets/layout/images/add-image-bg.png'
	},
	DEFAULT_ICON: {
		ICON_DEFAULT: 'assets/layout/images/default-icon.png'
	}

}

export const OBJECT_ORDER = {
	STEP: {
		"cifCode": null,
        "bankAccId": null,
        "contractAddressId": null,
        "saleReferralCode": null,
        "keyword": '',
        "customerInfo": null,
		"productInfo": null,
        "saleInfo": null,
        "listBank": [],
        "listAddress": [],
        "activeIndex": 0,
        "isInvestor": true,
		"jointOwnerInfo": null,
		"foreignJointOwnership": null,
	},

	CREATE: {
		"cifCode": null,
		"contractAddressId": null,
		"saleReferralCode": null,
		"openSellId": null,
		"paymentType": null,
		"openSellDetailId": null,
	}, 

	UPDATE: {
		"id": 0,
		// "cifCode": null,
		"paymentType": null,
		"openSellDetailId":0,
		"contractAddressId": null,
		"saleReferralCode": null,
		"rstOrderCoOwners": null
	},
}
