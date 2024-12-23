using EfCore.Repository.Abstractions;
using EfCore.Repository.Concretes;
using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hsm.Application.Filters
{
    public class GenericSpecificNotFoundFilter<T>(IUnitOfWork<T> _unitOfWork) : IAsyncActionFilter
        where T : BaseEntity, IBaseEntity, new()
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var value = context.ActionArguments.FirstOrDefault().Value;
            
            if (value == null)
            {
                throw new Exception("The input is invalid. Please try to use a valid id.");
            }

            var idProperty = value.GetType().GetProperty("Id");

            if (idProperty == null)
            {
                throw new Exception("The provided object does not have an 'Id' property.");
            }
            
            var id = idProperty.GetValue(value);

            if (id == null || !(id is Guid))
            {
                throw new Exception("The Id property is invalid or not of type Guid.");
            }

            var entity = await _unitOfWork.GetReadRepository().GetByIdAsync(id);

            if (entity == null)
            {
                throw new Exception("The id does not exist. Please try to use existing id");
            }

            await next.Invoke();
        }
    }
}
