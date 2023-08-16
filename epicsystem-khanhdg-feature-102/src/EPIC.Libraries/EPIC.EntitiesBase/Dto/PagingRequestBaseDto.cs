using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.EntitiesBase.Dto
{
    /// <summary>
    /// Request chia trang
    /// </summary>
    public class PagingRequestBaseDto
    {
        /// <summary>
        /// Số phần tử mỗi trang
        /// </summary>
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Trang số mấy
        /// </summary>
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }

        private string _keyword { get; set; }
        /// <summary>
        /// Keyword tìm kiếm
        /// </summary>
        [FromQuery(Name = "keyword")]
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }

        /// <summary>
        /// Bỏ qua bao nhiêu bản ghi
        /// </summary>
        [NotMapped]
        public int Skip
        {
            get
            {
                int skip = (PageNumber - 1) * PageSize;
                if (skip < 0)
                {
                    skip = 0;
                }
                return skip;
            }
        }

        /// <summary>
        /// Sắp xếp theo trường nào
        /// </summary>
        public List<string> Sort { get; set; }
    }
}
