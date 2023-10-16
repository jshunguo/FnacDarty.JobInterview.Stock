using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Views
{
    /// <summary>
    /// Fournit un contrat pour les services permettant d'afficher des données dans une grille.
    /// </summary>
    public interface IGridService
    {
        /// <summary>
        /// Affiche des données fournies dans une vue de grille avec les colonnes spécifiées.
        /// </summary>
        /// <param name="data">Les données à afficher dans la grille.</param>
        /// <param name="columns">La liste des colonnes à utiliser pour structurer la grille.</param>
        /// <returns>Une vue de grille représentant les données fournies structurées par les colonnes spécifiées.</returns>
        IGridView DisplayDataInGrid(object data, IReadOnlyList<GridColumn> columns);
    }

}
