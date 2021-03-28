using Microsoft.AspNetCore.Mvc;
using SignalRApp.Business.Utilities.Results;

namespace SignalRApp.Utilities
{
    public static class ResponseCreator
    {
        public static ActionResult CreateResponse(Result result)
        {
            if (result.Success)
            {
                return new OkObjectResult(result.Message);
            }

            return new BadRequestObjectResult(result.Message);
        }

        public static ObjectResult CreateDataResponse<T>(DataResult<T> result) where T : class
        {
            if (result.Success)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestObjectResult(result);
        }
    }
}