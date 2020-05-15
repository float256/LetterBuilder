using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Dto
{
    public interface ICollapsedNode
    {
        bool IsSelected { get; set; }
        bool IsOpened { get; set; }
    }
}
