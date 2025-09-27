using System.Collections.Concurrent;

namespace CadastroVeiculos.Data
{
    public static class PlacaRepository
    {
        private static ConcurrentDictionary<string, bool> _placas = new();

        public static bool Existe(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa)) return false;
            return _placas.ContainsKey(placa.ToUpperInvariant());
        }

        public static void Adicionar(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa)) return;
            _placas.TryAdd(placa.ToUpperInvariant(), true);
        }

        public static void Limpar()
        {
            _placas.Clear();
        }
    }
}