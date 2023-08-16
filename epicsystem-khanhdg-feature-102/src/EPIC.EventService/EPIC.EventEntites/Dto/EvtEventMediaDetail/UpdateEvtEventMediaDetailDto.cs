﻿using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMediaDetail
{
    public class UpdateEvtEventMediaDetailDto
    {
        public int Id { get; set; }

        private string _urlImage;
        [Required(ErrorMessage = "Đường dẫn file ảnh không được để trống")]
        [StringLength(512, ErrorMessage = "Đường dẫn file ảnh không được dài hơn 512 ký tự")]
        public string UrlImage
        {
            get => _urlImage;
            set => _urlImage = value?.Trim();
        }
    }
}
