using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Diagnostics;

namespace EntityFrameworkService.Validation
{
    public class ValidationErrorResponse
    {
        public static object GetValidationErrorResponse(ModelStateDictionary modelState, string traceId, string X_GP_Request_Id)
        {
            var errors = new Dictionary<string, List<string>>();

            foreach (var errorPair in modelState)
            {
                var errorList = new List<string>();
                foreach (var error in errorPair.Value.Errors)
                {
                    errorList.Add(error.ErrorMessage);
                }
                errors[errorPair.Key] = errorList;
            }

            return GetValidationErrorResult(traceId, X_GP_Request_Id, errors);
        }

        public static object GetValidationErrorResult(string traceId, string X_GP_Request_Id, Dictionary<string, List<string>> errors)
        {
            return new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "One or more validation errors occurred.",
                status = 400,
                traceId = traceId,
                errors = errors,
                x_gp_request_id = X_GP_Request_Id
            };
        }
    }
}
