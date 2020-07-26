using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace LetterBuilderCore.Models
{
    public class PictureResizer
    {
        /// <summary>
        /// Данный метод принимает изображение и сжимает его до размера, 
        /// указанного в Constants.ImageMaxSize
        /// </summary>
        /// <param name="picture">
        /// Объект класса Picture, содержащий бинарные данные изображения. 
        /// Полученное сжатое изображение будет записано в поле BinaryData 
        /// данного объекта в формате JPG
        /// </param>
        public void ResizePicture(Picture picture)
        {
            Bitmap initialPicture;
            using (var memoryStream = new MemoryStream(picture.BinaryData))
            {
                initialPicture = new Bitmap(memoryStream);
            }
            while (picture.BinaryData.Length > Constants.ImageMaxSize)
            {
                Bitmap resizedPicture = new Bitmap(initialPicture,
                        Convert.ToInt32(initialPicture.Width * Constants.ScaleFactor),
                        Convert.ToInt32(initialPicture.Height * Constants.ScaleFactor));
                picture.BinaryData = GetPictureBinaryData(resizedPicture);
                initialPicture = resizedPicture;
            }
        }

        /// <summary>
        /// Данная функция переводит изображение в JPG формат с уровнем сжатия, указанным 
        /// в Constants.JPGCompressionLevel и возварщает его в виде массива байтов
        /// </summary>
        /// <param name="bitmap">Объект типа Bitmap. Входное изображение</param>
        /// <returns></returns>
        public byte[] GetPictureBinaryData(Bitmap bitmap)
        {
            byte[] binaryData;
            using (var memoryStream = new MemoryStream())
            {
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders()
                    .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(encoder, Constants.JPGCompressionLevel);

                bitmap.Save(memoryStream, jpgCodec, encoderParameters);
                binaryData = memoryStream.ToArray();
            }
            return binaryData;
        }
    }
}
