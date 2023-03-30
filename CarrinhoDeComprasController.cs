using FluentResults;
using LojaApi.Data.Dtos.CarrinhoDeCompras;
using LojaApi.Data.Dtos;
using LojaApi.Models;
using LojaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LojaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarrinhoDeComprasController : ControllerBase
    {
        private readonly CarrinhosDeComprasService _comprasService;

        public CarrinhoDeComprasController(CarrinhosDeComprasService comprasService)
        {
            _comprasService = comprasService;
        }

        [HttpPost("CriaCarrinho")]
        public async Task<CarrinhoDeCompras> CriaCarrinho([FromBody] CreateCarrinhoDeComprasDto carrinhoDeComprasDto)
        {
            return await _comprasService.CriaCarrinho(carrinhoDeComprasDto);
        }

        [HttpGet("{id}")]
        public IActionResult PesquisaCarrinhoPorId(int id)
        {
            ReadCarrinhoDeComprasDto carrinhoDto = _comprasService.PesquisaCarrinhoPorId(id);
            if (carrinhoDto == null) return NotFound("Carrinho de compras não encontrado");
            return Ok(carrinhoDto);
        }

        [HttpGet()]
        public IActionResult PesquisaCarrinho()
        {
            var carrinhoDto = _comprasService.PesquisaCarrinho();
            if (carrinhoDto == null) return NotFound();
            return Ok(carrinhoDto);
        }

        [HttpDelete("{id}")]
        public IActionResult ApagaCarrinho(int id)
        {
            Result resultado = _comprasService.ApagaCarrinho(id);
            if (resultado.IsFailed) return NotFound("Carrinho não encontrado");
            return NoContent();
        }

        [HttpPut("/AdicionaProdutoNoCarrinho")]
        public IActionResult AdicionaProduto([FromBody] CreateProdutoNoCarrinhoDto Dto)
        {
            try
            {
                CreateProdutoNoCarrinhoDto produtonocarrinho =  _comprasService.AdicionaProduto(Dto);
                return CreatedAtAction(nameof(PesquisaCarrinhoPorId), new { Id = produtonocarrinho.ProdutoId }, produtonocarrinho);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Carrinho de compras não existe");
            }
            catch (ArgumentException)
            {
                return BadRequest("Só é possível adicionar produtos ativos");
            }
        }

        
    }
}
