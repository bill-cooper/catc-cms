using ceenq.com.Core.Applications;

namespace ceenq.com.DashboardApp
{
    public class DynamicApplicationEventHandler: IDynamicApplicationEventHandler
    {
        private readonly IDashboardApplicationService _dashboardApplicationService;
        public DynamicApplicationEventHandler(IDashboardApplicationService dashboardApplicationService)
        {
            _dashboardApplicationService = dashboardApplicationService;
        }

        public void FindDynamicApplications(DynamicApplicationContext context)
        {
            context.Applications.Add(_dashboardApplicationService.BuildDashboardApplication());
        }
    }
}