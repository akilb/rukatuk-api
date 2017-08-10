using Autofac;
using RukatukApi.Services;

namespace RukatukApi.IOC
{
    public class RukatukModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();
            builder.RegisterType<EventService>().As<IEventService>().SingleInstance();
            builder.RegisterType<EventbriteClient>().As<IEventbriteClient>().SingleInstance();
        }
    }
}
