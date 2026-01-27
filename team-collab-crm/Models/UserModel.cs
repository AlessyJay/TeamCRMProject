using System.ComponentModel.DataAnnotations;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class UserModel : AuditedModelBase
    {
        [MaxLength(320)]
        public string Email { get; set; } = null!;
        [MaxLength(120)]
        public string DisplayName { get; set; } = null!;
        public string? PasswordHash { get; set; }
        [MaxLength(30)]
        public string Status { get; set; } = "ACTIVE";
        public ICollection<MembershipModel> Membership { get; set; } = new List<MembershipModel>();
    }
}
