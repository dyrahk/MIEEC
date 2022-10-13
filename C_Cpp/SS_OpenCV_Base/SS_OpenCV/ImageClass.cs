using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SS_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        public static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3)
                {// image in RGB
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //PIXEL PROCESSING
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            dataPtr[0] = (byte)(255 - blue);
                            dataPtr[1] = (byte)(255 - green);
                            dataPtr[2] = (byte)(255 - red);

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }
                        //at the end of the line advance the pointer by the alignment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Convert to gray
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)Math.Round(((int)blue + green + red) / 3.0);

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int blue, green, red;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            blue = (int)Math.Round(contrast * dataPtr[0] + bright);
                            green = (int)Math.Round(contrast * dataPtr[1] + bright);
                            red = (int)Math.Round(contrast * dataPtr[2] + bright);


                            if (blue > 255) blue = 255;
                            else
                                if (blue < 0) blue = 0;
                            if (green > 255) green = 255;
                            else
                                if (green < 0) green = 0;
                            if (red > 255) red = 255;
                            else
                                if (red < 0) red = 0;

                            dataPtr[0] = (byte)blue;
                            dataPtr[1] = (byte)green;
                            dataPtr[2] = (byte)red;

                            dataPtr += nChan;
                        }
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)

                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {   // store in the image
                            dataPtr[0] = dataPtr[2];
                            dataPtr[1] = dataPtr[2];
                            dataPtr[2] = dataPtr[2];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void ConvertToGreen(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // store in the image
                            dataPtr[0] = dataPtr[1];
                            dataPtr[1] = dataPtr[1];
                            dataPtr[2] = dataPtr[1];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void ConvertToBlue(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // store in the image
                            dataPtr[0] = dataPtr[0];
                            dataPtr[1] = dataPtr[0];
                            dataPtr[2] = dataPtr[0];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dx, int dy)
        {
            unsafe
            {
                int xo, yo, xd, yd, aux_o, aux_d;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels;

                for (yd = 0; yd < height; yd++)
                {
                    yo = yd - dy;
                    for (xd = 0; xd < width; xd++)
                    {
                        xo = xd - dx;
                        aux_d = yd * widthStep + xd * nChan;
                        if (xo <= 0 || yo <= 0 || xo >= width || yo >= height)
                        {
                            (dataPtrDes + aux_d)[0] = 0;
                            (dataPtrDes + aux_d)[1] = 0;
                            (dataPtrDes + aux_d)[2] = 0;
                        }
                        else
                        {
                            aux_o = yo * widthStep + xo * nChan;
                            (dataPtrDes + aux_d)[0] = (dataPtrOrig + aux_o)[0];
                            (dataPtrDes + aux_d)[1] = (dataPtrOrig + aux_o)[1];
                            (dataPtrDes + aux_d)[2] = (dataPtrOrig + aux_o)[2];
                        }
                    }
                }
            }
        }

        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                int xo, yo, xd, yd, aux_o, aux_d;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3

                for (yd = 0; yd < height; yd++)
                {
                    for (xd = 0; xd < width; xd++)
                    {
                        xo = (int)Math.Round((xd - (width / 2.0)) * Math.Cos(angle) - ((height / 2.0) - yd) * Math.Sin(angle) + width / 2.0);
                        yo = (int)Math.Round(height / 2.0 - (xd - (width / 2.0)) * Math.Sin(angle) - ((height / 2.0) - yd) * Math.Cos(angle));
                        aux_d = yd * widthStep + xd * nChan;

                        if (xo < 0 || yo < 0 || xo >= width || yo >= height)
                        {
                            (dataPtrDes + aux_d)[0] = 0;
                            (dataPtrDes + aux_d)[1] = 0;
                            (dataPtrDes + aux_d)[2] = 0;
                        }
                        else
                        {
                            aux_o = yo * widthStep + xo * nChan;
                            (dataPtrDes + aux_d)[0] = (dataPtrOrig + aux_o)[0];
                            (dataPtrDes + aux_d)[1] = (dataPtrOrig + aux_o)[1];
                            (dataPtrDes + aux_d)[2] = (dataPtrOrig + aux_o)[2];
                        }
                    }
                }
            }
        }

        public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                int origX, origY, destX, destY;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3

                for (destY = 0; destY < height; destY++)
                {
                    for (destX = 0; destX < width; destX++)
                    {
                        origX = (int)(destX / scaleFactor);
                        origY = (int)(destY / scaleFactor);
                        if (origX < 0 || origY < 0 || origX > width || origY > height)
                        {
                            (dataPtrDes + destY * widthStep + destX * nChan)[0] = 0;
                            (dataPtrDes + destY * widthStep + destX * nChan)[1] = 0;
                            (dataPtrDes + destY * widthStep + destX * nChan)[2] = 0;
                        }
                        else
                        {
                            (dataPtrDes + destY * widthStep + destX * nChan)[0] = (dataPtrOrig + origY * widthStep + origX * nChan)[0];
                            (dataPtrDes + destY * widthStep + destX * nChan)[1] = (dataPtrOrig + origY * widthStep + origX * nChan)[1];
                            (dataPtrDes + destY * widthStep + destX * nChan)[2] = (dataPtrOrig + origY * widthStep + origX * nChan)[2];
                        }

                    }
                }
            }


        }

        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        {
            unsafe
            {
                int origX, origY, destX, destY;
                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3

                for (destY = 0; destY < height; destY++)
                {
                    for (destX = 0; destX < width; destX++)
                    {
                        origX = (int)Math.Round((destX - width / 2) / scaleFactor + centerX);
                        origY = (int)Math.Round(-(height / 2 - destY) / scaleFactor + centerY);
                        if (origX < 0 || origY < 0 || origX >= width || origY >= height)
                        {
                            (dataPtrDes + destY * widthStep + destX * nChan)[0] = 0;
                            (dataPtrDes + destY * widthStep + destX * nChan)[1] = 0;
                            (dataPtrDes + destY * widthStep + destX * nChan)[2] = 0;
                        }
                        else
                        {
                            (dataPtrDes + destY * widthStep + destX * nChan)[0] = (dataPtrOrig + origY * widthStep + origX * nChan)[0];
                            (dataPtrDes + destY * widthStep + destX * nChan)[1] = (dataPtrOrig + origY * widthStep + origX * nChan)[1];
                            (dataPtrDes + destY * widthStep + destX * nChan)[2] = (dataPtrOrig + origY * widthStep + origX * nChan)[2];
                        }

                    }
                }
            }
        }

        public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                int x, y, sum = 0;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3
                int padding = orig.widthStep - orig.nChannels * orig.width;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            if (x == 0)
                            {
                                if (y == 0)
                                {
                                    dataPtrDes[0] = (byte)Math.Round((4 * dataPtrOrig[0] + 2 * (dataPtrOrig + widthStep)[0] + 2 * (dataPtrOrig + nChan)[0] + (dataPtrOrig + widthStep + nChan)[0]) / 9.0);
                                    dataPtrDes[1] = (byte)Math.Round((4 * dataPtrOrig[1] + 2 * (dataPtrOrig + widthStep)[1] + 2 * (dataPtrOrig + nChan)[1] + (dataPtrOrig + widthStep + nChan)[1]) / 9.0);
                                    dataPtrDes[2] = (byte)Math.Round((4 * dataPtrOrig[2] + 2 * (dataPtrOrig + widthStep)[2] + 2 * (dataPtrOrig + nChan)[2] + (dataPtrOrig + widthStep + nChan)[2]) / 9.0);
                                }
                                else
                                {
                                    if (y == height - 1)
                                    {
                                        dataPtrDes[0] = (byte)Math.Round((4 * dataPtrOrig[0] + 2 * (dataPtrOrig - widthStep)[0] + 2 * (dataPtrOrig + nChan)[0] + (dataPtrOrig - widthStep + nChan)[0]) / 9.0);
                                        dataPtrDes[1] = (byte)Math.Round((4 * dataPtrOrig[1] + 2 * (dataPtrOrig - widthStep)[1] + 2 * (dataPtrOrig + nChan)[1] + (dataPtrOrig - widthStep + nChan)[1]) / 9.0);
                                        dataPtrDes[2] = (byte)Math.Round((4 * dataPtrOrig[2] + 2 * (dataPtrOrig - widthStep)[2] + 2 * (dataPtrOrig + nChan)[2] + (dataPtrOrig - widthStep + nChan)[2]) / 9.0);
                                    }
                                    else
                                    {

                                        dataPtrDes[0] = (byte)Math.Round((2 * dataPtrOrig[0] + 2 * (dataPtrOrig - widthStep)[0] + 2 * (dataPtrOrig + widthStep)[0] + (dataPtrOrig - widthStep + nChan)[0] + (dataPtrOrig + nChan)[0] + (dataPtrOrig + widthStep + nChan)[0]) / 9.0);
                                        dataPtrDes[1] = (byte)Math.Round((2 * dataPtrOrig[1] + 2 * (dataPtrOrig - widthStep)[1] + 2 * (dataPtrOrig + widthStep)[1] + (dataPtrOrig - widthStep + nChan)[1] + (dataPtrOrig + nChan)[1] + (dataPtrOrig + widthStep + nChan)[1]) / 9.0);
                                        dataPtrDes[2] = (byte)Math.Round((2 * dataPtrOrig[2] + 2 * (dataPtrOrig - widthStep)[2] + 2 * (dataPtrOrig + widthStep)[2] + (dataPtrOrig - widthStep + nChan)[2] + (dataPtrOrig + nChan)[2] + (dataPtrOrig + widthStep + nChan)[2]) / 9.0);


                                    }
                                }
                            }
                            else
                            {
                                if (x == width - 1)
                                {
                                    if (y == 0)
                                    {
                                        dataPtrDes[0] = (byte)Math.Round((4 * dataPtrOrig[0] + 2 * (dataPtrOrig + widthStep)[0] + 2 * (dataPtrOrig - nChan)[0] + (dataPtrOrig + widthStep - nChan)[0]) / 9.0);
                                        dataPtrDes[1] = (byte)Math.Round((4 * dataPtrOrig[1] + 2 * (dataPtrOrig + widthStep)[1] + 2 * (dataPtrOrig - nChan)[1] + (dataPtrOrig + widthStep - nChan)[1]) / 9.0);
                                        dataPtrDes[2] = (byte)Math.Round((4 * dataPtrOrig[2] + 2 * (dataPtrOrig + widthStep)[2] + 2 * (dataPtrOrig - nChan)[2] + (dataPtrOrig + widthStep - nChan)[2]) / 9.0);
                                    }
                                    else
                                    {
                                        if (y == height - 1)
                                        {
                                            dataPtrDes[0] = (byte)Math.Round((4 * dataPtrOrig[0] + 2 * (dataPtrOrig - widthStep)[0] + 2 * (dataPtrOrig - nChan)[0] + (dataPtrOrig - widthStep - nChan)[0]) / 9.0);
                                            dataPtrDes[1] = (byte)Math.Round((4 * dataPtrOrig[1] + 2 * (dataPtrOrig - widthStep)[1] + 2 * (dataPtrOrig - nChan)[1] + (dataPtrOrig - widthStep - nChan)[1]) / 9.0);
                                            dataPtrDes[2] = (byte)Math.Round((4 * dataPtrOrig[2] + 2 * (dataPtrOrig - widthStep)[2] + 2 * (dataPtrOrig - nChan)[2] + (dataPtrOrig - widthStep - nChan)[2]) / 9.0);
                                        }
                                        else
                                        {
                                            dataPtrDes[0] = (byte)Math.Round((2 * dataPtrOrig[0] + 2 * (dataPtrOrig - widthStep)[0] + 2 * (dataPtrOrig + widthStep)[0] + (dataPtrOrig - widthStep - nChan)[0] + (dataPtrOrig - nChan)[0] + (dataPtrOrig + widthStep - nChan)[0]) / 9.0);
                                            dataPtrDes[1] = (byte)Math.Round((2 * dataPtrOrig[1] + 2 * (dataPtrOrig - widthStep)[1] + 2 * (dataPtrOrig + widthStep)[1] + (dataPtrOrig - widthStep - nChan)[1] + (dataPtrOrig - nChan)[1] + (dataPtrOrig + widthStep - nChan)[1]) / 9.0);
                                            dataPtrDes[2] = (byte)Math.Round((2 * dataPtrOrig[2] + 2 * (dataPtrOrig - widthStep)[2] + 2 * (dataPtrOrig + widthStep)[2] + (dataPtrOrig - widthStep - nChan)[2] + (dataPtrOrig - nChan)[2] + (dataPtrOrig + widthStep - nChan)[2]) / 9.0);
                                        }
                                    }
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        dataPtrDes[0] = (byte)Math.Round((2 * dataPtrOrig[0] + 2 * (dataPtrOrig - nChan)[0] + 2 * (dataPtrOrig + nChan)[0] + (dataPtrOrig + widthStep - nChan)[0] + (dataPtrOrig + widthStep)[0] + (dataPtrOrig + widthStep + nChan)[0]) / 9.0);
                                        dataPtrDes[1] = (byte)Math.Round((2 * dataPtrOrig[1] + 2 * (dataPtrOrig - nChan)[1] + 2 * (dataPtrOrig + nChan)[1] + (dataPtrOrig + widthStep - nChan)[1] + (dataPtrOrig + widthStep)[1] + (dataPtrOrig + widthStep + nChan)[1]) / 9.0);
                                        dataPtrDes[2] = (byte)Math.Round((2 * dataPtrOrig[2] + 2 * (dataPtrOrig - nChan)[2] + 2 * (dataPtrOrig + nChan)[2] + (dataPtrOrig + widthStep - nChan)[2] + (dataPtrOrig + widthStep)[2] + (dataPtrOrig + widthStep + nChan)[2]) / 9.0);
                                    }
                                    else
                                    {
                                        dataPtrDes[0] = (byte)Math.Round((2 * dataPtrOrig[0] + 2 * (dataPtrOrig - nChan)[0] + 2 * (dataPtrOrig + nChan)[0] + (dataPtrOrig - widthStep - nChan)[0] + (dataPtrOrig - widthStep)[0] + (dataPtrOrig - widthStep + nChan)[0]) / 9.0);
                                        dataPtrDes[1] = (byte)Math.Round((2 * dataPtrOrig[1] + 2 * (dataPtrOrig - nChan)[1] + 2 * (dataPtrOrig + nChan)[1] + (dataPtrOrig - widthStep - nChan)[1] + (dataPtrOrig - widthStep)[1] + (dataPtrOrig - widthStep + nChan)[1]) / 9.0);
                                        dataPtrDes[2] = (byte)Math.Round((2 * dataPtrOrig[2] + 2 * (dataPtrOrig - nChan)[2] + 2 * (dataPtrOrig + nChan)[2] + (dataPtrOrig - widthStep - nChan)[2] + (dataPtrOrig - widthStep)[2] + (dataPtrOrig - widthStep + nChan)[2]) / 9.0);

                                    }
                                }
                            }
                        }
                        else
                        {

                            sum = (dataPtrOrig - 1 * widthStep - 1 * nChan)[0];
                            sum += (dataPtrOrig + 0 * widthStep - 1 * nChan)[0];
                            sum += (dataPtrOrig + 1 * widthStep - 1 * nChan)[0];
                            sum += (dataPtrOrig - 1 * widthStep + 0 * nChan)[0];
                            sum += (dataPtrOrig + 0 * widthStep + 0 * nChan)[0];
                            sum += (dataPtrOrig + 1 * widthStep + 0 * nChan)[0];
                            sum += (dataPtrOrig - 1 * widthStep + 1 * nChan)[0];
                            sum += (dataPtrOrig + 0 * widthStep + 1 * nChan)[0];
                            sum += (dataPtrOrig + 1 * widthStep + 1 * nChan)[0];

                            dataPtrDes[0] = (byte)Math.Round(sum / 9.0);

                            sum = (dataPtrOrig - 1 * widthStep - 1 * nChan)[1];
                            sum += (dataPtrOrig + 0 * widthStep - 1 * nChan)[1];
                            sum += (dataPtrOrig + 1 * widthStep - 1 * nChan)[1];
                            sum += (dataPtrOrig - 1 * widthStep + 0 * nChan)[1];
                            sum += (dataPtrOrig + 0 * widthStep + 0 * nChan)[1];
                            sum += (dataPtrOrig + 1 * widthStep + 0 * nChan)[1];
                            sum += (dataPtrOrig - 1 * widthStep + 1 * nChan)[1];
                            sum += (dataPtrOrig + 0 * widthStep + 1 * nChan)[1];
                            sum += (dataPtrOrig + 1 * widthStep + 1 * nChan)[1];

                            dataPtrDes[1] = (byte)Math.Round(sum / 9.0);

                            sum = (dataPtrOrig - 1 * widthStep - 1 * nChan)[2];
                            sum += (dataPtrOrig + 0 * widthStep - 1 * nChan)[2];
                            sum += (dataPtrOrig + 1 * widthStep - 1 * nChan)[2];
                            sum += (dataPtrOrig - 1 * widthStep + 0 * nChan)[2];
                            sum += (dataPtrOrig + 0 * widthStep + 0 * nChan)[2];
                            sum += (dataPtrOrig + 1 * widthStep + 0 * nChan)[2];
                            sum += (dataPtrOrig - 1 * widthStep + 1 * nChan)[2];
                            sum += (dataPtrOrig + 0 * widthStep + 1 * nChan)[2];
                            sum += (dataPtrOrig + 1 * widthStep + 1 * nChan)[2];

                            dataPtrDes[2] = (byte)Math.Round(sum / 9.0);

                        }
                        dataPtrDes += nChan;
                        dataPtrOrig += nChan;
                    }
                    dataPtrDes += padding;
                    dataPtrOrig += padding;
                }
            }
        }

        //Recebe a imagem a alterar, uma cópia da imagem, uma matrix 3x3 contendo os coeficientes e o peso
        public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] matrix, float matrixWeight)
        {
            unsafe
            {
                int x, y;
                float sum;
                double finalSum;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3
                int padding = orig.widthStep - orig.nChannels * orig.width;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            if (x == 0)
                            {
                                if (y == 0) //canto superior esquerdo
                                {
                                    sum = (matrix[0, 0] + matrix[0, 1] + matrix[1, 0] + matrix[1, 1]) * dataPtrOrig[0] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + widthStep)[0] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + nChan)[0] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[0];
                                    finalSum = Math.Round(sum / matrixWeight);
                                    if (finalSum > 255) finalSum = 255;
                                    else if (finalSum < 0) finalSum = 0;
                                    dataPtrDes[0] = (byte)finalSum;


                                    sum = (matrix[0, 0] + matrix[0, 1] + matrix[1, 0] + matrix[1, 1]) * dataPtrOrig[1] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + widthStep)[1] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + nChan)[1] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[1];
                                    finalSum = Math.Round(sum / matrixWeight);
                                    if (finalSum > 255) finalSum = 255;
                                    else if (finalSum < 0) finalSum = 0;
                                    dataPtrDes[1] = (byte)finalSum;


                                    sum = (matrix[0, 0] + matrix[0, 1] + matrix[1, 0] + matrix[1, 1]) * dataPtrOrig[2] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + widthStep)[2] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + nChan)[2] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[2];
                                    finalSum = Math.Round(sum / matrixWeight);
                                    if (finalSum > 255) finalSum = 255;
                                    else if (finalSum < 0) finalSum = 0;
                                    dataPtrDes[2] = (byte)finalSum;

                                }
                                else
                                {
                                    if (y == height - 1) //canto inferior esquerdo
                                    {

                                        sum = (matrix[0, 1] + matrix[1, 1] + matrix[0, 2] + matrix[1, 2]) * dataPtrOrig[0] + (matrix[0, 0] + matrix[1, 0]) * (dataPtrOrig - widthStep)[0] + (matrix[2, 1] + matrix[2, 2]) * (dataPtrOrig + nChan)[0] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[0];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[0] = (byte)finalSum;

                                        sum = (matrix[0, 1] + matrix[1, 1] + matrix[0, 2] + matrix[1, 2]) * dataPtrOrig[1] + (matrix[0, 0] + matrix[1, 0]) * (dataPtrOrig - widthStep)[1] + (matrix[2, 1] + matrix[2, 2]) * (dataPtrOrig + nChan)[1] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[1];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[1] = (byte)finalSum;

                                        sum = (matrix[0, 1] + matrix[1, 1] + matrix[0, 2] + matrix[1, 2]) * dataPtrOrig[2] + (matrix[0, 0] + matrix[1, 0]) * (dataPtrOrig - widthStep)[2] + (matrix[2, 1] + matrix[2, 2]) * (dataPtrOrig + nChan)[2] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[2];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[2] = (byte)finalSum;

                                    }
                                    else //coluna direita
                                    {

                                        sum = (matrix[0, 1] + matrix[1, 1]) * dataPtrOrig[0] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - widthStep)[0] + (matrix[0, 2] + matrix[1, 2]) * (dataPtrOrig + widthStep)[0] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[0] + matrix[2, 1] * (dataPtrOrig + nChan)[0] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[0];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[0] = (byte)finalSum;

                                        sum = (matrix[0, 1] + matrix[1, 1]) * dataPtrOrig[1] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - widthStep)[1] + (matrix[0, 2] + matrix[1, 2]) * (dataPtrOrig + widthStep)[1] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[1] + matrix[2, 1] * (dataPtrOrig + nChan)[1] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[1];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[1] = (byte)finalSum;

                                        sum = (matrix[0, 1] + matrix[1, 1]) * dataPtrOrig[2] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - widthStep)[2] + (matrix[0, 2] + matrix[1, 2]) * (dataPtrOrig + widthStep)[2] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[2] + matrix[2, 1] * (dataPtrOrig + nChan)[2] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[2];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[2] = (byte)finalSum;

                                    }
                                }
                            }
                            else
                            {
                                if (x == width - 1)
                                {
                                    if (y == 0) //canto superior direito
                                    {

                                        sum = (matrix[1, 0] + matrix[2, 0] + matrix[1, 1] + matrix[2, 1]) * dataPtrOrig[0] + (matrix[1, 2] + matrix[2, 2]) * (dataPtrOrig + widthStep)[0] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - nChan)[0] + matrix[0, 2] * (dataPtrOrig + widthStep - nChan)[0];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[0] = (byte)finalSum;

                                        sum = (matrix[1, 0] + matrix[2, 0] + matrix[1, 1] + matrix[2, 1]) * dataPtrOrig[1] + (matrix[1, 2] + matrix[2, 2]) * (dataPtrOrig + widthStep)[1] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - nChan)[1] + matrix[0, 2] * (dataPtrOrig + widthStep - nChan)[1];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[1] = (byte)finalSum;

                                        sum = (matrix[1, 0] + matrix[2, 0] + matrix[1, 1] + matrix[2, 1]) * dataPtrOrig[2] + (matrix[1, 2] + matrix[2, 2]) * (dataPtrOrig + widthStep)[2] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - nChan)[2] + matrix[0, 2] * (dataPtrOrig + widthStep - nChan)[2];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[2] = (byte)finalSum;

                                    }
                                    else
                                    {
                                        if (y == height - 1) //canto inferior direito
                                        {
                                            sum = (matrix[1, 1] + matrix[2, 1] + matrix[1, 2] + matrix[2, 2]) * dataPtrOrig[0] + (matrix[1, 0] + matrix[2, 0]) * (dataPtrOrig - widthStep)[0] + (matrix[0, 1] + matrix[0, 2]) * (dataPtrOrig - nChan)[0] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[0];
                                            finalSum = Math.Round(sum / matrixWeight);
                                            if (finalSum > 255) finalSum = 255;
                                            else if (finalSum < 0) finalSum = 0;
                                            dataPtrDes[0] = (byte)finalSum;

                                            sum = (matrix[1, 1] + matrix[2, 1] + matrix[1, 2] + matrix[2, 2]) * dataPtrOrig[1] + (matrix[1, 0] + matrix[2, 0]) * (dataPtrOrig - widthStep)[1] + (matrix[0, 1] + matrix[0, 2]) * (dataPtrOrig - nChan)[1] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[1];
                                            finalSum = Math.Round(sum / matrixWeight);
                                            if (finalSum > 255) finalSum = 255;
                                            else if (finalSum < 0) finalSum = 0;
                                            dataPtrDes[1] = (byte)finalSum;

                                            sum = (matrix[1, 1] + matrix[2, 1] + matrix[1, 2] + matrix[2, 2]) * dataPtrOrig[2] + (matrix[1, 0] + matrix[2, 0]) * (dataPtrOrig - widthStep)[2] + (matrix[0, 1] + matrix[0, 2]) * (dataPtrOrig - nChan)[2] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[2];
                                            finalSum = Math.Round(sum / matrixWeight);
                                            if (finalSum > 255) finalSum = 255;
                                            else if (finalSum < 0) finalSum = 0;
                                            dataPtrDes[2] = (byte)finalSum;
                                        }
                                        else //coluna direita
                                        {
                                            sum = (matrix[1, 1] + matrix[2, 1]) * dataPtrOrig[0] + (matrix[1, 0] + matrix[2, 0]) * (dataPtrOrig - widthStep)[0] + (matrix[1, 2] + matrix[2, 2]) * (dataPtrOrig + widthStep)[0] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[0] + matrix[0, 1] * (dataPtrOrig - nChan)[0] + matrix[0, 2] * (dataPtrOrig + widthStep - nChan)[0];
                                            finalSum = Math.Round(sum / matrixWeight);
                                            if (finalSum > 255) finalSum = 255;
                                            else if (finalSum < 0) finalSum = 0;
                                            dataPtrDes[0] = (byte)finalSum;

                                            sum = (matrix[1, 1] + matrix[2, 1]) * dataPtrOrig[1] + (matrix[1, 0] + matrix[2, 0]) * (dataPtrOrig - widthStep)[1] + (matrix[1, 2] + matrix[2, 2]) * (dataPtrOrig + widthStep)[1] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[1] + matrix[0, 1] * (dataPtrOrig - nChan)[1] + matrix[0, 2] * (dataPtrOrig + widthStep - nChan)[1];
                                            finalSum = Math.Round(sum / matrixWeight);
                                            if (finalSum > 255) finalSum = 255;
                                            else if (finalSum < 0) finalSum = 0;
                                            dataPtrDes[1] = (byte)finalSum;

                                            sum = (matrix[1, 1] + matrix[2, 1]) * dataPtrOrig[2] + (matrix[1, 0] + matrix[2, 0]) * (dataPtrOrig - widthStep)[2] + (matrix[1, 2] + matrix[2, 2]) * (dataPtrOrig + widthStep)[2] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[2] + matrix[0, 1] * (dataPtrOrig - nChan)[2] + matrix[0, 2] * (dataPtrOrig + widthStep - nChan)[2];
                                            finalSum = Math.Round(sum / matrixWeight);
                                            if (finalSum > 255) finalSum = 255;
                                            else if (finalSum < 0) finalSum = 0;
                                            dataPtrDes[2] = (byte)finalSum;
                                        }
                                    }
                                }
                                else
                                {
                                    if (y == 0) //linha superior
                                    {
                                        sum = (matrix[1, 0] + matrix[1, 1]) * dataPtrOrig[0] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - nChan)[0] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + nChan)[0] + matrix[2, 0] * (dataPtrOrig + widthStep - nChan)[0] + matrix[2, 1] * (dataPtrOrig + widthStep)[0] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[0];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[0] = (byte)finalSum;

                                        sum = (matrix[1, 0] + matrix[1, 1]) * dataPtrOrig[1] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - nChan)[1] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + nChan)[1] + matrix[2, 0] * (dataPtrOrig + widthStep - nChan)[1] + matrix[2, 1] * (dataPtrOrig + widthStep)[1] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[1];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[1] = (byte)finalSum;

                                        sum = (matrix[1, 0] + matrix[1, 1]) * dataPtrOrig[2] + (matrix[0, 0] + matrix[0, 1]) * (dataPtrOrig - nChan)[2] + (matrix[2, 0] + matrix[2, 1]) * (dataPtrOrig + nChan)[2] + matrix[2, 0] * (dataPtrOrig + widthStep - nChan)[2] + matrix[2, 1] * (dataPtrOrig + widthStep)[2] + matrix[2, 2] * (dataPtrOrig + widthStep + nChan)[2];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[2] = (byte)finalSum;
                                    }
                                    else //linha inferior
                                    {
                                        sum = (matrix[1, 1] + matrix[1, 2]) * dataPtrOrig[0] + (matrix[0, 1] + matrix[0, 2]) * (dataPtrOrig - nChan)[0] + (matrix[2, 1] + matrix[2, 2]) * (dataPtrOrig + nChan)[0] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[0] + matrix[1, 0] * (dataPtrOrig - widthStep)[0] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[0];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[0] = (byte)finalSum;

                                        sum = (matrix[1, 1] + matrix[1, 2]) * dataPtrOrig[1] + (matrix[0, 1] + matrix[0, 2]) * (dataPtrOrig - nChan)[1] + (matrix[2, 1] + matrix[2, 2]) * (dataPtrOrig + nChan)[1] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[1] + matrix[1, 0] * (dataPtrOrig - widthStep)[1] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[1];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[1] = (byte)finalSum;

                                        sum = (matrix[1, 1] + matrix[1, 2]) * dataPtrOrig[2] + (matrix[0, 1] + matrix[0, 2]) * (dataPtrOrig - nChan)[2] + (matrix[2, 1] + matrix[2, 2]) * (dataPtrOrig + nChan)[2] + matrix[0, 0] * (dataPtrOrig - widthStep - nChan)[2] + matrix[1, 0] * (dataPtrOrig - widthStep)[2] + matrix[2, 0] * (dataPtrOrig - widthStep + nChan)[2];
                                        finalSum = Math.Round(sum / matrixWeight);
                                        if (finalSum > 255) finalSum = 255;
                                        else if (finalSum < 0) finalSum = 0;
                                        dataPtrDes[2] = (byte)finalSum;
                                    }
                                }
                            }
                        }
                        else
                        {
                            sum = matrix[-1 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep - 1 * nChan)[0];
                            sum += matrix[0 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep + 0 * nChan)[0];
                            sum += matrix[1 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep + 1 * nChan)[0];
                            sum += matrix[-1 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep - 1 * nChan)[0];
                            sum += matrix[0 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep + 0 * nChan)[0];
                            sum += matrix[1 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep + 1 * nChan)[0];
                            sum += matrix[-1 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep - 1 * nChan)[0];
                            sum += matrix[0 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep + 0 * nChan)[0];
                            sum += matrix[1 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep + 1 * nChan)[0];

                            finalSum = Math.Round(sum / matrixWeight);
                            if (finalSum > 255) finalSum = 255;
                            else if (finalSum < 0) finalSum = 0;
                            dataPtrDes[0] = (byte)finalSum;

                            sum = matrix[-1 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep - 1 * nChan)[1];
                            sum += matrix[0 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep + 0 * nChan)[1];
                            sum += matrix[1 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep + 1 * nChan)[1];
                            sum += matrix[-1 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep - 1 * nChan)[1];
                            sum += matrix[0 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep + 0 * nChan)[1];
                            sum += matrix[1 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep + 1 * nChan)[1];
                            sum += matrix[-1 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep - 1 * nChan)[1];
                            sum += matrix[0 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep + 0 * nChan)[1];
                            sum += matrix[1 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep + 1 * nChan)[1];

                            finalSum = Math.Round(sum / matrixWeight);
                            if (finalSum > 255) finalSum = 255;
                            else if (finalSum < 0) finalSum = 0;
                            dataPtrDes[1] = (byte)finalSum;

                            sum = matrix[-1 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep - 1 * nChan)[2];
                            sum += matrix[0 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep + 0 * nChan)[2];
                            sum += matrix[1 + 1, -1 + 1] * (dataPtrOrig - 1 * widthStep + 1 * nChan)[2];
                            sum += matrix[-1 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep - 1 * nChan)[2];
                            sum += matrix[0 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep + 0 * nChan)[2];
                            sum += matrix[1 + 1, 0 + 1] * (dataPtrOrig + 0 * widthStep + 1 * nChan)[2];
                            sum += matrix[-1 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep - 1 * nChan)[2];
                            sum += matrix[0 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep + 0 * nChan)[2];
                            sum += matrix[1 + 1, 1 + 1] * (dataPtrOrig + 1 * widthStep + 1 * nChan)[2];

                            finalSum = Math.Round(sum / matrixWeight);
                            if (finalSum > 255) finalSum = 255;
                            else if (finalSum < 0) finalSum = 0;
                            dataPtrDes[2] = (byte)finalSum;

                        }
                        dataPtrDes += nChan;
                        dataPtrOrig += nChan;
                    }
                    dataPtrDes += padding;
                    dataPtrOrig += padding;
                }
            }
        }

        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                // obter apontador do inicio da imagem de origem
                MIplImage m = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();

                // obter apontador do inicio da imagem de destino
                MIplImage md = img.MIplImage;
                byte* dataPtrd = (byte*)md.imageData.ToPointer();

                int w = img.Width; //igual para ambas
                int h = img.Height;//igual para ambas
                int widthstep = md.widthStep;//igual para ambas
                int nC = md.nChannels; // number of channels - 3. igual

                int blueSum, greenSum, redSum;
                int x, y;
                double[] mult = { 1, 2, 1 };
                //processamento do centro da imagem

                for (y = 0; y < h; y++)
                {
                    for (x = 0; x < w; x++)
                    {

                        if (y == 0)
                        {

                            if (x == 0) //processamento canto superior esquerdo
                            {

                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2]))
                                            + Math.Abs(((dataPtr + y * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2])));


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[2])
                                                                 - ((dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2]))
                                                        + Math.Abs(((dataPtr + y * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[2])
                                                                 - ((dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2])));

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[2])
                                                                 - ((dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2]))
                                                        + Math.Abs(((dataPtr + y * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[2])
                                                                 - ((dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2])));
                            }
                            else if (x == w - 1) //processamento canto superior direito
                            {

                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + x * nC)[0] * mult[2])
                                                     - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[2]))
                                            + Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + y * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[2])));


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + x * nC)[1] * mult[2])
                                                     - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[2]))
                                            + Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[2])
                                                     - ((dataPtr + y * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[2])));

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + x * nC)[2] * mult[2])
                                                     - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[2]))
                                            + Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[2])
                                                     - ((dataPtr + y * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[2])));

                            }
                            else //processamento da primeira linha, excepto cantos
                            {
                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2]))
                                            + Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2])));


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[2])
                                                     - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2]))
                                            + Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2])));

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[2])
                                                     - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2]))
                                            + Math.Abs(((dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2])));

                            }
                        }
                        else if (y == h - 1)
                        {
                            if (x == 0) //processamento canto inferior esquerdo
                            {

                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + y * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + x * nC)[0] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[2])));


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[2])
                                                     - ((dataPtr + y * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + x * nC)[1] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[2])));

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[2])
                                                     - ((dataPtr + y * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + x * nC)[2] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[2])));
                            }
                            else if (x == w - 1) //processamento canto inferior direito
                            {
                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + x * nC)[0] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + x * nC)[0] * mult[2])));


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + x * nC)[1] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + x * nC)[1] * mult[2])));

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + x * nC)[2] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + x * nC)[2] * mult[2])));

                            }
                            else //processamento da última linha, excepto cantos
                            {
                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[2])));


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[2])));

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[2])
                                                     - ((dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[2]))
                                            + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[2])
                                                     - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[1] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[2])));

                            }
                        }
                        else
                        {
                            if (x == 0)
                            {
                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2]))
                                                        + Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2])));  // |7|8|9|


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2]))
                                                        + Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2])));  // |7|8|9|

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2]))
                                                        + Math.Abs(((dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2])));  // |7|8|9|

                            }
                            else if (x == w - 1)
                            {
                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[2]))
                                                        + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[0] + (dataPtr + y * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[2])));  // |7|8|9|


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[2]))
                                                        + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[0] + (dataPtr + y * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[2])));  // |7|8|9|

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[2]))
                                                        + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[0] + (dataPtr + y * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[2])));  // |7|8|9|

                            }
                            else
                            {
                                blueSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2]))
                                                        + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[0] * mult[2])    // |1|2|3|
                                                                   - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[0] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[0] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[0] * mult[2])));  // |7|8|9|


                                greenSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[2])    // |1|2|3|
                                                                    - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2]))
                                                    + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[1] * mult[2])    // |1|2|3|
                                                               - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[1] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[1] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[1] * mult[2])));  // |7|8|9|

                                redSum = (int)Math.Round(Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y - 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[2])    // |1|2|3|
                                                             - ((dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + (y + 1) * widthstep + x * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2]))
                                                  + Math.Abs(((dataPtr + (y - 1) * widthstep + (x - 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x - 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x - 1) * nC)[2] * mult[2])    // |1|2|3|
                                                             - ((dataPtr + (y - 1) * widthstep + (x + 1) * nC)[2] * mult[0] + (dataPtr + y * widthstep + (x + 1) * nC)[2] * mult[1] + (dataPtr + (y + 1) * widthstep + (x + 1) * nC)[2] * mult[2])));  // |7|8|9|
                            }
                        }


                        if (blueSum > 255) blueSum = 255;
                        if (greenSum > 255) greenSum = 255;
                        if (redSum > 255) redSum = 255;
                        (dataPtrd + y * widthstep + x * nC)[0] = (byte)blueSum;
                        (dataPtrd + y * widthstep + x * nC)[1] = (byte)greenSum;
                        (dataPtrd + y * widthstep + x * nC)[2] = (byte)redSum;

                    }
                }

            }
        }

        public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                int x, y, aux, sum;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3
                int padding = orig.widthStep - orig.nChannels * orig.width;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (x == width - 1)
                        {
                            if (y == height - 1)
                            {
                                (dataPtrDes + y * widthStep + x * nChan)[0] = 0;
                                (dataPtrDes + y * widthStep + x * nChan)[1] = 0;
                                (dataPtrDes + y * widthStep + x * nChan)[2] = 0;
                            }
                            else
                            {
                                for (aux = 0; aux < 3; aux++)
                                {
                                    sum = (int)Math.Round((double)(Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[aux] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[aux])));
                                    if (sum > 255) sum = 255;
                                    (dataPtrDes + y * widthStep + x * nChan)[aux] = (byte)sum;
                                }
                            }
                        }
                        else
                        {
                            if (y == height - 1)
                            {
                                for (aux = 0; aux < 3; aux++)
                                {
                                    sum = (int)Math.Round((double)(Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[aux] - (dataPtrOrig + y * widthStep + (x + 1) * nChan)[aux])));
                                    if (sum > 255) sum = 255;
                                    (dataPtrDes + y * widthStep + x * nChan)[aux] = (byte)sum;
                                }
                            }
                            else
                            {
                                for (aux = 0; aux < 3; aux++)
                                {
                                    sum = (int)Math.Round((double)(Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[aux] - (dataPtrOrig + y * widthStep + (x + 1) * nChan)[aux]) + Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[aux] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[aux])));
                                    if (sum > 255) sum = 255;
                                    (dataPtrDes + y * widthStep + x * nChan)[aux] = (byte)sum;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Median(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                imgCopy.SmoothMedian(3).CopyTo(img);
            }
        }

        public static int[] Histogram_Gray(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[] histograma = new int[256];
                int x, y, gray;

                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels; // number of channels - 3

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        gray = (int)Math.Round(((dataPtr + y * widthStep + x * nChan)[1] + (dataPtr + y * widthStep + x * nChan)[0] + (dataPtr + y * widthStep + x * nChan)[2]) / 3.0);
                        histograma[gray] += 1;
                    }
                }

                return histograma;
            }
        }

        public static int[,] Histogram_RGB(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[,] histograma = new int[3, 256];
                int x, y;

                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels; // number of channels - 3

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        histograma[0, (dataPtr + y * widthStep + x * nChan)[0]] += 1;
                        histograma[1, (dataPtr + y * widthStep + x * nChan)[1]] += 1;
                        histograma[2, (dataPtr + y * widthStep + x * nChan)[2]] += 1;
                    }
                }
                return histograma;
            }
        }

        public static int[,] Histogram_All(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[,] histograma = new int[4, 256];
                int x, y, gray;

                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels; // number of channels - 3

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        gray = (int)Math.Round(((dataPtr + y * widthStep + x * nChan)[1] + (dataPtr + y * widthStep + x * nChan)[0] + (dataPtr + y * widthStep + x * nChan)[2]) / 3.0);
                        histograma[0, gray] += 1;
                        histograma[1, (dataPtr + y * widthStep + x * nChan)[0]] += 1;
                        histograma[2, (dataPtr + y * widthStep + x * nChan)[1]] += 1;
                        histograma[3, (dataPtr + y * widthStep + x * nChan)[2]] += 1;
                    }
                }
                return histograma;
            }
        }

        public static void Equalization(Image<Bgr, byte> img)
        {
            unsafe
            {
                Image<Ycc, byte> img2 = img.Convert<Ycc, byte>();

                int[] histograma = new int[256];
                int[] newH = new int[256];
                int x, y, YCbCr, acumHist = 0, acumHistmin = 0;

                MIplImage imagem = img2.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img2.Width;
                int height = img2.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels; // number of channels - 3

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        YCbCr = (dataPtr + y * widthStep + x * nChan)[0];
                        histograma[YCbCr] += 1;
                    }
                }

                /*acumHist = histograma[0];
                if (acumHistmin == 0) acumHistmin = acumHist;
                newH[0] = (int)Math.Round(((acumHist - acumHistmin) / (width * height - acumHistmin)) * 255.0);*/

                for (x = 0; x < 256; x++)
                {
                    acumHist += histograma[x];
                    if (acumHistmin == 0) acumHistmin = acumHist;
                    newH[x] = (int)Math.Round((((double)acumHist - acumHistmin) / (width * height - acumHistmin)) * 255.0);
                }

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        (dataPtr + y * widthStep + x * nChan)[0] = (byte)newH[(dataPtr + y * widthStep + x * nChan)[0]];
                    }
                }

                img.ConvertFrom<Ycc, byte>(img2);
            }
        }

        public static void ConvertToBW(Emgu.CV.Image<Bgr, byte> img, int threshold)
        {
            unsafe
            {
                int x, y, gray;

                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        gray = (int)Math.Round(((dataPtr + y * widthStep + x * nChan)[0] + (dataPtr + y * widthStep + x * nChan)[1] + (dataPtr + y * widthStep + x * nChan)[2]) / 3.0);
                        if (gray <= threshold)
                        {
                            (dataPtr + y * widthStep + x * nChan)[0] = 0;
                            (dataPtr + y * widthStep + x * nChan)[1] = 0;
                            (dataPtr + y * widthStep + x * nChan)[2] = 0;
                        }
                        else
                        {
                            (dataPtr + y * widthStep + x * nChan)[0] = 255;
                            (dataPtr + y * widthStep + x * nChan)[1] = 255;
                            (dataPtr + y * widthStep + x * nChan)[2] = 255;
                        }
                    }
                }
            }
        }

        public static int OTSU(int[] hist)
        {
            int nPix = 0; //número total de pixeis
            int threshold = 0;
            int x, y; //variáveis auxiliares (cicloes for)
            float q1, q2, media1, media2, variancia, varianciaM = 0;

            for (x = 0; x < 256; x++)
            {
                nPix += hist[x];
            }


            for (x = 1; x < 256; x++)
            {
                q1 = 0;
                for (y = 0; y <= x; y++)
                {
                    q1 += hist[y];
                }
                q2 = (nPix - q1) / nPix;
                q1 /= nPix;

                media1 = 0;
                media2 = 0;
                for (y = 0; y <= x; y++)
                {
                    media1 += y * hist[y];
                }
                media1 /= q1;
                for (y = x + 1; y < 256; y++)
                {
                    media2 += y * hist[y];
                }
                media2 /= q2;

                variancia = q1 * q2 * ((media1 - media2) * (media1 - media2));

                if (varianciaM < variancia)
                {
                    threshold = x;
                    varianciaM = variancia;
                }
            }

            return threshold;
        }

        public static void ConvertToBW_Otsu(Emgu.CV.Image<Bgr, byte> img)
        {
            int[] hist = Histogram_Gray(img);
            int threshold = OTSU(hist);
            ConvertToBW(img, threshold);
        }

        public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                int x, y, sum;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3
                int padding = orig.widthStep - orig.nChannels * orig.width;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (x == width - 1)
                        {
                            if (y == height - 1)
                            {
                                (dataPtrDes + y * widthStep + x * nChan)[0] = 0;
                                (dataPtrDes + y * widthStep + x * nChan)[1] = 0;
                                (dataPtrDes + y * widthStep + x * nChan)[2] = 0;

                            }
                            else
                            {
                                sum = (int)Math.Round((double)(2 * Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[0] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[0])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[0] = (byte)sum;

                                sum = (int)Math.Round((double)(2 * Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[1] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[1])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[1] = (byte)sum;

                                sum = (int)Math.Round((double)(2 * Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[2] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[2])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[2] = (byte)sum;

                            }
                        }
                        else
                        {
                            if (y == height - 1)
                            {

                                sum = (int)Math.Round((double)(2 * Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[0] - (dataPtrOrig + y * widthStep + (x + 1) * nChan)[0])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[0] = (byte)sum;

                                sum = (int)Math.Round((double)(2 * Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[1] - (dataPtrOrig + y * widthStep + (x + 1) * nChan)[1])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[1] = (byte)sum;

                                sum = (int)Math.Round((double)(2 * Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[2] - (dataPtrOrig + y * widthStep + (x + 1) * nChan)[2])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[2] = (byte)sum;

                            }
                            else
                            {

                                sum = (int)Math.Round((double)(Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[0] - (dataPtrOrig + (y + 1) * widthStep + (x + 1) * nChan)[0]) + Math.Abs((dataPtrOrig + y * widthStep + (x + 1) * nChan)[0] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[0])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[0] = (byte)sum;

                                sum = (int)Math.Round((double)(Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[1] - (dataPtrOrig + (y + 1) * widthStep + (x + 1) * nChan)[1]) + Math.Abs((dataPtrOrig + y * widthStep + (x + 1) * nChan)[1] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[1])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[1] = (byte)sum;

                                sum = (int)Math.Round((double)(Math.Abs((dataPtrOrig + y * widthStep + x * nChan)[2] - (dataPtrOrig + (y + 1) * widthStep + (x + 1) * nChan)[2]) + Math.Abs((dataPtrOrig + y * widthStep + (x + 1) * nChan)[2] - (dataPtrOrig + (y + 1) * widthStep + x * nChan)[2])));
                                if (sum > 255) sum = 255;
                                (dataPtrDes + y * widthStep + x * nChan)[2] = (byte)sum;

                            }
                        }
                    }
                }
            }
        }

        public static void Rotation_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                int xd, yd, aux_d;
                int intpartX, intpartY;
                double xo, yo, decpartX, decpartY;
                byte Bx, Bx1;
                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3

                for (yd = 0; yd < height; yd++)
                {
                    for (xd = 0; xd < width; xd++)
                    {
                        xo = ((xd - (width / 2.0)) * Math.Cos(angle) - ((height / 2.0) - yd) * Math.Sin(angle) + width / 2.0);
                        yo = (height / 2.0 - (xd - (width / 2.0)) * Math.Sin(angle) - ((height / 2.0) - yd) * Math.Cos(angle));
                        aux_d = yd * widthStep + xd * nChan;

                        //Fora dos limites da imagem 
                        if (xo < -1 || yo < -1 || xo >= width || yo >= height)
                        {
                            (dataPtrDes + aux_d)[0] = 0;
                            (dataPtrDes + aux_d)[1] = 0;
                            (dataPtrDes + aux_d)[2] = 0;
                        }
                        //Nas bordas
                        else if ((xo >= -1 && xo < 0) || (yo >= -1 && yo < 0) || (xo >= width - 1 && xo < width) || (yo >= height - 1 && yo < height))
                        {
                            if (xo >= -1 && xo < 0)
                            {
                                intpartX = -1;
                                decpartX = xo * intpartX;
                                //Canto superior esquerdo
                                if (yo >= -1 && yo < 0)
                                {
                                    intpartY = -1;
                                    decpartY = yo * intpartY;
                                    Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                                    (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx1);
                                    Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                                    (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx1);
                                    Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                                    (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx1);
                                }
                                else
                                {
                                    //Canto inferior esquerdo
                                    if (yo >= height - 1 && yo < height)
                                    {
                                        intpartY = height - 1;
                                        decpartY = yo - intpartY;
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round(decpartY * Bx);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round(decpartY * Bx);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx);

                                    }
                                    //Coluna esquerda
                                    //Bx = decpart * X,Y + (1-decpart) * (X+1),Y -> inverteu
                                    //Bx = decpart * X,(Y+1) + (1-decpart) * (X+1),(Y+1) -> inverteu
                                    else
                                    {
                                        intpartY = (int)yo;
                                        decpartY = yo - intpartY;
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                    }
                                }
                            }
                            else
                            {
                                if (xo >= width - 1 && xo < width)
                                {
                                    intpartX = width - 1;
                                    decpartX = xo - intpartX;
                                    //Canto superior direito
                                    if (yo >= -1 && yo < 0)
                                    {
                                        intpartY = -1;
                                        decpartY = yo * intpartY;
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx1);
                                    }
                                    else
                                    {
                                        //Canto inferior direito
                                        if (yo >= height - 1 && yo < height)
                                        {
                                            intpartY = height - 1;
                                            decpartY = yo - intpartY;
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0]);
                                            (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1]);
                                            (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2]);
                                            (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx);
                                        }
                                        //Coluna direita
                                        else
                                        {
                                            intpartY = (int)yo;
                                            decpartY = yo - intpartY;
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0]);
                                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0]);
                                            (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1]);
                                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1]);
                                            (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2]);
                                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2]);
                                            (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                        }
                                    }
                                }
                                else
                                {
                                    intpartX = (int)xo;
                                    decpartX = xo - intpartX;
                                    //Linha inferior
                                    if (yo >= height - 1 && yo < height)
                                    {
                                        intpartY = height - 1;
                                        decpartY = yo - intpartY;
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx);
                                    }
                                    //Linha superior
                                    else
                                    {
                                        intpartY = -1;
                                        decpartY = yo * intpartY;
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            intpartX = (int)xo;
                            intpartY = (int)yo;
                            decpartX = xo - intpartX;
                            decpartY = yo - intpartY;

                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                            (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);

                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                            (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);

                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                            (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                        }
                    }
                }
            }
        }

        //Funções usadas para a deteção do código de barras 
        public static void RemoveLastObject(Image<Bgr, byte> img, int[,] etiquetas)
        {
            unsafe
            {
                int x, y, maxEti = 0;

                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels; // number of channels - 3

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (etiquetas[y, x] > maxEti)
                            maxEti = etiquetas[y, x];
                    }
                }

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (etiquetas[y, x] == maxEti)
                        {
                            (dataPtr + y * widthStep + x * nChan)[0] = 255;
                            (dataPtr + y * widthStep + x * nChan)[1] = 255;
                            (dataPtr + y * widthStep + x * nChan)[2] = 255;
                        }
                    }
                }
            }
        }

        public static void RotationBC(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                int xo, yo, xd, yd, aux_o, aux_d;

                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3

                for (yd = 0; yd < height; yd++)
                {
                    for (xd = 0; xd < width; xd++)
                    {
                        xo = (int)Math.Round((xd - (width / 2.0)) * Math.Cos(angle) - ((height / 2.0) - yd) * Math.Sin(angle) + width / 2.0);
                        yo = (int)Math.Round(height / 2.0 - (xd - (width / 2.0)) * Math.Sin(angle) - ((height / 2.0) - yd) * Math.Cos(angle));
                        aux_d = yd * widthStep + xd * nChan;

                        if (xo < 0 || yo < 0 || xo >= width || yo >= height)
                        {
                            (dataPtrDes + aux_d)[0] = 255;
                            (dataPtrDes + aux_d)[1] = 255;
                            (dataPtrDes + aux_d)[2] = 255;
                        }
                        else
                        {
                            aux_o = yo * widthStep + xo * nChan;
                            (dataPtrDes + aux_d)[0] = (dataPtrOrig + aux_o)[0];
                            (dataPtrDes + aux_d)[1] = (dataPtrOrig + aux_o)[1];
                            (dataPtrDes + aux_d)[2] = (dataPtrOrig + aux_o)[2];
                        }
                    }
                }
            }
        }

        public static void RotationBilinearBC(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                int xd, yd, aux_d;
                int intpartX, intpartY;
                double xo, yo, decpartX, decpartY;
                byte Bx, Bx1;
                MIplImage orig = imgCopy.MIplImage;
                byte* dataPtrOrig = (byte*)orig.imageData.ToPointer();

                MIplImage des = img.MIplImage;
                byte* dataPtrDes = (byte*)des.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = orig.widthStep;
                int nChan = orig.nChannels; // number of channels - 3

                for (yd = 0; yd < height; yd++)
                {
                    for (xd = 0; xd < width; xd++)
                    {
                        xo = ((xd - (width / 2.0)) * Math.Cos(angle) - ((height / 2.0) - yd) * Math.Sin(angle) + width / 2.0);
                        yo = (height / 2.0 - (xd - (width / 2.0)) * Math.Sin(angle) - ((height / 2.0) - yd) * Math.Cos(angle));
                        aux_d = yd * widthStep + xd * nChan;

                        //Fora dos limites da imagem 
                        if (xo < -1 || yo < -1 || xo >= width || yo >= height)
                        {
                            (dataPtrDes + aux_d)[0] = 255;
                            (dataPtrDes + aux_d)[1] = 255;
                            (dataPtrDes + aux_d)[2] = 255;
                        }
                        //Nas bordas
                        else if ((xo >= -1 && xo < 0) || (yo >= -1 && yo < 0) || (xo >= width - 1 && xo < width) || (yo >= height - 1 && yo < height))
                        {
                            if (xo >= -1 && xo < 0)
                            {
                                intpartX = -1;
                                decpartX = xo * intpartX;
                                //Canto superior esquerdo
                                if (yo >= -1 && yo < 0)
                                {
                                    intpartY = -1;
                                    decpartY = yo * intpartY;
                                    Bx1 = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                                    (dataPtrDes + aux_d)[0] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                    Bx1 = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                                    (dataPtrDes + aux_d)[1] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                    Bx1 = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                                    (dataPtrDes + aux_d)[2] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                }
                                else
                                {
                                    //Canto inferior esquerdo
                                    if (yo >= height - 1 && yo < height)
                                    {
                                        intpartY = height - 1;
                                        decpartY = yo - intpartY;
                                        Bx = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round(decpartY * Bx + (1 - decpartY) * 255);
                                        Bx = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round(decpartY * Bx + (1 - decpartY) * 255);
                                        Bx = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * 255);

                                    }
                                    //Coluna esquerda
                                    //Bx = decpart * X,Y + (1-decpart) * (X+1),Y -> inverteu
                                    //Bx = decpart * X,(Y+1) + (1-decpart) * (X+1),(Y+1) -> inverteu
                                    else
                                    {
                                        intpartY = (int)yo;
                                        decpartY = yo - intpartY;
                                        Bx = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                                        Bx1 = (byte)Math.Round(decpartX *  255 + (1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                        Bx = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                                        Bx1 = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                        Bx = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                                        Bx1 = (byte)Math.Round(decpartX * 255 + (1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                    }
                                }
                            }
                            else
                            {
                                if (xo >= width - 1 && xo < width)
                                {
                                    intpartX = width - 1;
                                    decpartX = xo - intpartX;
                                    //Canto superior direito
                                    if (yo >= -1 && yo < 0)
                                    {
                                        intpartY = -1;
                                        decpartY = yo * intpartY;
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0] + decpartX * 255);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1] + decpartX * 255);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2] + decpartX * 255);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                    }
                                    else
                                    {
                                        //Canto inferior direito
                                        if (yo >= height - 1 && yo < height)
                                        {
                                            intpartY = height - 1;
                                            decpartY = yo - intpartY;
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0] + decpartX * 255);
                                            (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * 255);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1] + decpartX * 255);
                                            (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * 255);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2] + decpartX * 255);
                                            (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * 255);
                                        }
                                        //Coluna direita
                                        else
                                        {
                                            intpartY = (int)yo;
                                            decpartY = yo - intpartY;
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0] + decpartX * 255);
                                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0] + decpartX * 255);
                                            (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1] + decpartX * 255);
                                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1] + decpartX * 255);
                                            (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2] + decpartX * 255);
                                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2] + decpartX * 255);
                                            (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                                        }
                                    }
                                }
                                else
                                {
                                    intpartX = (int)xo;
                                    decpartX = xo - intpartX;
                                    //Linha inferior
                                    if (yo >= height - 1 && yo < height)
                                    {
                                        intpartY = height - 1;
                                        decpartY = yo - intpartY;
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * 255);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * 255);
                                        Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * 255);
                                    }
                                    //Linha superior
                                    else
                                    {
                                        intpartY = -1;
                                        decpartY = yo * intpartY;
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                                        (dataPtrDes + aux_d)[0] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                                        (dataPtrDes + aux_d)[1] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                        Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                                        (dataPtrDes + aux_d)[2] = (byte)Math.Round(decpartY * 255 + (1 - decpartY) * Bx1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            intpartX = (int)xo;
                            intpartY = (int)yo;
                            decpartX = xo - intpartX;
                            decpartY = yo - intpartY;

                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[0]);
                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[0] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[0]);
                            (dataPtrDes + aux_d)[0] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);

                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[1]);
                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[1] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[1]);
                            (dataPtrDes + aux_d)[1] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);

                            Bx = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + intpartY * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + intpartY * widthStep + (intpartX + 1) * nChan)[2]);
                            Bx1 = (byte)Math.Round((1 - decpartX) * (dataPtrOrig + (intpartY + 1) * widthStep + intpartX * nChan)[2] + decpartX * (dataPtrOrig + (intpartY + 1) * widthStep + (intpartX + 1) * nChan)[2]);
                            (dataPtrDes + aux_d)[2] = (byte)Math.Round((1 - decpartY) * Bx + decpartY * Bx1);
                        }
                    }
                }
            }
        }

        public static void Dilatacao(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy) 
        {
            unsafe
            {
                int x, y;
                //Máscara = [1,1,1; 1,1,1; 1,1,1]
                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                MIplImage imagemCopy = imgCopy.MIplImage;
                byte* dataPtrCopy = (byte*)imagemCopy.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            if (x == 0)
                            {
                                if (y == 0)
                                {
                                    if ((dataPtrCopy + y * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 0 ||
                                        (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 0)
                                    {
                                        (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                        (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                        (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                    }
                                }
                                else
                                {
                                    if (y == height - 1)
                                    {
                                        if ((dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 0 ||
                                            (dataPtrCopy + y * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 0)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                        }
                                    }
                                    else
                                    {
                                        if ((dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 0 ||
                                            (dataPtrCopy + y * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 0 ||
                                            (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 0)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (x == width - 1)
                                {
                                    if (y == 0)
                                    {
                                        if ((dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 0 || 
                                            (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 0)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (y == height - 1)
                                        {
                                            if ((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 0 ||
                                                (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 0)
                                            {
                                                (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                                (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                                (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                            }
                                        }
                                        else
                                        {
                                            if ((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 0||
                                                (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 0||
                                                (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 0)
                                            {
                                                (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                                (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                                (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        if ((dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 0 ||
                                            (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 0)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                        }
                                    }
                                    else
                                    {
                                        if ((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 0 ||
                                            (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 0)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 0;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 0 ||
                               (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 0 ||
                               (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 0 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 0 )
                            {
                                (dataPtr + y * widthStep + x * nChan)[0] = 0;
                                (dataPtr + y * widthStep + x * nChan)[1] = 0;
                                (dataPtr + y * widthStep + x * nChan)[2] = 0;
                            }
                        }
                    }
                }
            }
        }

        public static void Erosao(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                int x, y;
                //Máscara = [1,1,1; 1,1,1; 1,1,1]
                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                MIplImage imagemCopy = imgCopy.MIplImage;
                byte* dataPtrCopy = (byte*)imagemCopy.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            if (x == 0)
                            {
                                if (y == 0)
                                {
                                    if ((dataPtrCopy + y * widthStep + x * nChan)[0] == 255|| (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 255 ||
                                        (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 255)
                                    {
                                        (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                        (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                        (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                    }
                                }
                                else
                                {
                                    if (y == height - 1)
                                    {
                                        if ((dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 255 ||
                                            (dataPtrCopy + y * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 255)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                        }
                                    }
                                    else
                                    {
                                        if ((dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 255 ||
                                            (dataPtrCopy + y * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 255 ||
                                            (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 255)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (x == width - 1)
                                {
                                    if (y == 0)
                                    {
                                        if ((dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 255 ||
                                            (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 255)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                        }
                                    }
                                    else
                                    {
                                        if (y == height - 1)
                                        {
                                            if ((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 255 ||
                                                (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 255)
                                            {
                                                (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                                (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                                (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                            }
                                        }
                                        else
                                        {
                                            if ((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 255 ||
                                                (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 255 ||
                                                (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 255)
                                            {
                                                (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                                (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                                (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        if ((dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 255 ||
                                            (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 255)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                        }
                                    }
                                    else
                                    {
                                        if ((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 255 ||
                                            (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 255)
                                        {
                                            (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                            (dataPtr + y * widthStep + x * nChan)[2] = 255;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if ((dataPtrCopy + (y - 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y - 1) * widthStep + (x + 1) * nChan)[0] == 255 ||
                               (dataPtrCopy + y * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + y * widthStep + (x + 1) * nChan)[0] == 255 ||
                               (dataPtrCopy + (y + 1) * widthStep + (x - 1) * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + x * nChan)[0] == 255 || (dataPtrCopy + (y + 1) * widthStep + (x + 1) * nChan)[0] == 255)
                            {
                                (dataPtr + y * widthStep + x * nChan)[0] = 255;
                                (dataPtr + y * widthStep + x * nChan)[1] = 255;
                                (dataPtr + y * widthStep + x * nChan)[2] = 255;
                            }
                        }
                    }
                }
            }
        }

        public static int[,] Etiquetas(Image<Bgr, byte> img)
        {
            unsafe
            {
                int x, y, aux_x, aux_y;
                int num = 1;
                bool changed = true;

                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels;

                int[,] etiquetas = new int[height, width];

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if((dataPtr + y * widthStep + x * nChan)[0] == 0)
                        {
                            etiquetas[y,x] = num;
                            num += 1;
                        }
                        else if((dataPtr + y * widthStep + x * nChan)[0] == 255)
                        {
                            etiquetas[y,x] = 0;
                        }
                    }
                }

                //Propagação 
                while(changed)
                {
                    changed = false;
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (etiquetas[y, x] == 0)
                            {
                                continue;
                            }
                            num = width * height;
                            if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                            {
                                if (x == 0)
                                {
                                    if (y == 0)
                                    {
                                        for (aux_y = 0; aux_y < 2; aux_y++)
                                        {
                                            for (aux_x = 0; aux_x < 2; aux_x++)
                                            {
                                                if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                {
                                                    num = etiquetas[y + aux_y, x + aux_x];
                                                }
                                            }
                                        }
                                        if (num < etiquetas[y, x])
                                        {
                                            etiquetas[y, x] = num;
                                            changed = true;
                                        }     
                                    }
                                    else
                                    {
                                        if (y == height - 1)
                                        {
                                            for (aux_y = -1; aux_y < 1; aux_y++)
                                            {
                                                for (aux_x = 0; aux_x < 1; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            for (aux_y = -1; aux_y < 2; aux_y++)
                                            {
                                                for (aux_x = 0; aux_x < 2; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (x == width - 1)
                                    {
                                        if (y == 0)
                                        {
                                            for (aux_y = 0; aux_y < 2; aux_y++)
                                            {
                                                for (aux_x = -1; aux_x < 1; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            if (y == height - 1)
                                            {
                                                for (aux_y = -1; aux_y < 1; aux_y++)
                                                {
                                                    for (aux_x = -1; aux_x < 1; aux_x++)
                                                    {
                                                        if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                        {
                                                            num = etiquetas[y + aux_y, x + aux_x];
                                                        }
                                                    }
                                                }
                                                if (num < etiquetas[y, x])
                                                {
                                                    etiquetas[y, x] = num;
                                                    changed = true;
                                                }
                                            }
                                            else
                                            {
                                                for (aux_y = -1; aux_y < 2; aux_y++)
                                                {
                                                    for (aux_x = -1; aux_x < 1; aux_x++)
                                                    {
                                                        if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                        {
                                                            num = etiquetas[y + aux_y, x + aux_x];
                                                        }
                                                    }
                                                }
                                                if (num < etiquetas[y, x])
                                                {
                                                    etiquetas[y, x] = num;
                                                    changed = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (y == 0)
                                        {
                                            for (aux_y = 0; aux_y < 2; aux_y++)
                                            {
                                                for (aux_x = -1; aux_x < 2; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            for (aux_y = -1; aux_y < 1; aux_y++)
                                            {
                                                for (aux_x = -1; aux_x < 2; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for(aux_y = -1; aux_y < 2; aux_y++)
                                {
                                    for(aux_x = -1; aux_x < 2; aux_x++)
                                    {
                                        if(num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                        {
                                            num = etiquetas[y + aux_y, x + aux_x];
                                        }
                                    }
                                }
                                if (num < etiquetas[y, x])
                                {
                                    etiquetas[y, x] = num;
                                    changed = true;
                                }
                            }
                        }
                    }
                    if (!changed) break;
                    for (y = height - 1; y >= 0; y--)
                    {
                        for (x = width - 1; x >= 0; x--)
                        {
                            if (etiquetas[y, x] == 0)
                            {
                                continue;
                            }
                            num = width * height;
                            if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                            {
                                if (x == 0)
                                {
                                    if (y == 0)
                                    {
                                        for (aux_y = 0; aux_y < 2; aux_y++)
                                        {
                                            for (aux_x = 0; aux_x < 2; aux_x++)
                                            {
                                                if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                {
                                                    num = etiquetas[y + aux_y, x + aux_x];
                                                }
                                            }
                                        }
                                        if (num < etiquetas[y, x])
                                        {
                                            etiquetas[y, x] = num;
                                            changed = true;
                                        }
                                    }
                                    else
                                    {
                                        if (y == height - 1)
                                        {
                                            for (aux_y = -1; aux_y < 1; aux_y++)
                                            {
                                                for (aux_x = 0; aux_x < 1; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            for (aux_y = -1; aux_y < 2; aux_y++)
                                            {
                                                for (aux_x = 0; aux_x < 2; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (x == width - 1)
                                    {
                                        if (y == 0)
                                        {
                                            for (aux_y = 0; aux_y < 2; aux_y++)
                                            {
                                                for (aux_x = -1; aux_x < 1; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            if (y == height - 1)
                                            {
                                                for (aux_y = -1; aux_y < 1; aux_y++)
                                                {
                                                    for (aux_x = -1; aux_x < 1; aux_x++)
                                                    {
                                                        if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                        {
                                                            num = etiquetas[y + aux_y, x + aux_x];
                                                        }
                                                    }
                                                }
                                                if (num < etiquetas[y, x])
                                                {
                                                    etiquetas[y, x] = num;
                                                    changed = true;
                                                }
                                            }
                                            else
                                            {
                                                for (aux_y = -1; aux_y < 2; aux_y++)
                                                {
                                                    for (aux_x = -1; aux_x < 1; aux_x++)
                                                    {
                                                        if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                        {
                                                            num = etiquetas[y + aux_y, x + aux_x];
                                                        }
                                                    }
                                                }
                                                if (num < etiquetas[y, x])
                                                {
                                                    etiquetas[y, x] = num;
                                                    changed = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (y == 0)
                                        {
                                            for (aux_y = 0; aux_y < 2; aux_y++)
                                            {
                                                for (aux_x = -1; aux_x < 2; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            for (aux_y = -1; aux_y < 1; aux_y++)
                                            {
                                                for (aux_x = -1; aux_x < 2; aux_x++)
                                                {
                                                    if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                                    {
                                                        num = etiquetas[y + aux_y, x + aux_x];
                                                    }
                                                }
                                            }
                                            if (num < etiquetas[y, x])
                                            {
                                                etiquetas[y, x] = num;
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (aux_y = -1; aux_y < 2; aux_y++)
                                {
                                    for (aux_x = -1; aux_x < 2; aux_x++)
                                    {
                                        if (num > etiquetas[y + aux_y, x + aux_x] && etiquetas[y + aux_y, x + aux_x] != 0)
                                        {
                                            num = etiquetas[y + aux_y, x + aux_x];
                                        }
                                    }
                                }
                                if (num < etiquetas[y, x])
                                {
                                    etiquetas[y, x] = num;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
                return etiquetas;
            }
        }

        public static int NumEtiquetas(int[,] etiquetas, int y, int width)
        {
            int []numEtiquetas = new int[width];
            int x, aux_x, arrayLength = 0;
            
            for(x = 0; x < width; x++)
            {
                if(etiquetas[y,x] != 0)
                {
                    aux_x = 0;
                    while (aux_x != arrayLength)
                    {
                        if(numEtiquetas[aux_x] == etiquetas[y, x])
                        {
                            break;
                        }
                        aux_x++;
                    }
                    if (aux_x == arrayLength)
                    {
                        numEtiquetas[arrayLength] = etiquetas[y, x];
                        arrayLength++;
                    }
                }
            }
            return arrayLength;
        }

        public static double CompareBitmaps(Bitmap bmp1, Bitmap bmp2)
        {
            int bytes = bmp1.Width * bmp1.Height * (Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

            double valorTotal = 0, valoresIguais = 0;
            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] == b2bytes[n])
                {
                    valoresIguais += 1;
                }
                valorTotal += 1;
            }

            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return (double)(valoresIguais / valorTotal) * 100;
        }

        public static string ReadBC(Image<Bgr, byte> img, int right_limit, int left_limit, int upper_limit, int bottom_limit)
        {
            unsafe
            {
                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels;

                string[] D_Inicial = { "LLLLLL", "LLGLGG", "LLGGLG", "LLGGGL", "LGLLGG", "LGGLLG", "LGGGLL", "LGLGLG", "LGLGGL", "LGGLGL" };

                string[] L_Code = { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };
                string[] G_Code = { "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111" };
                string[] R_Code = { "1110010", "1100110", "1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100" };

                string codeRead = null;
                string splitedCode = null;
                string finalCode = null;
                string firstNumber = null;

                int x, y, index;
                int pixelArea, arrayLenght = 0;
                int nextColor = 0, currentColor = 0, colorPosition, barNum;
                int numP = 0, numB = 1;
                bool colorChanged = false;
                //Estas variáveis vão guardar os valores originais do limite superior e inferior
                int bottom_limit_copy = bottom_limit;
                
                upper_limit = (upper_limit + bottom_limit) / 2 - 2;
                bottom_limit = upper_limit + 5;
                
                
                

                while (arrayLenght != 92 && bottom_limit <= bottom_limit_copy)
                {
                    x = left_limit;
                    upper_limit += 1;
                    bottom_limit += 1;
                    arrayLenght = 0;
                    
                    //Para passar pixeis brancos caso haja antes de começar o código de barras
                    while (numB > numP)
                    {
                        numB = 0;
                        numP = 0;
                        for (y = upper_limit; y < bottom_limit; y++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0 && (dataPtr + y * widthStep + x * nChan)[1] == 0 && (dataPtr + y * widthStep + x * nChan)[2] == 0)
                                numP += 1;
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 255 && (dataPtr + y * widthStep + x * nChan)[1] == 255 && (dataPtr + y * widthStep + x * nChan)[2] == 255)
                                numB += 1;
                        }
                        x++;
                    }

                    //Primeira barra preta
                    numP = 1; numB = 0;
                    while (numP > numB)
                    {
                        numB = 0;
                        numP = 0;
                        for (y = upper_limit; y < bottom_limit; y++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0 && (dataPtr + y * widthStep + x * nChan)[1] == 0 && (dataPtr + y * widthStep + x * nChan)[2] == 0)
                                numP += 1;
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 255 && (dataPtr + y * widthStep + x * nChan)[1] == 255 && (dataPtr + y * widthStep + x * nChan)[2] == 255)
                                numB += 1;
                        }
                        x++;
                    }
                    //Primeira barra branca
                    numB = 1; numP = 0;
                    while (numB > numP)
                    {
                        numB = 0;
                        numP = 0;
                        for (y = upper_limit; y < bottom_limit; y++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0 && (dataPtr + y * widthStep + x * nChan)[1] == 0 && (dataPtr + y * widthStep + x * nChan)[2] == 0)
                                numP += 1;
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 255 && (dataPtr + y * widthStep + x * nChan)[1] == 255 && (dataPtr + y * widthStep + x * nChan)[2] == 255)
                                numB += 1;
                        }
                        x++;
                    }
                    //Segunda barra preta
                    colorPosition = x;
                    numP = 1; numB = 0;
                    while (numP > numB)
                    {
                        numB = 0;
                        numP = 0;
                        for (y = upper_limit; y < bottom_limit; y++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0 && (dataPtr + y * widthStep + x * nChan)[1] == 0 && (dataPtr + y * widthStep + x * nChan)[2] == 0)
                                numP += 1;
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 255 && (dataPtr + y * widthStep + x * nChan)[1] == 255 && (dataPtr + y * widthStep + x * nChan)[2] == 255)
                                numB += 1;
                        }
                        x++;
                    }
                    pixelArea = x - colorPosition;

                    codeRead = "101";
                    colorPosition = x;

                    while (arrayLenght < 92 && x < right_limit + pixelArea)
                    {
                        numP = 0;
                        numB = 0;
                        for (y = upper_limit; y <= bottom_limit; y++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0 && (dataPtr + y * widthStep + x * nChan)[1] == 0 && (dataPtr + y * widthStep + x * nChan)[2] == 0)
                                numP += 1;
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 255 && (dataPtr + y * widthStep + x * nChan)[1] == 255 && (dataPtr + y * widthStep + x * nChan)[2] == 255)
                                numB += 1;
                        }

                        if (numP > numB)
                        {
                            if (currentColor != 1)
                            {
                                nextColor = 1;
                                colorChanged = true;
                            }
                        }
                        if (numB > numP)
                        {
                            if (currentColor != 0)
                            {
                                nextColor = 0;
                                colorChanged = true;
                            }
                        }

                        if (colorChanged)
                        {
                            if (currentColor == 0)
                            {
                                barNum = (int)Math.Round((double)((x + 1 - colorPosition) / pixelArea));
                                if (barNum == 0) barNum = 1;
                                while (barNum > 0)
                                {
                                    codeRead += "0";
                                    arrayLenght++;
                                    barNum--;
                                }
                            }
                            else if (currentColor == 1)
                            {
                                barNum = (int)Math.Round((double)((x + 1 - colorPosition) / pixelArea));
                                if (barNum == 0) barNum = 1;
                                while (barNum > 0)
                                {
                                    codeRead += "1";
                                    arrayLenght++;
                                    barNum--;
                                }
                            }
                            colorChanged = false;
                            currentColor = nextColor;
                            colorPosition = x;
                        }
                        x++;
                    }
                }
                
                y = 3; //As três primeiras barras não entram para o código nem as últimas três
                //Seis primeiros números
                for (x = 1; x < 7; x++)
                {
                    index = 0;
                    splitedCode = codeRead.Substring(y, 7);
                    while (index<10)
                    {
                        if (splitedCode == L_Code[index])
                        {
                            finalCode += index.ToString();
                            firstNumber += "L";
                            break;
                        }
                        else if (splitedCode == G_Code[index])
                        {
                            finalCode += index.ToString();
                            firstNumber += "G";
                            break;
                        }
                        index++;
                    }
                    y += 7;
                }


                y += 5; //Por causa das cinco barras do meio
                for(x = 7; x < 13; x++)
                {
                    index = 0;
                    splitedCode = codeRead.Substring(y, 7);
                    while (index < 10)
                    {
                        if (splitedCode == R_Code[index])
                        {
                            finalCode += index.ToString();
                            break;
                        }
                        index++;
                    }
                    y += 7;
                }

                index = 0;
                while (index < 10)
                {
                    if (firstNumber == D_Inicial[index])
                    {
                        finalCode = index.ToString() + finalCode;
                        break;
                    }
                    index++;
                }
                return finalCode;
            }
        }

        public static string ReadNums(Image<Bgr, byte> img, int[,] etiquetas, int right_limit, int left_limit, int upper_limit, int bottom_limit)
        {
            unsafe
            {
                int width = img.Width;
                int height = img.Height;
                int x, y, i, j, k, arrayLength = 0, etiquetasln; 
                double minDiference, Diference; //i, j, k são variáveis para dar suporte em ciclos for
                int[] etiqs = new int[13];
                int right_limit_num, left_limit_num, upper_limit_num, bottom_limit_num;
                string numLido = "0";
                string bc_image = null;

                Image<Bgr, byte> imgNo;
                Rectangle rect;
                Point bc_centroid = Point.Empty;
                Size bc_size = Size.Empty;
                Bitmap bmp1, bmp2;

                //1- Encontrar linha com 13 etiquetas
                //2 - Guardar as etiquetas
                for (y = height - 1; y > bottom_limit; y--)
                {
                    etiquetasln = NumEtiquetas(etiquetas, y, width);

                    if (etiquetasln == 13)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (etiquetas[y, x] != 0)
                            {                               
                                i = 0;
                                while (i != arrayLength)
                                {
                                    if (etiqs[i] == etiquetas[y, x])
                                    {
                                        break;
                                    }
                                    i++;
                                }
                                if (i == arrayLength)
                                {
                                    etiqs[arrayLength] = etiquetas[y, x];
                                    arrayLength++;                                   
                                }
                            } 
                        }
                        break;
                    }
                }
                //3 - Centroides para cada número
                for (i = 0; i < 13; i++)
                {                    
                    right_limit_num = 0; 
                    left_limit_num = width; 
                    upper_limit_num = height; 
                    bottom_limit_num = bottom_limit;

                    for (y = bottom_limit; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {                         

                            if (etiqs[i] == etiquetas[y, x] && right_limit_num < x)
                                right_limit_num = x;

                            if (etiqs[i] == etiquetas[y, x] && left_limit_num > x)
                                left_limit_num = x;

                            if (etiqs[i] == etiquetas[y, x] && upper_limit_num > y)
                                upper_limit_num = y;

                            if (etiqs[i] == etiquetas[y, x] && bottom_limit_num < y)
                                bottom_limit_num = y;
                        }
                    }
                    if (upper_limit_num <= bottom_limit && i > 0)
                    {
                        upper_limit_num = height;
                        for (y = bottom_limit; y < height; y++)
                        {
                            for (x = 0; x < width; x++)
                            {

                                if (etiqs[i-1] == etiquetas[y, x] && upper_limit_num > y)
                                    upper_limit_num = y;

                            }
                        }
                    }

                    bc_centroid = new Point((right_limit_num + left_limit_num) / 2, (upper_limit_num + bottom_limit_num) / 2);
                    bc_size = new Size(right_limit_num - left_limit_num + 3, bottom_limit_num - upper_limit_num + 3);

                    rect = new Rectangle(bc_centroid.X - (bc_size.Width / 2), bc_centroid.Y - (bc_size.Height / 2), bc_size.Width, bc_size.Height);
                    imgNo = img.Copy();
                    imgNo.ROI = rect;
                    imgNo = imgNo.Resize(30, 40, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                    imgNo.ToBitmap().Save($"C:\\Users\\joao-\\Desktop\\Universidade\\7ºSemestre\\SS\\SS_OpenCV_Base\\Read\\{i}.png", ImageFormat.Png);

                    bmp1 = new Bitmap($"C:\\Users\\joao-\\Desktop\\Universidade\\7ºSemestre\\SS\\SS_OpenCV_Base\\Read\\{i}.png");
                    minDiference = 0;

                    for (j = 0; j < 10; j++)
                    {
                        for (k = 0; k < 2; k++)
                        {
                            bmp2 = new Bitmap($"C:\\Users\\joao-\\Desktop\\Universidade\\7ºSemestre\\SS\\SS_OpenCV_Base\\Dados\\{j}\\{k}.png");
                            Diference = CompareBitmaps(bmp1, bmp2);
                            if (Diference > minDiference)
                            {
                                minDiference = Diference;
                                numLido = j.ToString();
                            }
                        }
                    }      
                    bc_image += numLido;
                }
                return bc_image;
            }            
        }

        public static Image<Bgr, byte> BarCodeReader( Image<Bgr, byte> img, int type,
                                                      out Point bc_centroid1, out Size bc_size1,
                                                      out string bc_image1, out string bc_number1,
                                                      out Point bc_centroid2, out Size bc_size2,
                                                      out string bc_image2, out string bc_number2)
        {
            unsafe
            {
                int x, y, aux_y;
                float ang;

                MIplImage imagem = img.MIplImage;
                byte* dataPtr = (byte*)imagem.imageData.ToPointer();

                Image<Bgr, byte> imgCopy = img.Copy();
                MIplImage imagemCopy = imgCopy.MIplImage;
                byte* dataPtrCopy = (byte*)imagemCopy.imageData.ToPointer();


                int width = img.Width;
                int height = img.Height;
                int widthStep = imagem.widthStep;
                int nChan = imagem.nChannels;

                int[,] etiquetas = new int[height, width];

                // first barcode
                bc_image1 = "5601212323434";
                bc_number1 = "9780201379624";
                bc_centroid1 = Point.Empty;
                bc_size1 = Size.Empty;

                int right_limit = 0, left_limit = width, bottom_limit = 0, upper_limit = height; 

                //second barcode
                bc_image2 = null;
                bc_number2 = null;
                bc_centroid2 = Point.Empty;
                bc_size2 = Size.Empty;

                double Sx = 0, Sy = 0, Sxx = 0, Syy = 0, Sxy = 0, Area = 0;
                double Mxx, Myy, Mxy; 

                //Imagem do tipo 1
                if (type == 1)
                {
                    ConvertToBW_Otsu(img);
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                            {
                                upper_limit = y;
                                break;
                            }
                        }
                        if (upper_limit != height)
                            break;
                    }

                    for (y = height - 1; y > upper_limit; y--)
                    {
                        for (x = width - 1; x > 0; x--)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                            {
                                bottom_limit = y;
                                aux_y = y;
                                while(aux_y > ((bottom_limit + upper_limit) / 2))
                                {
                                    if((dataPtr + aux_y * widthStep + x * nChan)[0] != 0)
                                    {
                                        bottom_limit = 0;
                                        break;
                                    }
                                    aux_y--;
                                }
                            }
                            if (bottom_limit != 0)
                                break;
                        }
                        if (bottom_limit != 0)
                            break;
                    }

                    y = (upper_limit + bottom_limit) / 2;
                    for (x = ((right_limit + left_limit) / 2); x < width; x++) 
                    {
                        if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                            right_limit = x;
                    }

                    for (x = ((right_limit + left_limit) / 2); x > 0; x--)
                    {
                        if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                                left_limit = x;
                    }

                    y = (upper_limit + bottom_limit) / 2;
                    int numP, numPInicial = 0;
                    while (true)
                    {
                        numP = 0;
                        for (x = left_limit; x < right_limit; x++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                                numP += 1;
                        }
                        if (numPInicial < numP)
                        {
                            numPInicial = numP;
                        }
                        else if (numP < (numPInicial / 2))
                        {
                            bottom_limit = y;
                            break;
                        }
                        y++;
                    }

                    bc_number1 = ReadBC(img, right_limit, left_limit, upper_limit, bottom_limit);

                    etiquetas = Etiquetas(img);
                    bc_image1 = ReadNums(img, etiquetas, right_limit, left_limit, upper_limit, bottom_limit);

                    bc_centroid1 = new Point((right_limit + left_limit) / 2, (upper_limit + bottom_limit) / 2);
                    bc_size1 = new Size(right_limit - left_limit, bottom_limit - upper_limit);

                    img.Draw(new Rectangle(bc_centroid1.X - (bc_size1.Width / 2), bc_centroid1.Y - (bc_size1.Height / 2), bc_size1.Width, bc_size1.Height), 
                        new Bgr(0, 255, 0), 1);       
                    

                }

                //Imagem do tipo 2
                if (type == 2)
                {
                    ConvertToBW_Otsu(imgCopy);
                    Image<Bgr, byte> imgCopy2 = imgCopy.Copy(); //Para não perder a imagem original cria-se outra cópia
                    Dilatacao(imgCopy, imgCopy2);
                    imgCopy2 = imgCopy.Copy();
                    Dilatacao(imgCopy, imgCopy2);
                    etiquetas = Etiquetas(imgCopy);
                    RemoveLastObject(imgCopy, etiquetas);

                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if ((dataPtrCopy + y * widthStep + x * nChan)[0] == 0 && right_limit < x)
                                right_limit = x;

                            if ((dataPtrCopy + y * widthStep + x * nChan)[0] == 0 && left_limit > x)
                                left_limit = x;

                            if ((dataPtrCopy + y * widthStep + x * nChan)[0] == 0 && upper_limit > y)
                                upper_limit = y;

                            if ((dataPtrCopy + y * widthStep + x * nChan)[0] == 0 && bottom_limit < y)
                                bottom_limit = y;
                        }                           
                    }

                    for(y = upper_limit; y <= bottom_limit; y++)
                    {
                        for (x = left_limit; x <= right_limit; x++)
                        {
                            if((dataPtrCopy + y * widthStep + x * nChan)[0] == 0){
                                Sx += x;
                                Sy += y;
                                Sxx += x * x;
                                Syy += y * y;
                                Sxy += x * y;
                                Area += 1;
                            }
                        }
                    }
                    
                    Mxx = Sxx - ((Sx * Sx) / Area);
                    Myy = Syy - ((Sy * Sy) / Area);
                    Mxy = Sxy - ((Sy * Sx) / Area);

                    ang = (float)Math.Atan((Mxx - Myy + Math.Sqrt((Mxx - Myy) * (Mxx - Myy) - 4 * Mxy * Mxy))/(2 * Mxy));

                    if (ang < 0) ang = ang + (float)(Math.PI / 2.0);
                    else if (ang > 0) ang = ang - (float)(Math.PI / 2.0);
                    
                    imgCopy = img.Copy();
                    RotationBC(img, imgCopy, ang);
                    ConvertToBW_Otsu(img);

                    upper_limit = height;
                    bottom_limit = 0;
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                            {
                                upper_limit = y;
                                break;
                            }
                        }
                        if (upper_limit != height)
                            break;
                    }
                    for (y = height - 1; y > upper_limit; y--)
                    {
                        for (x = width - 1; x > 0; x--)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                            {
                                bottom_limit = y;
                                aux_y = y;
                                while (aux_y > ((bottom_limit + upper_limit) / 2))
                                {
                                    if ((dataPtr + aux_y * widthStep + x * nChan)[0] != 0)
                                    {
                                        bottom_limit = 0;
                                        break;
                                    }
                                    aux_y--;
                                }
                            }
                            if (bottom_limit != 0)
                                break;
                        }
                        if (bottom_limit != 0)
                            break;
                    }

                    y = (upper_limit + bottom_limit) / 2;
                    for (x = (width / 2); x < width; x++)
                    {
                        if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                            right_limit = x;
                    }

                    for (x = (width / 2); x > 0; x--)
                    {
                        if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                            left_limit = x;
                    }

                    y = (upper_limit + bottom_limit) / 2;
                    int numP, numPInicial = 0;
                    while (true)
                    {
                        numP = 0;
                        for (x = left_limit; x < right_limit; x++)
                        {
                            if ((dataPtr + y * widthStep + x * nChan)[0] == 0)
                                numP += 1;
                        }
                        if (numPInicial < numP)
                        {
                            numPInicial = numP;
                        }
                        else if (numP < (numPInicial / 2))
                        {
                            bottom_limit = y;
                            break;
                        }
                        y++;
                    }
                    
                    bc_number1 = ReadBC(img, right_limit, left_limit, upper_limit, bottom_limit);

                    etiquetas = Etiquetas(img);
                    bc_image1 = ReadNums(img, etiquetas, right_limit, left_limit, upper_limit, bottom_limit);

                    bc_centroid1 = new Point((right_limit + left_limit) / 2, (upper_limit + bottom_limit) / 2);
                    bc_size1 = new Size(right_limit - left_limit, bottom_limit - upper_limit);

                    img.Draw(new Rectangle(bc_centroid1.X - (bc_size1.Width / 2), bc_centroid1.Y - (bc_size1.Height / 2), bc_size1.Width, bc_size1.Height),
                        new Bgr(0, 255, 0), 1);
                }

                return img;
            }
            
        }
    }
}
