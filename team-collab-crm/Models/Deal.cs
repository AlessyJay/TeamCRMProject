using System.ComponentModel.DataAnnotations;
using team_collab_crm.Interfaces;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class Deal : OrgScopedModelBase, ISoftDelete
    {
        public Guid? AccountId { get; set; }
        public Account? Account { get; set; }
        public Guid? PrimaryContactId { get; set; }
        public Contact? PrimaryContact { get; set; }
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        public Guid? StageId { get; set; }
        public PipelineStages? Stage { get; set; }
        public long ValueCents { get; set; } = 0;

        [MaxLength(3)]
        public string Currency { get; set; } = "USD";
        public int Probability { get; set; } = 0;
        public DateOnly? ExpectedCloseDate { get; set; }
        public Guid? OwnerUserId { get; set; }
        public UserModel? OwnerUser { get; set; }

        public DealType Status { get; set; } = DealType.OPEN;
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
