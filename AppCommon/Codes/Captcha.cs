using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AppCommon
{
    public class MyCaptcha
    {
        public static string NewText()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static string Make(string _text, int _width, int _height)
        {
            Bitmap bmp = new(_width, _height);
            Graphics grap = Graphics.FromImage(bmp);

            RectangleF recF = new(0, 0, _width, _height);
            Brush brush = new HatchBrush(HatchStyle.Percent05, Color.Gainsboro, Color.White);
            grap.FillRectangle(brush, recF);
            SizeF text_size;
            Font the_font;
            float font_size = _height + 1;
            do
            {
                font_size -= 1;
                the_font = new Font("Cambria", font_size, FontStyle.Regular, GraphicsUnit.Pixel);
                text_size = grap.MeasureString(_text, the_font);
            }
            while (text_size.Width > _width || text_size.Height > _height);

            // Center the text.
            StringFormat string_format = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Convert the text into a path.
            GraphicsPath graphics_path = new();
            graphics_path.AddString(_text, the_font.FontFamily, 1, the_font.Size, recF, string_format);

            //Make random warping parameters.
            Random rnd = new();
            PointF[] pts = { new PointF((float)rnd.Next(_width) / 4, (float)rnd.Next(_height) / 4), new PointF(_width - (float)rnd.Next(_width) / 4, (float)rnd.Next(_height) / 4), new PointF((float)rnd.Next(_width) / 4, _height - (float)rnd.Next(_height) / 4), new PointF(_width - (float)rnd.Next(_width) / 4, _height - (float)rnd.Next(_height) / 4) };
            Matrix mat = new();
            graphics_path.Warp(pts, recF, mat, WarpMode.Perspective, 0);

            // Draw the text.
            brush = new HatchBrush(HatchStyle.Percent75, Color.Silver, Color.Silver);
            grap.FillPath(brush, graphics_path);

            graphics_path.Dispose();
            brush.Dispose();
            the_font.Dispose();
            grap.Dispose();
            string_format.Dispose();
            mat.Dispose();

            using MemoryStream ms = new();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bmp.Dispose();

            return "data:image/jpg;base64," + Convert.ToBase64String(ms.ToArray());

        }
    }
}
