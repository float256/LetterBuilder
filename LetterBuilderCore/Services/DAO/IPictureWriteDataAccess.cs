using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services.DAO
{
    public interface IPictureWriteDataAccess
    {
        void Add(Picture entity);
        void Delete(int id);
    }
}
