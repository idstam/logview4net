using System;
using System.Drawing;
using System.Windows.Forms;

namespace logview4net.Controls
{

	/// <summary>Contrôle permettant de sélectionner une couleur parmi toute une collection</summary>
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
		/// <summary>Contient la collection de couleurs à afficher</summary>
		private IColorCollection _colorCollection;
	#endregion Variables

	#region Construction
		/// <summary>Constructeur par défaut</summary>
		public ColorPicker() {
			DrawMode = DrawMode.OwnerDrawFixed;
			DropDownStyle = ComboBoxStyle.DropDownList;
		}
		/// <summary>Nettoyage des ressources utilisées - managées et non managées</summary>
		/// <param name="disposing">Préciser false pour libérer uniquement les ressources non managées</param>
		protected override void Dispose(bool disposing) {
			if(disposing) {
			}
			base.Dispose(disposing);
		}
	#endregion Construction

	#region Propriétés
		/// <summary>Obtient ou définit la collection de couleurs à afficher</summary>
		/// <remarks>Masque la collection Items de l'objet parent ComboBox</remarks>
		public new IColorCollection Items {
			get { return _colorCollection; }
			set {
				if(_colorCollection != value && value != null ) {
					_colorCollection = value;
					foreach(Color color in value) base.Items.Add(color.Name);
					// Redessiner le contrôle
					Refresh();
				}
			}
		}
		/// <summary>Obtient ou définit le nom de la couleur sélectionnée</summary>
		/// <remarks>Masque la propriété SelectedText de l'objet parent ComboBox</remarks>
		public new string SelectedText {
			get { return Items[SelectedIndex].Name; }
			set {
				var selidx = Items.IndexOf(value);
				if(selidx > 0) SelectedIndex = selidx;
			}
		}
	#endregion Propriétés

	#region Méthodes
	#endregion Méthodes

	#region Evènements
		/// <summary>Appelée en cas de modification de l'apparence visuelle du Picker, redessine un item</summary>
		/// <param name="e">Contient les paramètres de l'évènement nécessaires au dessin d'un item</param>
		protected override void OnDrawItem(DrawItemEventArgs e) {
			var grphcs = e.Graphics;
			Color blockColor;
			const int left = RectcolorLeft;
			// Dessiner l'arrière-plan de l'item en fonction de son état
			if(e.State == DrawItemState.Selected || e.State == DrawItemState.None) e.DrawBackground();
			// Récupérer la couleur à afficher
			if(e.Index == -1) blockColor = SelectedIndex < 0 ? BackColor : Color.FromName(SelectedText);
			else blockColor = Color.FromName((string)base.Items[e.Index]);
			// Peindre le rectangle représentant la couleur
			grphcs.FillRectangle(new SolidBrush(blockColor),left,e.Bounds.Top+RectcolorTop,RectcolorWidth,ItemHeight - 2 * RectcolorTop);
			// Dessiner un cadre noir autour du rectangle
			grphcs.DrawRectangle(Pens.Black,left,e.Bounds.Top+RectcolorTop,RectcolorWidth,ItemHeight - 2 * RectcolorTop);
			// Dessiner le nom de la couleur
			grphcs.DrawString(blockColor.Name,e.Font,new SolidBrush(ForeColor),new Rectangle(RecttextLeft,e.Bounds.Top,e.Bounds.Width-RecttextLeft,ItemHeight));
			// Appeller la méthode de base
			base.OnDrawItem(e);
		}

		/// <summary>Appelée lorsque la propriété DropDownStyle a été modifiée</summary>
		/// <param name="e">Contient les paramètres de l'évènement nécessaires</param>
		/// <remarks>Cette surcharge garantit que la propriété DropDownStylle restera à DropDownList</remarks>
		protected override void OnDropDownStyleChanged(EventArgs e) {
			if(DropDownStyle != ComboBoxStyle.DropDownList) DropDownStyle = ComboBoxStyle.DropDownList;
		}
	#endregion Evènements

	}
}
