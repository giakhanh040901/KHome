using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Contract
{
    public class PropertiesContractFile
    {
        #region Thông tin nhà đầu tư
        /// <summary>
        /// Tên nhà đầu tư (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_NAME = "{{CustomerName}}";

        /// <summary>
        /// Số giấy tờ (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_ID_NO = "{{CustomerIdNo}}";

        /// <summary>
        /// Loại giấy tờ (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_ID_TYPE = "{{CustomerIdType}}";

        /// <summary>
        /// Ngày cấp giấy tờ (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_ID_DATE = "{{CustomerIdDate}}";

        /// <summary>
        /// Nơi cấp giấy tờ (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_ID_ISSUER = "{{CustomerIdIssuer}}";

        /// <summary>
        /// Ngày hết hạn giấy tờ (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_ID_EXPIRED_DATE = "{{CustomerIdExpiredDate}}";

        /// <summary>
        /// Số điện thoại (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_PHONE = "{{CustomerPhone}}";

        /// <summary>
        /// Giới tính (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_SEX = "{{CustomerSex}}";

        /// <summary>
        /// Ngày sinh (người đại diện đối với doanh nghiệp)
        /// </summary>
        public const string CUSTOMER_BIRTH_DATE = "{{CustomerBirthDate}}";

        /// <summary>
        /// Địa chỉ 
        /// </summary>
        public const string CUSTOMER_RESIDENT_ADDRESS = "{{CustomerResidentAddress}}";

        /// <summary>
        /// EMAIL 
        /// </summary>
        public const string CUSTOMER_EMAIL = "{{CustomerEmail}}";

        /// <summary>
        /// Mã số thuế
        /// </summary>
        public const string CUSTOMER_TAX_CODE = "{{CustomerTaxCode}}";

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public const string CUSTOMER_BANK_ACC_NO = "{{CustomerBankAccNo}}";

        /// <summary>
        /// chủ tài khoản
        /// </summary>
        public const string CUSTOMER_BANK_ACC_NAME = "{{CustomerBankAccName}}";

        /// <summary>
        /// Tên viết tắt ngân hàng
        /// </summary>
        public const string CUSTOMER_BANK_NAME = "{{CustomerBankName}}";

        /// <summary>
        /// Tên đầy đủ ngân hàng
        /// </summary>
        public const string CUSTOMER_BANK_FULL_NAME = "{{CustomerBankFullName}}";

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public const string CUSTOMER_BANK_BRANCH = "{{CustomerBankBranch}}";

        /// <summary>
        /// Quốc tịch
        /// </summary>
        public const string CUSTOMER_NATIONNALITY = "{{CustomerNationality}}";

        /// <summary>
        /// Địa chỉ doanh nghiệp
        /// </summary>
        public const string CUSTOMER_ADDRESS = "{{CustomerAddress}}";

        /// <summary>
        /// Chức vụ người đại diện
        /// </summary>
        public const string CUSTOMER_REP_POSITION = "{{CustomerRepPosition}}";

        /// <summary>
        /// Địa chỉ người đại diện
        /// </summary>
        public const string CUSTOMER_REP_ADDRESS = "{{CustomerRepAddress}}";

        /// <summary>
        /// Địa chỉ trụ sở chính
        /// </summary>
        public const string CUSTOMER_TRADING_ADDRESS = "{{CustomerTradingAddress}}";

        /// <summary>
        /// Tên doanh nghiệp
        /// </summary>
        public const string BUSINESS_CUSTOMER_NAME = "{{BusinessCustomerName}}";

        /// <summary>
        /// Số quyết định
        /// </summary>
        public const string CUSTOMER_DECISION_NO = "{{CustomerDecisionNo}}";

        /// <summary>
        /// Ngày quyết định
        /// </summary>
        public const string CUSTOMER_DECISION_DATE = "{{CustomerDecisionDate}}";

        /// <summary>
        /// Nơi cấp giấy phép
        /// </summary>
        public const string CUSTOMER_LICENSE_ISSUER = "{{CustomerLicenseIssuer}}";

        /// <summary>
        /// Ngày cấp giấy phép
        /// </summary>
        public const string CUSTOMER_LICENSE_DATE = "{{CustomerLicenseDate}}";

        /// <summary>
        /// Địa chỉ hợp đồng
        /// </summary>
        public const string CUSTOMER_CONTRACT_ADDRESS = "{{CustomerContractAddress}}";

        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        public const string CUSTOMER_CONTACT_ADDRESS = "{{CustomerContactAddress}}";
        #endregion

        #region Hợp đồng
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public const string CONTRACT_CODE = "{{ContractCode}}";

        /// <summary>
        /// Ngày đặt lệnh
        /// </summary>
        public const string ORDER_CREATED_DATE = "{{OrderCreatedDate}}";

        /// <summary>
        /// Nội dung giao dịch
        /// </summary>
        public const string TRAN_CONTENT = "{{TranContent}}";

        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public const string TRAN_DATE = "{{TranDate}}";

        /// <summary>
        /// Ngày hợp đồng
        /// </summary>
        public const string DAY_CONTRACT = "{{DayContract}}";

        /// <summary>
        /// Tháng hợp đồng
        /// </summary>
        public const string MONTH_CONTRACT = "{{MonthContract}}";

        /// <summary>
        /// Năm hợp đồng
        /// </summary>
        public const string YEAR_CONTRACT = "{{YearContract}}";

        /// <summary>
        /// Thời gian hợp đồng (ngày, tháng, năm)
        /// </summary>
        public const string CONTRACT_DATE = "{{ContractDate}}";

        /// <summary>
        /// Kỳ hạn nhận lơi tức
        /// </summary>
        public const string INTEREST_PERIOD = "{{InterestPeriod}}";

        /// <summary>
        /// Kỳ hạn nhận lơi tức sau khi tái tục
        /// </summary>
        public const string INTEREST_PERIOD_RENEWALS = "{{InterestPeriodRenewals}}";

        /// <summary>
        /// thời gian đầu tư
        /// </summary>
        public const string TENOR = "{{Tenor}}";
        #endregion

        #region Ký hợp đồng
        /// <summary>
        /// Đã ký
        /// </summary>
        public const string SIGNATURE = "{{Signature}}";

        /// <summary>
        /// Đã ký hợp đồng rút tiền
        /// </summary>
        public const string SIGNATURE_WITHDRAWAL = "{{SignatureWithdrawal}}";

        /// <summary>
        /// Tên người ký
        /// </summary>
        public const string CUSTOMER_NAME_SIGNATURE = "{{CustomerNameSignature}}";
        #endregion

        #region Đại lý
        /// <summary>
        /// Tên đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_NAME = "{{TradingProviderName}}";

        /// <summary>
        /// Avatar đại lý
        /// </summary>
        public const string TRADING_PROVIDER_AVATAR = "{{TradingProviderAvatar}}";

        /// <summary>
        /// Tên viết tắt đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_SHORT_NAME = "{{TradingProviderShortName}}";

        /// <summary>
        /// Mã số thuế đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_TAX_CODE = "{{TradingProviderTaxCode}}";

        /// <summary>
        /// Số điện thoại đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_PHONE = "{{TradingProviderPhone}}";

        /// <summary>
        /// Số fax đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_FAX = "{{TradingProviderFax}}";

        /// <summary>
        /// Nơi cấp giấy phép đăng ký kinh doanh đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_LICENSE_ISSUER = "{{TradingProviderLicenseIssuer}}";

        /// <summary>
        /// Ngày cấp đăng ký kinh doanh đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_LICENSE_DATE = "{{TradingProviderLicenseDate}}";

        /// <summary>
        /// Địa chỉ đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_ADDRESS = "{{TradingProviderAddress}}";

        /// <summary>
        /// Họ tên người đại diện của đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_REP_NAME = "{{TradingProviderRepName}}";

        /// <summary>
        /// Chức vụ người đại diện của đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_REP_POSITION = "{{TradingProviderRepPosition}}";

        /// <summary>
        /// Số quyết định của đại lý sơ cấp
        /// </summary>
        public const string TRADING_PROVIDER_DECISION_NO = "{{TradingProviderDecisionNo}}";

        /// <summary>
        /// Tên tài khoản thụ hưởng của đại lý sơ cấp
        /// </summary>
        public const string TRADING_BANK_ACC_NAME = "{{TradingBankAccName}}";

        /// <summary>
        /// Số tài khoản thụ hưởng của đại lý sơ cấp
        /// </summary>
        public const string TRADING_BANK_ACC_NO = "{{TradingBankAccNo}}";

        /// <summary>
        /// Tên viết tắt ngân hàng
        /// </summary>
        public const string TRADING_BANK_NAME = "{{TradingBankName}}";

        /// <summary>
        /// Tên đầy đủ ngân hàng
        /// </summary>
        public const string TRADING_BANK_FULL_NAME = "{{TradingBankFullName}}";
        #endregion

        #region Tính toán
        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public const string ACTUALLY_PROFIT = "{{ActuallyProfit}}";

        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public const string PROFIT = "{{Profit}}";

        /// <summary>
        /// Thuế lợi nhuận (Invest - thuế thu nhập cá nhân)
        /// </summary>
        public const string TAX = "{{Tax}}";

        /// <summary>
        /// Phí rút
        /// </summary>
        public const string WITH_DRAWAL_FEE = "{{WithdrawalFee}}";

        /// <summary>
        /// Lợi tức khấu trừ
        /// </summary>
        public const string DEDUCTIBLE_PROFIT = "{{DeductibleProfit}}";

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public const string AMOUNT_RECEIVED = "{{AmountReceived}}";

        /// <summary>
        /// Số tiền rút
        /// </summary>
        public const string AMOUNT_MONEY = "{{AmountMoney}}";

        #endregion

        #region sản phẩm garner
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public const string PRODUCT_NAME = "{{ProductName}}";

        /// <summary>
        /// mã sản phẩm
        /// </summary>
        public const string PRODUCT_CODE = "{{ProductCode}}";
        
        /// <summary>
        /// Mệnh giá
        /// </summary>
        public const string CPS_PAR_VALUE = "{{CpsParValue}}";
        
        /// <summary>
        /// Mệnh giá bằng chữ
        /// </summary>
        public const string CPS_PAR_VALUE_TEXT = "{{CpsParValueText}}"; 
        
        /// <summary>
        /// Số lượng
        /// </summary>
        public const string CPS_QUANTITY = "{{CpsQuantity}}"; 
        
        /// <summary>
        /// Số lượng bằng chữ
        /// </summary>
        public const string CPS_QUANTITY_TEXT = "{{CpsQuantityText}}";


        /// <summary>
        /// giá trị
        /// </summary>
        public const string PRODUCT_PRICE = "{{ProductPrice}}";

        /// <summary>
        /// giá trị bằng chữ
        /// </summary>
        public const string PRODUCT_PRICE_TEXT = "{{ProductPriceText}}";
        #endregion

        #region chủ đầu tư invest
        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public const string OWNER_NAME = "{{OwnerName}}";
        /// <summary>
        /// Tên viết tắt chủ đầu tư
        /// </summary>
        public const string OWNER_SHORT_NAME = "{{OwnerShortName}}";
        #endregion

        #region tổng thầu invest
        /// <summary>
        /// Tên tổng thầu
        /// </summary>
        public const string GENERAL_CONTRACTOR_NAME = "{{GeneralContractorName}}";
        /// <summary>
        /// Tên viết tắt tổng thầu
        /// </summary>
        public const string GENERAL_CONTRACTOR_SHORT_NAME = "{{GeneralContractorShortName}}";
        #endregion

        #region dự án invest
        /// <summary>
        /// Tên dự án
        /// </summary>
        public const string PROJECT_NAME = "{{ProjectName}}";
        /// <summary>
        /// địa chỉ
        /// </summary>
        public const string PROJECT_ADDRESS = "{{ProjectAddress}}";
        #endregion

        #region dòng tiền
        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public const string INTEREST_CASHFLOW = "{{Interest}}";
        /// <summary>
        /// Thời gian bắt đầu đầu tư
        /// </summary>
        public const string START_DATE_CASHFLOW = "{{StartDate}}";
        /// <summary>
        /// Thời gian đáo hạn
        /// </summary>
        public const string END_DATE_CASHFLOW = "{{EndDate}}";
        /// <summary>
        /// ngày đáo hạn
        /// </summary>
        public const string DAY_END_DATE_CASHFLOW = "{{DayEndDate}}";
        /// <summary>
        /// tháng đáo hạn
        /// </summary>
        public const string MONTH_END_DATE_CASHFLOW = "{{MonthEndDate}}";
        /// <summary>
        /// năm đáo hạn
        /// </summary>
        public const string YEAR_END_DATE_CASHFLOW = "{{YearEndDate}}";
        /// <summary>
        /// số tiền đầu tư
        /// </summary>
        public const string INVEST_MONEY_CASHFLOW = "{{InvestMoney}}";
        
        /// <summary>
        /// thuế lợi tức cả kỳ
        /// </summary>
        public const string TAX_PROFIT_CASHFLOW = "{{TaxProfit}}";

        /// <summary>
        /// Tổng tiền nhận được cả gốc trừ đi thuế
        /// Tổng số tiền nhận được E (E = A + A * D * B/365)
        /// Tổng thu nhập cuối kỳ
        /// </summary>
        public const string TOTAL_RECEIVE_VALUE_CASHFLOW = "{{TotalReceiveValue}}";

        /// <summary>
        /// lợi nhuận thực nhận trừ đi thuế
        /// Tổng số tiền nhận được A * D * B/365 - thuế
        /// </summary>
        public const string ACTUALLY_PROFIT_CASHFLOW = "{{ActuallyProfit}}";

        /// <summary>
        /// Lợi nhuận trước thuế A * D * B/365
        /// </summary>
        public const string BEFORE_TAX_PROFIT_CASHFLOW = "{{BeforeTaxProfit}}";
        
        /// <summary>
        /// Số tiền đầu tư text
        /// </summary>
        public const string INVEST_MONEY_TEXT_CASHFLOW = "{{InvestMoneyText}}";

        #endregion

        #region sale
        /// <summary>
        /// Tên sale
        /// </summary>
        public const string CUSTOMER_SALE_NAME = "{{CustomerSaleName}}";

        /// <summary>
        /// Số điện thoại sale
        /// </summary>
        public const string CUSTOMER_SALE_PHONE = "{{CustomerSalePhone}}";

        #endregion

        #region chính sách
        /// <summary>
        /// Tên chính sách
        /// </summary>
        public const string POLICY_NAME = "{{PolicyName}}";
        #endregion
        
        #region Sổ lệnh
        /// <summary>
        /// Tiền đầu tư
        /// </summary>
        public const string TOTAL_VALUE = "{{TotalValue}}"; 
        
        /// <summary>
        /// Tiền đầu tư
        /// </summary>
        public const string TOTAL_VALUE_TEXT = "{{TotalValueText}}";
        #endregion

        #region Khác
        /// <summary>
        /// Nếu lấy địa chỉ thường trú trên hệ thống thì ghi thêm vào đoạn này: (theo thông tin tại CMND/CCCD) 
        /// </summary>
        public const string ACCORDING_IDENTITY = "{{AccordingIdentity}}";
        #endregion
    }
}
