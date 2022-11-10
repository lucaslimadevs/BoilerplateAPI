using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration
{
    public class AutoMapperProfileConfig: Profile
    {
        public AutoMapperProfileConfig()
        {
            //ReverseMap tambem faz o mapeamento reverso
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();
        }
    }
}
