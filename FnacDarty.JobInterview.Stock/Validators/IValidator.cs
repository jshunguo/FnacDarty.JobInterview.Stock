using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.JobInterview.Stock.Validators
{
    /// <summary>
    /// Définit un contrat pour la validation d'une entité de type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Le type de l'entité à valider.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Valide une entité donnée de type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="entity">L'entité à valider.</param>
        /// <returns>Le résultat de la validation encapsulé dans une structure <see cref="ValidatorResult"/>.</returns>
        ValidatorResult Validate(T entity);
    }

    /// <summary>
    /// Représente le résultat de la validation d'une entité.
    /// </summary>
    public struct ValidatorResult
    {
        /// <summary>
        /// Obtient une valeur indiquant si l'entité est valide.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Obtient l'exception associée à la validation, le cas échéant.
        /// </summary>
        public Exception Throws { get; }

        /// <summary>
        /// Initialise une nouvelle instance de la structure <see cref="ValidatorResult"/> avec les valeurs spécifiées.
        /// </summary>
        /// <param name="isValid">Indique si l'entité est valide.</param>
        /// <param name="throws">L'exception associée à la validation, le cas échéant.</param>
        public ValidatorResult(bool isValid, Exception throws)
        {
            IsValid = isValid;
            Throws = throws;
        }
    }

}
