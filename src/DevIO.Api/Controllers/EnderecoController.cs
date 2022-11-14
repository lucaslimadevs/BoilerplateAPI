using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    public class EnderecoController: MainController
    {
        private readonly IEnderecoService _enderecoService;
        private readonly IEnderecoRepository _enderecoRepository;        
        private readonly IMapper _mapper;
        public EnderecoController(
            IEnderecoService enderecoService,
            IEnderecoRepository enderecoRepository,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _enderecoRepository = enderecoRepository;
            _mapper = mapper;
            _enderecoService = enderecoService;
        }

        [HttpGet]
        public async Task<IEnumerable<EnderecoViewModel>> ObterTodos()
        {
            var enderecoViewModels = _mapper.Map<IEnumerable<EnderecoViewModel>>(await _enderecoRepository.ObterTodos());
            return enderecoViewModels;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EnderecoViewModel>> Atualizar(Guid id, [FromBody] EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificarErro("O Id informado não é o mesmo que foi passado no objeto");
                return CustomResponse(enderecoViewModel);
            }

            if (!ModelState.IsValid) CustomResponse(ModelState);

            await _enderecoService.Atualizar(_mapper.Map<Endereco>(enderecoViewModel));

            return CustomResponse(enderecoViewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnderecoViewModel>> ObterPorId(Guid id)
        {
            var enderecoViewModel = _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));

            if (enderecoViewModel == null) return NotFound();

            return enderecoViewModel;
        }

    }
}
