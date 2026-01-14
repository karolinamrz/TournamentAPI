namespace TournamentAPI
{
    public class Bracket
    {
        public int Id { get; set; }
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}