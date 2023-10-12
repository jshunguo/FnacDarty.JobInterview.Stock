using System;


namespace FnacDarty.JobInterview.Stock.Views
{
    /// <summary>
    /// Contrat permettant de definir une grille de données
    /// </summary>
    public interface IGridDefinition
    {
        /// <summary>
        /// Ajoute une colonne vide à la grille
        /// </summary>
        /// <returns></returns>
        IGridDefinition AddEmptyColumn();

        /// <summary>
        /// Ajoute une colonne à la grille
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IGridDefinition AddColumn(string columnName, Type typeName);

        /// <summary>
        /// Ajoute des colonnes à grille à partir des proprietes de la classe <typeparam name="T"></typeparam>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IGridDefinition AddColumns<T>();

        /// <summary>
        /// Construit la grille d'affichage sur la base de sa definition
        /// </summary>
        /// <returns></returns>
        IGridView CreateView();
    }
}
