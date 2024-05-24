using System.Reflection;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using Intent.RoslynWeaver.Attributes;
using Shesha;
using Shesha.Authorization;
using Shesha.Modules;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Boxfusion.Modules.Domain.Module", Version = "1.0")]

namespace Shesha.CRM.Domain
{
    [IntentManaged(Mode.Ignore)]
    /// <summary>
    /// CRMCommon Module
    /// </summary>
    [DependsOn(
        typeof(SheshaCoreModule),
        typeof(SheshaApplicationModule)
    )]
    public class CRMModule : SheshaModule
    {
        public override SheshaModuleInfo ModuleInfo => new SheshaModuleInfo("Shesha.CRM")
        {
            FriendlyName = "CRM",
            Publisher = "Shesha",
        };
        /// inheritedDoc
        public override void Initialize()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }

        /// inheritedDoc
        public override void PreInitialize()
        {
            base.PreInitialize();
        }

        /// inheritedDoc
        public override void PostInitialize()
        {
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(
                typeof(CRMModule).Assembly,
                moduleName: "CRMCommon",
                useConventionalHttpVerbs: true);
        }
    }
}
