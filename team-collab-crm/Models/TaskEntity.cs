using team_collab_crm.Services;

namespace team_collab_crm.Models
{
    public class TaskEntity : OrgScopedModelBase
    {
        public Enums.EntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.OPEN;
        public Guid? AssignedTo { get; set; }
        public UserModel? Assignee { get; set; }
        public Guid? CreatedBy { get; set; }
        public UserModel? Creator { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
