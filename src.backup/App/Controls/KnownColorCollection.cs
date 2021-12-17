using System;
using System.Collections;
using System.Drawing;

namespace logview4net.Controls
{

	/// <summary>Enum�re les filtres applicables lors de la cr�ation d'une instance de la classe KnownColorCollection</summary>
	public enum KnownColorFilter {
		/// <summary>Restreint les couleurs connues � celles utilis�es par le syst�me</summary>
		System,
		/// <summary>Restreint les couleurs connues � celles propos�es pour le web</summary>
		Web,
		/// <summary>Aucune restriction</summary>
		All
	};

	/// <summary>Classe de base qui encapsule les couleurs fournies par le Framework</summary>
	public class KnownColorCollection : IColorCollection {
		
		/// <summary>Contient le nombre d'objets que contient la collection</summary>
		protected readonly int COUNT;
		/// <summary>Contient l'index du premier �l�ment de la collection dans l'�num�ration KnownColor</summary>
		protected readonly int FIRST;
		/// <summary>Contient l'index du dernier �l�ment de la collection dans l'�num�ration KnownColor</summary>
		protected readonly int LAST;
		
		/// <summary>Constructeur unique</summary>
		public KnownColorCollection(KnownColorFilter filter) {
			switch(filter) {
				case KnownColorFilter.All: COUNT = 167; FIRST = 1; break;
				case KnownColorFilter.System: COUNT = 27; FIRST = 1; break;
				case KnownColorFilter.Web: COUNT = 140; FIRST = 28; break;
			}
			LAST = FIRST + COUNT;
		}
		/// <summary>Obtient le nombre d'objets <see cref="Color"/>Color de la collection</summary>
		public int Count {
			get { return COUNT; }
		}

		/// <summary>Obtient l'�num�rateur associ� � la collection</summary>
		/// <returns>Retourne un �num�rateur<seealso cref="KnownColorEnumerator"/>KnownColorEnumerator</returns>
		public IEnumerator GetEnumerator() {
			return new KnownColorEnumerator(this);
		}

		/// <summary>Retourne l'objet couleur situ� � l'emplacement sp�cifi� dans la collection</summary>
		/// <summary>Retourne l'objet couleur d�termin� par son nom</summary>
		public Color this[int iColor] {
			get { 
				if(iColor < 0 || iColor >= Count) throw new ArgumentOutOfRangeException();
				return Color.FromKnownColor((KnownColor)(iColor + FIRST)); }
		}

		/// <summary>Retourne l'objet couleur d�termin� par son nom</summary>
		public Color this[string szColor] {
			get {
				if(szColor.Length == 0) throw new ArgumentNullException();
				return Color.FromName(szColor);
			} 
		}

		/// <summary>Obtient l'index de la couleur sp�cifi�e dans la collection</summary>
		public int IndexOf(string colorName) {
			for(var i=FIRST; i<LAST; i++) {
				if(Color.FromKnownColor((KnownColor)i).Name.Equals(colorName)) return i-FIRST;
			}
			return -1;
		}

			#region KnownColorEnumerator
			/// <summary>Enumerateur sp�cifique aux collections d'objets KnownColor</summary>
			private class KnownColorEnumerator : IEnumerator
			{
				/// <summary>Contient la position du curseur dans la collection</summary>
				private int m_Location;
				/// <summary>Contient une r�f�rence vers l'objet collection parent</summary>
				private IColorCollection m_ColorCollection;

				/// <summary>Constructeur</summary>
				/// <param name="colors">R�f�rence sur l'objet collection parent</param>
				public KnownColorEnumerator(IColorCollection colors) {
					m_ColorCollection = colors;
					m_Location = -1;
				}
				/// <summary>D�place le curseur vers l'objet suivant dans la collection</summary>
				/// <returns>Renvoie true si l'op�ration a r�ussie et false sinon</returns>
				public bool MoveNext() {
					return (++m_Location < m_ColorCollection.Count);
				}
				/// <summary>Obtient l'objet sur lequel est plac� le curseur</summary>
				/// <remarks>Une exception du type <see cref="InvalidOperationException"/>
				/// InvalidOperationException est lev�e si le curseur se trouve hors de la plage</remarks>
				public Object Current {
					get {
						if(m_Location < 0 || m_Location > m_ColorCollection.Count) throw new InvalidOperationException();
						return m_ColorCollection[m_Location];
					}
				}
				/// <summary>Replace le curseur en position initiale</summary>
				public void Reset() {
					m_Location = -1;
				}
			}
		#endregion KnownColorEnumerator
	}
}
