namespace team_collab_crm.Models
{
    public class DailyMetric
    {
        public Guid OrgId { get; set; }
        public DateOnly Day { get; set; }
        public int LeadsCreated { get; set; }
        public int DealsCreated { get; set; }
        public int DealsWon { get; set; }
        public int DealsLost { get; set; }
        public int CallsLogged { get; set; }
        public int MeetingsLogged { get; set; }
        public OrganizationModel Organization { get; set; } = null!;
    }
}
