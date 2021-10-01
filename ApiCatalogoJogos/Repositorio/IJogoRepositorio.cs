using ApiCatalogoJogos.Entidades;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositorio
{
    public interface IJogoRepositorio : IDisposable
    {
        Task<List<Jogo>> Obter(int pagina, int quantidade);
        Task<Jogo> Obter(Guid id);
        Task<List<Jogo>> Obter(string nome, string produtora);
        Task<Jogo> Inserir(Jogo jogo);
        Task Atualizar(Jogo jogo);
        Task Remover(Guid id);

    }
}
