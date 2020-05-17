using LetterBuilderCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Dto
{
    public class TextBlockWithFieldVerifying
    {
        [Required(ErrorMessage = "Данное поле является обязательным")]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Данное поле является обязательным")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Данное поле является обязательным")]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Данное поле является обязательным")]
        [Display(Name = "ID родительского каталога")]
        public int ParentCatalogId { get; set; }

        [Required(ErrorMessage = "Данное поле является обязательным")]
        [HiddenInput(DisplayValue = false)]
        public int OrderInParentCatalog { get; set; }
    }
}

