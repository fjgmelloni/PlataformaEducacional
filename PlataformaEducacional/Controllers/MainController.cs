using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using System.Net;

namespace PlataformaEducacional.Api.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICurrentUser _identityUser;

        protected MainController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            ICurrentUser identityUser)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
            _identityUser = identityUser;
        }

        protected bool IsOperationValid()
        {
            return !_notifications.HasNotifications();
        }

        protected ActionResult CustomResponse(HttpStatusCode statusCode = HttpStatusCode.OK, object? data = null)
        {
            if (IsOperationValid())
            {
                return new ObjectResult(new
                {
                    Success = true,
                    Data = data,
                })
                {
                    StatusCode = (int)statusCode
                };
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Errors", _notifications.GetNotifications().Select(n => n.Value).ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            foreach (var error in modelState.Values.SelectMany(v => v.Errors))
            {
                NotifyError("ModelState", error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected IEnumerable<string> GetErrorMessages()
        {
            return _notifications.GetNotifications().Select(c => c.Value).ToList();
        }

        protected void NotifyError(string code, string message)
        {
            _mediatorHandler.PublishNotificationAsync(new DomainNotification(code, message));
        }
    }
}
