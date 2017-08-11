using Autofac;
using FlickrNet;
using Microsoft.WindowsAzure.Storage;
using RukatukApi.Services;

namespace RukatukApi.IOC
{
    public class RukatukModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var configuration = new Configuration();
            builder.RegisterInstance(configuration).As<IConfiguration>().SingleInstance();
            builder.RegisterType<EventService>().As<IEventService>().SingleInstance();
            builder.RegisterType<AzureStorageEventRepository>().As<IEventRepository>().SingleInstance();
            builder.RegisterType<EventbriteClient>().As<IEventbriteClient>().SingleInstance();
            builder.Register(c => new Flickr(configuration.FlickrApiKey, configuration.FlickrApiSecret)).SingleInstance();
        }
    }
}
