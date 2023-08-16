
export const OJBECT_DISTRIBUTION_CONST = {
	POLICY: "policy",
	POLICY_DETAIL: "policyDetail",
	BASE: {
		DISTRIBUTION: {
			id: 0,
			productId: 0,
			openCellDate: null,
			closeCellDate: null,
			tradingBankAccounts: [],
			tradingBankAccountPays: [],
		},
		POLICY: {
			id: 0,
			distributionId: 0,
			code: null, // Mã chính sách
			name: null, // Tên chính sách
			type: null, // Kiểu chính sách
			minMoney: null, // Số tiền đầu tư tối thổi
			incomeTax: null, // Thuế lợi nhuận
			transferTax: null, // Thuế chuyển nhượng
			isTransferAssets: "N", // Có cho phép chuyển nhượng không
			classify: 1, // Phân loại
			status: 'A', // Trạng thái
			policyDetail: [], // Chi tiết chính sách
			minWithDraw: null,    
			calculateType: null,  
			exitFee: null,   
			exitFeeType: null, 
			startDate: null, // Ngày bắt đầu
			endDate: null, // Ngày hết hiệu lực
			transferAssetsFee: null // Phí rút tiền
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

export const OBJECT_CONFIRMATION_DIALOG = {
	DEFAULT_IMAGE: {
		IMAGE_APPROVE: 'assets/layout/images/icon-dialog/icon-approve-modalDialog.svg',
		IMAGE_CLOSE: 'assets/layout/images/icon-dialog/icon-close-modalDialog.svg',
	},
};

// CÁC TRƯỜNG DÙNG CHUNG KHI TẠO SẢN PHẨM TÍCH LŨY

export const OBJECT_PRODUCT = {
	// THÔNG TIN CHUNG
	GENERAL: {
		// Thông tin chung
		"productType": null,			// Loại sản phẩm
		"name": "",						// Tên sản phẩm
		"code": "",						// Mã sản phẩm
		"startDate": null,				// Ngày bắt đầu
		"endDate": null,				// Ngày kết thúc
		"maxInvestor": null,			// Số KH tối đa
		"minInvestDay": null,			// Số ngày nắm giữ tối thiểu || Số ngày đầu tư tối thiểu
		"countType": null,				// Hình thức tính lãi của TCPH (1: Tính từ ngày phát hành, 2: Tính từ ngày thanh toán)
		"guaranteeOrganization": "",	// Tổ chức bảo lãnh
		"isPaymentGurantee": "",		// Bảo lãnh thanh toán (Y,N)
		"isCollateral": "",				// Tài sản đảm bảo	(Y,N)
		"summary": "",					// Mô tả thao tác
		"icon": "",					// icon
	},
	// THÔNG TIN ĐẦU TƯ CỔ PHẦN
	SHARE : {
		// Thông tin cổ phần tích lũy
		"cpsIssuerId": null,				// Tổ chức phát hành
		"cpsDepositProviderId": null,		// Đại lý lưu ký
		"cpsParValue": null,				// Mệnh giá
		"cpsPeriod": null,					// Kỳ hạn
		"cpsPeriodUnit": "",				// Đơn vị tính kỳ hạn Y,M,D
		"cpsInterestRate": null,			// Cổ tức
		"cpsInterestRateType": null,		// Kiểu trả cổ tức (1: Định kỳ, 2: Cuối kỳ)
		"cpsInterestPeriod": null,			// Số kỳ trả cổ tức
		"cpsInterestPeriodUnit": "",		// Đơn vị (Y,M,D)
		"cpsNumberClosePer": null,			// Số ngày chốt quyền
		"cpsQuantity": null,				// Số lượng cổ phần
		"cpsIsListing": "",					// Niểm yết (Y,N)
		"cpsIsAllowSBD": "",				// Cho phép bán lại trước hạn (Y,N)
	},
	// THÔNG TIN ĐẦU TƯ BĐS
	INVEST : {
		"invOwnerId": null,					// Chủ đầu tư
		"invGeneralContractorId": null,		// Tổng thầu thi công
		"invTotalInvestmentDisplay": null,	// Tổng mức đầu tư
		"invTotalInvestment": null,			// Hạn mức đầu tư
		"invArea": "",						// Diện tích
		"invLocationDescription": "",		// Mô tả vị trí
		"invLatitude": "",					// Kinh độ
		"invLongitude": "",					// Vĩ độ
		"invProductTypes": [],				// Loại hình dự án (1: Nhà riêng, 2: căn hộ chung cư, 3: Nhà phố, biệt thự dự án, 4: Đất nền dự án, 5: Biệt thự nghỉ dưỡng, 6: Condotel, 7: Shophouse, 8: Officetel)
	}
}

export const OBJECT_ORDER = {
	STEP: {
		"cifCode": null,
        "distributionId": null,
        "policyId": null,
        "policyDetailId": null,
        "productId": null,
        "totalValue": null,
        "bankAccId": null,
        "contractAddressId": null,
        "saleReferralCode": null,
        //
        "keyword": '',
        "customerInfo": null,
        "saleInfo": null,
        "listBank": [],
        "listAddress": [],
        "activeIndex": 0,
        "isInvestor": true,
	},

	CREATE: {
		"cifCode": null,
		"distributionId": null,
		"policyId": null,
		"policyDetailId": null,
		"productId": null,
		"totalValue": null,
		"businessCustomerBankAccId": null,
		"investorBankAccId": null,
		"contractAddressId": null,
		"saleReferralCode": null,
		//
		"departmentId": null,
	}, 

	UPDATE: {
		"id": 0,
		"cifCode": null,
		"policyId": null,
		// "policyDetailId": null,
		"totalValue": null,
		"businessCustomerBankAccId": null,
		"investorBankAccId": null,
		"contractAddressId": null,
		"saleReferralCode": null,
	},
}
