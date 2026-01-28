using System.ComponentModel.DataAnnotations;
using team_collab_crm.Interfaces;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class Lead : OrgScopedModelBase, ISoftDelete
    {
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(320)]
        public string? Email { get; set; }
        [MaxLength(40)]
        public string? Phone { get; set; }
        [MaxLength(80)]
        public string? Source { get; set; }
        public LeadStatus Status { get; set; } = LeadStatus.NEW;
        public Guid? OwnerUserId { get; set; }
        public UserModel? OwnerUser { get; set; }
        public Guid? ConvertedAccountId { get; set; }
        public Account? ConvertedAccount { get; set; }
        public Guid? ConvertedContactId { get; set; }
        public Contact? ConvertedContact { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
