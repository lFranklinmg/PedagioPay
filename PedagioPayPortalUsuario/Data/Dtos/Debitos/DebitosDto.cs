namespace PedagioPayPortalUsuario.Data.Dtos.Debitos
{
    public class DebitosDto
    {
        public int Id_Debito { get; set; }
        public int Id_Passagem { get; set; }
        public string Placa { get; set; }
        public string Concessao { get; set; }
        public decimal Valor {  get; set; }
        public int Eixos { get; set; }
        public DateTime Data { get; set; }
        public string Vencimento { get; set; }
        public DateTime? Pago_Em { get; set; }
        public string Cnpj {  get; set; }
        public string Portico { get; set; }
    }
}