using Autofac;
using FlickrNet;
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
            builder.Register(c => {
                var config = c.Resolve<IConfiguration>();
                return new Flickr(config.FlickrApiKey, config.FlickrApiSecret);
                }).SingleInstance();
        }
    }
}
