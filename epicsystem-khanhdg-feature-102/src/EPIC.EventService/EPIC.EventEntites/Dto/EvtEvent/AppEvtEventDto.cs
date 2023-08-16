﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    /// <summary>
    /// Tổng quan sự kiện
    /// </summary>
    public class AppEvtEventDto
    {
        /// <summary>
        /// Dah sách sự kiện nổi bật
        /// </summary>
        public IEnumerable<AppViewEventDto> Events { get; set; }
        /// <summary>
        /// Danh sách số lượng sự kiện theo loại hình sự kiện
        /// </summary>
        public IEnumerable<AppCountEvent> NumberEventTypes { get; set; }
    }

    public class AppCountEvent
    {
        /// <summary>
        /// Loại hình sự kiện
        /// </summary>
        public int EventType { get; set; }
        /// <summary>
        /// Số lượng sự kiện
        /// </summary>
        public int TotalEvent { get; set; }
    }
    public class AppViewEventDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ban tổ chức
        /// </summary>
        public string Organizator { get; set; }
        /// <summary>
        /// Loại hình sự kiện
        /// </summary>
        public IEnumerable<int> EventTypes { get; set; }
        /// <summary>
        /// Địa điểm tổ chức
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Mã Thành phố
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// Tên Thành phố
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Vĩ độ
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// Kinh độ
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Giá vé min
        /// </summary>
        public decimal MinTicketPrice { get; set; }
        /// <summary>
        /// Giá vé max
        /// </summary>
        public decimal MaxTicketPrice { get; set; }
        public string AvatarImageUrl { get; set; }
        public string BannerImageUrl { get; set; }
        /// <summary>
        /// Tất cả có free hay không
        /// </summary>
        public bool IsFree { get; set; }
    }
}
