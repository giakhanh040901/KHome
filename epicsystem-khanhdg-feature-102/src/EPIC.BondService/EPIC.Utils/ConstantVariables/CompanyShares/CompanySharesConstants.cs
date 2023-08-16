using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.CompanyShares
{
    /// <summary>
    /// Trạng thái phát hành sơ cấp
    /// </summary>
    public static class CpsSecondaryStatus
    {
        /// <summary>
        /// Nháp
        /// </summary>
        public const int TEMP = 1;
        /// <summary>
        /// Trình duyệt
        /// </summary>
        public const int SUBMIT = 2;
        /// <summary>
        /// dlsc duyệt
        /// </summary>
        public const int TRADING_PROVIDER_APPROVE = 3;
        /// <summary>
        /// EPIC duyệt
        /// </summary>
        public const int SUPER_ADMIN_APPROVE = 4;
    }

    #region Policy
    /// <summary>
    /// Trạng thái chính sách
    /// </summary>
    public static class CpsPolicyStatus
    {
        public const string ACTIVE = "A";
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Trạng thái chính sách mẫu
    /// </summary>
    public static class CpsPolicyTemplate
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Trạng thái kỳ hạn của chính sách mẫu
    /// </summary>
    public static class CpsPolicyDetailTemplate
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Kiểu chính sách sản phẩm
    /// </summary>
    public static class CpsPolicyType
    {
        /// <summary>
        /// Fix ngày bán cố định
        /// </summary>
        public const int FIX = 1;
        /// <summary>
        /// Ngày bán thay đổi
        /// </summary>
        public const int FLEXIBLE = 2;
    }

    /// <summary>
    /// Phân loại chính sách
    /// </summary>
    public static class CpsPolicyClassify
    {
        public const int PRO = 1;
        public const int PROA = 2;
        public const int PNOTE = 3;

        public static Dictionary<int, string> KeyValues = new()
        {
            { 0, "" },
            { CpsPolicyClassify.PNOTE, "PNOTE" },
            { CpsPolicyClassify.PRO, "PRO" },
            { CpsPolicyClassify.PROA, "PROA" },
        };
    }
    #endregion
}
