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
    public class BotaoXForms : Button
    {
        #region Atributos
        private Entidades.Enums.EBordaTipo eTipoBorda = Entidades.Enums.EBordaTipo.Nenhum;
        #endregion

        #region Propriedades Visual Studio
        [Category("XForms")]
        public Entidades.Enums.EBordaTipo TipoBorda
        {
            get
            {
                return eTipoBorda;
            }
            set
            {
                eTipoBorda = value;
                this.Invalidate();
            }
        }
        #endregion

        #region Construtores / Inicializadores
        public BotaoXForms()
        {
            this.Paint += BotaoXForms_Paint;
        }
        #endregion

        #region Metodos/Eventos
        private void BotaoXForms_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                GraphicsPath buttonPath = new GraphicsPath();
                Rectangle newRectangle = btn.ClientRectangle;
                newRectangle.Inflate(-3, -3);

                switch (TipoBorda)
                {
                    case Entidades.Enums.EBordaTipo.Completo:
                        buttonPath.AddArc(new Rectangle(newRectangle.X, newRectangle.Y, 10, 10), 180, 90);
                        buttonPath.AddArc(new Rectangle(newRectangle.Width - 10 + newRectangle.X, newRectangle.Y, 10, 10), 270, 90);
                        buttonPath.AddArc(new Rectangle(newRectangle.Width - 10 + newRectangle.X, newRectangle.Height - 10 + newRectangle.Y, 10, 10), 0, 90);
                        buttonPath.AddArc(new Rectangle(newRectangle.X, newRectangle.Height - 10 + newRectangle.Y, 10, 10), 90, 90);
                        break;
                    case Entidades.Enums.EBordaTipo.Esquerda:
                        buttonPath.AddArc(new Rectangle(newRectangle.X, newRectangle.Y, 10, 10), 180, 90);
                        buttonPath.AddLine(new Point(newRectangle.Width, 2), new Point(newRectangle.Width - newRectangle.X, newRectangle.Y));
                        buttonPath.AddLine(new Point(newRectangle.Width, newRectangle.Y), new Point(newRectangle.Width, newRectangle.Height - newRectangle.Y + 3));
                        buttonPath.AddArc(new Rectangle(newRectangle.X, newRectangle.Height - 12 + newRectangle.Y, 10, 10), 90, 90);
                        break;
                    case Entidades.Enums.EBordaTipo.Direita:
                        buttonPath.AddLine(new Point(newRectangle.X, newRectangle.Y), new Point(2, 2));
                        buttonPath.AddArc(new Rectangle(newRectangle.Width - 10 + newRectangle.X, newRectangle.Y, 10, 10), 270, 90);
                        buttonPath.AddArc(new Rectangle(newRectangle.Width - 10 + newRectangle.X, newRectangle.Height - 10 + newRectangle.Y, 10, 10), 0, 90);
                        buttonPath.AddLine(new Point(newRectangle.X, newRectangle.Height + 2), new Point(2, 2));
                        break;
                    default:
                        buttonPath.AddLine(new Point(newRectangle.X, newRectangle.Y), new Point(newRectangle.Width + newRectangle.X, newRectangle.Y));
                        buttonPath.AddLine(new Point(newRectangle.Width + newRectangle.X, newRectangle.Height + newRectangle.Y), new Point(newRectangle.X, newRectangle.Height + newRectangle.Y));
                        break;
                }
                buttonPath.CloseFigure();
                btn.Region = new Region(buttonPath);
            }
            catch (Exception Problema)
            {
                MessageBox.Show(Problema.Message);
            }
        }
        #endregion
    }
}
