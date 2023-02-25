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
    public class TextBoxXForms : Panel
    {
        #region Atributos
        private TextBox textbox;
        private Color borderColor = Color.MediumSlateBlue;
        private Color borderFocusColor = Color.HotPink;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private bool isFocused = false;
        private int borderRadius = 0;
        private Color placeholderColor = Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceholder = false;
        private bool isPasswordChar = false;
        private HorizontalAlignment alignText = HorizontalAlignment.Left;
        private HorizontalAlignment alignTextPlaceholder = HorizontalAlignment.Left;
        private Entidades.Enums.BorderTextBoxRadiusPosition borderRadiusPosition = Entidades.Enums.BorderTextBoxRadiusPosition.All;

        public event EventHandler textChanged;
        #endregion

        #region Propriedades Visual Studio
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
        public Color BorderFocusColor
        {
            get { return borderFocusColor; }
            set { borderFocusColor = value; }
        }
        [Category("XForms")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                if (value >= 1)
                {
                    borderSize = value;
                    this.Invalidate();
                }
            }
        }
        [Category("XForms")]
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }
        [Category("XForms")]
        public bool PasswordChar
        {
            get { return isPasswordChar; }
            set
            {
                isPasswordChar = value;
                if (!isPlaceholder)
                    textbox.UseSystemPasswordChar = value;
            }
        }
        [Category("XForms")]
        public bool Multiline
        {
            get { return textbox.Multiline; }
            set { textbox.Multiline = value; }
        }
        [Category("XForms")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                textbox.BackColor = value;
            }
        }
        [Category("XForms")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                textbox.ForeColor = value;
            }
        }
        [Category("XForms")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                textbox.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();
            }
        }
        [Category("XForms")]
        public string Content
        {
            get
            {
                if (isPlaceholder) return "";
                else return textbox.Text;
            }
            set
            {
                RemovePlaceholder();
                textbox.Text = value;
            }
        }
        [Category("XForms")]
        public HorizontalAlignment AlignContent
        {
            get
            {
                return this.alignText;
            }
            set
            {
                this.alignText = value;
                textbox.TextAlign = this.alignText;
            }
        }
        [Category("XForms")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                    this.Invalidate();//Redraw control
                }
            }
        }
        [Category("XForms")]
        public Color PlaceholderColor
        {
            get { return placeholderColor; }
            set
            {
                placeholderColor = value;
                if (isPlaceholder)
                    textbox.ForeColor = value;
            }
        }
        [Category("XForms")]
        public string PlaceholderText
        {
            get { return placeholderText; }
            set
            {
                placeholderText = value;
                textbox.Text = "";
                SetPlaceholder();
            }
        }
        [Category("XForms")]
        public HorizontalAlignment AlignTextPlaceholder
        {
            get
            {
                return this.alignTextPlaceholder;
            }
            set
            {
                this.alignTextPlaceholder = value;
                textbox.TextAlign = this.alignTextPlaceholder;
            }
        }
        [Category("XForms")]
        public Entidades.Enums.BorderTextBoxRadiusPosition BorderRadiusPosition
        {
            get { return borderRadiusPosition; }
            set
            {
                borderRadiusPosition = value;
                this.Invalidate();
            }
        }
        #endregion

        #region Construtores / Inicializadores
        public TextBoxXForms()
        {
            InicializarTextbox();
            this.Width = 232;
            this.Height = 48;
            this.Padding = new Padding(10, 5, 10, 5);
            this.BackColor = Color.White;
            this.Controls.Add(textbox);
        }
        private void InicializarTextbox()
        {
            textbox = new TextBox();
            textbox.BorderStyle = BorderStyle.None;
            textbox.Text = "Natanael Xavier";
            textbox.TextChanged += textBox_TextChanged;
            textbox.Enter += textBox_Enter;
            textbox.Leave += textBox_Leave;
            textbox.Click += textBox_Click;
            textbox.MouseEnter += textBox_MouseEnter;
            textbox.MouseLeave += textBox_MouseLeave;
            textbox.KeyPress += textBox_KeyPress;
            textbox.Dock = DockStyle.Fill;
        }
        #endregion

        #region Metodos
        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textbox.Text) && placeholderText != "")
            {
                isPlaceholder = true;
                textbox.Text = placeholderText;
                textbox.ForeColor = placeholderColor;
                textbox.TextAlign = this.alignTextPlaceholder;
                if (isPasswordChar)
                    textbox.UseSystemPasswordChar = false;
            }
        }
        private void RemovePlaceholder()
        {
            if (isPlaceholder && placeholderText != "")
            {
                isPlaceholder = false;
                textbox.Text = "";
                textbox.ForeColor = this.ForeColor;
                textbox.TextAlign = this.alignText;
                if (isPasswordChar)
                    textbox.UseSystemPasswordChar = true;
            }
        }
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
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
        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath pathTxt;
            if (Multiline)
            {
                pathTxt = GetFigurePath(textbox.ClientRectangle, borderRadius - borderSize);
                textbox.Region = new Region(pathTxt);
            }
            else
            {
                pathTxt = GetFigurePath(textbox.ClientRectangle, borderSize * 2);
                textbox.Region = new Region(pathTxt);
            }
            pathTxt.Dispose();
        }
        private void UpdateControlHeight()
        {
            if (textbox.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textbox.Multiline = true;
                textbox.MinimumSize = new Size(0, txtHeight);
                textbox.Multiline = false;
                this.Height = textbox.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }
        #endregion

        #region Eventos
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
                UpdateControlHeight();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;
            if (borderRadius > 1)
            {
                //Rounded TextBox
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;

                int topLeftRadius = 0;
                int topRightRadius = 0;
                int bottomLeftRadius = 0;
                int bottomRightRadius = 0;

                switch (borderRadiusPosition)
                {
                    case Entidades.Enums.BorderTextBoxRadiusPosition.All:
                        topLeftRadius = topRightRadius = bottomLeftRadius = bottomRightRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderTextBoxRadiusPosition.Left:
                        topLeftRadius = bottomLeftRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderTextBoxRadiusPosition.Right:
                        topRightRadius = bottomRightRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderTextBoxRadiusPosition.Top:
                        topLeftRadius = topRightRadius = BorderRadius;
                        break;
                    case Entidades.Enums.BorderTextBoxRadiusPosition.Bottom:
                        bottomLeftRadius = bottomRightRadius = BorderRadius;
                        break;
                    default:
                        break;
                }

                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius))
                using (Pen penBorderSmooth = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    //Drawing
                    this.Region = new Region(pathBorderSmooth);
                    if (borderRadius > 15) SetTextBoxRoundedRegion();
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                    if (isFocused) penBorder.Color = borderFocusColor;
                    if (underlinedStyle) //Line Style
                    {
                        //Draw border smoothing
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    else //Normal Style
                    {
                        //Draw border smoothing
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                        graph.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                //Square/Normal TextBox
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    if (isFocused) penBorder.Color = borderFocusColor;
                    if (underlinedStyle) //Line Style
                    {
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    else //Normal Style
                    {
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                    }
                }
            }
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (textChanged != null)
                textChanged.Invoke(sender, e);
        }
        private void textBox_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
        }
        private void textBox_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
            SetPlaceholder();
        }
        private void textBox_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
        private void textBox_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }
        private void textBox_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }
        public void Clear()
        {
            this.textbox.Clear();
            SetPlaceholder();
        }
        public bool IsPlaceHolder() => this.isPlaceholder;
        public new bool Focus()
        {
            base.Focus();
            return this.textbox.Focus();
        }
        #endregion
    }
}
