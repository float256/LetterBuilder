using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LetterBuilderCore.Models
{
    public interface IPictureResizer
    {
        void ResizePicture(Picture picture);
        byte[] GetPictureBinaryData(Bitmap bitmap);
    }
}
