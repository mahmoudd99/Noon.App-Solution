namespace Noon.App.Errors
{
    public class ApiValidationError :ApiResponse
    {

        public IEnumerable<string> Errors { get; set; }
        public ApiValidationError():base(400)
        {
            
        }
    }
}
