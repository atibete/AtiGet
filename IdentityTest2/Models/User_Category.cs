//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdentityTest2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Category
    {
        public int userId { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
