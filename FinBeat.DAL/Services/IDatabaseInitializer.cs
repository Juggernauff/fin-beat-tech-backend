namespace FinBeat.DAL.Services
{
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Ensures the database is created and applies any pending migrations.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
