using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LetterBuilderCore.Models
{
    class CatalogNotFoundException : Exception
    {
        public CatalogNotFoundException() : base() { }
        public CatalogNotFoundException(string message) : base(message) { }
        public CatalogNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
