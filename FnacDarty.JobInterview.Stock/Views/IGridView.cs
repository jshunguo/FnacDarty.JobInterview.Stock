using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FnacDarty.JobInterview.Stock.Views
{
    /// <summary>
    /// Interface définissant les opérations nécessaires pour la gestion et l'affichage d'une grille.
    /// </summary>
    public interface IGridView
    {
        /// <summary>
        /// Lie une source de données à la grille.
        /// </summary>
        /// <param name="source">Source de données à lier à la grille.</param>
        /// <exception cref="ArgumentNullException">Lancée si la source de données est nulle.</exception>
        void Bind(object source);

        /// <summary>
        /// Rend la grille dans un format textuel via un <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/> utilisé pour afficher la grille.</param>
        /// <exception cref="ArgumentNullException">Lancée si le <see cref="TextWriter"/> est nul.</exception>
        void Render(TextWriter writer);
    }

    /// <summary>
    /// Interface définissant une méthode pour créer une nouvelle instance de <see cref="IGridView"/>.
    /// </summary>
    public interface IGridViewFactory
    {
        /// <summary>
        /// Crée une nouvelle instance de <see cref="IGridView"/> avec des colonnes spécifiées.
        /// </summary>
        /// <param name="columns">Liste des colonnes qui définissent la structure de la grille.</param>
        /// <returns>Une nouvelle instance de <see cref="IGridView"/>.</returns>
        /// <exception cref="ArgumentNullException">Lancée si la liste des colonnes est nulle.</exception>
        IGridView CreateNew(IReadOnlyList<GridColumn> columns);
    }

}
