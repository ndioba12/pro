namespace SIGRHBack.Dtos.Shared
{
    public class ResponseDto<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<object>? Errors { get; set; }
        public int? CountTotal { get; set; }
        public T? Result { get; set; }
    }
    public class ResponseDto
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<object>? Errors { get; set; }
        public object? Result { get; set; }
    }
}
