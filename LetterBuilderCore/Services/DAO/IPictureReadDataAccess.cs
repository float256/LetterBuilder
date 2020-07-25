using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services.DAO
{
    public interface IPictureReadDataAccess
    {
        Picture GetById(int id);
    }
}
