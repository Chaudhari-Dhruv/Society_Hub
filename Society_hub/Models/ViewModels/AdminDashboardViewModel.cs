namespace Society_hub.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalResidents { get; set; }

        public int TotalFlats { get; set; }

        public int OccupiedFlats { get; set; }

        public int VacantFlats { get; set; }

        public int PendingComplaints { get; set; }

        public int TodaysVisitors { get; set; }

        public int UpcomingEvents { get; set; }
    }
}