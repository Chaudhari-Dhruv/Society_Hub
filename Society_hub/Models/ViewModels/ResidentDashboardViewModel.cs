namespace Society_hub.Models.ViewModels
{
    public class ResidentDashboardViewModel
    {
        public Resident? Resident { get; set; }

        public int ComplaintCount { get; set; }

        public int PendingComplaintCount { get; set; }

        public int UpcomingVisitorCount { get; set; }

        public int NoticeCount { get; set; }

        public int UpcomingEventCount { get; set; }
    }
}