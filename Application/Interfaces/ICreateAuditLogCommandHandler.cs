using Application.Commands;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICreateAuditLogCommandHandler
    {
        Task HandleAsync(CreateAuditLogCommand request);
    }
}
