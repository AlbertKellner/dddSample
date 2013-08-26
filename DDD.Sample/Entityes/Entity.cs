using System;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Entidade
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Criado por
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Data da Criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Modificado por
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Data da modificação
        /// </summary>
        public DateTime ModifiedAt { get; set; }
    }
}
