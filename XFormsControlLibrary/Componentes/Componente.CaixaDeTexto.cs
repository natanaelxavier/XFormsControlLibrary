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
        public new string Text
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
        public HorizontalAlignment AlignTexts
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
            if (borderRadius > 1)//Rounded TextBox
            {
                //-Fields
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;
                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penBorderSmooth = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    //-Drawing
                    this.Region = new Region(pathBorderSmooth);//Set the rounded region of UserControl
                    if (borderRadius > 15) SetTextBoxRoundedRegion();//Set the rounded region of TextBox component
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
            else //Square/Normal TextBox
            {
                //Draw border
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    if (isFocused) penBorder.Color = borderFocusColor;
                    if (underlinedStyle) //Line Style
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else //Normal Style
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
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
