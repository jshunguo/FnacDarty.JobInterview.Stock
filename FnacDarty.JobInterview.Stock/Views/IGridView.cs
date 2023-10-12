using System.Collections;
using System.IO;

namespace FnacDarty.JobInterview.Stock.Views
{
    /// <summary>
    /// Represente une grille d'affichage des données
    /// </summary>
    public interface IGridView
    {
        /// <summary>
        /// Effectue la liaison à la source de données
        /// </summary>
        /// <param name="source"></param>
        void Bind(object source);

        /// <summary>
        /// Effectue le rendue de la grille dans le flux passé en paramettre
        /// </summary>
        /// <param name="writer"></param>
        void Render(TextWriter writer);
    }
}
