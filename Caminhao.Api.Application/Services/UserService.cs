using Caminhao.Api.Domain.UserAggregate;
using Caminhao.Api.Infrastructure.Cryptography;
using Caminhao.Api.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Caminhao.Api.Application.Services
{
    public interface IUserService : ICrudService<User>
    {
        Task<User> Login(string username, string email, string password);
        Task<User> GetIfUsernameOrEmailExists(string usernameOrEmail);
        Task<User> ChangePassword(string id, string password);
        Task<User> ChangeProfile(User utilisateur);
        bool IsChangePasswordTokenValid(string token, string usernameOrEmail);
    }

    public class UserService : CrudService<User>, IUserService {
        private readonly ICryptography DesCryptography;

        public UserService(ILogger<CrudService<User>> logger, 
                                  DatabaseContext context, 
                                  ICryptography desCryptography) : base(logger, context) {
            DesCryptography = desCryptography;
        }

        public async Task<User> Login(string username, string email, string password) {
            try {
                return await Context.Users.
                       FirstOrDefaultAsync(u => (u.Username == username || u.Email == email) && u.Password == password);
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<User> GetIfUsernameOrEmailExists(string usernameOrEmail) {
            try {
                return await Context.Users.FirstOrDefaultAsync(u => u.Username.Equals(usernameOrEmail) || u.Email.Equals(usernameOrEmail));
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<User> ChangePassword(string usernameOrEmail, string password) {
            try {
                User utilisateur = await GetIfUsernameOrEmailExists(usernameOrEmail);
                if(utilisateur != null) {
                    utilisateur.Password = password;
                    await Context.SaveChangesAsync();
                    return utilisateur;
                } else
                    return null;
                
            } catch(DbUpdateException ex) {
                _logger.LogError(ex, ex.Message);
                throw ex;
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public bool IsChangePasswordTokenValid(string token, string usernameOrEmail) {
            try {
                string plain = DesCryptography.Decrypt(token);
                string[] flats = plain.Split('#');
                string changePasswordToken = flats[0];
                double timeout = double.Parse(flats[1]);
                DateTime oDate = DateTime.Parse(flats[2]);

                if(changePasswordToken != usernameOrEmail) {
                    return false;
                }

                DateTime datetimeout = oDate.AddMinutes(timeout);
                if(datetimeout.CompareTo(DateTime.Now) < 0) {
                    return false;
                }
                return true;
            } catch(CryptographicException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<User> ChangeProfile(User utilisateur) {
            try {
                Context.Users.Attach(utilisateur);
                EntityEntry<User> contextEntry = Context.Entry(utilisateur);
                contextEntry.Property(u => u.Firstname).IsModified = true;
                contextEntry.Property(u => u.Lastname).IsModified = true;
                contextEntry.Property(u => u.Middlename).IsModified = true;
                contextEntry.Property(u => u.Sex).IsModified = true;
                contextEntry.Property(u => u.Email).IsModified = true;
                contextEntry.Property(u => u.Username).IsModified = true;
                await Context.SaveChangesAsync();
                return utilisateur;
            } catch(DbUpdateException ex) {
                _logger.LogError(ex, ex.Message);
                throw ex;
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
            
        }
    }
}
