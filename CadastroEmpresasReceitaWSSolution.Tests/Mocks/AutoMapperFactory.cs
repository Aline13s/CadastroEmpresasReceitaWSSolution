using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Mappings;

namespace CadastroEmpresasReceitaWSSolution.Tests.Mocks
{
    public static class AutoMapperFactory
    {
        public static IMapper Create()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            return config.CreateMapper();
        }
    }
}
