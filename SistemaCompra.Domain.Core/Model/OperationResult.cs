using MediatR;

namespace SistemaCompra.Domain.Core.Model
{
    public class OperationResult : IRequest<OperationResult>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
