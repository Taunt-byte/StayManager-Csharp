using System.Text.Json;
using HotelApp.Models;

namespace HotelApp.Data;

/// <summary>Repositório genérico que guarda os dados em arquivos JSON (%AppData%\HotelApp).</summary>
public class JsonRepository<T> where T : class, IEntidade
{
    private static readonly JsonSerializerOptions Opcoes = new() { WriteIndented = true };
    private readonly string _caminho;
    private readonly List<T> _itens;

    public JsonRepository(string arquivo)
    {
        string pasta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HotelApp");
        Directory.CreateDirectory(pasta);
        _caminho = Path.Combine(pasta, arquivo);
        _itens = Carregar();
    }

    public IReadOnlyList<T> Todos() => _itens;

    public void Salvar(T item)
    {
        if (item.Id == 0)
        {
            item.Id = _itens.Count == 0 ? 1 : _itens.Max(i => i.Id) + 1;
            _itens.Add(item);
        }
        else
        {
            int idx = _itens.FindIndex(i => i.Id == item.Id);
            if (idx >= 0) _itens[idx] = item; else _itens.Add(item);
        }
        Persistir();
    }

    public void Remover(int id)
    {
        _itens.RemoveAll(i => i.Id == id);
        Persistir();
    }

    private List<T> Carregar()
    {
        try
        {
            if (File.Exists(_caminho))
                return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(_caminho)) ?? new();
        }
        catch (JsonException) { /* arquivo corrompido: começa vazio */ }
        return new();
    }

    private void Persistir() =>
        File.WriteAllText(_caminho, JsonSerializer.Serialize(_itens, Opcoes));
}
