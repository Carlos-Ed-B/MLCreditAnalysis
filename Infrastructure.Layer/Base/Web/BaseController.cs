using Infrastructure.Layer.Base.Interfaces;
using Infrastructure.Layer.Enums;
using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Layer.Web.Base
{
    public class BaseController : Controller
    {
        #region ApiResponse

        /// <summary>
        /// Retorna uma resposta com os padrões da API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="typeResult"></param>
        /// <returns></returns>
        protected internal static OkObjectResult ApiResponse<T>(T value, string message = "", CustomTypeResultEnum typeResult = CustomTypeResultEnum.Success)
        {
            if (message.IsNullOrEmpty())
            {
                if (typeResult == CustomTypeResultEnum.Success || typeResult == CustomTypeResultEnum.Warning)
                {
                    message = "Ação efetuada com sucesso.";
                }

                if (typeResult == CustomTypeResultEnum.Error || typeResult == CustomTypeResultEnum.CriticalError)
                {
                    message = "Não foi possível efetuar a ação solicitada.";
                }
            }

            var res = new DataResponse<T>
            {
                Message = message,
                Content = value,
                TypeResult = typeResult
            };

            return new OkObjectResult(res);
        }

        /// <summary>
        /// Retorna uma resposta com os padrões da API
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected internal OkObjectResult ApiResponse(bool status)
        {
            if (status) { return ApiResponse(CustomTypeResultEnum.Success); }

            return ApiResponse(CustomTypeResultEnum.Error);
        }

        /// <summary>
        /// Retorna uma resposta com os padrões da API
        /// </summary>
        /// <param name="message"></param>
        /// <param name="typeResult"></param>
        /// <returns></returns>
        protected internal OkObjectResult ApiResponse(string message, CustomTypeResultEnum typeResult)
        {
            return ApiResponse<object>(null, message, typeResult);
        }

        protected internal OkObjectResult ApiResponse(CustomTypeResultEnum typeResult)
        {
            return ApiResponse<object>(null, string.Empty, typeResult);
        }

        protected internal OkObjectResult ApiResponse<T>(T value, CustomTypeResultEnum typeResult)
        {
            return ApiResponse<T>(value, string.Empty, typeResult);
        }

        protected internal OkObjectResult ApiResponse(bool isValid, string errorMessage)
        {
            if (isValid)
            {
                return this.ApiResponseSuccess(string.Empty);
            }

            return this.ApiResponseError(string.Empty, errorMessage);
        }

        protected internal OkObjectResult ApiResponse<T>(T value, bool isValid, string message)
        {
            if (isValid)
            {
                return this.ApiResponseSuccess<T>(value, message);
            }

            return this.ApiResponseError<T>(value, message);
        }

        protected internal IActionResult ApiResponseFile(FileContentResult file, bool isValid, string message)
        {
            if (isValid)
            {
                return file;
            }

            return this.ApiResponseError<FileContentResult>(file, message);
        }

        protected internal IActionResult ApiResponseFile(byte[] contents, string fileName, IBaseCommunicationMessage communicationMessage)
        {
            var file = new FileContentResult(contents, "application/octet-stream");
            file.FileDownloadName = fileName;
            return this.ApiResponseFile(file, communicationMessage.IsValid(), communicationMessage.GetMessageByStatus());
        }

        protected internal OkObjectResult ApiResponse<T>(T value, IBaseCommunicationMessage communicationMessage)
        {
            return this.ApiResponse(value, communicationMessage.IsValid(), communicationMessage.GetMessageByStatus());
        }

        protected internal OkObjectResult ApiResponse(IBaseCommunicationMessage communicationMessage)
        {
            return this.ApiResponse(string.Empty, communicationMessage.IsValid(), communicationMessage.GetMessageByStatus());
        }

        protected internal OkObjectResult ApiResponseSuccess(string message)
        {
            return ApiResponse<object>(null, message, CustomTypeResultEnum.Success);
        }

        protected internal OkObjectResult ApiResponseSuccess<T>(T value, string message)
        {
            return ApiResponse<T>(value, message, CustomTypeResultEnum.Success);
        }

        protected internal OkObjectResult ApiResponseSuccess<T>(T value)
        {
            return ApiResponse<T>(value, string.Empty, CustomTypeResultEnum.Success);
        }

        protected internal OkObjectResult ApiResponseError(string message)
        {
            return ApiResponse<object>(null, message, CustomTypeResultEnum.Error);
        }

        protected internal OkObjectResult ApiResponseError<T>(T value)
        {
            return ApiResponse<T>(value, string.Empty, CustomTypeResultEnum.Error);
        }

        protected internal OkObjectResult ApiResponseError<T>(T value, string message)
        {
            return ApiResponse<T>(value, message, CustomTypeResultEnum.Error);
        }

        #endregion

    }
}