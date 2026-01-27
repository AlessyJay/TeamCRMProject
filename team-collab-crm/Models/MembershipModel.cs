using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class MembershipModel : OrgScopedModelBase
    {
        public Guid UserId { get; set; }
        public MembershipRoles Role { get; set; } = MembershipRoles.MEMBER;

        public UserModel User { get; set; } = null!;
        public OrganizationModel Organization { get; set; } = null!;
    }
}
