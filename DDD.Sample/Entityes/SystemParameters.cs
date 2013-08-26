namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Parâmetros do Sistema
    /// </summary>
    public class SystemParameters : Entity 
    {
        /// <summary>
        /// Código do parametro para depósito padrão
        /// </summary>
        public const string DefaultWarehouseCode = "DefaultWarehouse";

        /// <summary>
        /// Código
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        public string Value { get; set; }
    }
}
