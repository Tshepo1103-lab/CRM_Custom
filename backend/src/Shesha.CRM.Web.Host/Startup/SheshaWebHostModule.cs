using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Shesha.CRM.Web.Host.Startup
{
    [DependsOn(typeof(CRMWebCoreModule),
        typeof(AbpHangfireAspNetCoreModule))]
    public class SheshaWebHostModule: AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SheshaWebHostModule).GetAssembly());
        }
        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.UseHangfire();
        }
    }
}
