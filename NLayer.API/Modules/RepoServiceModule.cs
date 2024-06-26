using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;
namespace NLayer.API.Modules
{
    public class RepoServiceModule:Module //AutoFac kütüphanesine ait olan Module sınıfının miras alınması. Bağımlılıkların yönetilmesi için kullanılan bir kütüphanedir
    {//AutoFac bir IOC konteyneri'dir ve bağımlılıkları yönetmeye yarar

        protected override void Load(ContainerBuilder builder) //AutoFac'in Module sınıfından gelen bir metod olan Load, dependency yapılandırması için kullanılır
        {
            //builder sınıfı dependency'leri kaydetmek için kullanılır. Load metodu içerisinde builder nesnesi üzerinden kayıtlar yapılarak Autofac konteynerine eklenir
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope(); //GenericRepository'nin IGenericRepository'ye denk geldiği belirtilir ve InstancePerLifeTimeScope ile döngünün yaşam süresi belirlenir
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope(); //Service, IService'e denk gelir

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>(); //UnitOfWork sınıfı IUnitOfWork interface'ine karşılık gelir ve bu şekilde kaydedilir


            var apiAssembly = Assembly.GetExecutingAssembly(); //API projesinin assembly'si alınır
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //repository projesinin assembly'si AppDbContext sınıfından alınır
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); //service projesinin assembly'si MapProfile sınıfından alınır

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope(); //repository sınıfları IRepository interface'ine denk gelir ve bu şekilde kaydedilir


            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope(); //Service sınıfları IService interface'ine denk gelir ve bu şekilde kaydedilir


           // builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();

        }
    }
}
