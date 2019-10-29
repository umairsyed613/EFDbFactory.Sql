using System;
using System.Collections.Generic;

namespace AspCoreApi
{
    public partial class Book
    {
        public Book()
        {
            BookRating = new HashSet<BookRating>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }
        public virtual ICollection<BookRating> BookRating { get; set; }
    }
}
