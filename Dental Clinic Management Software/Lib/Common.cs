﻿using Kimtoo.BindingProvider;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dental_Clinic_Management_Software.Lib
{
    public static class Common
    {
        public static Image ToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
            ms.Position = 0; // this is important
            return Image.FromStream(ms, true);
        }
        public static byte[] ToBytes(this Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

    }
}
