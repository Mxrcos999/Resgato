namespace Application.Dtos.Default
{
    public class BaseResponse<T>
    {
        public BaseResponse() { }
        public T? Data { get; set; }
        public void AddError(string error) => Errors.Add(error);
        public void AddErrors(IEnumerable<string> errors) => Errors.AddRange(errors);
        public bool Success => Errors.Count == 0;
        public List<string> Errors { get; set; } = new List<string>(); // Certifique-se de inicializar a lista de erros.
    }
}
