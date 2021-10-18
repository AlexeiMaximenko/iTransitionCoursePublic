using Microsoft.Extensions.Configuration;

namespace iTransitionCourse.Service
{
    public class SettingService
    {
        private readonly IConfiguration _configuration;
        public string AdminUserName => _configuration["Admin:UserName"];
        public string AdminPassword => _configuration["Admin:Password"];
        public SettingService(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }
        public SettingService()
        { }
    }
}
