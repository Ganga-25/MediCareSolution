using MediCare.Application;
using MediCare.Application.Contracts;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.ServiceImplementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Infrastructure.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMediCareServices(this IServiceCollection service)
        {
            
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IUserService, UserService>();

            return service;
        } 
        
    }
}
