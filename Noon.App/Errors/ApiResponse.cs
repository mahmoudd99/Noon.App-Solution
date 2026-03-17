namespace Noon.App.Errors
{
    public class ApiResponse
    {


        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                400 => "A Bad Request , you have made",
                401 => "Authorized , you are not",
                404 => "Resourse was not Found",
                500 => "error path to fail fjwfjqlkfq ",
                 _  => null
            };

        }
    }
}
