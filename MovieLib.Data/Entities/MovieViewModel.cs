using MovieLib.Core;

namespace MovieLib.Data.Entities
{
    public class MovieViewModel : IIdentityEntity
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string DirectorName { get; set; }
        public string ReleaseYear { get; set; }
        public int NoOfReviews { get; set; }

        public int EntityId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
}
