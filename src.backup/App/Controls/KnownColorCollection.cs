using System;
using System.Collections;
using System.Drawing;

namespace logview4net.Controls
{

	/// <summary>Enumère les filtres applicables lors de la création d'une instance de la classe KnownColorCollection</summary>
	public enum KnownColorFilter {
		/// <summary>Restreint les couleurs connues à celles utilisées par le système</summary>
		System,
		/// <summary>Restreint les couleurs connues à celles proposées pour le web</summary>
		Web,
		/// <summary>Aucune restriction</summary>
		All
	};

	/// <summary>Classe de base qui encapsule les couleurs fournies par le Framework</summary>
	public class KnownColorCollection : IColorCollection {
		
		/// <summary>Contient le nombre d'objets que contient la collection</summary>
		protected readonly int COUNT;
		/// <summary>Contient l'index du premier élément de la collection dans l'énumération KnownColor</summary>
		protected readonly int FIRST;
		/// <summary>Contient l'index du dernier élément de la collection dans l'énumération KnownColor</summary>
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

		/// <summary>Obtient l'énumérateur associé à la collection</summary>
		/// <returns>Retourne un énumérateur<seealso cref="KnownColorEnumerator"/>KnownColorEnumerator</returns>
		public IEnumerator GetEnumerator() {
			return new KnownColorEnumerator(this);
		}

		/// <summary>Retourne l'objet couleur situé à l'emplacement spécifié dans la collection</summary>
		/// <summary>Retourne l'objet couleur déterminé par son nom</summary>
		public Color this[int iColor] {
			get { 
				if(iColor < 0 || iColor >= Count) throw new ArgumentOutOfRangeException();
				return Color.FromKnownColor((KnownColor)(iColor + FIRST)); }
		}

		/// <summary>Retourne l'objet couleur déterminé par son nom</summary>
		public Color this[string szColor] {
			get {
				if(szColor.Length == 0) throw new ArgumentNullException();
				return Color.FromName(szColor);
			} 
		}

		/// <summary>Obtient l'index de la couleur spécifiée dans la collection</summary>
		public int IndexOf(string colorName) {
			for(var i=FIRST; i<LAST; i++) {
				if(Color.FromKnownColor((KnownColor)i).Name.Equals(colorName)) return i-FIRST;
			}
			return -1;
		}

			#region KnownColorEnumerator
			/// <summary>Enumerateur spécifique aux collections d'objets KnownColor</summary>
			private class KnownColorEnumerator : IEnumerator
			{
				/// <summary>Contient la position du curseur dans la collection</summary>
				private int m_Location;
				/// <summary>Contient une référence vers l'objet collection parent</summary>
				private IColorCollection m_ColorCollection;

				/// <summary>Constructeur</summary>
				/// <param name="colors">Référence sur l'objet collection parent</param>
				public KnownColorEnumerator(IColorCollection colors) {
					m_ColorCollection = colors;
					m_Location = -1;
				}
				/// <summary>Déplace le curseur vers l'objet suivant dans la collection</summary>
				/// <returns>Renvoie true si l'opération a réussie et false sinon</returns>
				public bool MoveNext() {
					return (++m_Location < m_ColorCollection.Count);
				}
				/// <summary>Obtient l'objet sur lequel est placé le curseur</summary>
				/// <remarks>Une exception du type <see cref="InvalidOperationException"/>
				/// InvalidOperationException est levée si le curseur se trouve hors de la plage</remarks>
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
