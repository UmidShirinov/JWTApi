using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class Response<T> where T:class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccessfull { get; private set; }     
        public ErrorDto Error { get; private set; }

                    
        public static Response<T> Success(T data , int statusCode)
        {
            // data elave edildikde ve ya get edildikde isdifade olunur
            return new Response<T> { Data = data, StatusCode = statusCode , IsSuccessfull=true };
        }

        public static Response<T> Success(int statusCode)
        {

            // data update edildikde ve delete edildikde datani donmeye ehtiyac qalmir
            return new Response<T> { StatusCode = statusCode , IsSuccessfull = true };
        }

        public static Response<T> Failed(ErrorDto errorDto , int statusCode)
        {
            return new Response<T>
            {
                Error = errorDto,
                StatusCode = statusCode,
                IsSuccessfull = false
            };
        }

        public static Response<T> Failed(string errorMessage , int statusCode , bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);

            return new Response<T>
            {
                Error = errorDto,
                StatusCode = statusCode,
                IsSuccessfull = false

            };
        }
    }
}
