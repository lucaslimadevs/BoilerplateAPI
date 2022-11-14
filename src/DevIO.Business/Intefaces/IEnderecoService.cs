using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IEnderecoService : IDisposable
    {
        Task Atualizar(Endereco endereco);
    }
}