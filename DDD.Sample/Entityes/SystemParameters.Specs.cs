using DDD.Sample.Foundation;

namespace DDD.Sample.Entityes
{
    /// <summary>
    /// Obtém o Parametro pelo código 
    /// </summary>
    public class SystemParameterByCode
            : Specification<SystemParameters>
    {
        /// <summary>
        /// Código do Parametro
        /// </summary>
        public readonly string Code;

        public SystemParameterByCode(string code)
        {
            Code = code;
            SatisfiedBy = systemParameterByCode => systemParameterByCode.Code == code;
        }
    }

    /// <summary>
    /// Helper para obter parâmetros do sistema
    /// </summary>
    public static class SystemParameterHelper
    {
        /// <summary>
        /// Obtém o parâmetro pelo código
        /// </summary>
        public static SystemParameters GetByCode(this Repository<SystemParameters> repository, string code)
        {
            return repository.Get(new SystemParameterByCode(code));
        }
    }
}
