using MediatR;
using OrderService.Application.Models;

namespace OrderService.Application.Queries;

public class GetAllOrdersQuery : IRequest<List<OrderDto>>
{
    
}