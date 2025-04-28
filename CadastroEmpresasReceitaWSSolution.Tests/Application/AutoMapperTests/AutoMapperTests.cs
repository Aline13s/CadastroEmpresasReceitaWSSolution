using AutoMapper;
using CadastroEmpresasReceitaWSSolution.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroEmpresasReceitaWSSolution.Tests.Application.AutoMapperTests
{
    public class AutoMapperTests
    {
        [Fact]
        public void Deve_Validar_Todos_Mapeamentos()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            config.AssertConfigurationIsValid();
        }
    }
}
