using System.Collections.Generic;
using MovieLib.Core;

namespace MovieLib.Data.Entities
{
    public class Movie : IIdentityEntity
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string DirectorName { get; set; }
        public string ReleaseYear { get; set; }
        public virtual ICollection<MoviesReview> Reviews { get; set; }

        public int EntityId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
}
