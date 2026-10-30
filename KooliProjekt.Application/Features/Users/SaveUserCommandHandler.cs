using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var userNameTaken = await _dbContext.Users
                .AnyAsync(x => x.UserName == request.UserName && x.Id != request.Id, cancellationToken);
            if (userNameTaken)
            {
                return result.AddPropertyError("UserName", "User name '" + request.UserName + "' is already taken");
            }

            var emailTaken = await _dbContext.Users
                .AnyAsync(x => x.Email == request.Email && x.Id != request.Id, cancellationToken);
            if (emailTaken)
            {
                return result.AddPropertyError("Email", "Email '" + request.Email + "' is already in use");
            }

            User user;
            if (request.Id == 0)
            {
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return result.AddPropertyError("Password", "Password is required for a new user");
                }

                user = new User();
                _dbContext.Users.Add(user);
            }
            else
            {
                user = await _dbContext.Users.FindAsync(new object[] { request.Id }, cancellationToken);
                if (user == null)
                {
                    return result.AddError("User with ID " + request.Id + " was not found");
                }
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;

            // Parooli muudetakse ainult siis, kui uus parool anti
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.PasswordHash = PasswordHasher.Hash(request.Password);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
