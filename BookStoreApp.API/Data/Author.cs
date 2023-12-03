using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApp.API.Data;

public partial class Author
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Bio { get; set; }

    [NotMapped]
    public string FullName => FirstName + LastName;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
