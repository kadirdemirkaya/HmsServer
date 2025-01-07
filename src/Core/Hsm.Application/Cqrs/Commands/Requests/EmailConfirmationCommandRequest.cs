using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class EmailConfirmationCommandRequest : IEventRequest<EmailConfirmationCommandResponse>
    {
        public string Email { get; set; }
        public int Code { get; set; }
        public EmailConfirmationCommandRequest(string email, int code)
        {
            Email = email;
            Code = code;
        }


    }
}
