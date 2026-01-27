using System.Text.Json;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class AuditLog : BaseModel
    {
        public Guid OrgId { get; set; }
        public Guid? ActorUserId { get; set; }
        public UserModel? Actor { get; set; }
        public AuditAction Action { get; set; }
        public EntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public JsonDocument? BeforeJson { get; set; }
        public JsonDocument? AfterJson { get; set; }
    }
}
