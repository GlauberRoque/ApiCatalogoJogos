using ApiCatalogoJogos.Excessoes;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Servicos;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        //Criando um propriedade com readonly(joga a responsabilidade de dar uma instâcia para o asp.net )
        private readonly IJogoServico jogoServico;
        //Construtor da propriedade
        public JogosController(IJogoServico jogoServico)
        {
            this.jogoServico = jogoServico;
        }
        //metodos

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await jogoServico.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }

        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute]Guid idJogo)
        {
            var jogo = await jogoServico.Obter(idJogo);
            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await jogoServico.Inserir(jogoInputModel);
                return Ok(jogo);
            }
            catch(JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com esse nome para essa produtora!");
            }
            
        }

        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try 
            {
                await jogoServico.Atualizar(idJogo, jogoInputModel);
                return Ok();
            }
            catch(JogoNaoCadastradoException ex)
            {
                return NotFound("Não Existe esse jogo");
            }
            
        }

        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] double preco)
        {
            try
            {
                await jogoServico.Atualizar(idJogo, preco);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não Existe esse jogo");
            }

        }

        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> DeletarJogo([FromRoute]Guid idJogo)
        {
            try
            {
                await jogoServico.Remover(idJogo);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não Existe esse jogo");
            }
        }
    }
}
