namespace PedagioPayPortalUsuario.Data.Dtos
{
    public class PlacaDto
    {
        public int? Id { get; set; }
        public string TokenUsuario { get; set; }
        public string Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? Ano { get; set; }
        public string? Cor { get; set; }

    }
}
