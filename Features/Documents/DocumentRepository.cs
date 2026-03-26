using Dapper;
using Microsoft.Data.SqlClient;

namespace DocuMind.Features.Documents
{
    public class DocumentRepository
    {
        private readonly string _connectionString;

        public DocumentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> SaveAsync(Document document)
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql = """
            INSERT INTO Documents (Id, FileName, FileExtension, ExtractedText, Status, FileSizeBytes, CreatedAt)
            VALUES (@Id, @FileName, @FileExtension, @ExtractedText, @Status, @FileSizeBytes, @CreatedAt)
            """;

            document.Id = Guid.NewGuid();
            document.CreatedAt = DateTime.UtcNow;

            await connection.ExecuteAsync(sql, document);

            return document.Id;
        }

        public async Task<Document?> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql = "SELECT * FROM Documents WHERE Id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Document>(sql, new { Id = id });
        }
    }
}
