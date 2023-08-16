using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicyDetail
{
	public class UpdatePolicyDetailDto : CreatePolicyDetailDto
	{
		/// <summary>
		/// id kỳ hạn nếu là 0 thì sẽ là thêm mới nếu khác 0 thì sẽ là cập nhật
		/// </summary>
		public int Id { get; set; }
	}
}
