using System.Text.Json;
using team_collab_crm.Services;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class Activity : BaseModel
    {
        public Guid OrgId { get; set; }
        public Enums.EntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public ActivityType Type { get; set; }
        public string? Body { get; set; }
        public JsonDocument MetaJson { get; set; } = JsonDocument.Parse("{}");

        public Guid? CreatedBy { get; set; }
        public UserModel? Creator { get; set; }
    }
}
