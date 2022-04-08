using ApplicationServices.Login.ViewModel;
using ApplicationServices.Shared.BaseResponse;
using MediatR;

namespace ApplicationServices.Login
{
    public class ConnectTokenCommand : IRequest<Result<TokenViewModel>>
    {
        public string Username { get; set; } 
        public string Password { get; set; }
    }
}
