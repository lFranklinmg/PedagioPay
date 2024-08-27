using PedagioPayApiControlador.Data.Entidades;

namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class HeaderAutorizacaoDto
    {
        private Guid? _eventId;
        public Guid? EventId
        {
            get { return _eventId ?? (_eventId = Guid.NewGuid()); }
            set { _eventId = value; }
        }

        private string _eventDateTime;
        public string EventDateTime
        {
            get { return _eventDateTime ?? (_eventDateTime = DateTime.Now.ToString("yyyyMMddHHmmss")); }
            set { _eventDateTime = value; }
        }

        public TipoEventoEnum EventType { get; set; }
    }
}
