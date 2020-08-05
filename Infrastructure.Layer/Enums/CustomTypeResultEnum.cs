namespace Infrastructure.Layer.Enums
{
    /// <summary>
    /// CriticalError deve ser usado apenas para erros nao tratado pelo sistema
    /// </summary>
    public enum CustomTypeResultEnum
    {
        Success = 1,
        Warning = 2,
        Error = 3,
        CriticalError = 4
    }
}
