using Microsoft.EntityFrameworkCore;
using team_collab_crm.Models;
using static team_collab_crm.Services.Enums;

namespace team_collab_crm.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<OrganizationModel> Organization => Set<OrganizationModel>();
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<MembershipModel> Memberships => Set<MembershipModel>();
        public DbSet<OrgInvitation> OrgInvitations => Set<OrgInvitation>();
        public DbSet<PipelineStages> PipelineStages => Set<PipelineStages>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<Deal> Deals => Set<Deal>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<DailyMetric> DailyMetrics => Set<DailyMetric>();
        public DbSet<Attachment> Attachments => Set<Attachment>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.HasPostgresEnum<MembershipRoles>("membership_roles");
            b.HasPostgresEnum<LeadStatus>("lead_status");
            b.HasPostgresEnum<DealType>("deal_types");
            b.HasPostgresEnum<EntityType>("entity_type");
            b.HasPostgresEnum<ActivityType>("activity_type");
            b.HasPostgresEnum<Services.Enums.TaskStatus>("task_status");
            b.HasPostgresEnum<AuditAction>("audit_action");

            ConfigureOrganiztion(b);
            ConfigureUser(b);
            ConfigureMembership(b);
        }

        private static void ConfigureOrganiztion(ModelBuilder b)
        {
            var e = b.Entity<OrganizationModel>();

            e.ToTable("organizations");
            e.HasKey(o => o.Id);
            e.Property(o => o.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(o => o.Name).HasColumnName("name");
            e.Property(o => o.Plan).HasColumnName("plan");
            e.Property(o => o.TimeZone).HasColumnName("timezone");
            e.Property(o => o.createdAt).HasColumnName("createdat").HasDefaultValueSql("now()");
            e.Property(o => o.updatedAt).HasColumnName("updatedAt").HasDefaultValueSql("now()");
            e.Property(o => o.DeletedAt).HasColumnName("deletedAt");
        }

        private static void ConfigureUser(ModelBuilder b)
        {
            var e = b.Entity<UserModel>();
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(u => u.Email).HasColumnName("email");
            e.Property(u => u.DisplayName).HasColumnName("displayName");
            e.Property(u => u.PasswordHash).HasColumnName("passwordHash");
            e.Property(u => u.Status).HasColumnName("status");
            e.Property(u => u.createdAt).HasColumnName("createdAt").HasDefaultValueSql("now()");
            e.Property(u => u.updatedAt).HasColumnName("updatedAt").HasDefaultValueSql("now()");
            e.HasIndex(u => u.Email).IsUnique();
        }

        private static void ConfigureMembership(ModelBuilder b)
        {
            var e = b.Entity<MembershipModel>();
            e.ToTable("memberships");
            e.HasKey(m => m.Id);
            e.Property(m => m.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(m => m.OrgId).HasColumnName("orgId");
            e.Property(m => m.UserId).HasColumnName("userId");
            e.Property(m => m.Role).HasColumnName("role");
            e.Property(m => m.createdAt).HasColumnName("createdAt").HasDefaultValueSql("now()");
            e.Property(m => m.updatedAt).HasColumnName("updatedAt").HasDefaultValueSql("now()");

            e.HasOne(m => m.Organization)
                .WithMany(o => o.Membership)
                .HasForeignKey(x => x.OrgId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(m => m.User).WithMany(u => u.Membership).HasForeignKey(x => x.OrgId).OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => x.OrgId);
            e.HasIndex(x => x.UserId);
            e.HasIndex(x => new { x.OrgId, x.UserId }).IsUnique();
        }
    }
}
