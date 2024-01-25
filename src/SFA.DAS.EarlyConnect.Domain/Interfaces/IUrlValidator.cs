
namespace SFA.DAS.EarlyConnect.Domain.Interfaces
{
    public interface IUrlValidator
    {
        bool IsValidLepsCode(string lepsCode);
    }
}