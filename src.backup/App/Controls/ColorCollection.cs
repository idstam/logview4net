using System.Collections;
using System.Drawing;

namespace logview4net.Controls
{
	/// <summary>D�claration de l'interface IColorCollection</summary>
	public interface IColorCollection : IEnumerable
	{
		/// <summary>Obtient le nombre d'objets <see cref="Color"/>Color de la collection</summary>
		int Count { get; }
		/// <summary>Retourne l'objet couleur situ� � l'emplacement sp�cifi� dans la collection</summary>
		Color this[int i] { get; }
		/// <summary>Retourne l'objet couleur d�termin� par son nom</summary>
		Color this[string s] { get; }
		/// <summary>Obtient l'�num�rateur associ� � la collection</summary>
		/// <returns>Retourne un objet de type <seealso cref="IEnumerator"/>IEnumerator</returns>
		new IEnumerator GetEnumerator();
		/// <summary>Obtient l'index de la couleur sp�cifi�e dans la collection</summary>
		int IndexOf(string colorName);
	}
}
