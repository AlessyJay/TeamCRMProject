using System.ComponentModel.DataAnnotations;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Models
{
    public class Attachment : BaseModel
    {
        public Guid OrgId { get; set; }
        public EntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid? UploadedBy { get; set; }
        public UserModel? Uploader { get; set; }
        [MaxLength(255)]
        public string FileName { get; set; } = null!;
        [MaxLength(120)]
        public string? ContentType { get; set; }
        public long SizeBytes { get; set; } = 0;
        public string StorageKey { get; set; } = null!;
    }
}
