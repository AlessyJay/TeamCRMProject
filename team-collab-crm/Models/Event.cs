using System.Text.Json;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class Event : BaseModel
    {
        public Guid OrgId { get; set; }
        public Guid? UserId { get; set; }
        public UserModel? User { get; set; }
        public string Name { get; set; } = null!;
        public EntityType? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public JsonDocument Properties { get; set; } = JsonDocument.Parse("{}");
    }
}
