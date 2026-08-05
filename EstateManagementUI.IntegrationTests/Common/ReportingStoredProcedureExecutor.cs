using System.Data;
using Microsoft.Data.SqlClient;

namespace EstateManagementUI.IntegrationTests.Common;

internal sealed class ReportingStoredProcedureExecutor
{
    public async Task<DataTable> ExecuteAsync(
        String connectionString,
        string storedProcedureName,
        IReadOnlyDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await using var command = new SqlCommand(storedProcedureName, connection)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 120
        };

        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
            }
        }

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var results = new DataTable();
        results.Load(reader);
        return results;
    }

    public async Task RunSqlAsync(
        String connectionString,
        string sqlString,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await using var command = new SqlCommand(sqlString, connection)
        {
            CommandType = CommandType.Text,
            CommandTimeout = 120
        };


        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<DataTable> ExecuteTextAsync(
        String connectionString,
        string sqlString,
        IReadOnlyDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await using var command = new SqlCommand(sqlString, connection)
        {
            CommandType = CommandType.Text,
            CommandTimeout = 120
        };

        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
            }
        }

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var results = new DataTable();
        results.Load(reader);
        return results;
    }
}
