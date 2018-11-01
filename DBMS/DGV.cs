using System.Drawing;
using System.Windows.Forms;

namespace DBMS
{
    public class Dgv : DataGridView
    {
        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);
            var rectSource = new Rectangle(Location.X, Location.Y, Width, Height);
            var rectDest = new Rectangle(0, 0, rectSource.Width, rectSource.Height);

            var b = new Bitmap(Parent.ClientRectangle.Width, Parent.ClientRectangle.Height);
            Graphics.FromImage(b).DrawImage(Parent.BackgroundImage, Parent.ClientRectangle);


            graphics.DrawImage(b, rectDest, rectSource, GraphicsUnit.Pixel);
            SetCellsTransparent();
        }


        private void SetCellsTransparent()
        {
            EnableHeadersVisualStyles = false;
            ColumnHeadersDefaultCellStyle.BackColor = Color.Transparent;
            RowHeadersDefaultCellStyle.BackColor = Color.Transparent;


            foreach (DataGridViewColumn col in Columns)
            {
                col.DefaultCellStyle.BackColor = Color.Transparent;
                col.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            }
        }
    }
}
