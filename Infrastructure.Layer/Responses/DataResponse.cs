using Infrastructure.Layer.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Layer.Responses
{
    /// <summary>
    /// Objeto padrão de retorno das API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResponse<T>
    {
        /// <summary>
        /// Retorno da API
        /// </summary>
        public T Content { get; set; }
        /// <summary>
        /// Mensagem da API
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Tipo de retorno
        /// </summary>
        [EnumDataType(typeof(CustomTypeResultEnum))]
        public CustomTypeResultEnum TypeResult { get; set; }

        public DataResponse(string content)
        {
            var res = JsonConvert.DeserializeObject<DataResponse<T>>(content);
            this.Content = res.Content;
            this.Message = res.Message;
            this.TypeResult = res.TypeResult;
        }
        public DataResponse() { }
    }
}
