using Microsoft.AspNetCore.Http;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface
{
    public class CreateLoyLuckyRotationInterfaceWithProgramDto
    {
        public string Template { get; set; }

        /// <summary>
        /// Nút chơi game
        /// </summary>
        public bool ButtonPlay { get; set; }

        /// <summary>
        /// Nút lịch sử
        /// </summary>
        public bool ButtonHistory { get; set; }

        /// <summary>
        /// Nút xếp hạng
        /// </summary>
        public bool ButtonRank { get; set; }

        /// <summary>
        /// Mã màu các nút button Chơi game, lịch sử, xếp hạng
        /// </summary>
        public string ButtonColor { get; set; }

        /// <summary>
        /// Hiện icon Home hay ko
        /// </summary>
        public bool IconHome { get; set; }

        /// <summary>
        /// Hiện thông báo trúng thưởng hay không
        /// </summary>
        public bool WinText { get; set; }

        /// <summary>
        /// Hiện banner hay ko
        /// </summary>
        public bool ShowBanner { get; set; }

        /// <summary>
        /// ảnh icon nút Chơi game
        /// </summary>
        public string IconPlay { get; set; }

        /// <summary>
        /// ảnh icon nút lịch sử chơi
        /// </summary>
        public string IconHistory { get; set; }

        /// <summary>
        /// ảnh icon nút xếp hạng
        /// </summary>
        public string IconRank { get; set; }

        /// <summary>
        /// mã màu chữ thông báo trúng thưởng
        /// </summary>
        public string WinTextColor { get; set; }

        /// <summary>
        /// mã màu nền thông báo trúng thưởng
        /// </summary>
        public string WinTextBackgroundColor { get; set; }

        /// <summary>
        /// Ảnh banner
        /// </summary>
        public string Banner { get; set; }

        /// <summary>
        /// ảnh vòng quay
        /// </summary>
        public string RatationImage { get; set; }

        /// <summary>
        /// ảnh nền vòng quay
        /// </summary>
        public string RatationBackground { get; set; }

        /// <summary>
        /// Ảnh kim quay
        /// </summary>
        public string NeedleImage { get; set; }

        /// <summary>
        /// Hình nền
        /// </summary>
        public string Background { get; set; }
    }
}
