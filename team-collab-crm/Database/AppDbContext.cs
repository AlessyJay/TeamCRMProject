using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using team_collab_crm.Interfaces;
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
            ConfigureOrgInvitation(b);
            ConfigurePipelineStage(b);
            ConfigureAccount(b);
            ConfigureContact(b);
            ConfigureLead(b);
            ConfigureDeal(b);
            ConfigureActivity(b);
            ConfigureTask(b);
            ConfigureAuditLog(b);

            ConfigureEvent(b);
            ConfigureDailyMetric(b);

            ConfigureAttachment(b);

            AddSoftDeleteFilters(b);
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
            e.Property(o => o.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(o => o.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(o => o.DeletedAt).HasColumnName("deleted_at");
        }

        private static void ConfigureUser(ModelBuilder b)
        {
            var e = b.Entity<UserModel>();
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(u => u.Email).HasColumnName("email");
            e.Property(u => u.DisplayName).HasColumnName("display_name");
            e.Property(u => u.PasswordHash).HasColumnName("password_hash");
            e.Property(u => u.Status).HasColumnName("status");
            e.Property(u => u.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(u => u.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
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
            e.Property(m => m.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(m => m.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");

            e.HasOne(m => m.Organization)
                .WithMany(o => o.Membership)
                .HasForeignKey(x => x.OrgId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(m => m.User).WithMany(u => u.Membership).HasForeignKey(x => x.OrgId).OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => x.OrgId);
            e.HasIndex(x => x.UserId);
            e.HasIndex(x => new { x.OrgId, x.UserId }).IsUnique();
        }

        private static void ConfigureOrgInvitation(ModelBuilder b)
        {
            var e = b.Entity<OrgInvitation>();
            e.ToTable("org_invitations");
            e.HasKey(i => i.Id);
            e.Property(i => i.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(i => i.OrgId).HasColumnName("org_id");
            e.Property(i => i.Email).HasColumnName("email");
            e.Property(i => i.Role).HasColumnName("role");
            e.Property(i => i.TokenHashed).HasColumnName("token_hashed");
            e.Property(i => i.ExpiresAt).HasColumnName("expires_at");
            e.Property(i => i.AcceptedAt).HasColumnName("accepted_at");
            e.Property(i => i.CreatedBy).HasColumnName("created_by");
            e.Property(i => i.createdAt).HasColumnName("created_at");

            e.HasIndex(x => x.OrgId);
            e.HasIndex(x => x.Email);
            e.HasIndex(x => new { x.OrgId, x.Email }).IsUnique();

            e.HasOne(x => x.Organization)
                .WithMany()
                .HasForeignKey(x => x.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigurePipelineStage(ModelBuilder b)
        {
            var e = b.Entity<PipelineStages>();

            e.ToTable("pipeline_stages");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(p => p.OrgId).HasColumnName("org_id");
            e.Property(p => p.Name).HasColumnName("name");
            e.Property(p => p.SortOrder).HasColumnName("sort_order");
            e.Property(p => p.IsClosed).HasColumnName("is_closed");
            e.Property(p => p.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(p => p.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(p => p.DeletedAt).HasColumnName("deleted_at");

            e.HasIndex(p => new { p.OrgId, p.SortOrder });

            e.HasOne(p => p.Organization)
                .WithMany(o => o.PipelineStages)
                .HasForeignKey(x => x.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureAccount(ModelBuilder b)
        {
            var e = b.Entity<Account>();

            e.ToTable("account");
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(a => a.OrgId).HasColumnName("org_id");
            e.Property(a => a.Name).HasColumnName("name");
            e.Property(a => a.Domain).HasColumnName("domain");
            e.Property(a => a.Industry).HasColumnName("industry");
            e.Property(a => a.OwnerUserId).HasColumnName("owner_user_id");
            e.Property(a => a.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(a => a.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(a => a.DeletedAt).HasColumnName("deleted_at");

            e.HasIndex(a => new { a.OrgId, a.OwnerUserId });
            e.HasIndex(a => new { a.OrgId, a.createdAt });

            e.HasOne(a => a.OwnerUser)
                .WithMany()
                .HasForeignKey(a => a.OwnerUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureContact(ModelBuilder b)
        {
            var e = b.Entity<Contact>();
            e.ToTable("contact");
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(c => c.OrgId).HasColumnName("org_id");
            e.Property(c => c.AccountId).HasColumnName("account_id");
            e.Property(c => c.FirstName).HasColumnName("first_name");
            e.Property(c => c.LastName).HasColumnName("last_name");
            e.Property(c => c.Email).HasColumnName("email");
            e.Property(c => c.Phone).HasColumnName("phone");
            e.Property(c => c.Title).HasColumnName("title");
            e.Property(c => c.OwnerUserId).HasColumnName("owner_user_id");
            e.Property(c => c.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(c => c.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(c => c.DeletedAt).HasColumnName("deleted_at");

            e.HasIndex(c => new { c.OrgId, c.AccountId });
            e.HasIndex(c => new { c.OrgId, c.OwnerUserId });

            e.HasOne(c => c.Account)
                .WithMany(c => c.Contacts)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(c => c.OwnerUser)
                .WithMany()
                .HasForeignKey(c => c.OwnerUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureLead(ModelBuilder b)
        {
            var e = b.Entity<Lead>();

            e.ToTable("lead");
            e.HasKey(l => l.Id);
            e.Property(l => l.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(l => l.OrgId).HasColumnName("org_id");
            e.Property(l => l.Name).HasColumnName("name");
            e.Property(l => l.Email).HasColumnName("email");
            e.Property(l => l.Phone).HasColumnName("phone");
            e.Property(l => l.Source).HasColumnName("source");
            e.Property(l => l.Status).HasColumnName("status");
            e.Property(l => l.OwnerUserId).HasColumnName("owner_user_id");
            e.Property(l => l.ConvertedAccountId).HasColumnName("converted_account_id");
            e.Property(l => l.ConvertedContactId).HasColumnName("converted_contact_id");
            e.Property(l => l.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(l => l.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(l => l.DeletedAt).HasColumnName("deleted_at");

            e.HasIndex(l => new { l.OrgId, l.OwnerUserId });
            e.HasIndex(l => new { l.OrgId, l.Status });
            e.HasIndex(l => new { l.OrgId, l.createdAt });

            e.HasOne(l => l.OwnerUser)
                .WithMany()
                .HasForeignKey(l => l.OwnerUserId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(l => l.ConvertedAccount)
                .WithMany()
                .HasForeignKey(l => l.ConvertedAccountId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(l => l.ConvertedContact)
                .WithMany()
                .HasForeignKey(l => l.ConvertedContactId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureDeal(ModelBuilder b)
        {
            var e = b.Entity<Deal>();
            e.ToTable("deals");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(x => x.OrgId).HasColumnName("org_id");
            e.Property(x => x.AccountId).HasColumnName("account_id");
            e.Property(x => x.PrimaryContactId).HasColumnName("primary_contact_id");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.StageId).HasColumnName("stage_id");
            e.Property(x => x.ValueCents).HasColumnName("value_cents");
            e.Property(x => x.Currency).HasColumnName("currency");
            e.Property(x => x.Probability).HasColumnName("probability");
            e.Property(x => x.ExpectedCloseDate).HasColumnName("expected_close_date");
            e.Property(x => x.OwnerUserId).HasColumnName("owner_user_id");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(x => x.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(x => x.DeletedAt).HasColumnName("deleted_at");

            e.HasIndex(x => new { x.OrgId, x.StageId });
            e.HasIndex(x => new { x.OrgId, x.OwnerUserId });
            e.HasIndex(x => new { x.OrgId, x.Status });
            e.HasIndex(x => new { x.OrgId, x.ExpectedCloseDate });

            e.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.PrimaryContact).WithMany().HasForeignKey(x => x.PrimaryContactId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Stage).WithMany().HasForeignKey(x => x.StageId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.OwnerUser).WithMany().HasForeignKey(x => x.OwnerUserId).OnDelete(DeleteBehavior.SetNull);

            // Optional: constraints mirrored in DB via migrations
            e.Property(x => x.Probability).HasDefaultValue(0);
            e.Property(x => x.ValueCents).HasDefaultValue(0);
        }

        private static void ConfigureActivity(ModelBuilder b)
        {
            var e = b.Entity<Activity>();
            e.ToTable("activities");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(x => x.OrgId).HasColumnName("org_id");
            e.Property(x => x.EntityType).HasColumnName("entity_type");
            e.Property(x => x.EntityId).HasColumnName("entity_id");
            e.Property(x => x.Type).HasColumnName("type");
            e.Property(x => x.Body).HasColumnName("body");
            e.Property(x => x.CreatedBy).HasColumnName("created_by");
            e.Property(x => x.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");

            e.Property(x => x.MetaJson)
                .HasColumnName("meta_json")
                .HasColumnType("jsonb");

            e.HasIndex(x => new { x.OrgId, x.EntityType, x.EntityId, x.createdAt });
            e.HasIndex(x => new { x.OrgId, x.createdAt });

            e.HasOne(x => x.Creator).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureTask(ModelBuilder b)
        {
            var e = b.Entity<TaskEntity>();
            e.ToTable("tasks");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(x => x.OrgId).HasColumnName("org_id");
            e.Property(x => x.EntityType).HasColumnName("entity_type");
            e.Property(x => x.EntityId).HasColumnName("entity_id");
            e.Property(x => x.Title).HasColumnName("title");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.DueAt).HasColumnName("due_at");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.AssignedTo).HasColumnName("assigned_to");
            e.Property(x => x.CreatedBy).HasColumnName("created_by");
            e.Property(x => x.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(x => x.updatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(x => x.DeletedAt).HasColumnName("deleted_at");

            e.HasIndex(x => new { x.OrgId, x.AssignedTo, x.DueAt });
            e.HasIndex(x => new { x.OrgId, x.Status });

            e.HasOne(x => x.Assignee).WithMany().HasForeignKey(x => x.AssignedTo).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Creator).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureAuditLog(ModelBuilder b)
        {
            var e = b.Entity<AuditLog>();
            e.ToTable("audit_logs");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(x => x.OrgId).HasColumnName("org_id");
            e.Property(x => x.ActorUserId).HasColumnName("actor_user_id");
            e.Property(x => x.Action).HasColumnName("action");
            e.Property(x => x.EntityType).HasColumnName("entity_type");
            e.Property(x => x.EntityId).HasColumnName("entity_id");
            e.Property(x => x.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");

            e.Property(x => x.BeforeJson).HasColumnName("before_json").HasColumnType("jsonb");
            e.Property(x => x.AfterJson).HasColumnName("after_json").HasColumnType("jsonb");

            e.HasIndex(x => new { x.OrgId, x.EntityType, x.EntityId, x.createdAt });
            e.HasIndex(x => new { x.OrgId, x.ActorUserId, x.createdAt });

            e.HasOne(x => x.Actor).WithMany().HasForeignKey(x => x.ActorUserId).OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureEvent(ModelBuilder b)
        {
            var e = b.Entity<Event>();
            e.ToTable("events");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(x => x.OrgId).HasColumnName("org_id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.EntityType).HasColumnName("entity_type");
            e.Property(x => x.EntityId).HasColumnName("entity_id");
            e.Property(x => x.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(x => x.Properties).HasColumnName("properties").HasColumnType("jsonb");

            e.HasIndex(x => new { x.OrgId, x.createdAt });
            e.HasIndex(x => new { x.OrgId, x.Name, x.createdAt });

            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
        }

        private static void ConfigureDailyMetric(ModelBuilder b)
        {
            var e = b.Entity<DailyMetric>();
            e.ToTable("daily_metrics");

            e.HasKey(x => new { x.OrgId, x.Day });
            e.Property(x => x.OrgId).HasColumnName("org_id");
            e.Property(x => x.Day).HasColumnName("day");

            e.Property(x => x.LeadsCreated).HasColumnName("leads_created");
            e.Property(x => x.DealsCreated).HasColumnName("deals_created");
            e.Property(x => x.DealsWon).HasColumnName("deals_won");
            e.Property(x => x.DealsLost).HasColumnName("deals_lost");
            e.Property(x => x.CallsLogged).HasColumnName("calls_logged");
            e.Property(x => x.MeetingsLogged).HasColumnName("meetings_logged");

            e.HasOne(x => x.Organization)
                .WithMany()
                .HasForeignKey(x => x.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureAttachment(ModelBuilder b)
        {
            var e = b.Entity<Attachment>();
            e.ToTable("attachments");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            e.Property(x => x.OrgId).HasColumnName("org_id");
            e.Property(x => x.EntityType).HasColumnName("entity_type");
            e.Property(x => x.EntityId).HasColumnName("entity_id");
            e.Property(x => x.UploadedBy).HasColumnName("uploaded_by");
            e.Property(x => x.FileName).HasColumnName("file_name");
            e.Property(x => x.ContentType).HasColumnName("content_type");
            e.Property(x => x.SizeBytes).HasColumnName("size_bytes");
            e.Property(x => x.StorageKey).HasColumnName("storage_key");
            e.Property(x => x.createdAt).HasColumnName("created_at").HasDefaultValueSql("now()");

            e.HasIndex(x => new { x.OrgId, x.EntityType, x.EntityId, x.createdAt });

            e.HasOne(x => x.Uploader)
                .WithMany()
                .HasForeignKey(x => x.UploadedBy)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void AddSoftDeleteFilters(ModelBuilder b)
        {
            foreach (var entityType in b.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter),
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { b });
                }
            }
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder b) where TEntity : class, ISoftDelete
        {
            b.Entity<TEntity>().HasQueryFilter(e => e.DeletedAt == null);
        }

        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTimestamps()
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseModel baseEntity && entry.State == EntityState.Added)
                {
                    baseEntity.createdAt = now;
                    baseEntity.Id = baseEntity.Id == Guid.Empty ? Guid.NewGuid() : baseEntity.Id;
                }

                if (entry.Entity is AuditedModelBase audited && (entry.State == EntityState.Added || entry.State == EntityState.Modified))
                {
                    audited.updatedAt = now;
                }
            }
        }
    }
}

