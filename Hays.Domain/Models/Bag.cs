using Hays.Domain.Entities;

namespace Hays.Domain.Models
{
    public class Bag<T> where T : BaseEntity
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T? Data { get; set; }

        public Bag()
        {
            Errors = new List<string>();
        }

        public Bag(string erro)
        {
            Errors = new List<string>();
            Errors.Append(erro);
        }

        public Bag(List<string> erros)
        {
            Errors = erros;
        }
    }
}
