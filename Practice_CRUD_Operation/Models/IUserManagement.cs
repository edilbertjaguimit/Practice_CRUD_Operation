using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Practice_CRUD_Operation.Models
{
    public interface IUserManagement
    {
        Task<List<User>> ReadAsync();
        Task<bool> InsertAsync(User user);
        Task<bool> CheckEmailAsync(string email);
        Task<bool> CheckPasswordAsync(string email, string password);
        Task<bool> UpdateAsync(User user);
    }
}