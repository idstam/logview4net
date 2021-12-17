using System;
using System.Drawing;
using System.Windows.Forms;

namespace logview4net.Controls
{

	/// <summary>Contr�le permettant de s�lectionner une couleur parmi toute une collection</summary>
	public class ColorPicker : ComboBox
	{

	#region Constantes
		private const int RectcolorLeft = 4;
		private const int RectcolorTop = 2;
		private const int RectcolorWidth = 40;
		private const int RecttextMargin = 10;
		private const int RecttextLeft = RectcolorLeft + RectcolorWidth + RecttextMargin;
	#endregion Constantes

	#region Variables
		/// <summary>Contient la collection de couleurs � afficher</summary>
		private IColorCollection _colorCollection;
	#endregion Variables

	#region Construction
		/// <summary>Constructeur par d�faut</summary>
		public ColorPicker() {
			DrawMode = DrawMode.OwnerDrawFixed;
			DropDownStyle = ComboBoxStyle.DropDownList;
		}
		/// <summary>Nettoyage des ressources utilis�es - manag�es et non manag�es</summary>
		/// <param name="disposing">Pr�ciser false pour lib�rer uniquement les ressources non manag�es</param>
		protected override void Dispose(bool disposing) {
			if(disposing) {
			}
			base.Dispose(disposing);
		}
	#endregion Construction

	#region Propri�t�s
		/// <summary>Obtient ou d�finit la collection de couleurs � afficher</summary>
		/// <remarks>Masque la collection Items de l'objet parent ComboBox</remarks>
		public new IColorCollection Items {
			get { return _colorCollection; }
			set {
				if(_colorCollection != value && value != null ) {
					_colorCollection = value;
					foreach(Color color in value) base.Items.Add(color.Name);
					// Redessiner le contr�le
					Refresh();
				}
			}
		}
		/// <summary>Obtient ou d�finit le nom de la couleur s�lectionn�e</summary>
		/// <remarks>Masque la propri�t� SelectedText de l'objet parent ComboBox</remarks>
		public new string SelectedText {
			get { return Items[SelectedIndex].Name; }
			set {
				var selidx = Items.IndexOf(value);
				if(selidx > 0) SelectedIndex = selidx;
			}
		}
	#endregion Propri�t�s

	#region M�thodes
	#endregion M�thodes

	#region Ev�nements
		/// <summary>Appel�e en cas de modification de l'apparence visuelle du Picker, redessine un item</summary>
		/// <param name="e">Contient les param�tres de l'�v�nement n�cessaires au dessin d'un item</param>
		protected override void OnDrawItem(DrawItemEventArgs e) {
			var grphcs = e.Graphics;
			Color blockColor;
			const int left = RectcolorLeft;
			// Dessiner l'arri�re-plan de l'item en fonction de son �tat
			if(e.State == DrawItemState.Selected || e.State == DrawItemState.None) e.DrawBackground();
			// R�cup�rer la couleur � afficher
			if(e.Index == -1) blockColor = SelectedIndex < 0 ? BackColor : Color.FromName(SelectedText);
			else blockColor = Color.FromName((string)base.Items[e.Index]);
			// Peindre le rectangle repr�sentant la couleur
			grphcs.FillRectangle(new SolidBrush(blockColor),left,e.Bounds.Top+RectcolorTop,RectcolorWidth,ItemHeight - 2 * RectcolorTop);
			// Dessiner un cadre noir autour du rectangle
			grphcs.DrawRectangle(Pens.Black,left,e.Bounds.Top+RectcolorTop,RectcolorWidth,ItemHeight - 2 * RectcolorTop);
			// Dessiner le nom de la couleur
			grphcs.DrawString(blockColor.Name,e.Font,new SolidBrush(ForeColor),new Rectangle(RecttextLeft,e.Bounds.Top,e.Bounds.Width-RecttextLeft,ItemHeight));
			// Appeller la m�thode de base
			base.OnDrawItem(e);
		}

		/// <summary>Appel�e lorsque la propri�t� DropDownStyle a �t� modifi�e</summary>
		/// <param name="e">Contient les param�tres de l'�v�nement n�cessaires</param>
		/// <remarks>Cette surcharge garantit que la propri�t� DropDownStylle restera � DropDownList</remarks>
		protected override void OnDropDownStyleChanged(EventArgs e) {
			if(DropDownStyle != ComboBoxStyle.DropDownList) DropDownStyle = ComboBoxStyle.DropDownList;
		}
	#endregion Ev�nements

	}
}
