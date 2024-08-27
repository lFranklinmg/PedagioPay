using PedagioPayApiControlador.Data.Entidades;
using System.Reflection.PortableExecutable;

namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class PayloadRequestDto
    {
        public class RequestAutorizacaoDTO : BaseAutorizacaoDto<RequestAutorizacaoDTO.RequestAutorizacaoPayloadDTO>
        {
            public RequestAutorizacaoDTO()
            {
                Header.EventType = TipoEventoEnum.RequisicaoAutorizacaoPassagem;
            }


            public class RequestAutorizacaoPayloadDTO
            {
                public RequestAutorizacaoLocalDTO local { get; set; }
                public RequestAutorizacaoRequestDTO request { get; set; }
            }

            public class RequestAutorizacaoLocalDTO
            {
                public int concessionaireId { get; set; }
                public int tollPlazaId { get; set; }
                public string laneId { get; set; }
                public string lane { get; set; }
                public string hwSerialNumber { get; set; }
                public string laneSecId { get; set; }
                public string direction { get; set; }
            }

            public class RequestAutorizacaoRequestDTO
            {
                public string transactionId { get; set; }
                public string sgaRequestId { get; set; }
                public int? operationType { get; set; }
                public int? paymentTypeId { get; set; }
                public int? paymentTypeDetailId { get; set; }
                public int debitAmount { get; set; }
                public string debitType { get; set; }
                public string plate { get; set; }
                public string midiaUid { get; set; }
            }

        }
    }
}
