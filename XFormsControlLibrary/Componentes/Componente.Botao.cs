using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XFormsControlLibrary.Componentes
{
    /// <summary>
    /// Componente Botao da Biblioteca XForms
    /// </summary>
    public class ButtonXForms : Button
    {
        #region Atributos
        private int borderSize = 0;
        private int borderRadius = 20;
        private Color borderColor = Color.PaleVioletRed;
        private Entidades.Enums.BorderRadiusPosition borderRadiusPosition = Entidades.Enums.BorderRadiusPosition.All;
        #endregion

        #region Propriedades Visual Studio
        [Category("XForms")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }
        [Category("XForms")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }
        [Category("XForms")]
        public Entidades.Enums.BorderRadiusPosition BorderRadiusPosition
        {
            get { return borderRadiusPosition; }
            set
            {
                borderRadiusPosition = value;
                this.Invalidate();
            }
        }
        [Category("XForms")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("XForms")]
        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("XForms")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }
        #endregion

        #region Construtores / Inicializadores
        public ButtonXForms()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;
            this.Resize += new EventHandler(Button_Resize);
        }
        #endregion

        #region Metodos/Eventos
        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }
        private GraphicsPath GetFigurePath(Rectangle rect, int topLeftRadius, int topRightRadius, int bottomLeftRadius, int bottomRightRadius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();

            if (topLeftRadius > 0)
            {
                path.AddArc(rect.X, rect.Y, topLeftRadius * 2, topLeftRadius * 2, 180, 90);
            }
            else
            {
                path.AddLine(rect.X, rect.Y, rect.X, rect.Y);
            }

            if (topRightRadius > 0)
            {
                path.AddArc(rect.X + rect.Width - topRightRadius * 2, rect.Y, topRightRadius * 2, topRightRadius * 2, 270, 90);
            }
            else
            {
                path.AddLine(rect.X + rect.Width, rect.Y, rect.X + rect.Width, rect.Y);
            }

            if (bottomRightRadius > 0)
            {
                path.AddArc(rect.X + rect.Width - bottomRightRadius * 2, rect.Y + rect.Height - bottomRightRadius * 2, bottomRightRadius * 2, bottomRightRadius * 2, 0, 90);
            }
            else
            {
                path.AddLine(rect.X + rect.Width, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height);
            }

            if (bottomLeftRadius > 0)
            {
                path.AddArc(rect.X, rect.Y + rect.Height - bottomLeftRadius * 2, bottomLeftRadius * 2, bottomLeftRadius * 2, 90, 90);
            }
            else
            {
                path.AddLine(rect.X, rect.Y + rect.Height, rect.X, rect.Y + rect.Height);
            }

            path.CloseFigure();
            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;
            if (borderSize > 0)
                smoothSize = borderSize;
            if (borderRadius > 2) //Rounded button
            {
                int topLeftRadius = 0;
                int topRightRadius = 0;
                int bottomLeftRadius = 0;
                int bottomRightRadius = 0;

                switch (borderRadiusPosition)
                {
                    case Entidades.Enums.BorderRadiusPosition.All:
                        topLeftRadius = topRightRadius = bottomLeftRadius = bottomRightRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderRadiusPosition.Left:
                        topLeftRadius = bottomLeftRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderRadiusPosition.Right:
                        topRightRadius = bottomRightRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderRadiusPosition.Top:
                        topLeftRadius = topRightRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderRadiusPosition.Bottom:
                        bottomLeftRadius = bottomRightRadius = BorderRadius;
                        break;
                    default:
                        break;
                }


                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, topLeftRadius - BorderSize, topRightRadius - BorderSize, bottomLeftRadius - BorderSize, bottomRightRadius - BorderSize))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //Button surface
                    this.Region = new Region(pathSurface);
                    //Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);
                    //Button border                    
                    if (borderSize >= 1)
                        //Draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else //Normal button
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.None;
                //Button surface
                this.Region = new Region(rectSurface);
                //Button border
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }
        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        #endregion

    }
}
