namespace Hsm.Domain.Models.Page
{
    public class PageResponse<T>(IList<T> data, Page pageInfo)
    {
        public IList<T> Data { get; set; } = data;
        public Page PageInfo { get; set; } = pageInfo;


        public PageResponse() : this([], new Page())
        {

        }
    }
}
