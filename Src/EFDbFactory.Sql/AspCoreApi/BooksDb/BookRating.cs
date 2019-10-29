using System;
using System.Collections.Generic;

namespace AspCoreApi
{
    public partial class BookRating
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }

        public virtual Book Book { get; set; }
    }
}
