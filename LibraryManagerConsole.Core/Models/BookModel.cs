﻿namespace LibraryManagerConsole.Core.Models
{
    /// <summary>
    /// Book with Id
    /// </summary>
    public class BookModel : BookViewModel
    {
        public BookModel()
            : base()
        {

        }
        public int Id { get; set; }
    }
}
