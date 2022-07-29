using FluentValidation.Results;
using Hays.Domain.Entities;
using Hays.Domain.Models;

namespace Hays.Domain.Services
{
    public abstract class ServiceBase<T> where T : BaseEntity
    {
        protected Bag<T> ErroResult<T>(List<ValidationFailure>? erros = null, T? data = null) where T : BaseEntity
        {
            return new Bag<T>
            {
                Errors = (erros?.Any() ?? false) ? erros.Select(x => x.ErrorMessage) : new List<string>().AsEnumerable(),
                Data = data,
                IsSuccess = false,
            };
        }

        protected Bag<T> ErroResult<T>(string erro, T? data = null) where T : BaseEntity
        {
            return new Bag<T>
            {
                Errors = new List<string> { erro},
                Data = data,
                IsSuccess = false,
            };
        }

        protected Bag<T> OkResult<T>(T? data = null) where T : BaseEntity
        {
            return new Bag<T>
            {
                Data = data,
                IsSuccess = true,
            };
        }

        public Bag<T> RunSafe(Func<T> predicated)
        {
            try
            {
                var result = predicated();
                return new Bag<T>
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception)
            {
                return new Bag<T>("An erro ocurred while processing your request on the database.");
            }
        }

        public async Task<Bag<T>> RunSafe<T>(Func<Task> predicated, T data) where T : BaseEntity
        {
            try
            {
                await predicated();
                return new Bag<T>
                {
                    Data = data,
                    IsSuccess = true
                };
            }
            catch (Exception)
            {
                return new Bag<T>("An erro ocurred while processing your request on the database.");
            }
        }
    }
}
