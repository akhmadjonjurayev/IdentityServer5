namespace IdentityServer5.Models
{
    public class JsonResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public static JsonResponse DataResponse(object data)
        {
            return new JsonResponse { Data = data, IsSuccess = true };
        }

        public static JsonResponse SuccessResponce(string message)
        {
            return new JsonResponse { IsSuccess = true, Message = message };
        }

        public static JsonResponse ErrorResponse(string message)
        {
            return new JsonResponse { IsSuccess = false, Message = message };
        }
    }
}
