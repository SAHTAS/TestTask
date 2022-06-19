using Newtonsoft.Json;

namespace API.Models
{
    public class ErrorResponseModel : BaseResponseModel
    {
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}