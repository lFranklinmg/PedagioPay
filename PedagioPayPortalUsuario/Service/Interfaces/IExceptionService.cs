namespace PedagioPayPortalUsuario.Service.Interfaces
{
    public interface IExceptionService
    {
        public string AlreadyExistingException(string mensage);
        public string NotFoundException(string mensage);
        public string BussinessException(string mensage);
    }
}
