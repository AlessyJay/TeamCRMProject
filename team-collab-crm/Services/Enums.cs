namespace team_collab_crm.Services
{
    public class Enums
    {
        public enum MembershipRoles { OWNER, ADMIN, MEMBER, VIEWER }
        public enum LeadStatus { NEW, WORKING, QUALIFIED, DISQUALIFIED, CONVERTED }
        public enum DealType { OPEN, WON, LOST }
        public enum EntityType { ACCOUNT, CONTACT, LEAD, DEAL }
        public enum ActivityType { NOTE, CALL, MEETING, EMAIL, STATUS_CHANGE, STAGE_CHANGE, ASSIGNMENT_CHANGE }
        public enum TaskStatus { OPEN, DONE, CANCELLED }
        public enum AuditAction { CREATE, UPDATE, DELETE, RESTORE, CONVERT, ASSIGN, MOVE_STAGE }
    }
}
