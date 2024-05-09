namespace Application.Dtos.Default
{
    public class DefaultResponse : BaseResponse<DefaultResponse>
    {
        public DefaultResponse() { }
        public bool Sucess => Errors.Count == 0;
    }
}
