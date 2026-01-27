namespace team_collab_crm.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset createdAt { get; set; }
    }

    public abstract class AuditedModelBase : BaseModel
    {
        public DateTimeOffset updatedAt { get; set; }
    }

    public abstract class OrgScopedModelBase : AuditedModelBase
    {
        public Guid OrgId { get; set; }
    }
}
