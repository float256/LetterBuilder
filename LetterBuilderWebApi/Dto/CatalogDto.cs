using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebApi.Dto
{
    public class CatalogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderInParentCatalog { get; set; }
    }
}
