using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("USERS_CHAT_ROOM", Schema = DbSchemas.EPIC)]
    public class UsersChatRoom
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(UserId))]
        public int? UserId { get; set; }

        [ColumnSnackCase(nameof(RoomId))]
        public string RoomId { get; set; }

        [ColumnSnackCase(nameof(RoomToken))]
        public string RoomToken { get; set; }

        [ColumnSnackCase(nameof(RoomStartDate))]
        public DateTime? RoomStartDate { get; set; }

        [ColumnSnackCase(nameof(RoomEndDate))]
        public DateTime? RoomEndDate { get; set; }

        [ColumnSnackCase(nameof(AgentId))]
        public string AgentId { get; set; }

        [ColumnSnackCase(nameof(VisitorId))]
        public string VisitorId { get; set; }

        [ColumnSnackCase(nameof(VisitorToken))]
        public string VisitorToken { get; set; }

        [ColumnSnackCase(nameof(LastMessage))]
        public string LastMessage { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
    }
}
