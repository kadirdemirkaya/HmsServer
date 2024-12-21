using EfCore.Repository.Abstractions;
using Hsm.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hsm.Application.Filters
{
    public class GenericNotFoundFilter<T>(IUnitOfWork<T> _unitOfWork) : IAsyncActionFilter
        where T : class, IBaseEntity, new()
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var value = context.ActionArguments.FirstOrDefault().Value;
            if (value == null)
            {
                throw new Exception("The input is invalid. Please try to use valid id");
            }

            var id = (Guid)value!;
            var entity = await _unitOfWork.GetReadRepository().GetByIdAsync(id);

            if (entity == null)
            {
                throw new Exception("The id does not exist. Please try to use existing id");
            }

            await next.Invoke();
            return;

        }
    }
}
