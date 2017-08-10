using Autofac;

namespace RukatukApi.IOC
{
    public static class Container
    {
        private static readonly IContainer _instance;

        static Container()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<RukatukModule>();

            _instance = builder.Build();
        }

        public static IContainer Instance => _instance;
    }
}
