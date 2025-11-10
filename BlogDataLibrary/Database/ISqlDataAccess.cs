using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataLibrary.Database
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string sql, U parameters, string connectionStringName);
        Task<int> SaveData<T>(string sql, T parameters, string connectionStringName);
    }
}
