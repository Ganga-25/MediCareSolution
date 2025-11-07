using Dapper;
using MediCare.Application.Contracts.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
namespace MediCare.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly string _connectionString;

    public GenericRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("defaultconnection")
                            ?? throw new ArgumentNullException("Connection string 'defaultconnection' not found.");
    }

    // Helper method to manage connection safely
    private async Task<TResult> WithConnection<TResult>(Func<IDbConnection, Task<TResult>> func)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return await func(connection);
    }

    public Task<IEnumerable<T>> GetAllAsync(string spName)
    {
        return WithConnection(async conn =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", "GETALL");
            return await conn.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        });
    }

    public Task<T?> GetByIdAsync(string spName, string idParameterName, object id)
    {
        return WithConnection(async conn =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", "GETBYID");
            parameters.Add(idParameterName, id);
            return await conn.QueryFirstOrDefaultAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        });
    }

    public Task<int> AddAsync(string spName, T entity)
    {
        return WithConnection(async conn =>
        {
            var parameters = BuildParameters(entity, "INSERT");
            return await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        });
    }

    public Task<int> UpdateAsync(string spName, T entity)
    {
        return WithConnection(async conn =>
        {
            var parameters = BuildParameters(entity, "UPDATE");
            return await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        });
    }

    public Task<int> DeleteAsync(string spName, string idParameterName, object id, object deletedBy)
    {
        return WithConnection(async conn =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", "DELETE");
            parameters.Add(idParameterName, id);
            parameters.Add("@DELETEDBY", deletedBy);
            return await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        });
    }

        private DynamicParameters BuildParameters(T entity, string flag)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", flag);

            // Properties to exclude (from BaseEntity)
            var excludedProperties = new HashSet<string>
            {
                "CreatedOn", "ModifiedAt", 
                "DeletedOn",  "IsDeleted"
            };

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Skip excluded properties
                if (excludedProperties.Contains(prop.Name))
                    continue;

                var value = prop.GetValue(entity);

                // Handle enums: both nullable and non-nullable
                if (value != null)
                {
                    // Check if the actual value is an enum
                    var valueType = value.GetType();
                    if (valueType.IsEnum)
                    {
                        // Convert enum to string
                        value = value.ToString();
                    }
                }
                else
                {
                    // For null values, check if the property type is a nullable enum
                    var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (underlyingType != null && underlyingType.IsEnum)
                    {
                        // Keep value as null (already null)
                        value = null;
                    }
                }

                parameters.Add("@" + prop.Name.ToUpper(), value);
            }

            return parameters;
        }
    }
}
