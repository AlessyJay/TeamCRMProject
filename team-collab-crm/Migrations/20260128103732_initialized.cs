using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace team_collab_crm.Migrations
{
    /// <inheritdoc />
    public partial class initialized : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:activity_type.activity_type", "note,call,meeting,email,status_change,stage_change,assignment_change")
                .Annotation("Npgsql:Enum:audit_action.audit_action", "create,update,delete,restore,convert,assign,move_stage")
                .Annotation("Npgsql:Enum:deal_types.deal_type", "open,won,lost")
                .Annotation("Npgsql:Enum:entity_type.entity_type", "account,contact,lead,deal")
                .Annotation("Npgsql:Enum:lead_status.lead_status", "new,working,qualified,disqualified,converted")
                .Annotation("Npgsql:Enum:membership_roles.membership_roles", "owner,admin,member,viewer")
                .Annotation("Npgsql:Enum:task_status.task_status", "open,done,cancelled");

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    plan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    timezone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    display_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "daily_metrics",
                columns: table => new
                {
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<DateOnly>(type: "date", nullable: false),
                    leads_created = table.Column<int>(type: "integer", nullable: false),
                    deals_created = table.Column<int>(type: "integer", nullable: false),
                    deals_won = table.Column<int>(type: "integer", nullable: false),
                    deals_lost = table.Column<int>(type: "integer", nullable: false),
                    calls_logged = table.Column<int>(type: "integer", nullable: false),
                    meetings_logged = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_metrics", x => new { x.org_id, x.day });
                    table.ForeignKey(
                        name: "FK_daily_metrics_organizations_org_id",
                        column: x => x.org_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pipeline_stages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pipeline_stages", x => x.id);
                    table.ForeignKey(
                        name: "FK_pipeline_stages_organizations_org_id",
                        column: x => x.org_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    domain = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    industry = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<int>(type: "integer", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    body = table.Column<string>(type: "text", nullable: true),
                    meta_json = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.id);
                    table.ForeignKey(
                        name: "FK_activities_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<int>(type: "integer", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    uploaded_by = table.Column<Guid>(type: "uuid", nullable: true),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content_type = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    size_bytes = table.Column<long>(type: "bigint", nullable: false),
                    storage_key = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_attachments_users_uploaded_by",
                        column: x => x.uploaded_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    action = table.Column<int>(type: "integer", nullable: false),
                    entity_type = table.Column<int>(type: "integer", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    before_json = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    after_json = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    entity_type = table.Column<int>(type: "integer", nullable: true),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    properties = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_events_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "memberships",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    orgId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memberships", x => x.id);
                    table.ForeignKey(
                        name: "FK_memberships_organizations_orgId",
                        column: x => x.orgId,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_memberships_users_orgId",
                        column: x => x.orgId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "org_invitations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    token_hashed = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    accepted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_org_invitations", x => x.id);
                    table.ForeignKey(
                        name: "FK_org_invitations_organizations_org_id",
                        column: x => x.org_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_org_invitations_users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    entity_type = table.Column<int>(type: "integer", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    due_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    assigned_to = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_tasks_users_assigned_to",
                        column: x => x.assigned_to,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tasks_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    first_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    last_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    phone = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    title = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact", x => x.id);
                    table.ForeignKey(
                        name: "FK_contact_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_contact_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "deals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    primary_contact_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    stage_id = table.Column<Guid>(type: "uuid", nullable: true),
                    value_cents = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    probability = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    expected_close_date = table.Column<DateOnly>(type: "date", nullable: true),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deals", x => x.id);
                    table.ForeignKey(
                        name: "FK_deals_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_deals_contact_primary_contact_id",
                        column: x => x.primary_contact_id,
                        principalTable: "contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_deals_pipeline_stages_stage_id",
                        column: x => x.stage_id,
                        principalTable: "pipeline_stages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_deals_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "lead",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    phone = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    source = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    converted_account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    converted_contact_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lead", x => x.id);
                    table.ForeignKey(
                        name: "FK_lead_account_converted_account_id",
                        column: x => x.converted_account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_lead_contact_converted_contact_id",
                        column: x => x.converted_contact_id,
                        principalTable: "contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_lead_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_org_id_created_at",
                table: "account",
                columns: new[] { "org_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_account_org_id_owner_user_id",
                table: "account",
                columns: new[] { "org_id", "owner_user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_account_owner_user_id",
                table: "account",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_activities_created_by",
                table: "activities",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_activities_org_id_created_at",
                table: "activities",
                columns: new[] { "org_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_activities_org_id_entity_type_entity_id_created_at",
                table: "activities",
                columns: new[] { "org_id", "entity_type", "entity_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_attachments_org_id_entity_type_entity_id_created_at",
                table: "attachments",
                columns: new[] { "org_id", "entity_type", "entity_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_attachments_uploaded_by",
                table: "attachments",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_actor_user_id",
                table: "audit_logs",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_org_id_actor_user_id_created_at",
                table: "audit_logs",
                columns: new[] { "org_id", "actor_user_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_org_id_entity_type_entity_id_created_at",
                table: "audit_logs",
                columns: new[] { "org_id", "entity_type", "entity_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_contact_account_id",
                table: "contact",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_org_id_account_id",
                table: "contact",
                columns: new[] { "org_id", "account_id" });

            migrationBuilder.CreateIndex(
                name: "IX_contact_org_id_owner_user_id",
                table: "contact",
                columns: new[] { "org_id", "owner_user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_contact_owner_user_id",
                table: "contact",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_deals_account_id",
                table: "deals",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_deals_org_id_expected_close_date",
                table: "deals",
                columns: new[] { "org_id", "expected_close_date" });

            migrationBuilder.CreateIndex(
                name: "IX_deals_org_id_owner_user_id",
                table: "deals",
                columns: new[] { "org_id", "owner_user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_deals_org_id_stage_id",
                table: "deals",
                columns: new[] { "org_id", "stage_id" });

            migrationBuilder.CreateIndex(
                name: "IX_deals_org_id_status",
                table: "deals",
                columns: new[] { "org_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_deals_owner_user_id",
                table: "deals",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_deals_primary_contact_id",
                table: "deals",
                column: "primary_contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_deals_stage_id",
                table: "deals",
                column: "stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_events_org_id_created_at",
                table: "events",
                columns: new[] { "org_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_events_org_id_name_created_at",
                table: "events",
                columns: new[] { "org_id", "name", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_events_user_id",
                table: "events",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_lead_converted_account_id",
                table: "lead",
                column: "converted_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_lead_converted_contact_id",
                table: "lead",
                column: "converted_contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_lead_org_id_created_at",
                table: "lead",
                columns: new[] { "org_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_lead_org_id_owner_user_id",
                table: "lead",
                columns: new[] { "org_id", "owner_user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_lead_org_id_status",
                table: "lead",
                columns: new[] { "org_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_lead_owner_user_id",
                table: "lead",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_memberships_orgId",
                table: "memberships",
                column: "orgId");

            migrationBuilder.CreateIndex(
                name: "IX_memberships_orgId_userId",
                table: "memberships",
                columns: new[] { "orgId", "userId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_memberships_userId",
                table: "memberships",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_org_invitations_CreatorId",
                table: "org_invitations",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_org_invitations_email",
                table: "org_invitations",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_org_invitations_org_id",
                table: "org_invitations",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "IX_org_invitations_org_id_email",
                table: "org_invitations",
                columns: new[] { "org_id", "email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pipeline_stages_org_id_sort_order",
                table: "pipeline_stages",
                columns: new[] { "org_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_assigned_to",
                table: "tasks",
                column: "assigned_to");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_created_by",
                table: "tasks",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_org_id_assigned_to_due_at",
                table: "tasks",
                columns: new[] { "org_id", "assigned_to", "due_at" });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_org_id_status",
                table: "tasks",
                columns: new[] { "org_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "daily_metrics");

            migrationBuilder.DropTable(
                name: "deals");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "lead");

            migrationBuilder.DropTable(
                name: "memberships");

            migrationBuilder.DropTable(
                name: "org_invitations");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "pipeline_stages");

            migrationBuilder.DropTable(
                name: "contact");

            migrationBuilder.DropTable(
                name: "organizations");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
