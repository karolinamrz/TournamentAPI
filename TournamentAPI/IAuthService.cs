namespace TournamentAPI
{
    public interface IAuthService
    {
        string CreateJwtToken(User user);
        Task<User?> AuthenticateUser(string email, string password);
        string ComputePasswordHash(string password);
        bool VerifyPasswordHash(string password, string storedHash);
    }

    public interface IQueryService
    {
        IQueryable<Tournament> GetTournaments();
        Task<IEnumerable<Match>> GetMyMatches();
        Task<Tournament?> GetTournamentById(int id);
        Task<IEnumerable<User>> GetTournamentParticipants(int tournamentId);
        Task<IEnumerable<Match>> GetTournamentMatches(int tournamentId);
        Task<User?> GetCurrentUser();
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<Bracket>> GetAllBrackets();
    }

    public interface IMutationService
    {
        Task<User> RegisterUser(string firstName, string lastName, string email, string password);
        Task<string> LoginUser(string email, string password);
        Task<Tournament> CreateTournament(string name);
        Task<bool> AddParticipant(int tournamentId, int userId);
        Task<bool> StartTournament(int tournamentId);
        Task<Match> PlayMatch(int matchId, int winnerId);
        Task<bool> FinishTournament(int tournamentId);
        Task<bool> UpdateTournamentStatus(int tournamentId, string newStatus);
        Task<bool> RemoveParticipant(int tournamentId, int userId);
        Task<User> UpdateUserProfile(int userId, string? firstName, string? lastName, string? email);
        Task<bool> DeleteTournament(int tournamentId);
    }
}