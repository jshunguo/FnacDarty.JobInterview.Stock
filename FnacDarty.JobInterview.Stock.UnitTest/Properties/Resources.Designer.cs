﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FnacDarty.JobInterview.Stock.UnitTest.Properties {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FnacDarty.JobInterview.Stock.UnitTest.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à +---------------------+----------+
        ///|                     | ean12345 |
        ///+---------------------+----------+
        ///| 12/10/2023 00:00:00 | 18       |
        ///+---------------------+----------+
        ///.
        /// </summary>
        internal static string Expected_GetCurrentStockForProduct {
            get {
                return ResourceManager.GetString("Expected_GetCurrentStockForProduct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à +---------------------+----------+
        ///|                     | ean12345 |
        ///+---------------------+----------+
        ///| 12/10/2023 00:00:00 | 78       |
        ///+---------------------+----------+
        ///.
        /// </summary>
        internal static string Expected_GetProductsInStock {
            get {
                return ResourceManager.GetString("Expected_GetProductsInStock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à +---------------------+----------+
        ///|                     | ean12345 |
        ///+---------------------+----------+
        ///| 12/10/2023 00:00:00 | 45       |
        ///+---------------------+----------+
        ///.
        /// </summary>
        internal static string Expected_GetStockForProductAtDate {
            get {
                return ResourceManager.GetString("Expected_GetStockForProductAtDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à +----------+---------------------+---------------------+--------------------+------------------+
        ///|          | 12/10/2023 00:00:00 | 15/10/2023 00:00:00 | Expected_Variation | Actual_Variation |
        ///+----------+---------------------+---------------------+--------------------+------------------+
        ///| ean12345 | 10                  | 50                  | 40                 | 40               |
        ///+----------+---------------------+---------------------+--------------------+------------------+
        ///.
        /// </summary>
        internal static string Expected_GetStockVariationForProduct {
            get {
                return ResourceManager.GetString("Expected_GetStockVariationForProduct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à +---------------------+-------+
        ///|                     | Total |
        ///+---------------------+-------+
        ///| 12/10/2023 00:00:00 | 108   |
        ///+---------------------+-------+
        ///.
        /// </summary>
        internal static string Expected_GetTotalProductsInStock {
            get {
                return ResourceManager.GetString("Expected_GetTotalProductsInStock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à +---------------------+------------+----------+----------+
        ///| Date                | Label      | Quantity | Product  |
        ///+---------------------+------------+----------+----------+
        ///| 01/01/2020 00:00:00 | Achat N°1  | 10       | ean00001 |
        ///| 01/01/2020 00:00:00 | Achat N°2  | 10       | ean00002 |
        ///| 01/01/2020 00:00:00 | Achat N°3  | 10       | ean00003 |
        ///| 02/01/2020 00:00:00 | Cmd N°1    | -3       | ean00001 |
        ///| 02/01/2020 00:00:00 | Cmd N°1    | -3       | ean00002 |
        ///| 02/01/2020 00:00:00 | Cmd N°1  [le reste de la chaîne a été tronqué]&quot;;.
        /// </summary>
        internal static string Expected_Render_Content {
            get {
                return ResourceManager.GetString("Expected_Render_Content", resourceCulture);
            }
        }
    }
}
