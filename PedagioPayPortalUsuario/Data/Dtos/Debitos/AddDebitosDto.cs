namespace PedagioPayPortalUsuario.Data.Dtos.Debitos
{
    public class AddDebitosDto
    {
        public string? TOKEN_USUARIO { get; set; }
        public long ID_PASSAGEM { get; set; }
        public byte ID_OSA { get; set; }
        public string CATEGORIA { get; set; }
        public int EIXOS { get; set; }
        public decimal VALOR {  get; set; }
        public string? VENCIMENTO { get; set; }
        public DateTime? PAGO_EM {  get; set; }
        public DateTime DH_PASSAGEM { get; set; }
        public string CONCESSAO { get; set; }
        public string PLACA { get; set; }
        public string? TAG {  get; set; }
    }
}
