﻿using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQuery : IRequest<GetStudentTriageDataBySurveyIdResult>
    {
        public string SurveyGuid { get; set; }
    }
}