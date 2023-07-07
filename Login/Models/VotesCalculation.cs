using System.IO;

namespace Login.Models
{
    public class VotesCalculation
    {
        public int key { get; set; }
        public string NepaliCongress { get; set; }
        public string RastriyaPrajatantraParty { get; set; }
        public string JanamatParty { get; set; }
        public string RastriyaSwatantraParty { get; set; }
        public string CommunistPartyofNepal { get; set; }
        public string VotedBy { get; set; }
        public string VotesCount { get; set; }


    }
    public class VoteCount
    {
        public int VotesCount { get; set; }
    }
}
