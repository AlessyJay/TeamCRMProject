using System.ComponentModel.DataAnnotations;
using team_collab_crm.Interfaces;

namespace team_collab_crm.Models
{
    public class PipelineStages : OrgScopedModelBase, ISoftDelete
    {
        [MaxLength(80)]
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsClosed { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public OrganizationModel Organization { get; set; } = null!;
    }
}
