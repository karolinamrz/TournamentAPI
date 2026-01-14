using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data;

namespace TournamentAPI
{
    public class Query
    {
        public IQueryable<Tournament> GetTournaments([Service] AppDbContext context)
        {
            return context.Tournaments
                .Include(t => t.Participants)
                .Include(t => t.Bracket)
                .ThenInclude(b => b!.Matches)
                .ThenInclude(m => m.Player1)
                .Include(t => t.Bracket)
                .ThenInclude(b => b!.Matches)
                .ThenInclude(m => m.Player2)
                .Include(t => t.Bracket)
                .ThenInclude(b => b!.Matches)
                .ThenInclude(m => m.Winner);
        }

        public async Task<List<Match>> GetMyMatches(
            [Service] AppDbContext context,
            ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return new List<Match>();
            }

            return await context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Where(m => m.Player1Id == userId || m.Player2Id == userId)
                .OrderBy(m => m.Round)
                .ThenBy(m => m.Id)
                .ToListAsync();
        }

        public List<User> GetUsers([Service] AppDbContext context)
        {
            return context.Users
                .OrderBy(u => u.Id)
                .ToList();
        }

        public async Task<Tournament?> GetTournament(
            [Service] AppDbContext context,
            int id)
        {
            return await context.Tournaments
                .Include(t => t.Participants)
                .Include(t => t.Bracket)
                .ThenInclude(b => b!.Matches)
                .ThenInclude(m => m.Player1)
                .Include(t => t.Bracket)
                .ThenInclude(b => b!.Matches)
                .ThenInclude(m => m.Player2)
                .Include(t => t.Bracket)
                .ThenInclude(b => b!.Matches)
                .ThenInclude(m => m.Winner)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public List<Match> GetMatches([Service] AppDbContext context)
        {
            return context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Include(m => m.Bracket)
                .OrderBy(m => m.Round)
                .ThenBy(m => m.Id)
                .ToList();
        }
    }
}