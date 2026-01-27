using System.ComponentModel.DataAnnotations;

namespace team_collab_crm.Models
{
    public class Contact : OrgScopedModelBase
    {
        public Guid? AccountId { get; set; }
        public Account? Account { get; set; }
        [MaxLength(120)]
        public string? FirstName { get; set; }
        [MaxLength(120)]
        public string? LastName { get; set; }
        [MaxLength(320)]
        public string? Email { get; set; }
        [MaxLength(40)]
        public string? Phone { get; set; }
        [MaxLength(120)]
        public string? Title { get; set; }
        public Guid? OwnerUserId { get; set; }
        public UserModel? OwnerUser { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
