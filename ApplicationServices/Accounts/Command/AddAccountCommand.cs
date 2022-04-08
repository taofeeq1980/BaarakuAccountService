using ApplicationServices.Shared.BaseResponse;
using AutoMapper;
using Domain;
using Domain.Entities;
using MediatR;
using System;

namespace ApplicationServices.Accounts.Command
{
    public class AddAccountCommand : IRequest<Result>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Image { get; set; }
        public string MimeType { get; set; }
        public AccountType AccountType { get; set; }
    }
}
