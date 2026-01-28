using System.ComponentModel.DataAnnotations;
using team_collab_crm.Interfaces;

namespace team_collab_crm.Models
{
    public class Account : OrgScopedModelBase, ISoftDelete
    {
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(255)]
        public string? Domain { get; set; }
        [MaxLength(120)]
        public string? Industry { get; set; }

        public Guid? OwnerUserId { get; set; }
        public UserModel? OwnerUser { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
