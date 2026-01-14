using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data;

namespace TournamentAPI
{
    public class Mutation
    {
        public async Task<string> RegisterUser(
            [Service] AppDbContext context,
            string firstName,
            string lastName,
            string email,
            string password)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = password
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return $"User {email} registered successfully";
        }

        public async Task<string> LoginUser(
            [Service] AppDbContext context,
            [Service] AuthService authService,
            string email,
            string password)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

            if (user == null)
                throw new Exception("Invalid email or password");

            return authService.GenerateToken(user);
        }

        public async Task<Tournament> CreateTournament(
            [Service] AppDbContext context,
            string name)
        {
            var tournament = new Tournament
            {
                Name = name,
                StartDate = DateTime.Now,
                Status = "Created"
            };

            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();
            return tournament;
        }

        public async Task<bool> AddParticipant(
            [Service] AppDbContext context,
            int tournamentId,
            int userId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            var user = await context.Users.FindAsync(userId);

            if (tournament == null || user == null)
                return false;

            tournament.Participants.Add(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StartTournament(
            [Service] AppDbContext context,
            int tournamentId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null || tournament.Participants.Count < 2)
                return false;

            tournament.Status = "Started";

            var bracket = new Bracket();
            var participants = tournament.Participants.ToList();

            for (int i = 0; i < participants.Count; i += 2)
            {
                if (i + 1 < participants.Count)
                {
                    bracket.Matches.Add(new Match
                    {
                        Round = 1,
                        Player1 = participants[i],
                        Player2 = participants[i + 1]
                    });
                }
            }

            context.Brackets.Add(bracket);
            tournament.Bracket = bracket;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Match> PlayMatch(
            [Service] AppDbContext context,
            int matchId,
            int winnerId)
        {
            var match = await context.Matches
                .Include(m => m.Bracket)
                .ThenInclude(b => b.Matches)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
                throw new Exception("Match not found");

            if (match.WinnerId != null)
                throw new Exception("Match already played");

            if (match.Player1Id != winnerId && match.Player2Id != winnerId)
                throw new Exception("Player not in this match");

            match.WinnerId = winnerId;

            var bracket = match.Bracket;
            var nextRound = match.Round + 1;

            var nextMatch = bracket.Matches
                .FirstOrDefault(m => m.Round == nextRound && (m.Player1Id == null || m.Player2Id == null));

            if (nextMatch != null)
            {
                if (nextMatch.Player1Id == null)
                    nextMatch.Player1Id = winnerId;
                else
                    nextMatch.Player2Id = winnerId;
            }

            await context.SaveChangesAsync();
            return match;
        }
    }
}