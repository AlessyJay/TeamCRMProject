using System.ComponentModel.DataAnnotations;
using team_collab_crm.Interfaces;

namespace team_collab_crm.Models
{
    public class PipelineStages : OrgScopedModelBase
    {
        [MaxLength(80)]
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsClosed { get; set; }
        public DateTimeOffset? DeleteAt { get; set; }
        public OrganizationModel Organization { get; set; } = null!;
    }
}
