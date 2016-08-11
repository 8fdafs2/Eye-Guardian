using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// (((double)Value - (double)Minimum) / ((double)Maximum - (double)Minimum))

namespace EyeGuardian
{
    public class CusProgBar : ProgressBar
    {
        public CusProgBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // None... Helps control the flicker.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const int inset = 1; // A single inset value to control teh sizing of the inner rect.

            using (Image offscreenImage = new Bitmap(this.Width, this.Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.

                    double scaleFactor = (((double)this.Value - (double)this.Minimum) / ((double)this.Maximum - (double)this.Minimum));

                    rect.Width = (int)(rect.Width * scaleFactor);
                    if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                    LinearGradientBrush brush = new LinearGradientBrush(rect, this.BackColor, this.ForeColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);

                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    //offscreenImage.Dispose();
                }
            }
        }
    }
}
