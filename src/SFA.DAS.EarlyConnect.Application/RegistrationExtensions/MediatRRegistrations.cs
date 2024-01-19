using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;

namespace SFA.DAS.EarlyConnect.Application.RegistrationExtensions
{
    public static class MediatRRegistrations
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<GetStudentTriageDataBySurveyIdQuery>());
            return services;
        }
    }
}
