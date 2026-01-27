using System.ComponentModel.DataAnnotations;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class OrgInvitation : BaseModel
    {
        public Guid OrgId { get; set; }

        [MaxLength(320)]
        public string Email { get; set; } = null!;
        public MembershipRoles Role { get; set; } = MembershipRoles.MEMBER;
        public string TokenHashed { get; set; } = null!;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? AcceptedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public OrganizationModel Organization { get; set; } = null!;
        public UserModel? Creator { get; set; }
    }
}
