using System.ComponentModel.DataAnnotations;

namespace team_collab_crm.Models
{
    public class OrganizationModel : AuditedModelBase
    {
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        [MaxLength(50)]
        public string Plan { get; set; } = "FREE";
        [MaxLength(64)]
        public string TimeZone { get; set; } = "UTC";
        public DateTimeOffset? DeletedAt { get; set; }
        public ICollection<MembershipModel> Membership { get; set; } = new List<MembershipModel>();
        public ICollection<PipelineStages> PipelineStages { get; set; } = new List<PipelineStages>();
    }
}
