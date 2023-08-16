using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface
{
    public class LoyLuckyRotationInterfaceDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Id kịch bản
        /// </summary>
        public int LuckyScenarioId { get; set; }

        /// <summary>
        /// Mẫu vòng quay
        /// </summary>
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
        public string RotationImage { get; set; }

        /// <summary>
        /// ảnh nền vòng quay
        /// </summary>
        public string RotationBackground { get; set; }

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
